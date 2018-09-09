import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Theme } from './Models/theme';

import { environment } from '../environments/environment';
import { map } from 'rxjs-compat/operator/map';

@Injectable({providedIn: 'root'})

export class ThemeService {
  private baseUrl: string;

  constructor(private http: HttpClient) {
	this.baseUrl = environment.apiEndPoint;
  }
  
	getIdeaVoteAudits(idList: number[]) : Observable<any> {
		return this.http.post(this.baseUrl + 'ideas/vote/audit/', idList);
	}
  	getThemeById(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'themes/' + id, {observe: 'response'});
	  }
  	getThemes(): Observable<any> {
		return this.http.get(this.baseUrl + 'themes/');
	} 
	
	postIdea(content: string, themeId: number): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
		});
		
		var idea = {
			"message": content,
			"themeId": themeId,
		};
		
		return this.http.post(this.baseUrl + 'ideas/', idea, {headers});
	}

	postTheme(theme: Theme): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
		});	
		
		theme.description = theme.description.replace(/\n/g, '<br/>');

		return this.http.post(this.baseUrl + 'themes/', theme, {headers});
	}

	sendReminder(id: number): Observable<any>{
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
		});	

		return this.http.post(this.baseUrl + 'themes/send/' + id, id, {headers});
	}

	editTheme(theme: Theme): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'PATCH'
		});
		
		var themePatch = [
            {
                "op": "replace",
                "path": "/title",
                "value": theme.title
			},
			{
                "op": "replace",
                "path": "/description",
                "value": theme.description
			},
			{
                "op": "replace",
                "path": "/closeDt",
                "value": theme.closeDt
            }
        ];

		return this.http.patch(this.baseUrl + 'themes/edit/' + theme.themeId, themePatch, {headers});
	}
}


