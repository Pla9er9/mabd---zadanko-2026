import { Injectable } from '@angular/core';
import { Task } from './task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private nextId = 4;
  private tasks: Task[] = [
    {
      id: 1,
      title: 'Przygotuj landing page',
      description: 'Stwórz prosty interfejs powitalny dla systemu zarządzania zadaniami.',
      status: 'In Progress',
      category: 'Projekt',
      dueDate: new Date().toISOString().split('T')[0]
    },
    {
      id: 2,
      title: 'Dodaj filtry zadań',
      description: 'Zaimplementuj filtrowanie po statusie, kategorii i terminie.',
      status: 'Open',
      category: 'Rozwój',
      dueDate: new Date(Date.now() + 3 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
    },
    {
      id: 3,
      title: 'Przygotuj widok szczegółów zadania',
      description: 'Dodaj stronę, na której użytkownik zobaczy wszystkie informacje o zadaniu.',
      status: 'Done',
      category: 'Testy',
      dueDate: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
    }
  ];

  getStatuses(): string[] {
    return ['Open', 'In Progress', 'Done'];
  }

  getCategories(): string[] {
    const categories = new Set<string>(this.tasks.map((task) => task.category));
    return Array.from(categories);
  }

  getAllTasks(filters: { status?: string; category?: string; dueDate?: string } = {}): Task[] {
    return this.tasks
      .filter((task) => {
        return (
          (!filters.status || task.status === filters.status) &&
          (!filters.category || task.category === filters.category) &&
          (!filters.dueDate || task.dueDate === filters.dueDate)
        );
      })
      .sort((a, b) => a.dueDate.localeCompare(b.dueDate));
  }

  getTask(id: number): Task | undefined {
    return this.tasks.find((task) => task.id === id);
  }

  saveTask(task: Task): void {
    const existing = this.getTask(task.id);
    if (existing) {
      existing.title = task.title;
      existing.description = task.description;
      existing.status = task.status;
      existing.category = task.category;
      existing.dueDate = task.dueDate;
      return;
    }

    this.tasks.push({ ...task, id: this.nextId++ });
  }
}
