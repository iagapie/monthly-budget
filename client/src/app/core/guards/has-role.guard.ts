import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CoreModule } from '../core.module';
import { SecurityService } from '../services/security.service';

@Injectable({
  providedIn: CoreModule
})
export class HasRoleGuard implements CanActivate {
  constructor(private router: Router, private security: SecurityService) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const roles = next.data.roles;

    if (roles && this.security.hasRole(roles)) {
      return true;
    }

    console.warn('You don\'t have permission to view this page.');
    this.router.navigate(['/login']);

    return false;
  }

}
