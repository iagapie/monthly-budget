import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { SecurityService } from '../../core/services/security.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {
  constructor(
    private container: ViewContainerRef,
    private template: TemplateRef<any>,
    private security: SecurityService
  ) {
  }

  @Input() set hasRole(rol: any) {
    this.container.clear();

    if (this.security.hasRole(rol)) {
      // Add template to DOM
      this.container.createEmbeddedView(this.template);
    } else {
      // Remove template from DOM
      this.container.clear();
    }
  }
}
