import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Theme }    from '../Models/theme';
import { ThemeService } from '../theme.service';

import * as moment from 'moment';

@Component({
  selector: 'app-theme-form',
  templateUrl: './theme-form.component.html',
  styleUrls: ['./theme-form.component.scss']
})
export class ThemeFormComponent implements OnInit{
  @Input() theme: Theme;
  @Input() isEdit: boolean;
  isValidFormSubmitted: boolean;
  model: Theme;
  minDate: string;
 
  constructor(private _themeService: ThemeService) {} 

  ngOnInit() {
    this.model = !this.isEdit ? new Theme() : this.theme ;
    this.minDate = moment(new Date()).add(1, 'days').format('YYYY-MM-DD');
    this.isValidFormSubmitted = false;
  }

  onSubmit(form: NgForm) { 
    if(form.invalid){
       return alert("Please fill form with correct information");
    }
    this.isValidFormSubmitted = true;
    this.isEdit ? this.edit() : this.send();
    // window.location.reload();
    // this.isEdit ? alert("Theme edited successfully.") : alert("Theme submitted successfully.")
  }

  send() {
    this._themeService.postTheme(this.model).subscribe(res => {
      console.log(res);
      window.location.reload();
    }, error => alert('Could not send theme ' + JSON.stringify(this.model)));
  }
  
  edit(){
    this._themeService.editTheme(this.model).subscribe(res => {
      console.log(res);
    }, error => alert('Could not edit theme ' + JSON.stringify(this.model)));
    window.location.reload();
  }
  
  get diagnostic() { return JSON.stringify(this.model) }
  get properties() { return JSON.stringify(this.isEdit)}
}
