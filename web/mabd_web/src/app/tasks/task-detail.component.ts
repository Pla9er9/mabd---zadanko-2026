import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

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

  constructor(route: ActivatedRoute, taskService: TaskService) {
    const id = Number(route.snapshot.paramMap.get('id'));
    this.task = taskService.getTask(id);
  }
}
