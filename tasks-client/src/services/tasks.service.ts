import { HttpClient, httpResource } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Task } from '../models/task';
import { configuration } from '../app/configuration';

@Injectable({
  providedIn: 'root'
})
export class TasksService {

  http = inject(HttpClient);
  baseUrl = configuration.baseUrl;

  getUserTasks(){
    return httpResource<Task[]>(this.baseUrl + '/tasks');
  }

  createNewTask() {
    return this.http.post<Task>(this.baseUrl + '/tasks', { title: '', description: '' })
  }

  saveTask(task: Task){
    return this.http.put(`${this.baseUrl}/tasks/${encodeURIComponent(task.id)}`, { title: task.title, description: task.description });
  }

  deleteTask(task: Task) {
    return this.http.delete(`${this.baseUrl}/tasks/${encodeURIComponent(task.id)}`);
  }

}
