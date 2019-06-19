import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { CoreModule } from '../core.module';
import { SecurityService } from '../services/security.service';

@Injectable({
  providedIn: CoreModule
})
export class NoAuthGuard implements CanActivate {
  constructor(private router: Router, private security: SecurityService) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.security.isAuthenticated) {
      console.warn('Only Unauthorized users.');
      this.router.navigate(['/projects']);

      return false;
    }

    return true;
  }
}
