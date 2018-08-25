import { Component, OnInit } from '@angular/core';
import { UserPrincipal } from '../Models/UserPrincipal';
import { UserService } from '../user.service';
import { Notification } from '../Models/notification';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  User: UserPrincipal;
  notificationList: Notification[];

  constructor(
    private _userPrincipal : UserPrincipal,
    public _userService: UserService
  ) {}

  ngOnInit()
  {
    this.User = this._userPrincipal;
    this.getNotifications();
  }
  
  getNotifications() {
    this._userService.getNotifications().subscribe(
      notifications => this.notificationList = notifications,
      error => console.log('Could not get notifications')
    );
  }

  get diagnostic() { return JSON.stringify(this._userPrincipal) }

}