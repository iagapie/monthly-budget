import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { FindProjectsGQL, Project } from '../../core/graphql/projects.graphql';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProjectsResolverService implements Resolve<Project[]> {
  constructor(private allGQL: FindProjectsGQL) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Project[]> {
    return this
      .allGQL
      .fetch(undefined, {fetchPolicy: 'network-only'})
      .pipe(map(result => result.data.projects));
  }
}
