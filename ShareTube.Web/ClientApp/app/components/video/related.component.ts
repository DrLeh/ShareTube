import { Component, Input } from '@angular/core';
import { RoomComponent } from '../rooms/room.component';
import { Room } from '../rooms/Room';
import { Video } from './Video';
import { VideoService } from './video.service';
import { YouTubeSearch } from './YouTubeSearch';
import { YouTubeSearchService } from './youtubeSearch.service';


@Component({
    selector: 'related',
    templateUrl: './related.html'
})
export class RelatedComponent {
    @Input() currentVideo: Video;
    @Input() roomCmp: RoomComponent;
    public videos: Video[];

    constructor(private service: VideoService, private searchService: YouTubeSearchService) {

    }

    ngOnInit() {
        this.loadRelatedVids();
    }

    private loadRelatedVids() {
        if (!this.currentVideo) {
            return;
        }
        this.searchService.related(this.currentVideo.ID).subscribe(result => {
            let temp: Video[] = [];
            for (let vid of result.items) {
                let v = new Video();
                v.ID = vid.id.videoId;
                v.Title = vid.snippet.title;
                v.Author = vid.snippet.channelTitle;
                v.ThumbnailUrl = vid.snippet.thumbnails.default.url;
                temp.push(v);
            }
            this.videos = temp;
        });
    }
}