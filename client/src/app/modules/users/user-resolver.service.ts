import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { FindUserGQL, User } from '../../core/graphql/users.graphql';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SecurityService } from '../../core/services/security.service';

@Injectable({
  providedIn: 'root'
})
export class UserResolverService implements Resolve<User> {
  constructor(private findGQL: FindUserGQL, private security: SecurityService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User> {
    const id: number = this.security.hasRole('admin') && route.paramMap.has('id') ? +route.paramMap.get('id') : null;

    return this
      .findGQL
      .fetch({id})
      .pipe(map(result => result.data.user));
  }
}
