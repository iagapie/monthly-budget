import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class AppValidators {
  static get positiveNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = +control.value;
      return isNaN(value) || value < 0 ? {positiveNumber: true} : null;
    };
  }

  static get dateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      return !control.value || isNaN(Date.parse(control.value)) ? {date: true} : null;
    };
  }
}
