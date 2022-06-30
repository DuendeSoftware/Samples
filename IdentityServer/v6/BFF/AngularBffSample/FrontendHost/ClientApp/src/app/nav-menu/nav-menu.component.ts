import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { UserSessionInfo } from '../types';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavMenuComponent {
  @Input() public loggedIn: boolean | null = null;

  @Output() public readonly login = new EventEmitter<void>();

  @Output() public readonly logout = new EventEmitter<void>();

  public isExpanded: boolean = false;

  public collapse(): void {
    this.isExpanded = false;
  }

  public toggle(): void {
    this.isExpanded = !this.isExpanded;
  }
}
