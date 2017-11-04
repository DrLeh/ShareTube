/*
 * 
 * "-preTS" notations are the pre-typescript conversion - DJL 11/15/2014
 */


/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/notify/notify.d.ts" />
///// <reference path="typings/signalr/signalr.d.ts" />
/// <reference path="typings/youtube/youtube.d.ts" />
///// <reference path="Hubs.d.ts" />
/// <reference path="helpers.ts" />

//#region divs
var loggingOn = false;

function log(msg: Object) {
    if (loggingOn)
        console.log(msg);
}

var videoQueueID = 'videoQueue';


var $relatedResults: JQuery;
var $relatedResultsWrapper: JQuery;
var $searchResults: JQuery;
var $searchResultWrapper: JQuery;
var $video: JQuery;
var $videoPlaceHolder: JQuery;
var $videoQueue: JQuery;
var $userList: JQuery;
var $searchQuery: JQuery;
var $videoUrl: JQuery;

var $username: JQuery;
var $roomID: JQuery;

var $chatMessage: JQuery;

var $btnLoadSubscriptions: JQuery;
var $btnSendmessage: JQuery;
var $btnSearch: JQuery;
var $btnSearchPrev: JQuery;
var $btnSearchNext: JQuery;
var $btnVideoAdd: JQuery;

var userSubscriptionUrl = "https://gdata.youtube.com/feeds/api/users/default/subscriptions?v=2";//v2
var devKey = "AI39si6JBRNsBozHilzD5xL9WoPFS_pJK1LMOrV7go48pIGOqWCauZNi4wfoHU-FFtAxjB-ikJeLy22j3aDaFvHwQxCuZojEYA";

//v3 keys
var apiKey: string = "AIzaSyD0bdaiVO819HDjNEAZ1SqjTFAC4jdBRaI";
var videoQueryPart: string = "id";


//#endregion divs


//#region Start

var debugEnabled = true;
var shareTubeHub;
var roomID;

var username: string = "";
var userIsHost: boolean = false;
$(function () {

    $relatedResults = $('#relatedResults');
    $relatedResultsWrapper = $('#relatedResultsDiv');
    $searchResults = $('#searchResults');
    $searchResultWrapper = $('#');
    $video = $('#video');
    $videoPlaceHolder = $('#videoPlaceholder');
    videoQueueID = 'videoQueue';
    $videoQueue = $('#' + videoQueueID);
    $userList = $('#userlist');
    $searchQuery = $('#searchQuery');
    $videoUrl = $('#videourl');

    $username = $('#username');
    $roomID = $('#roomID');

    $chatMessage = $('#message');

    $btnLoadSubscriptions = $('#btnLoadSubscriptions');
    $btnSendmessage = $('#sendmessage');
    $btnSearch = $('#searchButton');
    $btnSearchPrev = $('#searchPrevPage');
    $btnSearchNext = $('#searchNextPage');
    $btnVideoAdd = $('#videoadd');

    shareTubeHub = $.connection.shareTubeHub;

    username = $username.val() as string;
    setTimeout(function () {
        var msg = "";
        if ($('#userhascookie').val() == "True")
            msg = "Welcome back, " + username + "!";
        else
            msg = "You have joined the room as user: " + username;

        $.notify(msg, "success");
    }, 500);

    roomID = $roomID.val();
    $roomID.change(function () {
        roomID = $roomID.val();
    });

    //wire up all methods
    shareTubeHub.client.isHostCallback = isHostCallback;
    shareTubeHub.client.notifyNewHost = notifyNewHost;
    shareTubeHub.client.broadcastMessage = broadcastMessage;
    shareTubeHub.client.fullQueue = fullQueue;
    shareTubeHub.client.videoAdded = videoAdded;
    shareTubeHub.client.broadcastUserList = broadcastUserList;
    shareTubeHub.client.loadVideoFromYoutubeUrl = loadVideoFromYoutubeUrl;
    shareTubeHub.client.broadcastStatus = broadcastStatus;
    shareTubeHub.client.broadcastCurrentTime = broadcastCurrentTime;
    shareTubeHub.client.broadcastLoopStatus = broadcastLoopStatus;
    shareTubeHub.client.getCurrentVideoUrlCallback = getCurrentVideoUrlCallback;

    $.connection.hub.start().done(hubStarted);

    $searchResultWrapper.css('display', 'none');
    $relatedResultsWrapper.css('display', 'none');
});


$(function () {
    $chatMessage.keyup(function (event) {
        if (event.keyCode == 13) {
            //event.preventDefault();
            $btnSendmessage.click();
        }
    });

    $('#tab-search-selector').click(function () {
        setTimeout(function () { $searchQuery.focus(); }, 50);
    });

    //loadResult
    $('body').on('click', '.yt-result', function () {
        var obj = $(this);

        var url = obj.find('#url').val() as string;
        var name = obj.find('.yt-title').text();
        var author = obj.find('.yt-author').text();
        enqueueVideo(url, name, author);
        obj.animate({ backgroundColor: "#0a0" }, 200);
        obj.animate({ backgroundColor: "#fff" }, 500);
    });

    //load video from queue
    $('body').on('click', '.queuedVideo', function () {
        var obj = $(this);
        var url = obj.attr('url');
        shareTubeHub.server.server_SelectNewVideo(url);
    });

    $searchQuery.keyup(function (event) {
        if (event.keyCode == 13) {
            //event.preventDefault();
            $btnSearch.click();
        }
    });
    //used for prev/next buttons;
    var cachedquery: string;
    $btnSearch.click(function () {
        cachedquery = $searchQuery.val() as string;
        search(cachedquery);
    });

    $btnSearchPrev.click(function () {
        search(cachedquery, $(this).attr('token'));
    });

    $btnSearchNext.click(function () {
        search(cachedquery, $(this).attr('token'));
    });



    $(window).bind('beforeunload', function () {
        shareTubeHub.server.server_LeaveRoom();
    });
});


var cycleCheckInterval = 1000;
var hubStarted = function () {
    //replacing existing conncetion disabled. - DJL 11/15/2014
    //var replacingExistingConnection = $('#replaceConnID').val();
    //if (!replacingExistingConnection)
    shareTubeHub.server.server_JoinRoom(roomID, username);
    //else
    //    shareTubeHub.server.server_JoinRoom(roomID, username, replacingExistingConnection);


    $btnSendmessage.click(sendMessage);
    $btnVideoAdd.click(function () {
      var videoUrl: string = $videoUrl.val() as string;

        var videoID = getParameterByName(videoUrl, "v");
        var titleQueryUrl: string = `https://www.googleapis.com/youtube/v3/videos?key=${apiKey}&part=snippet&id=${videoID}`;
        var settings: JQueryAjaxSettings = {
            url: titleQueryUrl,
            type: 'GET',
        };
        $.ajax(settings).done(function (data) {
            console.log(data);
            var title = data.items[0].snippet.title;
            console.log(title);
        });
    });
}

var connectionID;
var userIsLoggingIn = false;
function isHostCallback(isHost, connID) {

    userIsHost = isHost;
    connectionID = connID;
    $('#connectionID').val(connectionID);
    var loginForm = $('#socialLoginList').parent();
    var loginAction = loginForm.attr('action');
    loginForm.attr('action', loginAction + "%3FloginFromConnectionID%3D" + connectionID);


    if (userIsHost)
        setInterval(cycleTimeCheck, cycleCheckInterval);
    shareTubeHub.server.caller_GetQueue();
    shareTubeHub.server.server_GetUserList();
    shareTubeHub.server.server_GetCurrentVideo();

    if (!userIsHost) {
        $('#btnPrevVideo').addClass("disabled");
        $('#btnNextVideo').addClass("disabled");
        $('#btnLoop').addClass("disabled");
    }
}

//you are now the host.
function notifyNewHost() {
    userIsHost = true;

    if (userIsHost)
        setInterval(cycleTimeCheck, cycleCheckInterval);

    $('#btnPrevVideo').removeClass("disabled");
    $('#btnNextVideo').removeClass("disabled");
    $('#btnLoop').removeClass("disabled");

    $.notify('You are now the host!', 'success');
}

$(function () {
    $('#loginDiv').click(function () {
        userIsLoggingIn = true;
    });
});



//#endregion


//#region Hub Message Senders

var sendMessage = function () {
    var message = $('#message').val();
    shareTubeHub.server.server_SendChatMessage(message);
    $('#message').val('').focus();
}


var enqueueVideo = function (url: string, name: string, author: string) {
    shareTubeHub.server.server_EnqueueVideo(url, name, author);
}

var cycleTimeCheck = function () {
    sendCurrentTimeToServer();
}
//TODO: create a new chrome for the youtube controls only available to the host. ugh. - DJL 11/16/2014
//also has to have controls for sound, fullscreen, and quality available for everyone.
var sendCurrentTimeToServer = function () {
    if (player) {
        if (userIsHost) {
            if (player) {
                if (player.getPlayerState() == YT.PlayerState.PLAYING) { //don't update time if it's not playing.
                    var currentTime = player.getCurrentTime();
                    console.log(currentTime);
                    var sentTime = new Date().getTime();
                    log("sending time update at: " + sentTime + " for player time: " + currentTime);
                    shareTubeHub.server.server_UpdatePlayerTime(currentTime, sentTime);
                }
            }
        }
    }
}

var syncFudgeFactor = .8;
var syncLeewayMS = 2000;
var broadcastCurrentTime = function (time, sentTime) {

    log('time broadcast: ' + time + " current time is: " + player.getCurrentTime());
    if (!userIsHost) {
        var now = new Date().getTime();

        //this calculation only can apply if the vid is playing. hrm. - DJL 11/16/20s14
        //var diff = now - sentTime;
        //var seekTimeFinal = time + diff / 1000;
        //console.log('time: ' + time);
        //console.log('sentTime: ' + sentTime);
        //console.log('now: ' + now);
        //console.log('diff: ' + diff);
        //console.log('diff / 1k: ' + diff / 1000);
        //console.log('seekTimeFinal: ' + seekTimeFinal);

        var seekTimeFinal = time + syncFudgeFactor;

        var currentPlayerTime = player.getCurrentTime();
        if (Math.abs(seekTimeFinal - currentPlayerTime) > syncLeewayMS / 1000) {
            player.seekTo(seekTimeFinal, true);
            log('seeked to: ' + seekTimeFinal);
            setTimeout(function () { log(' ; New Time is: ' + player.getCurrentTime()) }, 1000);
        }
    }
}

//#endregion


//#region Chat


var scrollChat = function () {
    var objDiv = document.getElementById("messagelog");
    objDiv.scrollTop = objDiv.scrollHeight;
}

//#endregion


//#region Button Handlers

var addVideo = function () {
    var url: string = $('#videourl').attr("url");
    var name: string = $('#videourl').val("name").val() as string; //used to just be val("name");
    var author: string = "";
    enqueueVideo(url, name, author);
}

var loopOn = false;
$(function () {
    $('#btnLoop').click(function () {
        if (userIsHost) {
            loopOn = !loopOn;
            shareTubeHub.server.server_SetLoop(loopOn);
        }
    });
    $('#btnPrevVideo').click(function () {
        if (userIsHost) {
            shareTubeHub.server.server_PrevVideo();
        }
    });
    $('#btnNextVideo').click(function () {
        if (userIsHost) {
            shareTubeHub.server.server_NextVideo();
        }
    });
});


var broadcastLoopStatus = function (loop) {
    if (!player) return;
    loopOn = loop;
    if (loopOn) {
        $('#btnLoop').addClass("btn-success");
    } else {
        $('#btnLoop').removeClass("btn-success");
    }
}

//#endregion


//#region Hub Handlers


var messageTypes = {
    UserJoin: 1,
    UserLeave: 2,
    UserMessage: 3,
    VideoQueue: 4,
};

var broadcastMessage = function (name, message: string, type) {

    var fullMessage = message;
    if (type == messageTypes.UserJoin) {
        fullMessage = '<li class="text-success">' + message + '</li>';
    }
    else if (type == messageTypes.UserLeave) {
        fullMessage = '<li class="text-danger">' + message + '</li>';
    }
    else if (type == messageTypes.UserMessage) {
        fullMessage = '<li><strong>' + name + '</strong>:&nbsp;&nbsp;' + message + '</li>';
    }
    else if (type == messageTypes.VideoQueue) {
        fullMessage = '<li class="text-info">' + message + '</li>';
    }
    console.log(fullMessage);
    fullMessage = replaceTwitchEmotes(fullMessage);
    $('#discussion').append(fullMessage);
    scrollChat();
}

//had to put style="cursor:pointer" here because it wasn't working in css. no clue why. - DJL 11/15/2014s
var videoQueue: Video[] = [];

function fullQueue(videosJson: string) {

    var videos: Video[] = JSON.parse(videosJson);
    videoQueue = videos;
    loadQueue();
}
var videoAdded = function (videoJson) {
    var video: Video = JSON.parse(videoJson);
    if (video.IsCurrent) {
        for (var i = 0; i < videoQueue.length; i++) {
            var video = videoQueue[i];
            video.IsCurrent = false;
        }
        loadVideoFromYoutubeUrl(video.YouTubeUrl);
    }
    //add to list later. the added video shouldn't ever really come as IsCurrent = true but meh. - DJL 11/15/2014
    videoQueue.push(video);
    loadVideoIntoQueue(video);
}

function loadQueue() {
    $videoQueue.html("");
    for (var i = 0; i < videoQueue.length; i++) {
        var video = videoQueue[i];
        loadVideoIntoQueue(video);
    }
}

function loadVideoIntoQueue(video: Video) {
    var prevOrder: number = video.Order - 1;
    var currentClass = video.IsCurrent ? "success" : "";

    var html: string = `<tr class="queuedVideo ${currentClass}" url="${video.YouTubeUrl}" order="${video.Order}" style="cursor:pointer"><td><span class="glyphicon glyphicon-play"></span>${video.Title}</td></tr>`;
    if (prevOrder > 0) {
        var prevVideoSelector = '#' + videoQueueID + ' [order="' + prevOrder + '"]';
        $(html).insertAfter(prevVideoSelector);
    } else {
        $videoQueue.append(html);
    }
}

//called as response to the line above: getCurrentVideoUrl
var getCurrentVideoUrlCallback = function (url) {
    $videoQueue.find('tr').removeClass("success");
    $videoQueue.find('tr[url="' + url + '"]').addClass("success");
}

//TODO: Measure this based on ping?

var broadcastUserList = function (userlist) {
    var users = JSON.parse(userlist);
    $userList.empty();

    for (var i = 0; i < users.length; i++) {
        var name = users[i];
        if (name.indexOf(username) != -1) {
            name = '<a class="btn btn-link" id="btnMyName" title="Click to change name" href="#">' + name + '</button>';

            $userList.append('<li>' + name + '</li>');
            $('#btnMyName').click(changeUserName);
        } else {

            $userList.append('<li>' + name + '</li>');
        }

    }
}

function changeUserName() {
    var newName = prompt("Enter a new name", username);
    var d = new Date();
    d.setTime(d.getTime() + (365 * 24 * 60 * 60 * 1000 * 50));
    var expires = "expires=" + d.toUTCString();
    //document.cookie = "ShareTubeUserName" + "=" + newName + "; " + expires;
    username = newName;

    shareTubeHub.server.server_ChangeName(newName);
}


var playVideoWaitTime = 2000;
var loadVideoFromYoutubeUrl = function (url) {
    if (!url)
        return;

    if (debugEnabled)
        log("loading youtube url: " + url);

    //TODO: add checks to make sure the id we found actually loaded a vid before loading it.
    var youtubeID = getParameterByName(url, "v");

    $videoQueue.find('tr').removeClass("success");
    $videoQueue.find('tr[url="' + url + '"]').addClass("success");

    if (!player) {
        var height = window.innerHeight ||
            document.documentElement.clientHeight ||
            document.body.clientHeight;
        //$('#video').height(0);

        $video.append('<div id="youtube" />');

        var events: YT.Events = {
            onReady: onPlayerReady,
            onStateChange: onPlayerStateChange
        };
        var playerOpts: YT.PlayerOptions = {
            height: '100%',
            width: '100%',
            videoId: youtubeID,
            events: events
        };
        player = new YT.Player('youtube', playerOpts);

        $videoPlaceHolder.animate({ height: '0px' }, 1000, function () {
            $(this).hide();
        });

        //$('#video').animate({ height: height }, 1000);

        //$('html,body').animate({
        //    scrollTop: 0
        //}, 1000);

    } else {
        player.stopVideo();
        setTimeout(function () {
            player.loadVideoById(youtubeID);
            setTimeout(function () {
                player.playVideo();
            }, playVideoWaitTime);
        }, queueStatusWaitTime + 1000);
    }

    loadRelatedVideos(youtubeID);
}

//with duration
//var searchAndRelatedVideoResultFormat: string = '<tr class="yt-result pointer"><td><div class="media"><input type="hidden" id="url" value="{0}" /> \
//                <div class="pull-left"><img class="yt-thumb media-object" src="{1}" /><div class="video-time">{2}</div></div> \
//                <div class="media-body"><strong class="yt-title"><h4 class="media-heading">{3}</h4></strong><div class="yt-author">{4}</div></div></div></td></tr>';

//without duration
var searchAndRelatedVideoResultFormat: string = '<tr class="yt-result pointer"><td><div class="media"><input type="hidden" id="url" value="{0}" /> \
                <div class="pull-left"><img class="yt-thumb media-object" src="{1}" /></div> \
                <div class="media-body"><strong class="yt-title"><h4 class="media-heading">{3}</h4></strong><div class="yt-author">{4}</div></div></div></td></tr>';

function loadRelatedVideos(youtubeID: string) {
    var videos: Video[] = [];

    var relatedUrl: string = `https://www.googleapis.com/youtube/v3/search?key=${apiKey}&part=snippet&relatedToVideoId=${youtubeID}&type=video`;

    $.get(relatedUrl).done(function (data) {
        $relatedResultsWrapper.css('display', '');
        var entries = data.items;
        console.log(data);

        $relatedResults.html('');

        for (var i = 0; i < entries.length; i++) {
            var entry = entries[i];
            console.log(entry);
            var snippet = entry.snippet;
            var title = snippet.title;
            var author = snippet.channelTitle;
            var id = entry.id.videoId;

            var alt = i % 2 == 1;
            var firstThumbnail = snippet.thumbnails.medium.url;
            var duration = "";

            //the <Video> casts it to a video, allowing you to skip initializing every value.
            var video: Video = <Video>{
                Author: author,
                Title: title,
                Length: duration,
                LengthString: duration,
                ThumbnailUrl: firstThumbnail,
                YouTubeUrl: "https://youtube.com/watch?v=" + id,
                ID: id,
                IsCurrent: false,
            };

            var html = getResultHtml(video.YouTubeUrl, video.ThumbnailUrl, video.Title, video.Author);
            $relatedResults.append(html);
        }
    });
}

var broadcastStatus = function (statusID: YT.PlayerState, force: boolean) {
    currentStatus = statusID;
    log('broadcasting status');
    if (!player) {
        log('player is null.');
        return;
    }
    if (!userIsHost || force) {
        //YT.PlayerState.Unstarted doesn't exist in TS? Why? should I add it?
        if (statusID == YT.PlayerState.UNSTARTED || statusID == YT.PlayerState.ENDED) {
            player.stopVideo();
        } else if (statusID == YT.PlayerState.PLAYING) {
            player.playVideo();
        } else if (statusID == YT.PlayerState.PAUSED || statusID == YT.PlayerState.BUFFERING) {
            player.pauseVideo();
        }

    }
}

//#endregion

//#region Youtube


// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');

tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

var player: YT.Player;
function onYouTubeIframeAPIReady() {
}

function onPlayerReady(event) {
    setTimeout(function () { event.target.playVideo() }, playVideoWaitTime); //wait 3 seconds, then start
}

var currentStatus: YT.PlayerState;
var waitToSendNextVideoTime = 1500;
function onPlayerStateChange(event) {
    if (userIsHost) {
        if (event.data == YT.PlayerState.ENDED) {
            if (loopOn) {
                player.seekTo(0, true);
                player.playVideo();
            } else {
                shareTubeHub.server.server_UpdateStatus(YT.PlayerState.ENDED);
                setTimeout(shareTubeHub.server.server_NextVideo(), waitToSendNextVideoTime);
            }
        }
        else {
            var statusID = parseInt(event.data);
            queueUpdateStatus(statusID);
        }
    } else {
        //change the status back. thats a bad non-host, bad.
        broadcastStatus(currentStatus, false);
    }
}

var queuedStatusID: number;
var waiting: boolean = false;
var sent: boolean = false;
//wait this long to send a status change. this is because the status can change multiple times in <500ms
var queueStatusWaitTime: number = 500;
function queueUpdateStatus(statusID) {
    if (player) {
        queuedStatusID = statusID;
        setTimeout(function () {
            waiting = false;
        }, queueStatusWaitTime);
        setTimeout(function () {
            if (waiting) {
                //do nothing
            } else {
                if (player) {
                    if (!sent) {
                        sendCurrentTimeToServer();
                        shareTubeHub.server.server_UpdateStatus(queuedStatusID);
                        sent = true;
                        setTimeout(function () { sent = false; }, queueStatusWaitTime);
                    }
                }
            }
        }, queueStatusWaitTime + 50);
    }
}

$(function () {
    $btnLoadSubscriptions.click(loadUserSubscriptions);
});


var includeKeyInUrl = true;

function loadUserSubscriptions() {
    var userToken = $('#googleAccessToken').val() as string;
    var prefix = "urn:tokens:google:accesstoken: ".length;
    userToken = userToken.substring(prefix, userToken.length);
    log(userToken);

    if (includeKeyInUrl)
        userSubscriptionUrl += "&key=" + devKey;
    log("url: " + userSubscriptionUrl);
    log("devKey: " + devKey);
    $.ajax(userSubscriptionUrl, {
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "GoogleLogin auth=" + userToken);
            if (!includeKeyInUrl)
                xhr.setRequestHeader("X-GData-Key: key=", devKey);
        },
        success: function (data) {
            log("success");
            log(data);
            $('#divSubscriptions').html();
        },
        error: function (data) {
            log("error");
            log(data);
        },
        complete: function (data) {
            log("complete");
            log(data);
        }
    });
}


//#endregion Youtube


//#region Search

var firstResult = 1;
var pagesize = 20;

function search(searchQuery: string, pageToken?: string) {

    $searchResults.empty();
    searchQuery = searchQuery.replace(" ", "+");

    var orderby = $("input:radio[name='orderby']:checked").val();
    var type = $("input:radio[name='type']:checked").val();
    
    var fullSearchString = `https://www.googleapis.com/youtube/v3/search?key=${apiKey}&part=snippet&q=${searchQuery}&maxResults=${pagesize.toString()}&order=${orderby}&type=${type}`;
    if (pageToken) {
        fullSearchString = fullSearchString + "&pageToken=" + pageToken;
    }

    $.get(fullSearchString).done(function (data) {
        $searchResultWrapper.css('display', '');
        if (data.nextPageToken) {
            $btnSearchNext.prop('disabled', false);
            $btnSearchNext.attr('token', data.nextPageToken);
        }
        else {
            $btnSearchNext.prop('disabled', true);
            $btnSearchNext.attr('token', '');
        }
        if (data.prevPageToken) {
            $btnSearchPrev.prop('disabled', false);
            $btnSearchPrev.attr('token', data.prevPageToken);
        }
        else {
            $btnSearchPrev.prop('disabled', true);
            $btnSearchPrev.attr('token', '');
        }

        var entries = data.items;
        var datas = new Array();
        for (var i = 0; i < entries.length; i++) {
            var entry = entries[i];
            var snippet = entry.snippet;
            var thumbNails = snippet.thumbnails;
            var defThumb = thumbNails.medium;

            var videoId = entry.id.videoId;

            

            //the <Video> casts it to a video, allowing you to skip initializing every value.
            var video: Video = <Video>{
                Author: snippet.channelTitle,
                Title: snippet.title,
                LengthString: "",
                ThumbnailUrl: defThumb.url,
                YouTubeUrl: "http://youtube.com/?v=" + videoId,
            };
            var html = getResultHtml(video.YouTubeUrl, video.ThumbnailUrl, video.Title, video.Author);
            $searchResults.append(html);
        }
    });
}

function getDurationString(seconds) {
    var ret: string = "";
    var minutes: number = Math.floor(seconds / 60);
    var hours: number = Math.floor(minutes / 60);
    minutes = minutes - (hours * 60);
    seconds = seconds - (minutes * 60);
    var minutesStr: string = minutes.toString();
    if (hours > 0)
        ret += hours + ":";
    if (minutes < 10 && hours > 0)
        minutesStr = "0" + minutes;
    if (seconds < 10)
        seconds = "0" + seconds;
    ret += minutesStr + ":" + seconds;
    return ret;
}

function getResultHtml(youtubeUrl:string, thumnailUrl:string, title:string, author:string){
    return `<tr class="yt-result pointer"><td><div class="media"><input type="hidden" id="url" value="${youtubeUrl}" />
<div class="pull-left"><img class="yt-thumb media-object" src="${thumnailUrl}" /></div>
<div class="media-body"><strong class="yt-title"><h4 class="media-heading">${title}</h4></strong><div class="yt-author">${author}</div></div></div></td></tr>`;
}
//#endregion search
