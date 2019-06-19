import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { FindMovementsGQL, Movement } from '../../core/graphql/movements.graphql';

@Injectable({
  providedIn: 'root'
})
export class MovementsResolverService implements Resolve<Movement[]> {
  constructor(private allGQL: FindMovementsGQL) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Movement[]> {
    const id = +route.paramMap.get('id');

    return this
      .allGQL
      .fetch({projectId: id}, {fetchPolicy: 'network-only'})
      .pipe(map(result => result.data.movements));
  }
}
