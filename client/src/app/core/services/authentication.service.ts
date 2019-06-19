import { Injectable } from '@angular/core';
import { EMPTY, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenService } from './token.service';
import { environment } from '../../../environments/environment';
import { catchError, map, tap } from 'rxjs/operators';
import { Apollo } from 'apollo-angular';
import { Router } from '@angular/router';

export interface LoginContext {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private httpClient: HttpClient, private apollo: Apollo, private token: TokenService, private router: Router) {
  }

  login(context: LoginContext): Observable<any> {
    return this.httpClient.post<any>(environment.login_url, context, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      map(event => event.data),
      tap(event => {
        this.token.access = event.token.access;
        this.token.refresh = event.token.refresh;
      }),
      catchError(error => throwError(error.error.error))
    );
  }

  logout(): Observable<any> {
    this.forceLogout();
    return EMPTY;
  }

  forceLogout() {
    this.token.clear();
    this.apollo.getClient().resetStore().then(
      () => this.router.navigate(['/login'])
    );
  }

  refreshToken(): Observable<any> {
    return this.httpClient.post<any>(environment.refresh_token_url, {}, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token.access}`,
        'Refresh-Token': this.token.refresh
      })
    }).pipe(
      map(event => {
        this.token.access = event.data.token.access;
        this.token.refresh = event.data.token.refresh;

        return event.data;
      }),
      catchError(error => {
        this.forceLogout();
        return throwError(error);
      })
    );
  }
}
