import { Component, OnInit } from '@angular/core';
import { Match, MatchResult } from '../../models/match';
import { MatchService } from '../../services/match.service';

@Component({
  selector: 'app-fetch-match',
  templateUrl: './fetch-matches.component.html',
  styleUrls: ['./fetch-matches.component.css']
})

export class FetchMatchesComponent implements OnInit {
  public matches: Match[] = [];
  eMatchResultType = MatchResult;

  constructor(private matchService: MatchService) { }

  ngOnInit(): void {
    this.getMatches();    
  }

  getMatches(): void {    
    this.matchService
      .getMatches()
      .subscribe((result) => this.matches = result);
  }  
}
