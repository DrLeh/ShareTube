import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatService } from './chat.service';
import { ChatMessage } from './ChatMessage';


@Component({
  selector: 'chat-list',
  templateUrl: './chat.html'
})
export class ChatComponent {
  public messages: ChatMessage[];

  constructor(service: ChatService) {
      //subscribe to chat message
  }
}