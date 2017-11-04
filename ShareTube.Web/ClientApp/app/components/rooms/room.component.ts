import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from './Room';
import { RoomsService } from './rooms.service';

import { Video } from '../video/video';
import { VideoService } from '../video/video.service';
import { QueueComponent } from '../video/queue.component';


@Component({
  selector: 'room',
  templateUrl: './room.html',
})
export class RoomComponent {
  public room: Room;
  public currentVideo: Video;

  constructor(private service: RoomsService, private videoService: VideoService, private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      let id: string = params['id'];
      this.service.get(id).subscribe(room => {
        this.room = room;
        this.loadCurrentVideo();
      });
    });
  }

  public loadCurrentVideo(): void {
    this.videoService.current(this.room.IdEncoded).subscribe(result => {
      this.currentVideo = result;
    });
  }

  public setCurrentVideo(video: Video): void {
    this.videoService.set(this.room.IdEncoded, video).subscribe(result => {
      this.currentVideo = video;
    });
  }

  public enqueue(video: Video): void {
    this.videoService.enqueue(this.room.IdEncoded, video).subscribe(result => {

    });
  }
}