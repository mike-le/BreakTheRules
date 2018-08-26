import { Component, OnInit } from '@angular/core';
import { UserPrincipal } from './Models/UserPrincipal';
import { UserService } from './user.service';
import { MatSnackBar, MatSnackBarConfig } from '../../node_modules/@angular/material';
import { CommentService } from './comment.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    private _userService: UserService,
    private _userPrincipal: UserPrincipal,
    private _commentService: CommentService,
    private snackBar: MatSnackBar
  ) { 
    // this._commentService.subj_notification.subscribe(message => {
    //   snackBar.open(message, undefined, 
    //     {
    //       duration: 2500, 
    //       panelClass: ['success-snackbar']
    //     }
    //   );
    // });
} 

  ngOnInit(){
    //this.getUserPrincipal();
  }

  getUserPrincipal() {
    this._userService.getUserPrincipal().subscribe(user => {
        this._userPrincipal.isAppAdmin = user.isAppAdmin;
        this._userPrincipal.displayName = user.displayName;
        this._userPrincipal.isBlacklisted = user.isBlackListed;
    },
    error => alert('Could not get User information')
    );
  }
}
