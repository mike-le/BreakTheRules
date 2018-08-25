import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Status } from './Models/status';

import 'rxjs/add/operator/map';
import { environment } from '../environments/environment';
import { Subject } from '../../node_modules/rxjs';

@Injectable({providedIn: 'root'})

export class CommentService  {
    private baseUrl: string;
    public subj_notification: Subject<string> = new Subject();

    constructor(private http: HttpClient) {
      this.baseUrl = environment.apiEndPoint;
    }
    getIdeaVoteAudits(idList: number[]) : Observable<any> {
		return this.http.post(this.baseUrl + 'ideas/vote/audit/', idList);
	}
    getCommentVoteAudits(idList: number[]) : Observable<any> {
		return this.http.post(this.baseUrl + 'comments/vote/audit/', idList);
	}
    getCommentsByParentId(parentId: number): Observable<any> {
        return this.http.get(this.baseUrl + 'comments/' + parentId);
    }

    getStatus(ideaId: number): Observable<any> { 
        return this.http.get(this.baseUrl + 'status/' + ideaId);
    } 

    postSubcomment(content: string, parentId: number): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
        });	
        
        content = content.replace(/\n/g,'<br/>');
        
        var comment =
        {
            "message": content,
            "parentIdeaId": null,
            "parentCommentId": parentId
        };		
		
		return this.http.post(this.baseUrl + 'comments/', comment, {headers});
    }
    
    postComment(content: string, parentId: number): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
		});	
        
        content = content.replace(/\n/g,'<br/>');
        
        var comment =
        {
            "message": content,
            "parentIdeaId": parentId,
            "parentCommentId": null
        };		
		
		return this.http.post(this.baseUrl + 'comments/', comment, {headers});
    }

    postStatus(status: Status, ideaId: number): Observable<any> {
        const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
        });	

        var message = status.response.replace(/\n/g,'<br/>');

        var response =
        {
            "statusCode": status.statusCode,
            "response": message,
            "ideaId": ideaId
        };	

        return this.http.post(this.baseUrl + 'status/' + ideaId, response, {headers});
    }

    deleteIdea(postId: number): Observable<any> {
        //add AntiForgeryToken
        const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'DELETE'
        });	
        return this.http.delete(this.baseUrl + 'ideas/' + postId, {headers});
    }

    deleteComment(postId: number): Observable<any> {
        //add AntiForgeryToken
        const headers = new HttpHeaders({
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'DELETE'
        });	
        return this.http.delete(this.baseUrl + 'comments/' + postId, {headers});
    }

    patchIdea(message: string, ideaId: number): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'PATCH'
		});

		var ideaPatch = [
            {
                "op": "replace",
                "path": "/message",
                "value": message
            }
        ];

		return this.http.patch(this.baseUrl + 'ideas/edit/' + ideaId, ideaPatch, {headers});
    }

    patchComment(message: string, commentId: number): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'PATCH'
		});

		var commentPatch = [
            {
                "op": "replace",
                "path": "/message",
                "value": message
            }
        ];

		return this.http.patch(this.baseUrl + 'comments/edit/' + commentId, commentPatch, {headers});
    }

    changeIdeaVote(ideaId: number, direction: number): Observable<any> {
		const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
		});
		
		return this.http.post(this.baseUrl + 'ideas/vote/' + ideaId + '?direction=' + direction, {headers});
    }

    changeCommentVote(commentId: number, direction: number): Observable<any> {
        const headers = new HttpHeaders({
			'Content-Type': 'application/json',
			'Access-Control-Allow-Origin': '*',
			'Request-Method': 'POST'
		});

        console.log(this.baseUrl + 'comments/vote/' + commentId + '?direction=' + direction);
        var string = this.baseUrl + 'comments/vote/' + commentId + '?direction=' + direction;
		return this.http.post(string, {headers});
    }
}


