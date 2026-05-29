import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, map, of, switchMap, throwError } from 'rxjs';

import { AuthService } from '../auth/auth.service';
import { Task, TaskPayload, TaskStatus } from './task.model';

interface ApiTask {
  id: number;
  created_at: string;
  title: string;
  description: string;
  category: string;
  due_date?: string;
  dueDate?: string;
  isDone: boolean;
  user_id: number;
}

interface ApiTaskRequest {
  title: string;
  description: string;
  category: string;
  due_date: string;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly http = inject(HttpClient);
  private readonly authService = inject(AuthService);
  private readonly apiUrl = 'http://localhost:8080/api/tasks';

  getStatuses(): TaskStatus[] {
    return ['Open', 'Done'];
  }

  getAllTasks(filters: { status?: string; category?: string; dueDate?: string } = {}): Observable<Task[]> {
    let params = new HttpParams();
    if (filters.category) {
      params = params.set('category', filters.category);
    }
    if (filters.status === 'Done') {
      params = params.set('isDone', true);
    }
    if (filters.status === 'Open') {
      params = params.set('isDone', false);
    }

    return this.http.get<ApiTask[]>(this.apiUrl, { headers: this.authHeaders(), params }).pipe(
      map((tasks) =>
        tasks
          .map((task) => this.fromApiTask(task))
          .filter((task) => !filters.dueDate || task.dueDate === filters.dueDate)
      ),
      catchError((error) => this.handleTaskError(error))
    );
  }

  getTask(id: number): Observable<Task | undefined> {
    return this.http.get<ApiTask>(`${this.apiUrl}/${id}`, { headers: this.authHeaders() }).pipe(
      map((task) => this.fromApiTask(task)),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          return of(undefined);
        }
        return this.handleTaskError(error);
      })
    );
  }

  saveTask(task: TaskPayload, id?: number): Observable<Task> {
    const request = this.toApiTaskRequest(task);
    const savedTask = id
      ? this.http.put<ApiTask>(`${this.apiUrl}/${id}`, request, { headers: this.authHeaders() })
      : this.http.post<ApiTask>(this.apiUrl, request, { headers: this.authHeaders() });

    return savedTask.pipe(
      map((apiTask) => this.fromApiTask(apiTask)),
      switchMap((apiTask) => this.syncDoneStatus(apiTask, task.status === 'Done')),
      catchError((error) => this.handleTaskError(error))
    );
  }

  toggleTaskStatus(id: number): Observable<Task> {
    return this.http.patch<ApiTask>(`${this.apiUrl}/${id}/toggle`, {}, { headers: this.authHeaders() }).pipe(
      map((task) => this.fromApiTask(task)),
      catchError((error) => this.handleTaskError(error))
    );
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.authHeaders() }).pipe(
      catchError((error) => this.handleTaskError(error))
    );
  }

  private syncDoneStatus(task: Task, shouldBeDone: boolean): Observable<Task> {
    if (task.isDone === shouldBeDone) {
      return of(task);
    }

    return this.toggleTaskStatus(task.id);
  }

  private fromApiTask(task: ApiTask): Task {
    const dueDate = task.due_date ?? task.dueDate ?? '';
    return {
      id: task.id,
      title: task.title,
      description: task.description,
      status: task.isDone ? 'Done' : 'Open',
      category: task.category,
      dueDate,
      isDone: task.isDone
    };
  }

  private toApiTaskRequest(task: TaskPayload): ApiTaskRequest {
    return {
      title: task.title,
      description: task.description,
      category: task.category,
      due_date: task.dueDate
    };
  }

  private authHeaders(): HttpHeaders {
    const authorization = this.authService.getAuthorizationHeader();
    return authorization ? new HttpHeaders({ Authorization: authorization }) : new HttpHeaders();
  }

  private handleTaskError(error: HttpErrorResponse): Observable<never> {
    if (error.status === 401) {
      this.authService.logout();
      return throwError(() => new Error('Sesja wygasła. Zaloguj się ponownie.'));
    }
    if (error.status === 0) {
      return throwError(() => new Error('Brak połączenia z serwerem API.'));
    }
    if (error.error?.message) {
      return throwError(() => new Error(error.error.message));
    }
    if (typeof error.error === 'string' && error.error.trim()) {
      return throwError(() => new Error(error.error));
    }
    return throwError(() => new Error('Nie udało się wykonać operacji na zadaniu.'));
  }
}
