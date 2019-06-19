import { Component, EventEmitter, Inject, Input, LOCALE_ID, OnInit, Output } from '@angular/core';
import { Project } from '../../../../core/graphql/projects.graphql';
import { MovementItem } from '../../../../core/graphql/movements.graphql';
import { FormControl, Validators } from '@angular/forms';
import { AppValidators } from '../../../../shared/utils/app-validators';
import { formatDate, FormatWidth, getLocaleDateFormat } from '@angular/common';
import { DeleteModal } from '../../../../core/components/delete-modal/delete-modal.component';
import { ModalService } from '../../../../core/services/modal.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: '[app-project-transactions-table-row]',
  templateUrl: './project-transactions-table-row.component.html',
  styleUrls: ['./project-transactions-table-row.component.scss']
})
export class ProjectTransactionsTableRowComponent implements DeleteModal, OnInit {
  @Input() project: Project;
  @Input() categories: { id: number, name: string }[];
  @Input() movementName: string;
  @Input() movementItem: MovementItem;

  @Output() removeEvent: EventEmitter<number> = new EventEmitter();
  @Output() updateEvent: EventEmitter<{ name: string, id: number }> = new EventEmitter();

  edit = false;
  dateFormat: string;

  dateControl: FormControl = new FormControl('', [
    Validators.required,
    AppValidators.dateValidator
  ]);

  amountControl: FormControl = new FormControl('', [
    Validators.required,
    AppValidators.positiveNumberValidator
  ]);

  categoryControl: FormControl = new FormControl('', [
    Validators.required
  ]);

  descriptionControl: FormControl = new FormControl('', [
    Validators.maxLength(200)
  ]);

  constructor(@Inject(LOCALE_ID) private locale: string, private modalService: ModalService) {
  }

  ngOnInit() {
    this.dateFormat = getLocaleDateFormat(this.locale, FormatWidth.Short);
    this.dateControl.setValue(formatDate(this.movementItem.date, this.dateFormat, this.locale));
    this.amountControl.setValue(this.movementItem.amount);
    this.categoryControl.setValue(this.movementItem.movementId);
    this.descriptionControl.setValue(this.movementItem.description);
  }

  update() {
    if (this.dateControl.invalid) {
      this.dateControl.markAsTouched();
      return;
    }

    if (this.amountControl.invalid) {
      this.amountControl.markAsTouched();
      return;
    }

    if (this.categoryControl.invalid) {
      this.categoryControl.markAsTouched();
      return;
    }

    if (this.descriptionControl.invalid) {
      this.descriptionControl.markAsTouched();
      return;
    }

    this.movementItem.date = new Date(Date.parse(this.dateControl.value));
    this.movementItem.amount = +this.amountControl.value;
    this.movementItem.movementId = +this.categoryControl.value;
    this.movementItem.description = this.descriptionControl.value;

    this.movementName = this.categories.filter(x => +x.id === +this.movementItem.movementId)[0].name;

    this.edit = false;

    this.updateEvent.emit({
      name: this.movementName,
      id: +this.movementItem.id
    });
  }

  remove() {
    this.modalService.open('delete-modal', {
      ref: this,
      data: this.movementItem.id
    });
  }

  delete(data: any): void {
    this.removeEvent.emit(data);
  }
}
