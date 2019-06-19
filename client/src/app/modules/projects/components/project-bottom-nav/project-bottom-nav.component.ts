import { Component, Input } from '@angular/core';
import { Project } from '../../../../core/graphql/projects.graphql';

@Component({
  selector: 'app-project-bottom-nav',
  templateUrl: './project-bottom-nav.component.html',
  styleUrls: ['./project-bottom-nav.component.scss']
})
export class ProjectBottomNavComponent {
  @Input() project: Project;
}
