import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { UserSessionInfo } from './types';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly userSessionInfo =
    new BehaviorSubject<UserSessionInfo | null>(null);
  public readonly userSessionInfo$: Observable<UserSessionInfo | null> =
    this.userSessionInfo;

  public readonly loggedIn$: Observable<boolean> = this.userSessionInfo$.pipe(
    map(Boolean)
  );

  public readonly usersName$: Observable<string | undefined> =
    this.userSessionInfo$.pipe(
      map(
        (userSessionInfo) =>
          userSessionInfo?.find((claim) => claim.type === 'name')?.value
      )
    );

  public constructor(private http: HttpClient, private router: Router) {}

  public fetchUserSessionInfo(): void {
    console.log(this.router.url);
    this.http
      .get<UserSessionInfo>('bff/user')
      .pipe(
        catchError((err) => {
          console.error(err);
          return of(null);
        })
      )
      .subscribe((userSessionInfo) => {
        this.userSessionInfo.next(userSessionInfo);
      });
  }

  public login(): void {
    const returnUrl = this.router.url;
    document.location.href = `/bff/login?returnUrl=${returnUrl}`;
  }

  public logout(): void {
    const userSessionInfo = this.userSessionInfo.getValue();
    const logoutUrl =
      userSessionInfo?.find((claim) => claim.type === 'bff:logout_url')
        ?.value ?? '/bff/logout';

    document.location.href = `${logoutUrl}&returnUrl=/home`;
  }
}
