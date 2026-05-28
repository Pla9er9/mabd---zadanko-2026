import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUser = new BehaviorSubject<string | null>(null);
  private users = new Map<string, string>();

  register(username: string, password: string): { success: boolean; message: string } {
    if (this.users.has(username)) {
      return { success: false, message: 'Użytkownik już istnieje' };
    }

    this.users.set(username, password);
    this.currentUser.next(username);
    return { success: true, message: 'Rejestracja zakończona sukcesem' };
  }

  login(username: string, password: string): { success: boolean; message: string } {
    const saved = this.users.get(username);
    if (!saved || saved !== password) {
      return { success: false, message: 'Nieprawidłowy login lub hasło' };
    }

    this.currentUser.next(username);
    return { success: true, message: 'Zalogowano pomyślnie' };
  }

  logout(): void {
    this.currentUser.next(null);
  }

  isAuthenticated(): boolean {
    return !!this.currentUser.value;
  }
}
