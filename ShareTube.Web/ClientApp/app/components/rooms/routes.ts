import { Route } from '@angular/router';
import { RoomComponent } from './room.component';


export const routes: Route[] = [{
  path: 'rooms/room/:id',
  component: RoomComponent,
}];