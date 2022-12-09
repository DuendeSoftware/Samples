import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService, Session } from '../authentication.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-session',
  templateUrl: './user-session.component.html',
  styleUrls: ['./user-session.component.css']
})
export class UserSessionComponent {
  public session$: Observable<Session>;
  public authenticated$: Observable<boolean>;

  constructor(auth: AuthenticationService) {
    this.session$ = auth.session$
    this.authenticated$ = auth.authenticated$;
  }
}
