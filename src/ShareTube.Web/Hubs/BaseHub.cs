using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace ShareTube.Web.Hubs
{
    public abstract class BaseHub<T> : Hub<T>
        where T : class
    {
        protected static void BroadcastDynamic(dynamic clients, object[] args, [CallerMemberName] string methodName = null)
        {
            //transform the method name
            methodName = methodName.Substring(methodName.IndexOf('_'));
            methodName = methodName.Substring(0, 1).ToLower() + methodName.Substring(1);
            var method = clients.GetType().GetMethod(methodName);
            method.Invoke(clients, args);
        }

        protected void Broadcast(object[] args, [CallerMemberName]string methodName = null)
        {
            BroadcastDynamic(Clients.All, args, methodName);
        }

        protected void BroadcastGroup(string groupName, object[] args, [CallerMemberName]string methodName = null)
        {
            BroadcastDynamic(Clients.Group(groupName), args, methodName);
        }
    }
}
