import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { TodosService } from './todos.service';

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TodosComponent implements OnInit {
  public todos$ = this.todosService.todos$;

  public todoDate: string = '';

  public todoName: string = '';

  public constructor(private todosService: TodosService) {}

  public ngOnInit(): void {
    this.todosService.fetchTodos();
  }

  public createTodo(): void {
    this.todosService.createTodo(this.todoName, this.todoDate);
  }

  public deleteTodo(id: number): void {
    this.todosService.deleteTodo(id);
  }
}
