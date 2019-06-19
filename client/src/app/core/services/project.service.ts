import { EventEmitter, Injectable } from '@angular/core';
import { Project } from '../graphql/projects.graphql';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  public projectEmitter$: EventEmitter<Project>;

  constructor() {
    this.projectEmitter$ = new EventEmitter();
  }

  projectEmit(project: Project) {
    this.projectEmitter$.emit(project);
  }
}
