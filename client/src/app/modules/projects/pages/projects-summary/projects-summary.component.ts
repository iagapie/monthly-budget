import { ChangeDetectorRef, Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Project } from '../../../../core/graphql/projects.graphql';
import { ActivatedRoute } from '@angular/router';
import { TitleService } from '../../../../core/services/title.service';
import { Movement } from '../../../../core/graphql/movements.graphql';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';

@Component({
  selector: 'app-projects-summary',
  templateUrl: './projects-summary.component.html',
  styleUrls: ['./projects-summary.component.scss'],
  animations: [fadeInAnimation]
})
export class ProjectsSummaryComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  project$: Project;
  expenses$: Movement[];
  income$: Movement[];

  private inActual = 0;
  private outActual = 0;

  constructor(private route: ActivatedRoute, private title: TitleService, private cdRef: ChangeDetectorRef) {
  }

  ngOnInit() {
    this.project$ = this.route.snapshot.data.project;
    this.title.set(this.project$.name);
    this.title.add('Title.Summary');

    this.expenses$ = this.movements(-1);
    this.income$ = this.movements(1);
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  get saved(): number {
    return this.inActual - this.outActual;
  }

  onInActual(actual) {
    this.inActual = actual;
    this.cdRef.detectChanges();
  }

  onOutActual(actual) {
    this.outActual = actual;
    this.cdRef.detectChanges();
  }

  private movements(direction: number): Movement[] {
    return this.route.snapshot.data.movements
      .filter(x => +x.direction.id === direction)
      .sort((a: Movement, b: Movement) => {
        if (a.createdAt > b.createdAt) {
          return 1;
        }

        if (a.createdAt < b.createdAt) {
          return -1;
        }

        return 0;
      });
  }
}
