using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Fleck;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.Timer;
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
        
       // public  readonly WebSocketServer Server =
       //     new WebSocketServer("wss://0.0.0.0:8181" );

        

        public MySocketServer()
        {
            /*X509Certificate2 Certificate = new X509Certificate2(HttpContext.Current.Server.MapPath("~/pk/test.com.pfx")
                , "1234");
                //, "mohammadjafariyan");
            
            if (Certificate != null)
            {
                Server.Certificate = Certificate;

                //Server.EnabledSslProtocols =
               //     SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

                  Server.EnabledSslProtocols = SslProtocols.Tls12;
            }
            
            Server.RestartAfterListenError = true;
            Server.Start(socket =>
            {
                
                socket.OnOpen = () =>
                {
                    // console.WriteLine("Open!");
                };
                socket.OnClose = () =>
                {
                    // console.WriteLine("Close!");
                    
                    
                };
                socket.OnMessage = async message =>
                {
                  //  socket.Send(message);
                    
                  
                  TimerService.Config();

                    try
                    {
                        await WebSocketRequestThreadMaker.HandleRequest(message, socket);
                    }
                    catch (Exception)
                    {

                        // خطاها سعی شده در لایه های پایین گرفته شود ، اگر اینجا خطا داد لازم نیست 
                        // برنامه استوپ شود
                    }

                };
            });*/
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
                    // console.WriteLine(Convert.ToBase64String(buffer).Substring(0, i));                    

                    // console.WriteLine("\n\nPress enter to send data to client");
                    console.Read();

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