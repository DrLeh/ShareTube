import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { PlayerComponent } from './player.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgbModule.forRoot(),
  ],
  declarations: [
    PlayerComponent,
  ],
  exports: [
    PlayerComponent,
  ],
  providers: [
  ]
})
export class PlayerModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: PlayerModule,
      providers: [
        //YouTubeSearchService
      ]
    };
  }
}
