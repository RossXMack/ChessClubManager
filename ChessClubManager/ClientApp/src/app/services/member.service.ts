import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  
  baseURL = '/api/Member/';

  constructor(private http: HttpClient) { }
  
  getMembers(): Observable<Member[]> {
    return this.http.get<Member[]>(this.baseURL);
  }

  getMemberById(id: string): Observable<Member> {
    return this.http.get<Member>(this.baseURL + id);
  }

  addMember(member: Member): Observable<any> {    
    return this.http.post(this.baseURL, member);
  }

  updateMember(member: Member): Observable<any> {
    return this.http.put(this.baseURL, member);
  }

  deleteMember(id: string): Observable<any> {
    return this.http.delete(this.baseURL + id);
  }
}
