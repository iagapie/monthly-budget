import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsRoutingModule } from './projects-routing.module';
import { ProjectsListComponent } from './pages/projects-list/projects-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProjectsSummaryComponent } from './pages/projects-summary/projects-summary.component';
import { ProjectSummaryTableComponent } from './components/project-summary-table/project-summary-table.component';
import { ProjectTransactionsTableComponent } from './components/project-transactions-table/project-transactions-table.component';
import { CurrencyComponent } from './components/currency/currency.component';
import { ProjectSummaryTableRowComponent } from './components/project-summary-table-row/project-summary-table-row.component';
import { ProjectTransactionsTableRowComponent } from './components/project-transactions-table-row/project-transactions-table-row.component';
import { ProjectsTransactionsComponent } from './pages/projects-transactions/projects-transactions.component';
import { ProjectBottomNavComponent } from './components/project-bottom-nav/project-bottom-nav.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    CurrencyComponent,
    ProjectsListComponent,
    ProjectsSummaryComponent,
    ProjectSummaryTableComponent,
    ProjectTransactionsTableComponent,
    ProjectsTransactionsComponent,
    ProjectSummaryTableRowComponent,
    ProjectTransactionsTableRowComponent,
    ProjectBottomNavComponent,
  ],
  imports: [
    CommonModule,
    ProjectsRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    TranslateModule
  ]
})
export class ProjectsModule {
}
