import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { TaskService } from './task.service';
import { Task, TaskPayload } from './task.model';
import { materialImports } from '../material';

@Component({
  selector: 'app-task-editor',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, ...materialImports],
  templateUrl: './task-editor.component.html',
  styleUrls: ['./task-editor.component.scss']
})
export class TaskEditorComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private taskService = inject(TaskService);

  task?: Task;
  taskId = Number(this.route.snapshot.paramMap.get('id'));
  form = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    status: ['Open', Validators.required],
    category: ['', Validators.required],
    dueDate: ['', Validators.required]
  });
  statusOptions = this.taskService.getStatuses();
  feedback: string | null = null;
  isLoading = false;
  isSaving = false;

  ngOnInit(): void {
    if (!this.taskId) {
      return;
    }

    this.isLoading = true;
    this.taskService.getTask(this.taskId).subscribe({
      next: (task) => {
        this.task = task;
        this.isLoading = false;
        if (!task) {
          this.feedback = 'Nie odnaleziono zadania.';
          return;
        }

        this.form.setValue({
          title: task.title,
          description: task.description,
          status: task.status,
          category: task.category,
          dueDate: task.dueDate
        });
      },
      error: (error: Error) => {
        this.feedback = error.message;
        this.isLoading = false;
      }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.feedback = 'Uzupełnij wymagane pola zadania.';
      return;
    }

    const payload: TaskPayload = {
      title: this.form.value.title ?? '',
      description: this.form.value.description ?? '',
      status: this.form.value.status as TaskPayload['status'],
      category: this.form.value.category ?? '',
      dueDate: this.form.value.dueDate ?? ''
    };

    this.isSaving = true;
    this.feedback = null;
    this.taskService.saveTask(payload, this.task?.id).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: (error: Error) => {
        this.feedback = error.message;
        this.isSaving = false;
      }
    });
  }
}
