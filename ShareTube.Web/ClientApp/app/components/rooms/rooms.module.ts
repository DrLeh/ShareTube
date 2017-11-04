import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { CommonModule } from '@angular/common';

import { RoomComponent } from './room.component';
import { RoomListComponent } from './roomlist.component';
import { RoomsService } from './rooms.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { routes } from './routes';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgbModule.forRoot(),
  ],
  declarations: [
    RoomListComponent,
    RoomComponent
  ],
  exports: [
    RoomListComponent,
    RoomComponent
  ],
  providers: [RoomsService]
})
export class RoomsModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RoomsModule,
      providers: [
        RoomsService
      ]
    };
  }
}
