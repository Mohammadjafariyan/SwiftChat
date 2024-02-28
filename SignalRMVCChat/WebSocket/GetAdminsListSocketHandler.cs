using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class GetAdminsListSocketHandler:ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            var myWebsiteService = Injector.Inject<MyWebsiteService>();

            if (currMySocketReq.ChatConnection.MyAccountId.HasValue==false)
            {
                throw new Exception("کاربر درخواست کننده کد ادمین ندارد ");
            }
            
            var accountProvider = Injector.Inject<MyAccountProviderService>();


            var rootAdminId=await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin(_request,currMySocketReq);

            var admins = accountProvider.GetQuery()
                .Where(c => c.ParentId == rootAdminId).ToList();
            /*var admins= myWebsiteService.GetQuery()
                 .Include(c => c.Admins)
                 .Include("Admins.MyAccount")
                 .Where(c => c.Id == currMySocketReq.MyWebsite.Id)
                 .SelectMany(c => c.Admins.Select(ad => ad.MyAccount).ToList())
                 .Where(c=>c!=null).DistinctBy(c=>c.Id).ToList()
                ;*/

           admins= admins.Where(c => c.ParentId.HasValue && c.AccessWebsites.Contains(currMySocketReq.MyWebsite.Id) 
           && c.Id!=currMySocketReq.ChatConnection.MyAccountId).ToList();
            /*currMySocketReq.MySocket.Socket.Send(new MyWebSocketResponse
            {

                Name = "GetAdminsListCallback",
                Content = new MyDataTableResponse<MyAccount>
                {
                    EntityList = admins
                }
            }.Serilize());*/

            return await Task.FromResult< MyWebSocketResponse>(new MyWebSocketResponse
            {

                Name = "GetAdminsListCallback",
                Content = new MyDataTableResponse<MyAccount>
                {
                    EntityList = admins
                }
            } );


        }
    }
}