using System;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.security.Controllers;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.WebsocketTests
{
    public class StartMockWebsocketServer
    {


        public static void Start()
        {
            Server = new MySocketServer();
        }

        public static  MySocketServer Server { get; set; }
    }
}