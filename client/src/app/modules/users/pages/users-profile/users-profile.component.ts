import { Component, HostBinding, OnInit } from '@angular/core';
import { fadeInAnimation } from '../../../../shared/animations/fade-in.animation';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../../../core/graphql/users.graphql';

@Component({
  selector: 'app-users-profile',
  templateUrl: './users-profile.component.html',
  styleUrls: ['./users-profile.component.scss'],
  animations: [fadeInAnimation]
})
export class UsersProfileComponent implements OnInit {
  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  user$: User;

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.user$ = this.route.snapshot.data.user;
  }
}
