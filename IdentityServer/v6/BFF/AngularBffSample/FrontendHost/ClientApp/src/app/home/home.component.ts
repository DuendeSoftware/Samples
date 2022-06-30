import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent {
  public readonly usersName$ = this.authService.usersName$;

  public constructor(private authService: AuthService) {}
}
