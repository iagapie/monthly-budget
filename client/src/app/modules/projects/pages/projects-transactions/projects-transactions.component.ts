import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TitleService } from '../../../../core/services/title.service';
import { Project } from '../../../../core/graphql/projects.graphql';
import { MovementItem } from '../../../../core/graphql/movements.graphql';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';

@Component({
  selector: 'app-projects-transactions',
  templateUrl: './projects-transactions.component.html',
  styleUrls: ['./projects-transactions.component.scss'],
  animations: [fadeInAnimation]
})
export class ProjectsTransactionsComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  project$: Project;

  expensesCategory$: { id: number, name: string }[];
  incomeCategory$: { id: number, name: string }[];

  expensesItems$: { movement: string, item: MovementItem }[];
  incomeItems$: { movement: string, item: MovementItem }[];

  constructor(private route: ActivatedRoute, private title: TitleService) {
  }

  ngOnInit() {
    this.project$ = this.route.snapshot.data.project;
    this.title.set(this.project$.name);
    this.title.add('Title.Transactions');

    this.expensesCategory$ = this.categories(-1);
    this.incomeCategory$ = this.categories(1);

    this.expensesItems$ = this.movementItems(-1);
    this.incomeItems$ = this.movementItems(1);
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  private categories(direction: number) {
    return this.route.snapshot.data.movements
      .filter(x => +x.direction.id === direction)
      .map(x => {
        return {id: x.id, name: x.name};
      })
      .sort((a, b) => {
        if (a.name > b.name) {
          return 1;
        }

        if (a.name < b.name) {
          return -1;
        }

        return 0;
      });
  }

  private movementItems(direction: number) {
    return this.route.snapshot.data.movements
      .filter(x => +x.direction.id === direction)
      .map(x => x.movementItems.map(item => {
        return {movement: x.name, item};
      }))
      .reduce((a, b) => a.concat(b), [])
      .sort((a, b) => {
        if (a.item.createdAt > b.item.createdAt) {
          return 1;
        }

        if (a.item.createdAt < b.item.createdAt) {
          return -1;
        }

        return 0;
      });
  }
}
