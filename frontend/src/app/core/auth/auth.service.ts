import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, RegisterRequest, LoginResponse } from './models/auth.models';
import { environment } from '../../../env/environment';
const TOKEN_KEY = 'auth_token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  isLoggedIn = signal<boolean>(!!localStorage.getItem(TOKEN_KEY));

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(environment.apiHost + 'auth/login', request).pipe(
      tap((response) => this.setToken(response.token))
    );
  }

  register(request: RegisterRequest): Observable<void> {
    return this.http.post<void>(environment.apiHost + 'auth/register', request);
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    this.isLoggedIn.set(false);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
    this.isLoggedIn.set(true);
  }
}