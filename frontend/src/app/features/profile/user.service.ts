import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../env/environment';
import { UpdateProfileRequest,UserProfile } from './models/user.models';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient)

  getProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${environment.apiHost}users/me`);
  }

  updateProfile(request: UpdateProfileRequest): Observable<UpdateProfileRequest> {
    return this.http.put<UpdateProfileRequest>(`${environment.apiHost}users`, request);
  }
}