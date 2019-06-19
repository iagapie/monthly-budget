import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor(private localStorage: LocalStorageService, private cookie: CookieService) { }

  clear(): void {
    this.cookie.delete('token-refresh');
    this.localStorage.remove('token-access');
  }

  get access(): string {
    return this.localStorage.get('token-access');
  }

  set access(token: string) {
    this.localStorage.set('token-access', token);
  }

  get refresh(): string {
    return this.cookie.get('token-refresh');
  }

  set refresh(token: string) {
    this.cookie.set('token-refresh', token);
  }
}
