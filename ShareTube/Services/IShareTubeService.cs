using System;
using System.Collections.Generic;

namespace ShareTube.Data
{
    public interface IShareTubeService
    {
        Room AddRoom(string name = null);
        void AddUser(Guid roomID, Guid userID, Guid connID, string username);
        Video AddVideo(Guid roomID, Video video);
        void ChangeName(Guid connID, string name);
        void ClearEmptyRooms();
        void RemoveRoomIfEmpty(Guid roomID);
        List<Room> GetAllRoomsUserIsIn(Guid connID);
        Video GetCurrentVideo(Guid roomID);
        Guid GetHostConnectionID(Guid roomID);
        Room GetOrAddRoom(Guid roomID);
        List<RoomListItem> GetPublicRooms();
        Room GetRoom(Guid roomID);
        string GetUniqueRoomName();
        User GetUser(Guid userID);
        User GetUserByConnection(Guid connID);
        string GetUserNameOrUnique(Guid userID);
        List<string> GetUserNames(Guid roomID);
        List<Video> GetVideos(Guid roomID);
        bool NextVideo(Guid roomID);
        bool PrevVideo(Guid roomID);
        Guid? RemoveConnection(Guid connID);
        void RemoveRoom(Guid roomID);
        void SetCurrentVideo(Guid roomID, string videoID);
        void SetLoop(Guid roomID, bool loop);
        void UpdateHost(Guid roomID);
        void UpdateStatus(Guid roomID, ShareTubePlayerStatus status);
        void UpdateTime(Guid roomID, double time);
    }
}