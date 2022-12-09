import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, filter, map, Observable, of } from 'rxjs';

const Anonymous: Session = null;

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  
  private readonly session = new BehaviorSubject<Session>(Anonymous)
  public readonly session$: Observable<Session> = this.session

  public readonly authenticated$ = this.session$.pipe(
    map(UserIsAuthenticated)
  );

  public readonly anonymous$ = this.session$.pipe(
    map(UserIsAnonymous)
  );

  public readonly username$ = this.session$.pipe(
    filter(UserIsAuthenticated),
    map(s => s.find(c => c.type === 'name')?.value)
    )

  constructor(private http: HttpClient) { }

  public getSession() {
    this.http.get<Session>('bff/user').pipe(
      catchError(err => {
        return of(Anonymous);
      })
    )
    .subscribe(session => {
        this.session.next(session);
    });
  }
}

export interface Claim {
  type: string;
  value: string;
}
export type Session = Claim[] | null;

function UserIsAuthenticated(s: Session): s is Claim[] {
  return s !== null;
}

function UserIsAnonymous(s: Session): s is null {
  return s === null;
}
