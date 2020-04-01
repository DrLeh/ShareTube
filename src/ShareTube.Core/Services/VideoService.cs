using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Services
{
    public interface IVideoService
    {
        //void SetCurrentVideo(Guid roomID, string videoID);
        //Video GetCurrentVideo(Guid roomID);
        //List<Video> GetVideos(Guid roomID);
        //Video AddVideo(Guid roomID, Video video);
        //bool NextVideo(Guid roomID);
        //bool PrevVideo(Guid roomID);
        //void SetLoop(Guid roomID, bool loop);
        //void UpdateStatus(Guid roomID, ShareTubePlayerStatus status);
        //void UpdateTime(Guid roomID, double time);
    }

    public class VideoService : IVideoService
    {


        //public List<Video> GetVideos(Guid roomID)
        //{
        //    return Context.Videos.Where(x => x.RoomID == roomID).OrderBy(x => x.Order).ToList();
        //}

        ///// <summary>
        ///// Doesn't currently restrict to not have dupes. I could implement this to just return distinct instead, not sure.
        ///// </summary>
        ///// <param name="roomID"></param>
        ///// <param name="video"></param>
        //public Video AddVideo(Guid roomID, Video video)
        //{
        //    int order = 0;
        //    //if room already has videos, get the order. otherwise stick with 0.
        //    if (Context.Videos.Any(x => x.RoomID == roomID))
        //        order = Context.Videos.Where(x => x.RoomID == roomID).Max(x => x.Order);

        //    //don't allow dupes. - DJL 11/15/2014
        //    if (Context.Videos.Any(x => x.RoomID == roomID && x.ID == video.ID))
        //        return null;

        //    video.Order = order + 1;
        //    video.RoomID = roomID;
        //    if (video.Order == 1)
        //        video.IsCurrent = true;
        //    Context.Videos.Add(video);
        //    Context.SaveChanges();
        //    if (video.Order == 1)
        //        SetCurrentVideo(roomID, video.ID);
        //    return video;
        //}

        //public Video GetCurrentVideo(Guid roomID)
        //{
        //    return GetRoom_Internal(roomID).CurrentVideo;
        //}

        //public void SetCurrentVideo(Guid roomID, string videoID)
        //{
        //    videoID = YouTubeHelper.GetYouTubeID(videoID);
        //    var newVid = Context.Videos.SingleOrDefault(x => x.RoomID == roomID && x.ID == videoID);
        //    if (newVid != null)
        //    {
        //        var currentVids = Context.Videos.Where(x => x.RoomID == roomID && x.IsCurrent);
        //        foreach (var vid in currentVids) //just in case there are somehow multiple.
        //            vid.IsCurrent = false;
        //        newVid.IsCurrent = true;
        //    }
        //    Context.SaveChanges();
        //}

        //public bool NextVideo(Guid roomID)
        //{
        //    return ChangeVideoByIncrement(roomID, 1);
        //}
        //public bool PrevVideo(Guid roomID)
        //{
        //    return ChangeVideoByIncrement(roomID, -1);
        //}

        //private bool ChangeVideoByIncrement(Guid roomID, int inc)
        //{
        //    bool anyVidNext = true;
        //    var currentVid = Context.Videos.SingleOrDefault(x => x.RoomID == roomID && x.IsCurrent);
        //    var nextIndex = 0;
        //    if (currentVid != null)
        //    {
        //        nextIndex = currentVid.Order + inc;
        //    }
        //    var nextVid = Context.Videos.SingleOrDefault(x => x.RoomID == roomID && x.Order == nextIndex);
        //    if (nextVid == null)
        //        anyVidNext = false;
        //    else
        //    {
        //        nextVid.IsCurrent = true;
        //        currentVid.IsCurrent = false;
        //    }
        //    Context.SaveChanges();
        //    return anyVidNext;
        //}

        //public void SetLoop(Guid roomID, bool loop)
        //{
        //    var room = GetRoom_Internal(roomID);
        //    room.Loop = loop;
        //    Context.SaveChanges();
        //}

        //public void ClearEmptyRooms()
        //{
        //    var roomsToDelete = Context.Rooms.Where(x => !x.UserConnections.Any());
        //    Context.Rooms.RemoveRange(roomsToDelete);
        //    Context.SaveChanges();
        //}

        //public void RemoveRoomIfEmpty(Guid roomID)
        //{
        //    var room = Context.Rooms.SingleOrDefault(x => x.ID == roomID);
        //    if (room == null)
        //        return;

        //    if (!room.UserConnections.Any())
        //    {
        //        Context.Rooms.Remove(room);
        //        Context.SaveChanges();
        //    }
        //}

    }
}
