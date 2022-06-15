import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchMembersComponent } from './components/fetch-members/fetch-members.component';
import { AddMemberComponent } from './components/add-member/add-member.component';
import { FetchMatchesComponent } from './components/fetch-matches/fetch-matches.component';
import { AddMatchComponent } from './components/add-match/add-match.component';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchMembersComponent,
    FetchMatchesComponent,
    AddMemberComponent,
    AddMatchComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'fetch-members', component: FetchMembersComponent },
      { path: 'fetch-matches', component: FetchMatchesComponent },
      { path: 'register-member', component: AddMemberComponent },
      { path: 'register-match', component: AddMatchComponent },
      { path: 'member/edit/:id', component: AddMemberComponent }
    ])
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
