import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-user-session',
  templateUrl: './user-session.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserSessionComponent {
  public readonly userSessionInfo$ = this.authService.userSessionInfo$;

  public constructor(private authService: AuthService) {}
}
