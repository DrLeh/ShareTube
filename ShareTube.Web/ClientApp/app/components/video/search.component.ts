import { Component, Input } from '@angular/core';
import { RoomComponent } from '../rooms/room.component';
import { Room } from '../rooms/Room';
import { Video } from './Video';
import { VideoService } from './video.service';
import { YouTubeSearch } from './YouTubeSearch';
import { YouTubeSearchService } from './youtubeSearch.service';


@Component({
    selector: 'search',
    templateUrl: './search.html'
})
export class SearchComponent {
  @Input() room: Room;
  @Input() roomCmp: RoomComponent;

  public model: YouTubeSearch;

    public videos: Video[] = [];

    constructor(private service: VideoService, private searchService: YouTubeSearchService) {
        this.model = new YouTubeSearch();
        this.model.Query = "";
        this.model.OrderBy = "relevance";
        this.model.PageSize = 10;
        this.model.Type = "video"
    }

    private search() {
        this.searchService.search(this.model).subscribe(result => {
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
            console.log(this.videos);
        });
    }
}