import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

import { Theme } from '../Models/theme';

import { ThemeService } from '../theme.service';
import { UserPrincipal } from '../Models/UserPrincipal';

@Component({
  selector: 'theme-detail',
  templateUrl: './theme-detail.component.html',
  styleUrls: ['./theme-detail.component.scss']
})
export class ThemeDetailComponent implements OnInit {
  theme: Theme;
  pageSub: any;
  themeId: number;
  input: string;
  User: UserPrincipal
  comment;

  constructor(
    private _userPrincipal: UserPrincipal,
    private _themeService: ThemeService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.User = this._userPrincipal;
    this.pageSub = this.route.params.subscribe(params => {
      this.themeId = +params['id'] ? +params['id'] : 1;	  
      
      this._themeService.getThemeById(this.themeId)
      .subscribe(
        res => {
          this.theme = res.body;
          this.theme.status = (new Date(this.theme.closeDt) > new Date()) ? 'open' : 'closed';
        },
      	error => {
          if (error.status === 403)
            this.router.navigate(['403']);
        }
      );
      });
    }

    closeThemeNow() {
      var x = new Date();
      x.setHours(x.getHours() - x.getTimezoneOffset() / 60);
      x.setMinutes(x.getMinutes() - 5);
      this.theme.closeDt = x;
      this._themeService.editTheme(this.theme).subscribe(
        res =>
        {
          this.theme = res;
        },
        error => console.log('Error closing theme... ' + error)
      )
    }

    send(input:string) {
      this._themeService.postIdea(this.input, this.themeId).subscribe(idea => {
        this.theme.ideas.push(idea);
        this.input = '';
      }, error => 
        {
          console.log('Could not send idea ' + this.input);
        }
      );
    }

    sendReminder(){
      this._themeService.sendReminder(this.themeId).subscribe(res => {
        console.log(res);
      }, error => console.log('Could not send reminder for theme: ' + this.themeId));
    }
  }
