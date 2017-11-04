import { Component } from '@angular/core';
import { Room } from '../rooms/Room';
import { RoomsService } from '../rooms/rooms.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    public room: Room;

    constructor(private service: RoomsService, private router: Router) {
        this.room = new Room();
    }

    public newRoom(): void {
        this.service.newRoom(this.room).subscribe(f => {
            const id = f.IdEncoded;
            this.router.navigate([`/rooms/room/${id}`]);
        });
    }
}
