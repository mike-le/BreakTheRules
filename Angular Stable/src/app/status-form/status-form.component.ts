import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { Idea } from '../Models/idea';
import { Status } from '../Models/status'
import { ideaStatus } from '../Enums/ideaStatus';

import { CommentService } from '../comment.service';

@Component({
  selector: 'app-status-form',
  templateUrl: './status-form.component.html',
  styleUrls: ['./status-form.component.scss']
})

export class StatusFormComponent implements OnInit {
  @Input() idea: Idea;
  ideaStatus = ideaStatus;
  collapseForm: boolean;
  isValidFormSubmitted: boolean;

  model: Status;
  statusOptions = ['Submitted', 'Under Review', 'Accepted', 'Implemented', 'Declined'];

  constructor(private _commentService: CommentService) {
    this.collapseForm = true;
    this.isValidFormSubmitted = false;
    this.model = new Status();
   }

  ngOnInit() {
  }

  //Update status dropdown box
  onSubmit(form: NgForm) { 
    if(form.invalid){
      alert("Please fill form with correct form values");
      return;
    }
    this.isValidFormSubmitted = true;
    this.postStatus();
    window.location.reload();
  }

  postStatus() {
    this._commentService.postStatus(this.model, this.idea.postId).subscribe(res => {
      console.log(res);
    }, error => console.log('Could not send item ' + JSON.stringify(this.model)));
  }

  get Diagnostic() { return JSON.stringify(this.model)};
}
