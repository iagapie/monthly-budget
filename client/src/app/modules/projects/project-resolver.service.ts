import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { FindProjectGQL, Project } from '../../core/graphql/projects.graphql';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProjectResolverService implements Resolve<Project> {
  constructor(private findGQL: FindProjectGQL) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Project> {
    const id = +route.paramMap.get('id');

    return this
      .findGQL
      .fetch({id})
      .pipe(map(result => result.data.project));
  }
}
