import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateProjectGQL, Project, UpdateProjectGQL } from '../../graphql/projects.graphql';
import { Modal, ModalService } from '../../services/modal.service';
import { ProjectService } from '../../services/project.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-project-form-modal',
  templateUrl: './project-form-modal.component.html'
})
export class ProjectFormModalComponent implements Modal, OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  project: Project;
  id: string;

  isActive: boolean;
  isSubmit = false;

  form: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(255)]],
    currency: ['', [Validators.required, Validators.maxLength(255)]]
  });

  constructor(
    private fb: FormBuilder,
    private modalService: ModalService,
    private projectService: ProjectService,
    private createGQL: CreateProjectGQL,
    private updateGQL: UpdateProjectGQL
  ) {
    this.id = 'project-form-modal';
  }

  submit() {
    if (this.form.valid) {
      this.isSubmit = true;
      const value = this.form.value;

      if (this.project) {
        this.updateGQL.mutate({
          ...value,
          id: this.project.id
        }).pipe(takeUntil(this.destroy)).subscribe(r => this.onSubscribe(r.data.updateProject), () => this.isSubmit = false);
      } else {
        this.createGQL.mutate(value).pipe(takeUntil(this.destroy)).subscribe(
          r => this.onSubscribe(r.data.createProject),
          () => this.isSubmit = false
        );
      }
    } else {
      this.form.get('name').markAsTouched();
      this.form.get('currency').markAsTouched();
    }
  }

  isInvalid(input: string): boolean {
    return this.form.get(input).invalid && this.form.get(input).touched;
  }

  close(): void {
    this.isActive = false;
    this.reset();
  }

  open(project?: any): void {
    if (project) {
      this.project = project;
      this.form.get('name').setValue(project.name);
      this.form.get('currency').setValue(project.currency);
    }

    this.isActive = true;
  }

  ngOnInit() {
    this.modalService.add(this);
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
    this.close();
    this.modalService.remove(this.id);
  }

  private reset() {
    this.form.get('name').setValue('');
    this.form.get('currency').setValue('');

    this.form.get('name').markAsUntouched();
    this.form.get('currency').markAsUntouched();

    this.project = undefined;

    this.isSubmit = false;
  }

  private onSubscribe(project: Project) {
    this.projectService.projectEmit(project);
    this.close();
  }
}
