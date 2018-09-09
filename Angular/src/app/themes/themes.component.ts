import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Theme } from '../Models/theme';
import { ThemeService } from '../theme.service';

enum SORT_OPTION{
  NEWEST,
  OLDEST,
  MOST,
  CLOSING_SOONEST,
  CLOSING_LATEST
}
enum FILTER_OPTION {
  FILTER_OPEN_THEMES,
  FILTER_CLOSED_THEMES
}

@Component({
  selector: 'app-themes',
  templateUrl: './themes.component.html',
  styleUrls: ['./themes.component.scss']
})

export class ThemesComponent implements OnInit {
  themes: Theme[];
  pageSub: any;
  SORT_OPTION: typeof SORT_OPTION = SORT_OPTION;
  FILTER_OPTION: typeof FILTER_OPTION = FILTER_OPTION;
  filter_options: Array<FILTER_OPTION> = [];
  sorting: SORT_OPTION = SORT_OPTION.CLOSING_SOONEST;

  page;
  
  constructor(
    private _themeService: ThemeService,
    private router: Router
  ) {}
  
  ngOnInit() {
    this.toggleFilter(FILTER_OPTION.FILTER_CLOSED_THEMES);
    // this.themes = this.loadThemes(require('../sampledata.json'));
    this._themeService.getThemes()
		.subscribe(
      themes => {
        this.themes = this.loadThemes(themes);
      },
			error => {
        console.log(error);
        if (error.status === 403 || error.status === 0)
          this.router.navigate(['403']);
        if (error.status === 404)
          this.router.navigate(['**']);
      });
    }
    
  loadThemes(themeSource: Array<Theme>) {
    for (let theme of themeSource) {
      theme.status = (new Date(theme.closeDt) > new Date()) ? 'open' : 'closed';
    }
    //this.sortThemes(themeSource, this.sorting);
    return themeSource as Array<Theme>;
  }

  filterThemes (themeSource: Array<Theme>)
  {
    return themeSource.filter( theme =>{
      if (this.filter_options.indexOf(FILTER_OPTION.FILTER_CLOSED_THEMES) >= 0 && 
        this.filter_options.indexOf(FILTER_OPTION.FILTER_OPEN_THEMES) >= 0)
        return false;
      if (this.filter_options.indexOf(FILTER_OPTION.FILTER_CLOSED_THEMES) >= 0)
        return theme.status === 'open';
      else if (this.filter_options.indexOf(FILTER_OPTION.FILTER_OPEN_THEMES) >= 0)
        return theme.status === 'closed';
      else
        return true;
    });
  }
  toggleFilter (filter: FILTER_OPTION){
    if (this.filter_options.indexOf(filter) >= 0)
      delete this.filter_options[this.filter_options.indexOf(filter)];
    else
      this.filter_options.push(filter);
  }

  filterActive(filter: FILTER_OPTION) {
    return this.filter_options.indexOf(filter) >= 0;
  }

  sortThemes(themeSource: Array<Theme>, sorting: SORT_OPTION){
    this.sorting = sorting;
    if (sorting === SORT_OPTION.MOST) {}
    else if (sorting === SORT_OPTION.NEWEST) {
      return themeSource.sort((t1: Theme, t2: Theme) => {
        if (t1.openDt > t2.openDt){
          return -1;
        }
        else if (t1.openDt < t2.openDt)
        {
          return 1;
        } 
        else 
        {
          return 0;
        }
      });
    }
    else if (sorting === SORT_OPTION.OLDEST) {
      return themeSource.sort((t1: Theme, t2: Theme) => {
        if (t1.openDt > t2.openDt){
          return 1;
        }
        else if (t1.openDt < t2.openDt)
        {
          return -1;
        } 
        else 
        {
          return 0;
        }
      });
    }
    else if (sorting === SORT_OPTION.CLOSING_SOONEST) {
      return themeSource.sort((t1: Theme, t2: Theme) => {
        if (t1.closeDt > t2.closeDt){
          return 1;
        }
        else if (t1.closeDt < t2.closeDt)
        {
          return -1;
        } 
        else 
        {
          return 0;
        }
      });
    }
    else if (sorting === SORT_OPTION.CLOSING_LATEST) {
      return themeSource.sort((t1: Theme, t2: Theme) => {
        if (t1.closeDt > t2.closeDt){
          return -1;
        }
        else if (t1.closeDt < t2.closeDt)
        {
          return 1;
        } 
        else 
        {
          return 0;
        }
      });
    }
  }
}
 