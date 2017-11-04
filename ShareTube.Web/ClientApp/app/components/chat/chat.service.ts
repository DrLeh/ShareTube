import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatMessage } from './ChatMessage';

@Injectable()
export class ChatService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    public send(): void {
        ;
    }
}
