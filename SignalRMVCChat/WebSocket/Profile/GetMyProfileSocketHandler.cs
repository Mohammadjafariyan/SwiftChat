using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Profile
{
    public class GetMyProfileSocketHandler:BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);
             
             if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
             {
                 Throw("اکانت نال است");
             }
             
             var myAccountService = DependencyInjection.Injector.Inject<MyAccountProviderService>();
             var imageService = DependencyInjection.Injector.Inject<ImageService>();
             
             
             var myAccount = myAccountService.GetById(currMySocketReq.MySocket.MyAccountId.Value, "اکانت شما یافت نشد")
                 .Single;
             string image = null;
             if (myAccount.ProfileImageId.HasValue)
             {
                 image=imageService.GetById(myAccount.ProfileImageId.Value, "عکس پروفایل یافت نشد").Single.Content;
             }

             myAccount.Username = null;
             myAccount.Password = null;
             
             return await Task.FromResult(new MyWebSocketResponse
             {
                 Name = "getMyProfileCallback",
                 Content = new
                 {
                     Image=image,
                     Name=myAccount.Name,
                     MyAccount=myAccount,
                     Id=myAccount.Id,
                     Prof="hiiiiiiiiiiiiiiiiiiiii"
                 }
                 
             });
        }
    }
}