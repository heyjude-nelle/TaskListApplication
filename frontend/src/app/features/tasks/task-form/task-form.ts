import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TaskItemStatus, TaskPriority } from '../../../core/models/task.model';
import { TaskService } from '../../../core/services/task.service';
import { StatusLabelPipe } from '../../../core/pipes/status-label.pipe';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, StatusLabelPipe],
  templateUrl: './task-form.html'
})
export class TaskForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private taskService = inject(TaskService);

  readonly priorities = Object.values(TaskPriority);
  readonly statuses = Object.values(TaskItemStatus);

  readonly taskId = signal<string | null>(null);
  readonly isEditMode = computed(() => this.taskId() !== null);
  readonly loading = signal(false);
  readonly submitting = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', Validators.maxLength(2000)],
    dueDate: [''],
    priority: [TaskPriority.Medium, Validators.required],
    category: ['', Validators.maxLength(100)],
    status: [TaskItemStatus.ToDo]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.taskId.set(id);
    this.loading.set(true);

    this.taskService.getById(id).subscribe({
      next: (task) => {
        this.form.patchValue({
          title: task.title,
          description: task.description ?? '',
          dueDate: task.dueDate ? task.dueDate.substring(0, 10) : '',
          priority: task.priority,
          category: task.category ?? '',
          status: task.status
        });
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Could not load this task. Please try again.');
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.errorMessage.set(null);

    const raw = this.form.getRawValue();
    const payload = {
      title: raw.title!,
      description: raw.description || null,
      dueDate: raw.dueDate ? new Date(raw.dueDate).toISOString() : null,
      priority: raw.priority!,
      category: raw.category || null
    };

    const id = this.taskId();

    const request = id
      ? this.taskService.update(id, { ...payload, status: raw.status! })
      : this.taskService.create(payload);

    request.subscribe({
      next: (task) => this.router.navigate(['/tasks', task.id]),
      error: () => {
        this.submitting.set(false);
        this.errorMessage.set('Could not save this task. Please check your input and try again.');
      }
    });
  }
}
