import { Component, inject } from '@angular/core';
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
export class TaskListComponent {
  private taskService = inject(TaskService);
  statuses = this.taskService.getStatuses();
  categories = this.taskService.getCategories();
  statusFilter = '';
  categoryFilter = '';
  dueDateFilter = '';
  filteredTasks: Task[] = this.taskService.getAllTasks();

  applyFilters(): void {
    this.filteredTasks = this.taskService.getAllTasks({
      status: this.statusFilter,
      category: this.categoryFilter,
      dueDate: this.dueDateFilter
    });
  }
}
