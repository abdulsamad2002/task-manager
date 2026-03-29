import { Routes } from '@angular/router';
import { authGuard, guestGuard, managerGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  {
    path: 'auth',
    canActivate: [guestGuard],
    loadComponent: () =>
      import('./pages/auth/auth.component').then((m) => m.AuthComponent),
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/layout/layout.component').then((m) => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./pages/dashboard/dashboard.component').then(
            (m) => m.DashboardComponent
          ),
      },
      {
        path: 'my-tasks',
        loadComponent: () =>
          import('./pages/my-tasks/my-tasks.component').then(
            (m) => m.MyTasksComponent
          ),
      },
      {
        path: 'all-tasks',
        canActivate: [managerGuard],
        loadComponent: () =>
          import('./pages/all-tasks/all-tasks.component').then(
            (m) => m.AllTasksComponent
          ),
      },
      {
        path: 'create-task',
        canActivate: [managerGuard],
        loadComponent: () =>
          import('./pages/create-task/create-task.component').then(
            (m) => m.CreateTaskComponent
          ),
      },
    ],
  },
  { path: '**', redirectTo: 'auth' },
];
