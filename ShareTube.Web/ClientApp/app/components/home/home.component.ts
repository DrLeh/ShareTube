import { Component } from '@angular/core';
import { Room } from '../rooms/Room';
import { RoomsService } from '../rooms/rooms.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
  public room: Room;
  private service: RoomsService;

  constructor(service: RoomsService) {
    this.service = service;
    this.room = new Room();
  }

  public newRoom(): void {
      this.service.newRoom(this.room).subscribe(f =>
    {
        const id = f.IdEncoded;
        console.log(id);
    });
  }

}
