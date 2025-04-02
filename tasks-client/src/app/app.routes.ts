import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: 'login', loadComponent: () => import('./login/login.component').then(c => c.LoginComponent)},
    { path: 'register', loadComponent: () => import('./register/register.component').then(c => c.RegisterComponent)},
    { path: 'tasks', canActivate: [authGuard], loadComponent: () => import('./tasks/tasks.component').then(c => c.TasksComponent)},
    { path: '', redirectTo: 'register', pathMatch: 'full' },
];
