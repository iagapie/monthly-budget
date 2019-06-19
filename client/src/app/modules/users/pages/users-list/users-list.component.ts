import { Component, HostBinding, OnInit } from '@angular/core';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';
import { User } from '../../../../core/graphql/users.graphql';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  animations: [fadeInAnimation]
})
export class UsersListComponent implements OnInit {
  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  users$: User[];

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.users$ = this.route.snapshot.data.users;
  }
}
