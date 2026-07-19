import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.model';

const API_URL = 'http://localhost:5077/api/auth';
const TOKEN_KEY = 'tasklist_token';
const USERNAME_KEY = 'tasklist_username';

@Injectable({ providedIn: 'root' })
export class AuthService {
  readonly currentUsername = signal<string | null>(localStorage.getItem(USERNAME_KEY));

  constructor(private http: HttpClient) {}

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_URL}/register`, request)
      .pipe(tap((response) => this.storeSession(response)));
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_URL}/login`, request)
      .pipe(tap((response) => this.storeSession(response)));
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USERNAME_KEY);
    this.currentUsername.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  private storeSession(response: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, response.token);
    localStorage.setItem(USERNAME_KEY, response.username);
    this.currentUsername.set(response.username);
  }
}
