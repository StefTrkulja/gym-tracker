import { environment } from '../../../env/environment';
import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of, map } from 'rxjs';
import { LoginRequest, RegisterRequest, CurrentUser } from './models/auth.models';
import { switchMap } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseUrl = 'https://localhost:7003/api/auth';

  isLoggedIn = signal<boolean>(false);
  currentUser = signal<CurrentUser | null>(null);

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<boolean> {
    return this.http.post<void>( environment.apiHost + `auth/login`, request).pipe(
      tap(() => this.isLoggedIn.set(true)),
      switchMap(() => this.fetchCurrentUser())

    );
  }

  register(request: RegisterRequest): Observable<boolean> {
    return this.http.post<void>(environment.apiHost + `auth/register`, request).pipe(
      tap(() => this.isLoggedIn.set(true)),
      switchMap(() => this.fetchCurrentUser())
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(environment.apiHost + `auth/logout`, {}).pipe(
      tap(() => {
        this.isLoggedIn.set(false);
        this.currentUser.set(null);
      })
    );
  }

  fetchCurrentUser(): Observable<boolean> {
    return this.http.get<CurrentUser>(environment.apiHost + `auth/me`).pipe(
      tap((user) => {
        this.currentUser.set(user);
        this.isLoggedIn.set(true);
      }),
      map(() => true),
      catchError(() => {
        this.isLoggedIn.set(false);
        this.currentUser.set(null);
        return of(false);
      })
    );
  }
}