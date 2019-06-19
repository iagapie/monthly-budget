import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateUserGQL } from '../../../../core/graphql/users.graphql';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users-create',
  templateUrl: './users-create.component.html',
  styleUrls: ['./users-create.component.scss'],
  animations: [fadeInAnimation]
})
export class UsersCreateComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

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
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ])
  });

  isSubmit = false;

  constructor(private createGQL: CreateUserGQL, private router: Router) {
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

  submit() {
    if (this.form$.valid) {
      this.isSubmit = true;
      this.createGQL.mutate(this.form$.value).pipe(takeUntil(this.destroy))
        .subscribe(() => this.router.navigate(['/users']), () => this.isSubmit = false);
    } else {
      for (const key of ['userName', 'email', 'role', 'firstName', 'lastName', 'password']) {
        this.form$.get(key).markAsTouched();
      }
    }
  }

  reset() {
    for (const key of ['userName', 'email', 'role', 'firstName', 'lastName', 'password']) {
      this.form$.get(key).setValue('');
      this.form$.get(key).markAsUntouched();
    }
  }
}
