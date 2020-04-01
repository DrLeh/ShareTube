using ShareTube.Core;
using ShareTube.Core.Extensions;
using ShareTube.Core.Helpers;
using ShareTube.Core.Models;
using ShareTube.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTube.Web.Hubs
{
    public interface IRoomClient
    {
        void BroadcastMessage(dynamic clients, string user, string msg, int type);
        void BroadcastUserList(dynamic clients, string json);
        void BroadcastCurrentTime(dynamic clients, double time, long timeSent);
        void BroadcastPlayerStatus(dynamic clients, int status, bool force);
        void BroadcastLoopStatus(dynamic clients, bool loop);
        void GetCurrentVideoUrl(dynamic clients, string url);
        void LoadVideoFromYoutubeUrl(dynamic clients, string url);
        void FullQueue(dynamic clients, string json);
        void VideoAdded(dynamic clients, string videoJson);
        void IsHostCallback(dynamic clients, bool userIsHost, string connID);


    }


    public class RoomHub : BaseHub<IRoomClient>
    {
        public IRoomService Service { get; set; }
        public IVideoService VideoService { get; }
        public ICookieHelper CookieHelper { get; set; }

        public RoomHub(IRoomService service, IVideoService videoService,   ICookieHelper cookieHelper)
        {
            CookieHelper = cookieHelper;
            Service = service;
            VideoService = videoService;
        }

        public async Task AddVideoAsync()
        {

        }



        //private bool _testing;


        //public bool IsUserHost
        //{
        //    get
        //    {
        //        if (_testing)
        //            return true;
        //        return Service.GetHostConnectionID(CurrentRoomID) == Guid.Parse(Context.ConnectionId);
        //    }
        //}

        //public static Dictionary<Guid, Guid> ConnectionRoomDictionary { get; set; } = new Dictionary<Guid, Guid>();


        //public Guid ConnectionID => Guid.Parse(Context.ConnectionId);

        //private User currentUser;
        //public User CurrentUser
        //{
        //    get
        //    {
        //        if (currentUser == null)
        //            currentUser = Service.GetUserByConnection(ConnectionID);
        //        return currentUser;
        //    }
        //}

        //private Room currentRoom;
        //public Room CurrentRoom
        //{
        //    get
        //    {
        //        if (currentRoom == null)
        //            currentRoom = Service.GetRoom(CurrentRoomID);
        //        return currentRoom;
        //    }
        //}
        //public Guid CurrentRoomID => ConnectionRoomDictionary[ConnectionID];

        //public dynamic RoomClients => Clients.Group(CurrentRoomID.ToString());
        //public dynamic Caller => Clients.Caller;




        ////public ShareTubeHub(IShareTubeService service, IShareTubeClientContract contract, ICookieHelper cookieHelper, bool testing)
        ////    : this(service, contract, cookieHelper)
        ////{
        ////    this._testing = testing;
        ////}








        //public override async Task OnConnectedAsync()
        //{
        //    await base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //    try
        //    {
        //        Server_LeaveRoom();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        await base.OnDisconnected(exception);
        //    }
        //}

        //#region Client Handlers




        ///// <summary>
        ///// roomID could be a Guid or an encoded GUID.
        ///// </summary>
        ////public void Server_JoinRoom(string roomID, string username, string existingConnection)
        ////{
        ////    var existingConnGuid = Guid.Parse(existingConnection);
        ////    var room = Repo.GetRoom(GuidEncoder.Decode(roomID));

        ////    var existingUser = room.Users.Single(x => x.ConnectionID == existingConnGuid);
        ////    existingUser.ConnectionID = ConnectionID;
        ////    existingUser.Name = HttpContext.Current.User.Identity.Name;

        ////    RoomClients_BroadcastUserList(CurrentRoomID);
        ////    Caller_UserIsHost();
        ////    Server_GetQueue();
        ////}

        ///// <summary>
        ///// roomID could be a Guid or an encoded GUID.
        ///// </summary>
        //public async Task Server_JoinRoom(string roomID, string username)
        //{
        //    if (username != null)
        //    {
        //        var roomGuid = GuidEncoder.DecodeGuid(roomID);
        //        Groups.AddToGroupAsync(Context.ConnectionId, roomGuid.ToString());
        //        ConnectionRoomDictionary.Add(Guid.Parse(Context.ConnectionId), roomGuid);

        //        var userID = CookieHelper.GetOrAddUserID();

        //        Service.AddUser(roomGuid, userID, ConnectionID, username);
        //        RoomClients_BroadcastUserJoinMessage(username);
        //        BroadcastUserList(CurrentRoomID);
        //        Caller_UserIsHost();
        //        Server_GetQueue();
        //    }
        //}

        ///// <summary>
        ///// roomID could be a Guid or an encoded GUID.
        ///// </summary>
        //public void Server_LeaveRoom()
        //{
        //    var username = CurrentUser.Name;
        //    var roomid = CurrentRoom.ID;

        //    var newHost = Service.RemoveConnection(Guid.Parse(Context.ConnectionId));
        //    if (newHost == Guid.Empty)
        //    {
        //        //room is kill
        //    }
        //    else
        //    {
        //        RoomClients_BroadcastUserLeaveMessage(username);
        //        BroadcastUserList(roomid);
        //        if (newHost != null)
        //        {
        //            //there's a new host
        //            Clients.Client(((Guid)newHost).ToString()).notifyNewHost();
        //        }
        //        else
        //        {
        //            //there's no new host, the host hasn't left or changed.
        //        }
        //    }
        //}


        //public void Server_ReportQueue()
        //{
        //    Server_GetQueue_Internal(RoomClients);
        //}
        //public void Server_GetQueue()
        //{
        //    Server_GetQueue_Internal(Caller);
        //}
        //private void Server_GetQueue_Internal(dynamic clients)
        //{
        //    var room = CurrentRoom;
        //    var urls = Service.GetVideos(CurrentRoomID)
        //        .Select(x => new Video
        //        {
        //            ID = x.ID,
        //            IsCurrent = x.IsCurrent,
        //            Length = x.Length,
        //            Order = x.Order,
        //            Author = x.Author,
        //            Title = x.Title
        //        })
        //        .OrderBy(x => x.Order)
        //        .ToList();
        //    var json = JsonConvert.SerializeObject(urls);
        //    ClientContract.FullQueue(clients, json);
        //}
        //public void Server_GetUserList()
        //{
        //    BroadcastUserList(CurrentRoomID, Caller);
        //}

        //public void Server_GetCurrentVideo()
        //{
        //    GetCurrentVid_Internal(Caller);
        //}

        //private void GetCurrentVid_Internal(dynamic clients)
        //{
        //    var vid = Service.GetCurrentVideo(CurrentRoomID);

        //    var currentVid = vid == null ? "" : vid.YouTubeUrl;

        //    ClientContract.LoadVideoFromYoutubeUrl(clients, currentVid);
        //}

        //public void Server_SendChatMessage(string message)
        //{
        //    RoomClients_BroadcastChatMessage(message);
        //}

        //public void Server_EnqueueVideo(string url, string name, string author)
        //{
        //    var room = CurrentRoom;
        //    if (room == null)
        //        return;

        //    var video = new Video
        //    {
        //        Author = author,
        //        Title = name,
        //        YouTubeUrl = url
        //    };

        //    var json = JsonConvert.SerializeObject(video);

        //    var videoAdded = Service.AddVideo(CurrentRoomID, video);
        //    if (videoAdded == null)
        //        return;

        //    ClientContract.VideoAdded(RoomClients, json);
        //    if (videoAdded.Order == 1)
        //        Server_SelectNewVideo(url);

        //    RoomClients_BroadcastVideoMessage(CurrentUser.Name, name, author);
        //}

        //public void Server_SelectNewVideo(string url) //TODO: also need to include the order of the vid for repeats
        //{
        //    if (!IsUserHost)
        //        return;
        //    Service.SetCurrentVideo(CurrentRoomID, url);
        //    ClientContract.LoadVideoFromYoutubeUrl(RoomClients, url);
        //}

        //public void Server_UpdatePlayerTime(double time, long sentTime)
        //{
        //    if (CurrentRoom == null)
        //        return;
        //    if (!IsUserHost)
        //        return;

        //    Service.UpdateTime(CurrentRoomID, time);

        //    BroadcastCurrentTime(sentTime, Clients.OthersInGroup(CurrentRoomID.ToString()));
        //    RoomClients_BroadcastPlayerStatus();
        //}

        //public void Server_UpdateStatus(int statusID)
        //{
        //    if (!IsUserHost)
        //        return;
        //    Service.UpdateStatus(CurrentRoomID, (ShareTubePlayerStatus)statusID);
        //    RoomClients_BroadcastPlayerStatus();
        //    BroadcastCurrentTime(0, Clients.OthersInGroup(CurrentRoomID.ToString()));
        //}

        //public void Server_GetCurrentVideoUrl()
        //{
        //    GetCurrentVid_Internal(Caller);
        //}

        //public void Server_ChangeName(string newName)
        //{
        //    Service.ChangeName(ConnectionID, newName);
        //    //broadcast to all rooms the user is in.
        //    foreach (var room in Service.GetAllRoomsUserIsIn(ConnectionID))
        //        BroadcastUserList(room.ID);
        //}

        //public void Server_NextVideo()
        //{
        //    if (!IsUserHost)
        //        return;
        //    if (Service.NextVideo(CurrentRoomID))
        //    {
        //        RoomClients_BroadcastCurrentVideo();
        //    }
        //    else
        //    {
        //        Service.UpdateStatus(CurrentRoomID, ShareTubePlayerStatus.Ended);
        //        RoomClients_BroadcastPlayerStatus();
        //    }
        //}

        //public void Server_PrevVideo()
        //{
        //    if (!IsUserHost)
        //        return;
        //    if (Service.PrevVideo(CurrentRoomID))
        //    {
        //        RoomClients_BroadcastCurrentVideo();
        //    }
        //    else
        //    {
        //        //when going backwards, if theres nothing before it, it's more unstarted than ended.
        //        Service.UpdateStatus(CurrentRoomID, ShareTubePlayerStatus.UnStarted);
        //        RoomClients_BroadcastPlayerStatus();
        //    }
        //}


        //public void Server_SetLoop(bool loop)
        //{
        //    if (!IsUserHost)
        //        return;
        //    Service.SetLoop(CurrentRoomID, loop);
        //    RoomClients_BroadcastLoopStatus();
        //}


        //#endregion



        //#region Client Callers

        //public void Caller_UserIsHost()
        //{
        //    Caller.IsHostCallback(Caller, IsUserHost, Context.ConnectionId);
        //}

        //public void Caller_GetQueue()
        //{
        //    Server_GetQueue_Internal(Caller);
        //}


        //#region Chat
        //public void RoomClients_BroadcastChatMessage(string message)
        //{
        //    message = message.Trim();
        //    if (message.Length > Constants.MAX_USER_MESSAGE_LENGTH)
        //        message = message.Substring(0, Constants.MAX_USER_MESSAGE_LENGTH) + "...";

        //    RoomClients_BroadcastChatMessage_Internal(message, MessageType.UserMessage);
        //}

        //public void RoomClients_BroadcastUserJoinMessage(string user)
        //{
        //    var message = user + " joined the room.";
        //    RoomClients_BroadcastChatMessage_Internal(message, MessageType.UserJoin);
        //}

        //public void RoomClients_BroadcastUserLeaveMessage(string user)
        //{
        //    var message = user + " left the room.";
        //    RoomClients_BroadcastChatMessage_Internal(message, MessageType.UserLeave);
        //}

        //public void RoomClients_BroadcastVideoMessage(string user, string videoName, string author)
        //{
        //    var message = user + " added a new video: " + videoName + " by " + author;
        //    RoomClients_BroadcastChatMessage_Internal(message, MessageType.VideoQueue);
        //}

        //private void RoomClients_BroadcastChatMessage_Internal(string message, MessageType mt)
        //{
        //    ClientContract.BroadcastMessage(RoomClients, CurrentUser.Name, message, (int)mt);
        //}

        //#endregion





        //#endregion

    }
}
