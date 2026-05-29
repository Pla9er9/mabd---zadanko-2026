import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

export interface AuthResult {
  success: boolean;
  message: string;
}

interface AuthResponse {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUser = new BehaviorSubject<string | null>(null);
  private readonly apiUrl = 'http://localhost:8080';
  private readonly isBrowser: boolean;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.currentUser.next(this.getStoredValue('auth_username'));
  }

  register(username: string, password: string): Observable<AuthResult> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, { username, password }).pipe(
      tap(() => this.setSession(username, password)),
      map((response) => ({
        success: true,
        message: response.message || 'Rejestracja zakończona sukcesem'
      })),
      catchError((error: HttpErrorResponse) =>
        of({
          success: false,
          message: this.authErrorMessage(error, 'Nie udało się zarejestrować użytkownika')
        })
      )
    );
  }

  login(username: string, password: string): Observable<AuthResult> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, { username, password }).pipe(
      tap(() => this.setSession(username, password)),
      map((response) => ({
        success: true,
        message: response.message || 'Zalogowano pomyślnie'
      })),
      catchError((error: HttpErrorResponse) =>
        of({
          success: false,
          message: this.authErrorMessage(error, 'Nie udało się zalogować')
        })
      )
    );
  }

  logout(): void {
    this.removeStoredValue('auth_username');
    this.removeStoredValue('auth_password');
    this.currentUser.next(null);
  }

  isAuthenticated(): boolean {
    return !!this.currentUser.value;
  }

  getAuthorizationHeader(): string | null {
    const username = this.getStoredValue('auth_username');
    const password = this.getStoredValue('auth_password');
    if (!username || !password) {
      return null;
    }

    return `Basic ${btoa(`${username}:${password}`)}`;
  }

  private setSession(username: string, password: string): void {
    this.setStoredValue('auth_username', username);
    this.setStoredValue('auth_password', password);
    this.currentUser.next(username);
  }

  private getStoredValue(key: string): string | null {
    return this.isBrowser ? localStorage.getItem(key) : null;
  }

  private setStoredValue(key: string, value: string): void {
    if (this.isBrowser) {
      localStorage.setItem(key, value);
    }
  }

  private removeStoredValue(key: string): void {
    if (this.isBrowser) {
      localStorage.removeItem(key);
    }
  }

  private authErrorMessage(error: HttpErrorResponse, fallback: string): string {
    if (error.status === 0) {
      return 'Brak połączenia z serwerem API';
    }
    if (error.status === 401) {
      return 'Nieprawidłowy login lub hasło';
    }
    if (error.status === 409) {
      return 'Użytkownik już istnieje';
    }
    if (error.error?.message) {
      return error.error.message;
    }
    if (typeof error.error === 'string' && error.error.trim()) {
      return error.error;
    }
    return fallback;
  }
}
