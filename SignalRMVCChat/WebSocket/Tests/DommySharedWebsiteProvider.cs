using System.Net.Sockets;
using Fleck;
using SignalRMVCChat.Service;

namespace TestProject1
{
    public class DommySharedWebsiteProvider
    {


        public void Init()
        {
            WebsiteSingleTon.WebsiteService.Websites.Add(new MyWebsite
            {
                Admins = { new ChatConnection
                {
                }},
                Customers = { new ChatConnection()},
            });
        }
    }
}