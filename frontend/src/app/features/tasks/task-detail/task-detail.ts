import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TaskItem } from '../../../core/models/task.model';
import { TaskService } from '../../../core/services/task.service';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './task-detail.html'
})
export class TaskDetail implements OnInit {
  readonly task = signal<TaskItem | null>(null);
  readonly loading = signal(true);
  readonly errorMessage = signal<string | null>(null);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;

    this.taskService.getById(id).subscribe({
      next: (task) => {
        this.task.set(task);
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(
          err.status === 404 ? 'Task not found.' : 'Could not load this task. Please try again.'
        );
      }
    });
  }

  deleteTask(): void {
    const task = this.task();
    if (!task || !confirm(`Delete "${task.title}"? This cannot be undone.`)) {
      return;
    }

    this.taskService.delete(task.id).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: () => this.errorMessage.set('Could not delete this task. Please try again.')
    });
  }
}
