import { Component, HostBinding, OnInit } from '@angular/core';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../../../core/graphql/users.graphql';

@Component({
  selector: 'app-users-update',
  templateUrl: './users-update.component.html',
  styleUrls: ['./users-update.component.scss'],
  animations: [fadeInAnimation]
})
export class UsersUpdateComponent implements OnInit {
  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  user$: User;

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.user$ = this.route.snapshot.data.user;
  }
}
