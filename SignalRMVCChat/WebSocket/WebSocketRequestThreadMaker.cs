using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using Fleck;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Bot.Execute;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.WebSocket
{
    public class WebSocketRequestThreadMaker
    {
        public static async Task HandleRequest(string result, IWebSocketConnection socket)
        {
            await DoChat(result, socket);
        }

        public static async Task DoChat(string result, IWebSocketConnection socket)
        {
            try
            {
                // برای کار های ارسال درخواست این کار انجام می شود
                HttpContext.Current = SocketSingleton.ExampleHttpContext;

                // هندلر مورد درخواست از کلاینت را بر میگرداند
                var handler = WebSocketDispacher.Dispatch(result);

                //فقط اینجا باید باشد
                var request = MyWebSocketRequest.Deserialize(result);

                // اینجا شناسایی می کنیم از کدام سایت است و چه کسی است ؟
                WebsiteSingleTon.WebsiteService.FindAndSet(socket, request);

                // فیلتر در این جا اطلاعاتی در مورد درخواست می فهمیم
                var filterHandler = new FilterHandler();
                await filterHandler.ExecuteAsync(result, request);

                MyWebSocketResponse response = await handler.ExecuteAsync(result, request);


                if (response != null)
                {
                    // send Respnse
                    var body = response.Serilize();


                    await socket.Send(body);
                }

                request.Name = request.Name ?? "";

                if (request.Name.Contains("LiveAssist") == false)
                {
                    try
                    {
                        await new AutomaticChatSenderHandler().ExecuteAsync(result, request);
                    }
                    catch (Exception e)
                    {
                        //todo:log 
                    }


                    try
                    {
                        await new TotalUserCountsChangedSocketHandler().ExecuteAsync(result, request);
                    }
                    catch (Exception e)
                    {
                        //todo:log 
                    }
                }


                #region Bot

                try
                {
                    await new BotExecuteSocketHandler().ExecuteAsync(result, request);
                }
                catch (Exception e)
                {
                    //todo:log 
                }

                #endregion
            }
            catch (FindAndSetExcaption e)
            {
                LogService.Log(e, result);
                await socket.Send(new MyWebSocketResponse
                {
                    Name = "ClearCookie",
                }.Serilize());
            }
            catch (Exception e)
            {
                LogService.Log(e, result);

                // Get stack trace for the exception with source file information
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                var method = frame.GetMethod().ToString();
                var file = frame.GetFileName();
                file = Path.GetFileName(file);
/*0930 948 3176 */
                var errJson = new MyWebSocketResponse
                {
                    Message = MyGlobal.RecursiveExecptionMsg(e) + "\n" + file + ":" + method + ":" + line,
                    Type = MyWebSocketResponseType.Fail
                }.Serilize();

                if (MyGlobal.IsUnitTestEnvirement)
                {
                    await DoChat(result, socket);
                }

                try
                {
                    await socket.Send(errJson);
                }
                catch (Exception exception)
                {
                    // console.WriteLine(exception);
                    //ignore
                }
            }
        }
    }

    public class WebSocketRequestThreadMakerBackup
    {
        public static void HandleRequest(string result)
        {
            Task.Factory.StartNew(() => DoChat(result));
        }

        private static async void DoChat(string result)
        {
            /*
            Socket client = null;
            string headerResponse = "";
            client =  SocketSingleton.ServerSocket.EndAccept((IAsyncResult)result );
                                                                            
                                                                            
            byte[] buffer = new byte[1024];
                                                        
            var i = client.Receive(buffer);
            headerResponse = (System.Text.Encoding.UTF8.GetString(buffer));
            // write received data to the console
            var handler= WebSocketDispacher.Dispatch(headerResponse);

            MyWebSocketResponse response= await handler.ExecuteAsync(headerResponse,client);
           
            // send Respnse
            var body = response.Serilize();

            var str = new StringWriter();
            var httpResponse = new HttpResponse(str);
            httpResponse.StatusCode = 200;
          
            await  str.WriteAsync(body);

           
            var json=Newtonsoft.Json.JsonConvert.SerializeObject(response);
           
            client.Send(System.Text.Encoding.UTF8.GetBytes(json));
            */
        }
    }
}