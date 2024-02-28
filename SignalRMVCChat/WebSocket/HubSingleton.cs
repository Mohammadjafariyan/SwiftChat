using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRMVCChat.Hubs;

namespace SignalRMVCChat.WebSocket
{
   

    public class HubSingleton
    {
       
        


        public static bool IsAvailable(string argSignalRConnectionId)
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<CustomerHub>();

            return hubContext.Clients.Client(argSignalRConnectionId) != null;
        }

        public static void Send(string argSignalRConnectionId, string serilize)
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<CustomerHub>();

            var client = hubContext.Clients.Client(argSignalRConnectionId);
            if (client == null)
                throw new Exception("کاربر افلاین است");

            client.onmessage(serilize);
        }
    }
}