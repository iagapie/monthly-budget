import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-currency',
  template: `{{ signChar }}{{ amount | currency:code }}`
})
export class CurrencyComponent {
  @Input() code: string;
  @Input() amount: number;
  @Input() sign: any = false;

  get signChar(): string {
    return this.sign ? this.sign : '';
  }
}
