import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersListComponent } from './pages/users-list/users-list.component';
import { UsersResolverService } from './users-resolver.service';
import { UsersCreateComponent } from './pages/users-create/users-create.component';
import { UsersUpdateComponent } from './pages/users-update/users-update.component';
import { UserResolverService } from './user-resolver.service';

const routes: Routes = [
  {
    path: '',
    component: UsersListComponent,
    pathMatch: 'full',
    resolve: {users: UsersResolverService},
    data: {title: 'Title.Users'}
  },
  {
    path: 'create',
    component: UsersCreateComponent,
    data: {title: 'Title.CreateUser'}
  },
  {
    path: ':id/update',
    component: UsersUpdateComponent,
    resolve: {user: UserResolverService},
    data: {title: 'Title.UpdateUser'}
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule {
}
