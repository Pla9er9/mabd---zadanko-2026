import { Routes } from '@angular/router';

import { LandingComponent } from './landing/landing.component';
import { AuthComponent } from './auth/auth.component';
import { TaskListComponent } from './tasks/task-list.component';
import { TaskDetailComponent } from './tasks/task-detail.component';
import { TaskEditorComponent } from './tasks/task-editor.component';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'auth', component: AuthComponent },
  { path: 'tasks', component: TaskListComponent, canActivate: [authGuard] },
  { path: 'tasks/new', component: TaskEditorComponent, canActivate: [authGuard] },
  { path: 'tasks/:id', component: TaskDetailComponent, canActivate: [authGuard] },
  { path: 'tasks/:id/edit', component: TaskEditorComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
