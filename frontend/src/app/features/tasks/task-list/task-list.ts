import { Component, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { TaskItem } from '../../../core/models/task.model';
import { TaskService } from '../../../core/services/task.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './task-list.html'
})
export class TaskList implements OnInit {
  readonly tasks = signal<TaskItem[]>([]);
  readonly loading = signal(true);
  readonly errorMessage = signal<string | null>(null);

  constructor(
    private taskService: TaskService,
    protected authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.taskService.getAll().subscribe({
      next: (tasks) => {
        this.tasks.set(tasks);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Could not load your tasks. Please try again.');
        this.loading.set(false);
      }
    });
  }

  deleteTask(task: TaskItem): void {
    if (!confirm(`Delete "${task.title}"? This cannot be undone.`)) {
      return;
    }

    this.taskService.delete(task.id).subscribe({
      next: () => this.tasks.update((tasks) => tasks.filter((t) => t.id !== task.id)),
      error: () => this.errorMessage.set('Could not delete this task. Please try again.')
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
