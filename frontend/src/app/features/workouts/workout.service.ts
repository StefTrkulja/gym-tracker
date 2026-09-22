import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Workout, WorkoutRequest } from './models/workout.models'
import { environment } from '../../../env/environment';

@Injectable({
  providedIn: 'root',
})
export class WorkoutService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<Workout[]> {
    return this.http.get<Workout[]>(environment.apiHost + "workouts");
  }

  create(request: WorkoutRequest): Observable<Workout> {
    return this.http.post<Workout>(environment.apiHost + "workouts", request);
  }

 update(id: number, request: WorkoutRequest): Observable<Workout> {
    return this.http.put<Workout>(`${environment.apiHost}workouts/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiHost}workouts/${id}`);
  }
}