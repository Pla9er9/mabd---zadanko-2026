export interface Task {
  id: number;
  title: string;
  description: string;
  status: 'Open' | 'In Progress' | 'Done';
  category: string;
  dueDate: string;
}
