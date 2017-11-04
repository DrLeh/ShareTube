import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { YouTubeSearch } from './YouTubeSearch';

@Injectable()
export class YouTubeSearchService {
    constructor(private http: HttpClient, @Inject('YouTubeApiKey') private apiKey: string) { }

    public search(terms: YouTubeSearch): Observable<any> {
        let base = `https://www.googleapis.com/youtube/v3/search?key=${this.apiKey}`;
        let url = `${base}&part=snippet&q=${terms.Query}&maxResults=${terms.PageSize}&order=${terms.OrderBy}&type=${terms.Type}`;
        return this.http.get(url);
    }

    public related(id:string): Observable<any> {
        let base = `https://www.googleapis.com/youtube/v3/search?key=${this.apiKey}`;
        let url = `${base}&part=snippet&relatedToVideoId=${id}&type=video`;
        return this.http.get(url);
    }
}
