import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { CommonModule } from '@angular/common';

import { RoomComponent } from './room.component';
import { RoomListComponent } from './roomlist.component';
import { RoomsService } from './rooms.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { YouTubeSearchService } from '../video/youtubeSearch.service';
import { VideoService } from '../video/video.service';
import { VideoModule } from '../video/video.module';

import { routes } from './routes';

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        NgbModule.forRoot(),
        VideoModule.forRoot()
    ],
    declarations: [
        RoomListComponent,
        RoomComponent,
    ],
    exports: [
        RoomListComponent,
        RoomComponent,
    ],
    providers: [RoomsService, VideoService, YouTubeSearchService]
})
export class RoomsModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: RoomsModule,
            providers: [
                RoomsService,
                VideoService,
                YouTubeSearchService
            ]
        };
    }
}
