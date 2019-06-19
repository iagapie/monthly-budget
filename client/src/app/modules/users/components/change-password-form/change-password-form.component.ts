import { Component, Input, OnDestroy } from '@angular/core';
import { ChangePasswordGQL, User } from '../../../../core/graphql/users.graphql';
import { Subject } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-change-password-form',
  templateUrl: './change-password-form.component.html',
  styleUrls: ['./change-password-form.component.scss']
})
export class ChangePasswordFormComponent implements OnDestroy {
  private destroy: Subject<any> = new Subject();

  @Input() user: User;

  form$: FormGroup = new FormGroup({
    currentPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ]),
    newPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ]),
  });

  isSubmit = false;
  isMessage = false;
  isError = false;

  constructor(private changePassword: ChangePasswordGQL) {
  }

  isInvalid(input: string): boolean {
    return this.form$.get(input).invalid && this.form$.get(input).touched;
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  submit() {
    this.isMessage = false;
    this.isError = false;

    if (this.form$.valid) {
      this.isSubmit = true;
      this.changePassword.mutate({
        ...this.form$.value,
        id: this.user.id
      }).pipe(takeUntil(this.destroy)).subscribe(() => {
        this.reset();
        this.isMessage = true;
        this.isSubmit = true;
      }, () => {
        this.reset();
        this.isError = true;
        this.isSubmit = false;
      });
    } else {
      this.form$.get('currentPassword').markAsTouched();
      this.form$.get('newPassword').markAsTouched();
    }
  }

  private reset() {
    this.form$.setValue({
      currentPassword: '',
      newPassword: ''
    });

    this.form$.get('currentPassword').markAsUntouched();
    this.form$.get('newPassword').markAsUntouched();
  }
}
