import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SecurityService } from '../../services/security.service';
import { ModalService } from '../../services/modal.service';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();
  private isLogin = false;

  burger = false;

  constructor(
    private authentication: AuthenticationService,
    private security: SecurityService,
    private modalService: ModalService,
    private router: Router
  ) {
  }

  get user(): string {
    return this.security.user;
  }

  get isAdmin(): boolean {
    return this.security.hasRole('admin');
  }

  get isAuthenticated(): boolean {
    return !this.isLogin && this.security.isAuthenticated;
  }

  ngOnInit(): void {
    this.router.events.pipe(takeUntil(this.destroy)).subscribe(
      (event: any) => {
        if (event instanceof NavigationEnd) {
          this.isLogin = this.router.url === '/login';
        }
      }
    );
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.unsubscribe();
  }

  onLogout(): void {
    this.authentication
      .logout()
      .pipe(takeUntil(this.destroy))
      .subscribe();
  }

  openModal() {
    this.modalService.open('project-form-modal');
  }
}
