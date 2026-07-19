export enum TaskPriority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High'
}

export enum TaskItemStatus {
  ToDo = 'ToDo',
  InProgress = 'InProgress',
  Completed = 'Completed'
}

export interface TaskItem {
  id: string;
  title: string;
  description: string | null;
  dueDate: string | null;
  priority: TaskPriority;
  category: string | null;
  status: TaskItemStatus;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateTaskRequest {
  title: string;
  description: string | null;
  dueDate: string | null;
  priority: TaskPriority;
  category: string | null;
}

export interface UpdateTaskRequest extends CreateTaskRequest {
  status: TaskItemStatus;
}
