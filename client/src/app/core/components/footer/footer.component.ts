import { Component, HostListener, Inject, OnDestroy, OnInit } from '@angular/core';
import { SecurityService } from '../../services/security.service';
import { DOCUMENT } from '@angular/common';
import { Subject } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {
  private destroy: Subject<any> = new Subject();
  private isLogin = false;
  private windowScrolled: boolean;

  constructor(@Inject(DOCUMENT) private document: Document, private security: SecurityService, private router: Router) {
  }

  get isAuthenticated(): boolean {
    return !this.isLogin && this.security.isAuthenticated;
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    if (window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop > 100) {
      this.windowScrolled = true;
    } else if (this.windowScrolled && window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop < 10) {
      this.windowScrolled = false;
    }
  }

  scrollToTop() {
    (function smoothscroll() {
      const currentScroll = document.documentElement.scrollTop || document.body.scrollTop;

      if (currentScroll > 0) {
        window.requestAnimationFrame(smoothscroll);
        window.scrollTo(0, currentScroll - (currentScroll / 8));
      }
    })();
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
}
