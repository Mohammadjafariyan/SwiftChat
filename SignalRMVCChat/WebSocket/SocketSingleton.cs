using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Fleck;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.WebSocket
{
    public class SocketSingleton
    {
 
        public static readonly  MySocketServer Listener=new MySocketServer();

        public static ControllerContext ExampleControllerContext { get; set; }
        public static HttpContext ExampleHttpContext { get; set; }
    }

    public class MySocketServer
    {
        
        public  readonly WebSocketServer Server =
            new WebSocketServer("ws://0.0.0.0:8181" );

        public MySocketServer()
        {
            Server.RestartAfterListenError = true;
            Server.Start(socket =>
            {
                
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    
                    
                };
                socket.OnMessage = async message =>
                {
                  //  socket.Send(message);
                    
                  
                 await   WebSocketRequestThreadMaker.HandleRequest(message,socket);

                };
            });
        }
    }

    public class MySocketServerBackup
    {
        public static readonly Socket ServerSocket = new Socket(AddressFamily.InterNetwork, 
            SocketType.Stream, ProtocolType.IP);
      public MySocketServerBackup()
      {
          Init();
      }
      
      public void Init()
      {
          ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 9090));
          ServerSocket.Listen(int.MaxValue);
          ServerSocket.BeginAccept(null, 0, OnAccept, null);  
      }
      
       private static void OnAccept(IAsyncResult result)
        {
            try
            {
               
                if (ServerSocket != null && ServerSocket.IsBound)
                {
                   // WebSocketRequestThreadMaker.HandleRequest(result);
                }
                /*if (client != null)
                {
                    /* Handshaking and managing ClientSocket #1#

                    var key = headerResponse.Replace("ey:", "`")
                              .Split('`')[1]                     // dGhlIHNhbXBsZSBub25jZQ== \r\n .......
                              .Replace("\r", "").Split('\n')[0]  // dGhlIHNhbXBsZSBub25jZQ==
                              .Trim();

                    // key should now equal dGhlIHNhbXBsZSBub25jZQ==
                    var test1 = AcceptKey(ref key);

                    var newLine = "\r\n";

                    var response = "HTTP/1.1 101 Switching Protocols" + newLine
                         + "Upgrade: websocket" + newLine
                         + "Connection: Upgrade" + newLine
                         + "Sec-WebSocket-Accept: " + test1 + newLine + newLine
                         //+ "Sec-WebSocket-Protocol: chat, superchat" + newLine
                         //+ "Sec-WebSocket-Version: 13" + newLine
                         ;

                    // which one should I use? none of them fires the onopen method
                    client.Send(System.Text.Encoding.UTF8.GetBytes(response));

                    var i = client.Receive(buffer); // wait for client to send a message

                    // once the message is received decode it in different formats
                    Console.WriteLine(Convert.ToBase64String(buffer).Substring(0, i));                    

                    Console.WriteLine("\n\nPress enter to send data to client");
                    Console.Read();

                    var subA = SubArray<byte>(buffer, 0, i);
                    client.Send(subA);
                    Thread.Sleep(10000);//wait for message to be send


                }*/
                return;
            }
            catch (SocketException exception)
            {
                throw exception;
            }
            /*finally
            {
                if (SocketSingleton.ServerSocket != null && SocketSingleton.ServerSocket.IsBound)
                {
                    SocketSingleton.ServerSocket.BeginAccept(null, 0, OnAccept, null);
                }
            }*/
        }

    }
}