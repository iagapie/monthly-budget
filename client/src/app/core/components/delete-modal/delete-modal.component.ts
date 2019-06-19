import { Component, OnDestroy, OnInit } from '@angular/core';
import { Modal, ModalService } from '../../services/modal.service';

const message = 'DeleteModal.Message';

export interface DeleteModal {
  delete(data: any): void;
}

@Component({
  selector: 'app-delete-modal',
  templateUrl: './delete-modal.component.html'
})
export class DeleteModalComponent implements Modal, OnInit, OnDestroy {
  id: string;
  isActive: boolean;
  message: string;

  private data: any;

  constructor(private modalService: ModalService) {
    this.id = 'delete-modal';
  }

  close() {
    this.isActive = false;
    this.data = undefined;
  }

  open(data?: any): void {
    this.message = data && data.message ? data.message : message;
    this.data = data;
    this.isActive = true;
  }

  delete() {
    if (this.data) {
      const model = this.data.ref as DeleteModal;

      if (model) {
        model.delete(this.data.data);
      }
    }

    this.close();
  }

  ngOnInit(): void {
    this.modalService.add(this);
  }

  ngOnDestroy(): void {
    this.close();
    this.modalService.remove(this.id);
  }
}
