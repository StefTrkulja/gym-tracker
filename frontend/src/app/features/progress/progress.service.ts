import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../env/environment';
import { MonthlyProgress } from './models/progress.models';

@Injectable({
  providedIn: 'root',
})
export class ProgressService {
  constructor(private http: HttpClient) {}

  getMonthlyProgress(year: number, month: number): Observable<MonthlyProgress> {
    return this.http.get<MonthlyProgress>(`${environment.apiHost}progress?year=${year}&month=${month}`);
  }
}