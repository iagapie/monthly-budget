import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import particles from '../../../../../assets/particles.json';
import '../../../../../assets/particles.js';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { Router } from '@angular/router';
import { takeUntil, tap } from 'rxjs/operators';

declare var particlesJS: any;

@Component({
  selector: 'app-auth-login',
  templateUrl: './auth-login.component.html',
  styleUrls: ['./auth-login.component.scss'],
  animations: [fadeInAnimation]
})
export class AuthLoginComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  form: FormGroup = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  isSubmit = false;
  errorMessage: string;

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  constructor(private fb: FormBuilder, private authentication: AuthenticationService, private router: Router) {
  }

  ngOnInit() {
    const jsonUri = 'data:text/plain;base64,' + window.btoa(JSON.stringify(particles));

    particlesJS.load('particles-js', jsonUri, null);
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  submit() {
    if (this.form.valid) {
      this.isSubmit = true;
      this.authentication
        .login(this.form.value)
        .pipe(tap(() => this.form.reset()), takeUntil(this.destroy))
        .subscribe(() => this.router.navigate(['/projects']), error => {
          this.form.controls.username.markAsTouched();
          this.form.controls.password.markAsTouched();

          if (error) {
            this.errorMessage = error.message;
          }

          this.isSubmit = false;
        });
    } else {
      this.form.controls.username.markAsTouched();
      this.form.controls.password.markAsTouched();
    }
  }

  isInvalid(input: string): boolean {
    return this.form.get(input).invalid && this.form.get(input).touched;
  }
}
