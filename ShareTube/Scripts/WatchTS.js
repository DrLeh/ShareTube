/*
 *
 * "-preTS" notations are the pre-typescript conversion - DJL 11/15/2014
 */
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/notify/notify.d.ts" />
/// <reference path="typings/signalr/signalr.d.ts" />
/// <reference path="typings/youtube/youtube.d.ts" />
///// <reference path="SignalrHubs.d.ts" />
/// <reference path="helpers.ts" />
//#region divs
var loggingOn = false;
function log(msg) {
    if (loggingOn)
        console.log(msg);
}
var videoQueueID = 'videoQueue';
var $relatedResults;
var $relatedResultsWrapper;
var $searchResults;
var $searchResultWrapper;
var $video;
var $videoPlaceHolder;
var $videoQueue;
var $userList;
var $searchQuery;
var $videoUrl;
var $username;
var $roomID;
var $chatMessage;
var $btnLoadSubscriptions;
var $btnSendmessage;
var $btnSearch;
var $btnSearchPrev;
var $btnSearchNext;
var $btnVideoAdd;
var userSubscriptionUrl = "https://gdata.youtube.com/feeds/api/users/default/subscriptions?v=2"; //v2
var devKey = "AI39si6JBRNsBozHilzD5xL9WoPFS_pJK1LMOrV7go48pIGOqWCauZNi4wfoHU-FFtAxjB-ikJeLy22j3aDaFvHwQxCuZojEYA";
//v3 keys
var apiKey = "AIzaSyD0bdaiVO819HDjNEAZ1SqjTFAC4jdBRaI";
var videoQueryPart = "id";
var videoQueryUrl = "https://www.googleapis.com/youtube/v3/videos?key=" + apiKey + "&part={0}&id={1}";
var searchStringFormat = "https://www.googleapis.com/youtube/v3/search?key=" + apiKey + "&part=snippet&q={0}&maxResults={1}&order={2}&type={3}";
var relatedUrlFormat = "https://www.googleapis.com/youtube/v3/search?key=" + apiKey + "&part=snippet&relatedToVideoId={0}&type=video";
//#endregion divs
//#region Start
var debugEnabled = true;
var shareTubeHub;
var roomID;
var username = "";
var userIsHost = false;
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
    username = $username.val();
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
        var url = obj.find('#url').val();
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
    var cachedquery;
    $btnSearch.click(function () {
        cachedquery = $searchQuery.val();
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
        var videoUrl = $videoUrl.val();
        var videoID = getParameterByName(videoUrl, "v");
        var titleQueryUrl = stringFormat(videoQueryUrl, "snippet", videoID);
        var settings = {
            url: titleQueryUrl,
            type: 'GET',
        };
        $.ajax(settings).done(function (data) {
            console.log(data);
            var title = data.items[0].snippet.title;
            console.log(title);
        });
    });
};
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
};
var enqueueVideo = function (url, name, author) {
    shareTubeHub.server.server_EnqueueVideo(url, name, author);
};
var cycleTimeCheck = function () {
    sendCurrentTimeToServer();
};
//TODO: create a new chrome for the youtube controls only available to the host. ugh. - DJL 11/16/2014
//also has to have controls for sound, fullscreen, and quality available for everyone.
var sendCurrentTimeToServer = function () {
    if (player) {
        if (userIsHost) {
            if (player) {
                if (player.getPlayerState() == YT.PlayerState.PLAYING) { //don't update time if it's not playing.
                    var currentTime = player.getCurrentTime();
                    var sentTime = new Date().getTime();
                    log("sending time update at: " + sentTime + " for player time: " + currentTime);
                    shareTubeHub.server.server_UpdatePlayerTime(currentTime, sentTime);
                }
            }
        }
    }
};
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
            setTimeout(function () { log(' ; New Time is: ' + player.getCurrentTime()); }, 1000);
        }
    }
};
//#endregion
//#region Chat
var scrollChat = function () {
    var objDiv = document.getElementById("messagelog");
    objDiv.scrollTop = objDiv.scrollHeight;
};
//#endregion
//#region Button Handlers
var addVideo = function () {
    var url = $('#videourl').attr("url");
    var name = $('#videourl').val("name").val(); //used to just be val("name");
    var author = "";
    enqueueVideo(url, name, author);
};
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
    if (!player)
        return;
    loopOn = loop;
    if (loopOn) {
        $('#btnLoop').addClass("btn-success");
    }
    else {
        $('#btnLoop').removeClass("btn-success");
    }
};
//#endregion
//#region Hub Handlers
var messageTypes = {
    UserJoin: 1,
    UserLeave: 2,
    UserMessage: 3,
    VideoQueue: 4,
};
var broadcastMessage = function (name, message, type) {
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
    //fullMessage = replaceTwitchEmotes(fullMessage);
    $('#discussion').append(fullMessage);
    scrollChat();
};
//had to put style="cursor:pointer" here because it wasn't working in css. no clue why. - DJL 11/15/2014s
var videoQueueEntryFormatString = '<tr class="queuedVideo {0}" url="{1}" order="{2}" style="cursor:pointer"><td><span class="glyphicon glyphicon-play"></span>{3}</td></tr>';
var videoQueue = [];
function fullQueue(videosJson) {
    var videos = JSON.parse(videosJson);
    videoQueue = videos;
    loadQueue();
}
var videoAdded = function (videoJson) {
    var video = JSON.parse(videoJson);
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
};
function loadQueue() {
    $videoQueue.html("");
    for (var i = 0; i < videoQueue.length; i++) {
        var video = videoQueue[i];
        loadVideoIntoQueue(video);
    }
}
function loadVideoIntoQueue(video) {
    var prevOrder = video.Order - 1;
    var currentClass = video.IsCurrent ? "success" : "";
    var html = stringFormat(videoQueueEntryFormatString, currentClass, video.YouTubeUrl, video.Order.toString(), video.Title);
    if (prevOrder > 0) {
        var prevVideoSelector = '#' + videoQueueID + ' [order="' + prevOrder + '"]';
        $(html).insertAfter(prevVideoSelector);
    }
    else {
        $videoQueue.append(html);
    }
}
//called as response to the line above: getCurrentVideoUrl
var getCurrentVideoUrlCallback = function (url) {
    $videoQueue.find('tr').removeClass("success");
    $videoQueue.find('tr[url="' + url + '"]').addClass("success");
};
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
        }
        else {
            $userList.append('<li>' + name + '</li>');
        }
    }
};
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
        var events = {
            onReady: onPlayerReady,
            onStateChange: onPlayerStateChange
        };
        var playerOpts = {
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
    }
    else {
        player.stopVideo();
        setTimeout(function () {
            player.loadVideoById(youtubeID);
            setTimeout(function () {
                player.playVideo();
            }, playVideoWaitTime);
        }, queueStatusWaitTime + 1000);
    }
    loadRelatedVideos(youtubeID);
};
//with duration
//var searchAndRelatedVideoResultFormat: string = '<tr class="yt-result pointer"><td><div class="media"><input type="hidden" id="url" value="{0}" /> \
//                <div class="pull-left"><img class="yt-thumb media-object" src="{1}" /><div class="video-time">{2}</div></div> \
//                <div class="media-body"><strong class="yt-title"><h4 class="media-heading">{3}</h4></strong><div class="yt-author">{4}</div></div></div></td></tr>';
//without duration
var searchAndRelatedVideoResultFormat = '<tr class="yt-result pointer"><td><div class="media"><input type="hidden" id="url" value="{0}" /> \
                <div class="pull-left"><img class="yt-thumb media-object" src="{1}" /></div> \
                <div class="media-body"><strong class="yt-title"><h4 class="media-heading">{3}</h4></strong><div class="yt-author">{4}</div></div></div></td></tr>';
function loadRelatedVideos(youtubeID) {
    var videos = [];
    var relatedUrl = stringFormat(relatedUrlFormat, youtubeID);
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
            var video = {
                Author: author,
                Title: title,
                Length: duration,
                LengthString: duration,
                ThumbnailUrl: firstThumbnail,
                YouTubeUrl: "https://youtube.com/watch?v=" + id,
                ID: id,
                IsCurrent: false,
            };
            var html = stringFormat(searchAndRelatedVideoResultFormat, video.YouTubeUrl, video.ThumbnailUrl, video.LengthString, video.Title, video.Author);
            $relatedResults.append(html);
        }
    });
}
var broadcastStatus = function (statusID, force) {
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
        }
        else if (statusID == YT.PlayerState.PLAYING) {
            player.playVideo();
        }
        else if (statusID == YT.PlayerState.PAUSED || statusID == YT.PlayerState.BUFFERING) {
            player.pauseVideo();
        }
    }
};
//#endregion
//#region Youtube
// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
var player;
function onYouTubeIframeAPIReady() {
}
function onPlayerReady(event) {
    setTimeout(function () { event.target.playVideo(); }, playVideoWaitTime); //wait 3 seconds, then start
}
var currentStatus;
var waitToSendNextVideoTime = 1500;
function onPlayerStateChange(event) {
    if (userIsHost) {
        if (event.data == YT.PlayerState.ENDED) {
            if (loopOn) {
                player.seekTo(0, true);
                player.playVideo();
            }
            else {
                shareTubeHub.server.server_UpdateStatus(YT.PlayerState.ENDED);
                setTimeout(shareTubeHub.server.server_NextVideo(), waitToSendNextVideoTime);
            }
        }
        else {
            var statusID = parseInt(event.data);
            queueUpdateStatus(statusID);
        }
    }
    else {
        //change the status back. thats a bad non-host, bad.
        broadcastStatus(currentStatus, false);
    }
}
var queuedStatusID;
var waiting = false;
var sent = false;
//wait this long to send a status change. this is because the status can change multiple times in <500ms
var queueStatusWaitTime = 500;
function queueUpdateStatus(statusID) {
    if (player) {
        queuedStatusID = statusID;
        setTimeout(function () {
            waiting = false;
        }, queueStatusWaitTime);
        setTimeout(function () {
            if (waiting) {
                //do nothing
            }
            else {
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
    var userToken = $('#googleAccessToken').val();
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
function search(searchQuery, pageToken) {
    $searchResults.empty();
    searchQuery = searchQuery.replace(" ", "+");
    var orderby = $("input:radio[name='orderby']:checked").val();
    var type = $("input:radio[name='type']:checked").val();
    var fullSearchString = stringFormat(searchStringFormat, searchQuery, pagesize.toString(), orderby, type);
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
            var video = {
                Author: snippet.channelTitle,
                Title: snippet.title,
                LengthString: "",
                ThumbnailUrl: defThumb.url,
                YouTubeUrl: "http://youtube.com/?v=" + videoId,
            };
            var html = stringFormat(searchAndRelatedVideoResultFormat, video.YouTubeUrl, video.ThumbnailUrl, video.LengthString, video.Title, video.Author);
            $searchResults.append(html);
        }
    });
}
function getDurationString(seconds) {
    var ret = "";
    var minutes = Math.floor(seconds / 60);
    var hours = Math.floor(minutes / 60);
    minutes = minutes - (hours * 60);
    seconds = seconds - (minutes * 60);
    var minutesStr = minutes.toString();
    if (hours > 0)
        ret += hours + ":";
    if (minutes < 10 && hours > 0)
        minutesStr = "0" + minutes;
    if (seconds < 10)
        seconds = "0" + seconds;
    ret += minutesStr + ":" + seconds;
    return ret;
}
//#endregion search
//# sourceMappingURL=WatchTS.js.map