﻿using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket
{
    public class ErrorSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var response = new MyWebSocketResponse {Message = " متد شناسایی نشد یا خطایی رخ داد " + request, Type = MyWebSocketResponseType.Fail};

            return response;
        }
    }
}