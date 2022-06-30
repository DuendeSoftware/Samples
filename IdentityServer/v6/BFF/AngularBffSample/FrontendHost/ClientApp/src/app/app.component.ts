import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit {
  public readonly loggedIn$ = this.authService.loggedIn$;

  public constructor(private authService: AuthService) {}

  public ngOnInit(): void {
    this.authService.fetchUserSessionInfo();
  }

  public login(): void {
    this.authService.login();
  }

  public logout(): void {
    this.authService.logout();
  }
}
