import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Project } from '../../../../core/graphql/projects.graphql';
import { CreateMovementGQL, Movement, RemoveMovementGQL, UpdateMovementGQL } from '../../../../core/graphql/movements.graphql';
import { Calc } from '../../../../shared/utils/calc';
import { FormControl, Validators } from '@angular/forms';
import { AppValidators } from '../../../../shared/utils/app-validators';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-project-summary-table',
  templateUrl: './project-summary-table.component.html',
  styleUrls: ['./project-summary-table.component.scss'],
})
export class ProjectSummaryTableComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @Input() title: string;
  @Input() project: Project;
  @Input() movements: Movement[];
  @Input() directionId: number;
  @Output() actualEvent: EventEmitter<number> = new EventEmitter();

  planned = 0;
  actual = 0;
  diff = 0;

  nameControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(2),
    Validators.maxLength(255)
  ]);

  planAmountControl: FormControl = new FormControl('0', [
    Validators.required,
    AppValidators.positiveNumberValidator
  ]);

  constructor(
    private createGQL: CreateMovementGQL,
    private updateGQL: UpdateMovementGQL,
    private removeGQL: RemoveMovementGQL
  ) {
  }

  ngOnInit() {
    this.calc();
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  reset() {
    this.nameControl.setValue('');
    this.planAmountControl.setValue('0');

    this.nameControl.markAsUntouched();
    this.planAmountControl.markAsUntouched();
  }

  add() {
    if (this.nameControl.invalid) {
      this.nameControl.markAsTouched();
      return;
    }

    if (this.planAmountControl.invalid) {
      this.planAmountControl.markAsTouched();
      return;
    }

    this.createGQL.mutate({
      projectId: this.project.id,
      name: this.nameControl.value,
      planAmount: +this.planAmountControl.value,
      directionId: this.directionId
    }).pipe(takeUntil(this.destroy)).subscribe(result => {
      this.reset();
      this.movements.push(result.data.createMovement);
      this.calc();
    });
  }

  remove(id: number) {
    const index = this.movements.findIndex(v => +v.id === +id);

    if (index === -1) {
      return;
    }

    this.removeGQL.mutate({id}).pipe(takeUntil(this.destroy)).subscribe(() => {
      this.movements.splice(index, 1);
      this.calc();
    });
  }

  update(id: number) {
    const index = this.movements.findIndex(v => +v.id === id);

    if (index === -1) {
      return;
    }

    this.updateGQL.mutate({
      ...this.movements[index],
      directionId: this.movements[index].direction.id
    }).pipe(takeUntil(this.destroy)).subscribe(result => {
      this.movements[index] = result.data.updateMovement;
      this.calc();
    });
  }

  get max(): number {
    return Math.max(this.planned, this.actual);
  }

  private totalPlanned(): number {
    return this.movements.map(x => x.planAmount).reduce((a, b) => a + b, 0);
  }

  private totalActual(): number {
    return this.movements.map(x => Calc.actual(x)).reduce((a, b) => a + b, 0);
  }

  private calc() {
    this.planned = this.totalPlanned();
    this.actual = this.totalActual();
    this.diff = Calc.diffFn(this.directionId)(this.planned, this.actual);
    this.actualEvent.emit(this.actual);
  }
}
