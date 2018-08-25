import { Routes, RouterModule } from '@angular/router';

import { ThemesComponent } from './themes/themes.component';
import { ThemeDetailComponent } from './theme-detail/theme-detail.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { NotfoundComponent } from './notfound/notfound.component';

const routes: Routes = [
  {path: '', component: ThemesComponent},
  {path: 'themes/:id', component: ThemeDetailComponent},
  {path: '403', component: ForbiddenComponent},
  {path: '**', component: NotfoundComponent}
];

export const routing = RouterModule.forRoot(routes);