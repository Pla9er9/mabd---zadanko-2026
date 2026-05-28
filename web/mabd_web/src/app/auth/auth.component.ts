import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';
import { materialImports } from '../material';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ...materialImports],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  selectedTab = 0;
  loginForm = this.fb.group({
    username: [''],
    password: ['']
  });
  registerForm = this.fb.group({
    username: [''],
    password: ['']
  });
  feedback: string | null = null;

  login(): void {
    const { username, password } = this.loginForm.value;
    if (!username || !password) {
      this.feedback = 'Wypełnij wszystkie pola logowania';
      return;
    }

    const result = this.authService.login(username, password);
    this.feedback = result.message;
    if (result.success) {
      this.router.navigate(['/tasks']);
    }
  }

  register(): void {
    const { username, password } = this.registerForm.value;
    if (!username || !password) {
      this.feedback = 'Wypełnij wszystkie pola rejestracji';
      return;
    }

    const result = this.authService.register(username, password);
    this.feedback = result.message;
    if (result.success) {
      this.router.navigate(['/tasks']);
    }
  }
}
