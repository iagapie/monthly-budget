import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Project, RemoveProjectGQL } from '../../../../core/graphql/projects.graphql';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';
import { ProjectService } from '../../../../core/services/project.service';
import { ModalService } from '../../../../core/services/modal.service';
import { DeleteModal } from '../../../../core/components/delete-modal/delete-modal.component';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss'],
  animations: [fadeInAnimation]
})
export class ProjectsListComponent implements DeleteModal, OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  projects$: Project[];

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private modalService: ModalService,
    private removeGQL: RemoveProjectGQL
  ) {
  }

  ngOnInit() {
    this.projects$ = this.route.snapshot.data.projects;
    this.projectService.projectEmitter$.pipe(takeUntil(this.destroy)).subscribe(item => this.onModalSubmit(item));
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  onModalSubmit(project: Project) {
    const index = this.projects$.findIndex(x => +x.id === +project.id);

    if (index === -1) {
      this.projects$.unshift(project);
    } else {
      this.projects$[index] = project;
    }
  }

  openModal(project: Project) {
    this.modalService.open('project-form-modal', project);
  }

  remove(id: number) {
    this.modalService.open('delete-modal', {
      ref: this,
      data: id
    });
  }

  delete(id: any): void {
    const index = this.projects$.findIndex(p => +p.id === +id);

    if (index === -1) {
      return;
    }

    this.removeGQL.mutate({id}).pipe(takeUntil(this.destroy)).subscribe(() => this.projects$.splice(index, 1));
  }
}
