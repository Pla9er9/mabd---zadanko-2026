import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { TaskService } from './task.service';
import { Task } from './task.model';
import { materialImports } from '../material';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, ...materialImports],
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.scss']
})
export class TaskDetailComponent {
  task: Task | undefined;
  isLoading = true;
  feedback: string | null = null;

  constructor(
    route: ActivatedRoute,
    private taskService: TaskService,
    private router: Router
  ) {
    const id = Number(route.snapshot.paramMap.get('id'));
    this.taskService.getTask(id).subscribe({
      next: (task) => {
        this.task = task;
        this.isLoading = false;
      },
      error: (error: Error) => {
        this.feedback = error.message;
        this.isLoading = false;
      }
    });
  }

  toggleDone(): void {
    if (!this.task) {
      return;
    }

    this.feedback = null;
    this.taskService.toggleTaskStatus(this.task.id).subscribe({
      next: (task) => (this.task = task),
      error: (error: Error) => (this.feedback = error.message)
    });
  }

  deleteTask(): void {
    if (!this.task) {
      return;
    }

    this.feedback = null;
    this.taskService.deleteTask(this.task.id).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: (error: Error) => (this.feedback = error.message)
    });
  }
}
