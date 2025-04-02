import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TasksService } from '../../services/tasks.service';
import { TaskComponent } from './task/task.component';
import { Task } from '../../models/task';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-tasks',
  imports: [TaskComponent, MatProgressSpinnerModule, MatButtonModule],
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TasksComponent {
  tasksService = inject(TasksService);

  tasks = this.tasksService.getUserTasks();

  onSave(task: Task) {
    this.tasksService.saveTask(task).subscribe({
      next: () => {
        console.log('Task saved', task);
      },
      error: () => {
        alert('Error saving task');
      }
    });
  }

  onDelete(task: Task) {
    this.tasksService.deleteTask(task).subscribe({
      next: () => {
        this.tasks.update(value => value ? value.filter(t => t.id !== task.id) : value);
      },
      error: () => {
        alert('Error deleting task');
      }
    })
  }

  addNewTask() {
    this.tasksService.createNewTask().subscribe({
      next: task => {
        this.tasks.update(value => value ? [...value, task] : value);
      },
      error: () => {
        alert('Error creating task');
      }
    })
  }
  
}
