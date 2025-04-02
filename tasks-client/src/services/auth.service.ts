import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { interval, tap } from 'rxjs';
import { configuration } from '../app/configuration';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = configuration.baseUrl;

  token?: string;
  refreshToken?: string;

  http = inject(HttpClient);

  constructor() {
    this.token = localStorage.getItem('token') || undefined;
    this.refreshToken = localStorage.getItem('refreshToken') || undefined;
    interval(1000 * 60 * 5).pipe(takeUntilDestroyed()).subscribe(() => {
      const prevRefreshToken = this.refreshToken;
      if (this.refreshToken) {
        this.refreshTokenRequest().subscribe({
          next: ({ token, refreshToken }) => {
            if (prevRefreshToken !== this.refreshToken) {
              return;
            }
            this.token = token;
            this.refreshToken = refreshToken;
            localStorage.setItem('token', token);
            localStorage.setItem('refreshToken', refreshToken);
          },
          error: () => {
            if (prevRefreshToken !== this.refreshToken) {
              return;
            }
            this.token = undefined;
            this.refreshToken = undefined;
          }
        });
      }
    });
  }

  register(username: string, password: string) {
    return this.http.post(this.baseUrl + '/register', { email: username, password });
  }

  login(username: string, password: string) {
    return this.http.post<{ token: string, refreshToken: string }>(this.baseUrl + '/login', { email: username, password }).pipe(tap(({ token, refreshToken }) => {
      this.token = token;
      this.refreshToken = refreshToken;
      localStorage.setItem('token', token);
      localStorage.setItem('refreshToken', refreshToken);
    }));
  }

  refreshTokenRequest() {
    return this.http.post<{ token: string, refreshToken: string }>(this.baseUrl + '/refresh-token', { refreshToken: this.refreshToken }).pipe(tap(({ token, refreshToken }) => {
      this.token = token;
      this.refreshToken = refreshToken;
    }));
  }

}
