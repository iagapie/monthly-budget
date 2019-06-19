import { Component, Inject, Input, LOCALE_ID, OnDestroy, OnInit } from '@angular/core';
import { Project } from '../../../../core/graphql/projects.graphql';
import {
  CreateMovementItemGQL,
  MovementItem,
  RemoveMovementItemGQL,
  UpdateMovementItemGQL
} from '../../../../core/graphql/movements.graphql';
import { Subject } from 'rxjs';
import { FormControl, Validators } from '@angular/forms';
import { AppValidators } from '../../../../shared/utils/app-validators';
import { takeUntil } from 'rxjs/operators';
import { FormatWidth, getLocaleDateFormat } from '@angular/common';

@Component({
  selector: 'app-project-transactions-table',
  templateUrl: './project-transactions-table.component.html',
  styleUrls: ['./project-transactions-table.component.scss']
})
export class ProjectTransactionsTableComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @Input() title: string;
  @Input() project: Project;
  @Input() categories: { id: number, name: string }[];
  @Input() movementItems: { movement: string, item: MovementItem }[];

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

  dateFormat: string;

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private createGQL: CreateMovementItemGQL,
    private updateGQL: UpdateMovementItemGQL,
    private removeGQL: RemoveMovementItemGQL
  ) {
  }

  ngOnInit(): void {
    this.dateFormat = getLocaleDateFormat(this.locale, FormatWidth.Short);
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  reset() {
    this.dateControl.setValue('');
    this.amountControl.setValue('');
    this.categoryControl.setValue('');
    this.descriptionControl.setValue('');

    this.dateControl.markAsUntouched();
    this.amountControl.markAsUntouched();
    this.categoryControl.markAsUntouched();
    this.descriptionControl.markAsUntouched();
  }

  add() {
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

    this.createGQL.mutate({
      movementId: +this.categoryControl.value,
      date: new Date(Date.parse(this.dateControl.value)),
      amount: +this.amountControl.value,
      description: this.descriptionControl.value
    }).pipe(takeUntil(this.destroy)).subscribe(result => {
      this.reset();
      const item = result.data.createMovementItem;
      const movement = this.categories.filter(x => +x.id === +item.movementId)[0].name;
      this.movementItems.push({movement, item});
    });
  }

  remove(id: number) {
    const index = this.movementItems.findIndex(v => +v.item.id === +id);

    if (index === -1) {
      return;
    }

    this.removeGQL
      .mutate({id})
      .pipe(takeUntil(this.destroy))
      .subscribe(() => this.movementItems.splice(index, 1));
  }

  update(data: { name: string, id: number }) {
    const index = this.movementItems.findIndex(v => +v.item.id === data.id);

    if (index === -1) {
      return;
    }

    this.movementItems[index].movement = data.name;

    this.updateGQL
      .mutate(this.movementItems[index].item)
      .pipe(takeUntil(this.destroy))
      .subscribe(result => this.movementItems[index].item = result.data.updateMovementItem);
  }
}
