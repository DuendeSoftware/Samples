import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
})
export class TodosComponent implements OnInit {
  private readonly todos = new BehaviorSubject<Todo[]>([]);
  public readonly todos$: Observable<Todo[]> = this.todos;

  private readonly errors = new BehaviorSubject<string>('');
  public readonly error$: Observable<string> = this.errors;

  public date = "";
  public name = "";

  public constructor(private http: HttpClient) { }

  public ngOnInit(): void {
    this.fetchTodos();
  }

  public createTodo(): void {
    this.http
      .post<Todo>('todos', {
        name: this.name,
        date: this.date,
      })
      .pipe(catchError(err => { this.errors.next(err); throw err; }))
      .subscribe((todo) => {
        const todos = [...this.todos.getValue(), todo];
        this.todos.next(todos);
      });
  }

  public deleteTodo(id: number): void {
    this.http.delete(`todos/${id}`)
      .pipe(catchError(err => { this.errors.next(err); throw err; }))
      .subscribe(() => {
      const todos = this.todos.getValue().filter((x) => x.id !== id);
      this.todos.next(todos);
    });
  }

  private fetchTodos(): void {
    this.http
      .get<Todo[]>('todos')
      .pipe(catchError(err => {
        this.errors.next(err.message);
        throw err;
      }))
      .subscribe((todos) => {
        this.todos.next(todos);
      });
  }
}

interface Todo {
  id: number;
  name: string;
  date: string;
  user: string;
}
