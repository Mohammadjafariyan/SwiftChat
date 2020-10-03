using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace SignalRMVCChat.WebsocketTests
{
    public class MockClientHolder
    {
        public static async Task<MyClientSocket> NewClientConnect(string url)
        {
            var socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri(url), CancellationToken.None);

             return   new MyClientSocket(socket);

        }
        
        

    }

    public class MyClientSocket
    {
        private readonly ClientWebSocket _socket;
        
        
        
        
        public Func<string,string> OnMessage { get; set; }
        

        public MyClientSocket(ClientWebSocket socket)
        {
            _socket = socket;

        }

        public async Task Send<T>(T data) =>
            await Send(_socket, Newtonsoft.Json.JsonConvert.SerializeObject(data));

        public  async Task Send(ClientWebSocket socket, string data) =>
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text,
                true,
                CancellationToken.None);

        public async Task Receive()
        {
           await  Task.Factory.StartNew( async() =>
            {
                var buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result;
                string msg = null;
                do
                {
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await _socket.ReceiveAsync(buffer, CancellationToken.None);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Close)
                            break;

                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            msg = await reader.ReadToEndAsync();
                            OnMessage(msg);
                        }
                    }
                } while (true);
            });

        }
    }
}