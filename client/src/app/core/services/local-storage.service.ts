import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  private localStorage: Storage = localStorage;
  private prefix = 'finance';

  constructor() {
    if (!localStorage) {
      throw new Error('The browser does not support Local Storage');
    }
  }

  set(key: string, value: any): void {
    this.localStorage[`${this.prefix}-${key}`] = JSON.stringify(value);
  }

  get(key: string): any {
    try {
      return JSON.parse(this.localStorage.getItem(`${this.prefix}-${key}`)) || false;
    } catch (e) {
      return false;
    }
  }

  remove(key: string) {
    this.localStorage.removeItem(`${this.prefix}-${key}`);
  }

  clear() {
    this.localStorage.clear();
  }
}
