import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { Todo } from '../types';

@Injectable({ providedIn: 'root' })
export class TodosService {
  private readonly todos = new BehaviorSubject<Todo[]>([]);
  public readonly todos$: Observable<Todo[]> = this.todos;

  public constructor(private http: HttpClient) {}

  public fetchTodos(): void {
    this.http
      .get<Todo[]>('todos')
      // .pipe(
      //   catchError((err) => {
      //     console.error(err);
      //     return of(null);
      //   })
      // )
      .subscribe((todos) => {
        this.todos.next(todos);
      });
  }

  public createTodo(name: string, date: string): void {
    this.http
      .post<Todo>('todos', {
        name,
        date,
      })
      .subscribe((todo) => {
        const todos = [...this.todos.getValue(), todo];
        this.todos.next(todos);
      });
  }

  public deleteTodo(id: number): void {
    this.http.delete(`todos/${id}`).subscribe(() => {
      const todos = this.todos.getValue().filter((x) => x.id !== id);
      this.todos.next(todos);
    });
  }
}
