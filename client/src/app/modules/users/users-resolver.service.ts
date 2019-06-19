import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { FindUsersGQL, User } from '../../core/graphql/users.graphql';
import { Observable } from 'rxjs';
import { SecurityService } from '../../core/services/security.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersResolverService implements Resolve<User[]> {

  constructor(private allGQL: FindUsersGQL, private security: SecurityService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User[]> | User[] {
    if (this.security.hasRole('admin')) {
      return this
        .allGQL
        .fetch(undefined, {fetchPolicy: 'network-only'})
        .pipe(map(result => result.data.users));
    }

    return [];
  }
}
