import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Movement } from '../../../../core/graphql/movements.graphql';
import { Project } from '../../../../core/graphql/projects.graphql';
import { Calc } from '../../../../shared/utils/calc';
import { FormControl, Validators } from '@angular/forms';
import { AppValidators } from '../../../../shared/utils/app-validators';
import { ModalService } from '../../../../core/services/modal.service';
import { DeleteModal } from '../../../../core/components/delete-modal/delete-modal.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: '[app-project-summary-table-row]',
  templateUrl: './project-summary-table-row.component.html',
  styleUrls: ['./project-summary-table-row.component.scss']
})
export class ProjectSummaryTableRowComponent implements DeleteModal, OnInit {
  @Input() project: Project;
  @Input() movement: Movement;
  @Output() removeEvent: EventEmitter<number> = new EventEmitter();
  @Output() updateEvent: EventEmitter<number> = new EventEmitter();

  actual: number;
  diff: number;

  edit = false;

  nameControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(2),
    Validators.maxLength(255)
  ]);

  planAmountControl: FormControl = new FormControl('0', [
    Validators.required,
    AppValidators.positiveNumberValidator
  ]);

  constructor(private modalService: ModalService) {
  }

  ngOnInit() {
    this.calc();
    this.nameControl.setValue(this.movement.name);
    this.planAmountControl.setValue(this.movement.planAmount);
  }

  update() {
    if (this.nameControl.invalid) {
      this.nameControl.markAsTouched();
      return;
    }

    if (this.planAmountControl.invalid) {
      this.planAmountControl.markAsTouched();
      return;
    }

    this.movement.name = this.nameControl.value;
    this.movement.planAmount = +this.planAmountControl.value;

    this.calc();

    this.edit = false;

    this.updateEvent.emit(+this.movement.id);
  }

  remove() {
    this.modalService.open('delete-modal', {
      ref: this,
      data: this.movement.id
    });
  }

  delete(data: any): void {
    this.removeEvent.emit(data);
  }

  private calc() {
    const data = Calc.calc(this.movement);
    this.actual = data.actual;
    this.diff = data.diff;
  }
}
