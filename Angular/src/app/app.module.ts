import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule }    from '@angular/common/http';
import { MatBadgeModule, MatSnackBarModule } from '@angular/material';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MomentModule } from 'angular2-moment/moment.module';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';

import { ThemesComponent } from './themes/themes.component';
import { ThemeDetailComponent } from './theme-detail/theme-detail.component';
import { IdeaComponent } from './idea/idea.component';
import { CommentTreeComponent } from './comment-tree/comment-tree.component';
import { ThemeFormComponent } from './theme-form/theme-form.component';
import { StatusFormComponent } from './status-form/status-form.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { NotfoundComponent } from './notfound/notfound.component';

import { TimeAgoPipe } from './Pipes/time-ago.pipe';
import { EnumToStringPipe } from './Pipes/enum-to-string.pipe';

import { routing } from './app.routes';

import { CustomMinDirective } from './Validation/custom-min-validator.directive';

import { UserPrincipal } from './Models/UserPrincipal';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    ThemesComponent,
    ThemeDetailComponent,
    IdeaComponent,
    CommentTreeComponent,
    FooterComponent,
    TimeAgoPipe,
    EnumToStringPipe,
    ThemeFormComponent,
    CustomMinDirective,
    StatusFormComponent,
    ForbiddenComponent,
    NotfoundComponent,
  ],
  imports: [
    BrowserModule,
    MatBadgeModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatSelectModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    MomentModule,
    routing
  ],
  providers: [UserPrincipal],
  bootstrap: [AppComponent]
})
export class AppModule { }
