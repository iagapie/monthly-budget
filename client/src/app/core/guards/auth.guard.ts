import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CoreModule } from '../core.module';
import { SecurityService } from '../services/security.service';

@Injectable({
  providedIn: CoreModule
})
export class AuthGuard implements CanActivate, CanLoad {
  constructor(private router: Router, private security: SecurityService) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.isAuthenticated;
  }

  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this.isAuthenticated;
  }

  private get isAuthenticated(): boolean {
    if (this.security.isAuthenticated) {
      return true;
    }

    console.warn('You don\'t have permission to view this page.');
    this.router.navigate(['/login']);

    return false;
  }
}
