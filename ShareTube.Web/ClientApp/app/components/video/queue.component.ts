import { Component, Input } from '@angular/core';
import { Room } from '../rooms/Room';
import { RoomComponent } from '../rooms/room.component';
import { Video } from './Video';
import { VideoService } from './video.service';


@Component({
    selector: 'queue',
    templateUrl: './queue.html'
})
export class QueueComponent {
    @Input() room: Room;
    @Input() roomCmp: RoomComponent;

    public videos: Video[] = [];

    ngOnInit() {
        this.load();
    }

    constructor(private service: VideoService) {
    }

    private load() {
        this.service.getQueue(this.room.ID).subscribe(result => {
            this.videos = result;
        });
    }
}