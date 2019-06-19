import { NgModule } from '@angular/core';
import { Routes, RouterModule, Router } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { HasRoleGuard } from './core/guards/has-role.guard';
import { AuthenticationService } from './core/services/authentication.service';
import { NoAuthGuard } from './core/guards/no-auth.guard';
import { UserResolverService } from './modules/users/user-resolver.service';
import { UsersProfileComponent } from './modules/users/pages/users-profile/users-profile.component';

const routes: Routes = [
  {path: '', redirectTo: '/projects', pathMatch: 'full'},
  {
    path: 'projects',
    loadChildren: './modules/projects/projects.module#ProjectsModule',
    canLoad: [AuthGuard],
    canActivate: [HasRoleGuard],
    data: {roles: ['user', 'admin']}
  },
  {
    path: 'users',
    loadChildren: './modules/users/users.module#UsersModule',
    canLoad: [AuthGuard],
    canActivate: [HasRoleGuard],
    data: {roles: ['admin']}
  },
  {
    path: 'profile',
    component: UsersProfileComponent,
    canLoad: [AuthGuard],
    canActivate: [HasRoleGuard],
    resolve: {user: UserResolverService},
    data: {
      roles: ['user', 'admin'],
      title: 'Title.Profile'
    }
  },
  {
    path: 'login',
    loadChildren: './modules/auth/auth.module#AuthModule',
    canActivate: [NoAuthGuard]
  },
  {
    path: '**',
    loadChildren: './modules/not-found/not-found.module#NotFoundModule',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
  constructor(private router: Router, private authentication: AuthenticationService) {
    this.router.errorHandler = () => {
      // this.authentication.forceLogout();
      // this.router.navigate(['/login']);
    };
  }
}
