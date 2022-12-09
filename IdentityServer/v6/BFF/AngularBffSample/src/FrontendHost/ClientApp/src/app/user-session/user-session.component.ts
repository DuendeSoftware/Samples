import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-session',
  templateUrl: './user-session.component.html',
  styleUrls: ['./user-session.component.css']
})
export class UserSessionComponent {
  public claims: Claim[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Claim[]>(baseUrl + 'bff/user', {
      headers: {
        "X-CSRF": "1"
      }
    }).subscribe(result => {
      this.claims = result;
    }, error => console.error(error));
  }
}

interface Claim {
  type: string;
  value: string;
}
