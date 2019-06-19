import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectsListComponent } from './pages/projects-list/projects-list.component';
import { ProjectsResolverService } from './projects-resolver.service';
import { ProjectResolverService } from './project-resolver.service';
import { ProjectsSummaryComponent } from './pages/projects-summary/projects-summary.component';
import { MovementsResolverService } from './movements-resolver.service';
import { ProjectsTransactionsComponent } from './pages/projects-transactions/projects-transactions.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectsListComponent,
    pathMatch: 'full',
    resolve: {projects: ProjectsResolverService},
    data: {title: 'Title.Projects'}
  },
  {
    path: ':id/summary',
    component: ProjectsSummaryComponent,
    resolve: {
      project: ProjectResolverService,
      movements: MovementsResolverService
    },
    data: {title: 'Title.SummaryProject'}
  },
  {
    path: ':id/transactions',
    component: ProjectsTransactionsComponent,
    resolve: {
      project: ProjectResolverService,
      movements: MovementsResolverService
    },
    data: {title: 'Title.TransactionsProject'}
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectsRoutingModule {
}
