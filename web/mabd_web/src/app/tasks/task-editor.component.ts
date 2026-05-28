import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { TaskService } from './task.service';
import { Task } from './task.model';
import { materialImports } from '../material';

@Component({
  selector: 'app-task-editor',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, ...materialImports],
  templateUrl: './task-editor.component.html',
  styleUrls: ['./task-editor.component.scss']
})
export class TaskEditorComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private taskService = inject(TaskService);

  task?: Task;
  taskId = Number(this.route.snapshot.paramMap.get('id'));
  form = this.fb.group({
    title: [''],
    description: [''],
    status: ['Open'],
    category: [''],
    dueDate: ['']
  });
  statusOptions = this.taskService.getStatuses();
  categoryOptions = this.taskService.getCategories();
  feedback: string | null = null;

  constructor() {
    if (this.taskId) {
      this.task = this.taskService.getTask(this.taskId);
    }
    if (this.task) {
      this.form.setValue({
        title: this.task.title,
        description: this.task.description,
        status: this.task.status,
        category: this.task.category,
        dueDate: this.task.dueDate
      });
    }
  }

  save(): void {
    if (this.form.invalid) {
      this.feedback = 'Uzupełnij wszystkie pola zadania.';
      return;
    }

    const payload: Task = {
      id: this.task?.id ?? 0,
      title: this.form.value.title ?? '',
      description: this.form.value.description ?? '',
      status: this.form.value.status as Task['status'],
      category: this.form.value.category ?? '',
      dueDate: this.form.value.dueDate ?? ''
    };

    this.taskService.saveTask(payload);
    this.router.navigate(['/tasks']);
  }
}
