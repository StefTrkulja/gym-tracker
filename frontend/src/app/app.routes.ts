import { Routes } from '@angular/router';
import { AuthLayout } from './layouts/auth-layout/auth-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { WorkoutList } from './features/workouts/workout-list/workout-list';
import { ProgressPage } from './features/progress/progress-page/progress-page';
import { authGuard } from './core/auth/auth-guard';

export const routes: Routes = [
  {
    path: '',
    component: AuthLayout,
    children: [
      { path: 'login', component: Login },
      { path: 'register', component: Register },
    ]
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    children: [
      { path: '', component: Dashboard },
      { path: 'workouts', component: WorkoutList },
      { path: 'progress', component: ProgressPage },
    ]
  },
  { path: '**', redirectTo: '' }
];