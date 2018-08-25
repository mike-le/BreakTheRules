import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../environments/environment';
import { Notification } from './Models/notification';

@Injectable({providedIn: 'root'})

export class UserService {
    private baseUrl: string;
  
    constructor(private http: HttpClient) {
      this.baseUrl = environment.apiEndPoint;
    }

    getUserPrincipal() : Observable<any> {
        return this.http.get(this.baseUrl + 'user/');
    }
    
    getNotifications() : Observable<any> {
        return this.http.get(this.baseUrl + 'notifications/');
    }
}