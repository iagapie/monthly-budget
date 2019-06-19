import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { UpdateUserGQL, User } from '../../../../core/graphql/users.graphql';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { SecurityService } from '../../../../core/services/security.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-update-user-form',
  templateUrl: './update-user-form.component.html',
  styleUrls: ['./update-user-form.component.scss']
})
export class UpdateUserFormComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @Input() user: User;

  roles$ = [{value: 'user', name: 'user'}, {value: 'admin', name: 'admin'}];

  form$: FormGroup = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(255)
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(255),
      Validators.email
    ]),
    role: new FormControl('', Validators.maxLength(5)),
    firstName: new FormControl('', Validators.maxLength(255)),
    lastName: new FormControl('', Validators.maxLength(255)),
  });

  isSubmit = false;
  isMessage = false;
  isError = false;

  constructor(private security: SecurityService, private updateGQL: UpdateUserGQL) {
  }

  ngOnInit() {
    this.reset();
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  isInvalid(input: string): boolean {
    return this.form$.get(input).invalid && this.form$.get(input).touched;
  }

  get withRoleSelect(): boolean {
    return this.security.hasRole('admin') && this.security.user !== this.user.userName;
  }

  submit() {
    this.isMessage = false;
    this.isError = false;

    if (this.form$.valid) {
      this.isSubmit = true;
      this.updateGQL.mutate({
        ...this.form$.value,
        id: this.user.id
      }).pipe(takeUntil(this.destroy)).subscribe(result => {
        this.user = result.data.updateUser;
        this.isMessage = true;
        this.isSubmit = false;
      }, () => {
        this.isError = true;
        this.isSubmit = false;
      });
    } else {
      for (const key of ['userName', 'email', 'role', 'firstName', 'lastName']) {
        this.form$.get(key).markAsTouched();
      }
      this.isSubmit = false;
    }
  }

  reset() {
    this.isMessage = false;
    this.isError = false;

    this.form$.setValue({
      userName: this.user.userName,
      email: this.user.email,
      role: this.user.role,
      firstName: this.user.firstName,
      lastName: this.user.lastName
    });

    for (const key of ['userName', 'email', 'role', 'firstName', 'lastName']) {
      this.form$.get(key).markAsUntouched();
    }
  }
}
