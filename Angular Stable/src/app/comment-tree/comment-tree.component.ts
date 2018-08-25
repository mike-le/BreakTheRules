import { Component, Input, OnInit } from '@angular/core';

import { Comment } from '../Models/comment';
import { CommentService } from '../comment.service';
import * as moment from 'moment';
import { UserPrincipal } from '../Models/UserPrincipal';

@Component({
  selector: 'comment-tree',
  templateUrl: './comment-tree.component.html',
  styleUrls: ['./comment-tree.component.scss']
})
export class CommentTreeComponent implements OnInit {
  @Input() comment: Comment;
  @Input() collapse: boolean;
  input: string;
  editComment: string;
  collapseReply: boolean;
  collapseEdit: boolean;
  subComments: Comment[];
  newcomment: any;
  User: UserPrincipal;

  constructor(
    private _userPrincipal: UserPrincipal,
    private _commentService: CommentService) {}

  ngOnInit() {
    this.User = this._userPrincipal;
    this.collapseReply = false; //collapsing replies to comments/ideas
    this.collapseEdit = false;
    this.editComment = this.comment.message;
  }

  loadSubComments (targetComment: Comment){
    if (targetComment.comments != null && targetComment.comments.length > 0){
      targetComment.hasBeenLoaded = true;
    }
    if (targetComment.hasBeenLoaded){
      return;
    }
      this._commentService.getCommentsByParentId(targetComment.commentId).subscribe(
      subComments => {
        targetComment.comments = subComments;
      },
      error => console.log('Could not load item ' + this.comment.commentId));
  }

  send(input:string) {
    this.newcomment = this.input;
    this.input = '';
    this.collapseReply = false;
    this._commentService.postSubcomment(this.newcomment, this.comment.commentId).subscribe(res => {
      if (this.comment.comments === null) { this.comment.comments = []; }
      this.comment.comments.push(res);
      this._commentService.subj_notification.next('Comment was submitted successfully');
    }, error => alert('Could not send comment: ' + this.newcomment));
  }

  patchComment(editComment: string) {
    var message = editComment;
    this.collapseEdit = false;
    this._commentService.patchComment(message, this.comment.commentId).subscribe(res => {
      this.comment = res;
    }, error => alert('Could not edit comment: ' + editComment));
  }

  deleteComment() {
    this._commentService.deleteComment(this.comment.commentId).subscribe(res => {
      delete this.comment;
      this._commentService.subj_notification.next('Comment was deleted successfully');
    }, error => alert('Could not delete comment ' + this.comment.commentId));
      window.location.reload();
      
  }

  upvote () {
    this._commentService.changeCommentVote(this.comment.commentId, 1)
    .subscribe( res => {
      this.comment.score = res;
      this.comment.userVoteDirection = (this.comment.userVoteDirection === 1) ? 0 : 1;
    }, error => console.log('Error upvoting... ' + this.comment.commentId));
    
  }
  
  downvote () {
    this._commentService.changeCommentVote(this.comment.commentId, -1)
    .subscribe( res => {
      this.comment.score = res;
      this.comment.userVoteDirection = (this.comment.userVoteDirection === -1) ? 0 : -1;
    }, error => console.log('Error downvoting... ' + this.comment.commentId));
  }
  canEdit() {
    return (this.User.displayName === this.comment.owner);
  }
  canDelete () {
    return (this.User.isAppAdmin || this.User.displayName === this.comment.owner);
  }
}
