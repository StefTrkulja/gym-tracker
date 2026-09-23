import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../env/environment';
import { UpdateProfileRequest } from './models/user.models';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}

  getProfile(): Observable<UpdateProfileRequest> {
    return this.http.get<UpdateProfileRequest>(`${environment.apiHost}users/me`);
  }

  updateProfile(request: UpdateProfileRequest): Observable<UpdateProfileRequest> {
    return this.http.put<UpdateProfileRequest>(`${environment.apiHost}users`, request);
  }
}