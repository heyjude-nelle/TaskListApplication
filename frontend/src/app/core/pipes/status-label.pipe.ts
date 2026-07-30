import { Pipe, PipeTransform } from '@angular/core';
import { TaskItemStatus } from '../models/task.model';

const LABELS: Record<TaskItemStatus, string> = {
  [TaskItemStatus.ToDo]: 'To do',
  [TaskItemStatus.InProgress]: 'In progress',
  [TaskItemStatus.Completed]: 'Completed'
};

@Pipe({ name: 'statusLabel', standalone: true })
export class StatusLabelPipe implements PipeTransform {
  transform(value: TaskItemStatus): string {
    return LABELS[value];
  }
}
