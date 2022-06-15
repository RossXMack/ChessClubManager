import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Match } from '../models/match';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  
  baseURL = '/api/Match/';

  constructor(private http: HttpClient) { }
  
  getMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.baseURL);
  }
  
  addMatch(match: Match): Observable<any> {    
    return this.http.post(this.baseURL, match);
  }  
}
