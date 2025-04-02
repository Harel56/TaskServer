import { ChangeDetectionStrategy, Component, Input, input, output } from '@angular/core';
import { Task } from '../../../models/task';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-task',
  imports: [FormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, MatCardModule],
  templateUrl: './task.component.html',
  styleUrl: './task.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskComponent {
  @Input({ required: true }) task!: Task;
  save = output<Task>();
  delete = output<Task>();

  onSave(){
    this.save.emit(this.task);
  }

  onDelete(){
    this.delete.emit(this.task);
  }
}
