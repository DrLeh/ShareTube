import { Component, Input } from '@angular/core';
import { Video } from './Video';
import { RoomComponent } from '../rooms/room.component';


@Component({
    selector: 'video-list',
    templateUrl: './video-list.html'
})
export class VideoListComponent {
  @Input() videos: any = [];
  @Input() roomCmp: RoomComponent;

    constructor() {
    }
}