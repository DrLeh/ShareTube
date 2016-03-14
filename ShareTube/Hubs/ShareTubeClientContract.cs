using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace ShareTube.Hubs
{
    public interface IShareTubeClientContract
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

    public class ShareTubeClientContract : IShareTubeClientContract
    {
        public void BroadcastMessage(dynamic clients, string user, string msg, int type)
        {
            clients.broadcastMessage(user, msg, type);
        }
        public void BroadcastUserList(dynamic clients, string json)
        {
            clients.broadcastUserList(json);
        }
        public void BroadcastCurrentTime(dynamic clients, double time, long timeSent)
        {
            clients.broadcastCurrentTime(time, timeSent);
        }
        public void BroadcastPlayerStatus(dynamic clients, int status, bool force)
        {
            clients.broadcastStatus(status, force);
        }
        public void BroadcastLoopStatus(dynamic clients, bool loop)
        {
            clients.broadcastLoopStatus(loop);
        }
        public void GetCurrentVideoUrl(dynamic clients, string url)
        {
            clients.getCurrentVideoUrlCallback(url);
        }
        public void LoadVideoFromYoutubeUrl(dynamic clients, string url)
        {
            clients.loadVideoFromYoutubeUrl(url);
        }
        public void FullQueue(dynamic clients, string json)
        {
            clients.fullQueue(json);
        }
        public void VideoAdded(dynamic clients, string videoJson)
        {
            clients.videoAdded(videoJson);
        }
        public void IsHostCallback(dynamic clients, bool userIsHost, string connID)
        {
            clients.isHostCallback(userIsHost, connID);
        }
    }
}