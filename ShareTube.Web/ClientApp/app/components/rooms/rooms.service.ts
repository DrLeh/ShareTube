import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Room } from './Room';
import { RoomSearchResult } from './RoomSearchResult';

@Injectable()
export class RoomsService {
  constructor(private http: HttpClient,@Inject('BASE_URL') private baseUrl: string) { }

  public search(): Observable<RoomSearchResult[]> {
    return this.http.get<RoomSearchResult[]>(this.baseUrl + 'api/v1/rooms');
  }

  public newRoom(room: Room): Observable<Room> {
    return this.http.post<Room>(this.baseUrl + 'api/v1/rooms', room);
  }

  public get(id: string): Observable<Room> {
    return this.http.get<Room>(this.baseUrl + 'api/v1/rooms/' + id);
  }
}
