import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListComponent } from './pages/users-list/users-list.component';
import { UsersCreateComponent } from './pages/users-create/users-create.component';
import { UsersUpdateComponent } from './pages/users-update/users-update.component';
import { UsersProfileComponent } from './pages/users-profile/users-profile.component';
import { UsersRoutingModule } from './users-routing.module';
import { ChangePasswordFormComponent } from './components/change-password-form/change-password-form.component';
import { UpdateUserFormComponent } from './components/update-user-form/update-user-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    UsersListComponent,
    UsersCreateComponent,
    UsersUpdateComponent,
    UsersProfileComponent,
    ChangePasswordFormComponent,
    UpdateUserFormComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    ReactiveFormsModule,
    TranslateModule
  ]
})
export class UsersModule { }
