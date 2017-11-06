import { Component, Input } from '@angular/core';
//import { Room } from '../rooms/Room';
//import { RoomComponent } from '../rooms/room.component';
import { Video } from '../video/Video';
//import * as YT from "../../../typings/global/youtube"
//import * as YT from 'youtube/YT'


@Component({
  selector: 'player',
  templateUrl: './player.html'
})
export class PlayerComponent {
  //@Input() room: Room;
  //@Input() roomCmp: RoomComponent;
  @Input() video: Video;

  player: YT.Player;

  playVideoWaitTime = 3000;

  ngOnInit() {
    this.init();
  }

  constructor() {
  }

  private init() {
    if (!this.video)
      return;

    var events: YT.Events = {
      onReady: this.onPlayerReady,
      onStateChange: this.onPlayerStateChange
    };
    let vars: YT.PlayerVars = {
      playlist: '', // perhaps use this for better autoplaying?
    };
    let playerOptions: YT.PlayerOptions = {
      //height: 100,
      //width: 100,
      videoId: this.video.ID,
      events: events
    };
    this.player = new YT.Player('youtube', playerOptions);
  }

  private onPlayerReady(event: YT.PlayerEvent) {
    setTimeout(function () { event.target.playVideo() }, this.playVideoWaitTime); //wait 3 seconds, then start
  }


  private onPlayerStateChange(event: YT.OnStateChangeEvent) {
    //if (userIsHost) {
    //  if (event.data == YT.PlayerState.ENDED) {
    //    if (loopOn) {
    //      player.seekTo(0, true);
    //      player.playVideo();
    //    } else {
    //      shareTubeHub.server.server_UpdateStatus(YT.PlayerState.ENDED);
    //      setTimeout(shareTubeHub.server.server_NextVideo(), waitToSendNextVideoTime);
    //    }
    //  }
    //  else {
    //    var statusID = parseInt(event.data);
    //    queueUpdateStatus(statusID);
    //  }
    //} else {
    //  //change the status back. thats a bad non-host, bad.
    //  broadcastStatus(currentStatus, false);
    //}
  }
}