import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Video } from './Video';

@Injectable()
export class VideoService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    public getQueue(roomId: string): Observable<Video[]> {
        return this.http.get<Video[]>(`${this.baseUrl}api/v1/videos/${roomId}`);
    }
    public enqueue(roomId: string, video: Video): Observable<Video> {
        return this.http.post<Video>(`${this.baseUrl}api/v1/videos/${roomId}`, video);
    }
    public current(roomId: string): Observable<Video> {
      return this.http.get<Video>(`${this.baseUrl}api/v1/videos/${roomId}/current`);
    }
    public set(roomId:string, video: Video): Observable<Video> {
      return this.http.post<Video>(`${this.baseUrl}api/v1/videos/${roomId}/current`, video);
    }
}
