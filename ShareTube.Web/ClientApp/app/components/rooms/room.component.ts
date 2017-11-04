import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, Input, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Room } from './Room';
import { RoomsService } from './rooms.service';


@Component({
  selector: 'room',
  templateUrl: './room.html'
})
export class RoomComponent {
  public room: Room;
  constructor(private service: RoomsService, private activatedRoute: ActivatedRoute) {
    this.room = new Room();
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      let id: string = params['id'];
      this.service.get(id).subscribe(room => {
        console.log(room);
        this.room = room;
      });
    });
  }
}