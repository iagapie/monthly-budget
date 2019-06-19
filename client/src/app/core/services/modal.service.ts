import { Injectable } from '@angular/core';

export interface Modal {
  id: string;
  open(data?: any): void;
  close(): void;
}

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modals: Modal[] = [];

  add(modal: Modal) {
    this.modals.push(modal);
  }

  remove(id: string) {
    this.modals = this.modals.filter(x => x.id !== id);
  }

  open(id: string, data?: any) {
    this.modals.filter(x => x.id === id)[0].open(data);
  }

  close(id: string) {
    this.modals.filter(x => x.id === id)[0].close();
  }
}
