export interface UserClaim {
  type: string | 'bff:logout-url';
  value: string;
}

export type UserSessionInfo = UserClaim[];

export interface Todo {
  id: number;
  date: string;
  name: string;
  user: string;
}
