import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HasRoleDirective } from './directives/has-role.directive';
import { IsAuthDirective } from './directives/is-auth.directive';

@NgModule({
  declarations: [
    HasRoleDirective,
    IsAuthDirective
  ],
  exports: [
    HasRoleDirective,
    IsAuthDirective
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
