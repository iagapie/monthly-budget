import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  constructor(private tokenService: TokenService, private jwtHelper: JwtHelperService) {
  }

  get isAuthenticated(): boolean {
    return !!this.tokenService.access && !!this.tokenService.refresh; // && !this.jwtHelper.isTokenExpired(this.tokenService.access);
  }

  get user(): string {
    if (this.isAuthenticated) {
      const payload = this.jwtHelper.decodeToken(this.tokenService.access);
      return payload.sub;
    }

    return 'anonymous';
  }

  hasRole(rol: any): boolean {
    if (typeof rol === 'string') {
      return this.isRoleValid(rol);
    }

    const roles: string[] = rol;

    for (const role of roles) {
      if (this.isRoleValid(role)) {
        return true;
      }
    }

    return false;
  }

  private isRoleValid(role: string): boolean {
    if (!this.isAuthenticated) {
      return false;
    }

    const payload = this.jwtHelper.decodeToken(this.tokenService.access);

    return payload.rol === role;
  }
}
