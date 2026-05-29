export type TaskStatus = 'Open' | 'Done';

export interface Task {
  id: number;
  title: string;
  description: string;
  status: TaskStatus;
  category: string;
  dueDate: string;
  isDone: boolean;
}

export interface TaskPayload {
  title: string;
  description: string;
  status: TaskStatus;
  category: string;
  dueDate: string;
}
