import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { WorkoutList } from './features/workouts/workout-list/workout-list';
import { ProgressPage } from './features/progress/progress-page/progress-page';
import { authGuard } from './core/auth/auth-guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
  { path: 'workouts', component: WorkoutList, canActivate: [authGuard] },
  { path: 'progress', component: ProgressPage, canActivate: [authGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];