import { Component } from '@angular/core';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  public username$ = this.auth.username$;
  public authenticated$ = this.auth.authenticated$;
  public anonymous$ = this.auth.anonymous$;

  constructor(private auth: AuthenticationService) {
    auth.getSession();
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
