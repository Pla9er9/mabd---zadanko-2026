import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { TaskService } from './task.service';
import { Task } from './task.model';
import { materialImports } from '../material';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ...materialImports],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  private taskService = inject(TaskService);

  statuses = this.taskService.getStatuses();
  categories: string[] = [];
  statusFilter = '';
  categoryFilter = '';
  dueDateFilter = '';
  filteredTasks: Task[] = [];
  isLoading = false;
  feedback: string | null = null;

  ngOnInit(): void {
    this.loadTasks();
  }

  applyFilters(): void {
    this.loadTasks();
  }

  clearFilters(): void {
    this.statusFilter = '';
    this.categoryFilter = '';
    this.dueDateFilter = '';
    this.loadTasks();
  }

  toggleDone(task: Task): void {
    this.feedback = null;
    this.taskService.toggleTaskStatus(task.id).subscribe({
      next: () => this.loadTasks(),
      error: (error: Error) => (this.feedback = error.message)
    });
  }

  deleteTask(task: Task): void {
    this.feedback = null;
    this.taskService.deleteTask(task.id).subscribe({
      next: () => this.loadTasks(),
      error: (error: Error) => (this.feedback = error.message)
    });
  }

  private loadTasks(): void {
    this.isLoading = true;
    this.feedback = null;
    this.taskService
      .getAllTasks({
        status: this.statusFilter,
        category: this.categoryFilter,
        dueDate: this.dueDateFilter
      })
      .subscribe({
        next: (tasks) => {
          this.filteredTasks = tasks;
          this.categories = Array.from(new Set(tasks.map((task) => task.category))).sort();
          this.isLoading = false;
        },
        error: (error: Error) => {
          this.filteredTasks = [];
          this.feedback = error.message;
          this.isLoading = false;
        }
      });
  }
}
