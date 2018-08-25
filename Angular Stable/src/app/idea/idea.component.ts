import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Idea } from '../Models/idea';
import { Comment } from '../Models/comment';
import { Theme } from '../Models/theme';
import { Status } from '../Models/status'

import { ideaStatus } from '../Enums/ideaStatus';

import { CommentService } from '../comment.service';
import * as moment from 'moment';
import { UserPrincipal } from '../Models/UserPrincipal';



@Component({
  selector: 'idea',
  templateUrl: './idea.component.html',
  styleUrls: ['./idea.component.scss']
})

export class IdeaComponent implements OnInit {
  @Input() idea: Idea;
  @Input() theme: Theme;
  @Input() collapse: boolean;
  input: string;
  editIdea: string;
  pageSub: any;
  collapseReply: boolean;
  collapseEdit: boolean;
  statuses: Status[];
  newcomment: any;
  ideaStatus = ideaStatus;
  User: UserPrincipal;

  constructor(
    private _userPrincipal: UserPrincipal,
    private _commentService: CommentService
  ) {}

  ngOnInit() {
    if(this.theme.status === 'closed') { this.getStatus();}
    this.User = this._userPrincipal;
    this.collapseReply = false;
    this.collapseEdit = false;
    this.collapse = false;
    this.editIdea = this.idea.message;
  }

  getComments() {
    this._commentService.getCommentsByParentId(this.idea.postId).subscribe(
      subComments => {
        this.idea.comments = subComments;
      },
      error => console.log('Could not load comments for idea: ' + this.idea.postId)
    );
  }

  getStatus() {
    this._commentService.getStatus(this.idea.postId).subscribe(
      status => this.statuses = status,
      error => console.log('Could not get status for idea: ' + this.idea.postId)
    );
  }
  
  send(input:string) {
    this.newcomment = this.input;
    this.input = '';
    this.collapseReply = false;
    this._commentService.postComment(this.newcomment, this.idea.postId).subscribe(res => {
      if (this.idea.comments === null) { this.idea.comments = []; }
      this.idea.comments.push(res);
      this._commentService.subj_notification.next('Idea was submitted successfully');}
    , error => alert('Could not submit reply: ' + this.newcomment));
  }

  patchIdea(editIdea:string) {
    let message = editIdea;
    this.collapseEdit = false;
    this._commentService.patchIdea(message, this.idea.postId).subscribe(res => {
      this.idea = res;
    }, error => alert('Could not send edit ' + editIdea));
  }

  deleteIdea() {
    this._commentService.deleteIdea(this.idea.postId).subscribe(res => {
      delete this.idea;
      this._commentService.subj_notification.next('Idea was deleted successfully');
    }, error => { 
      console.log('Could not delete idea ' + this.idea.postId);
    });
    window.location.reload();
  }

  upvote () {
    this._commentService.changeIdeaVote(this.idea.postId, 1)
    .subscribe( res => {
      this.idea.score = res;
      this.idea.userVoteDirection = (this.idea.userVoteDirection === 1) ? 0 : 1;
    }, error => console.log('Error upvoting... ' + this.idea.postId));
  }
  downvote () {
    this._commentService.changeIdeaVote(this.idea.postId, -1)
    .subscribe( res => {
      this.idea.score = res;
      this.idea.userVoteDirection = (this.idea.userVoteDirection === -1) ? 0 : -1;
    }, error => console.log('Error downvoting... ' + this.idea.postId));
  }
  
  canEdit() {
    return (this.User.displayName === this.idea.owner);
  }
  canDelete (){
    return (this.User.isAppAdmin || this.User.displayName === this.idea.owner);
  }
}
