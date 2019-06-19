import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { ProjectFormModalComponent } from './components/project-form-modal/project-form-modal.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DeleteModalComponent } from './components/delete-modal/delete-modal.component';
import { TranslateModule } from '@ngx-translate/core';

export function AppJwtTokenGetter() {
  return null;
}

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    ProjectFormModalComponent,
    DeleteModalComponent
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    ProjectFormModalComponent,
    DeleteModalComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    JwtModule.forRoot({config: {tokenGetter: AppJwtTokenGetter}}),
    SharedModule,
    ReactiveFormsModule,
    TranslateModule
  ],
  providers: [
    JwtHelperService
  ]
})
export class CoreModule { }
