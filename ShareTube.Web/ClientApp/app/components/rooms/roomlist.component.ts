import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { RoomsService } from './rooms.service';
import { RoomSearchResult } from './RoomSearchResult';


@Component({
    selector: 'room-list',
    templateUrl: './roomlist.html'
})
export class RoomListComponent {
    public rooms: RoomSearchResult[];

    constructor(private service: RoomsService) {
        this.search();
    }

    private search() {
        this.service.search().subscribe(result => {
            this.rooms = result;
        });
    }
}