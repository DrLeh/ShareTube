import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { QueueComponent } from './queue.component';
import { SearchComponent } from './search.component';
import { RelatedComponent } from './related.component';
import { VideoListComponent } from './videolist.component';

import { VideoService } from './video.service';
import { YouTubeSearchService } from './youtubeSearch.service';

//import { routes } from './routes';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        //RouterModule.forChild(routes),
        NgbModule.forRoot(),
    ],
    declarations: [
        QueueComponent,
        SearchComponent,
RelatedComponent,
        VideoListComponent,
    ],
    exports: [
        QueueComponent,
        SearchComponent,
RelatedComponent,
        VideoListComponent,
    ],
    providers: [
        VideoService,
        YouTubeSearchService
    ]
})
export class VideoModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: VideoModule,
            providers: [
                VideoService,
                YouTubeSearchService
            ]
        };
    }
}
