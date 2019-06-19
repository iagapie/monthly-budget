import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { SecurityService } from '../../core/services/security.service';

@Directive({
  selector: '[appIsAuth]'
})
export class IsAuthDirective {
  constructor(
    private container: ViewContainerRef,
    private template: TemplateRef<any>,
    private security: SecurityService
  ) {
  }

  @Input() set appIsAuth(isAuth: boolean) {
    this.container.clear();

    if (this.security.isAuthenticated === isAuth) {
      // Add template to DOM
      this.container.createEmbeddedView(this.template);
    } else {
      // Remove template from DOM
      this.container.clear();
    }
  }
}
