using System;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Profile
{
    public class SaveMyProfileSocketHandler : ISocketHandler
    {
        private LogService _logService = Injector.Inject<LogService>();

        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            _logService.LogFunc(request);

            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request == null)
            {
                Throw("درخواست نال است");
            }

            var name = _request.Body?.name?.ToString();
            var image = _request.Body?.image?.ToString();
            var Img = _request.Body?.Img?.ToString();
            if (string.IsNullOrEmpty(name))
            {
                Throw("عنوان وارد نشده است");
            }

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                Throw("اکانت نال است");
            }

            if (image == null)
            {
                image = Img;
            }

            var myAccountService = DependencyInjection.Injector.Inject<MyAccountProviderService>();
            var imageService = DependencyInjection.Injector.Inject<ImageService>();
            var myAccount = myAccountService.GetById(currMySocketReq.MySocket.MyAccountId.Value, "اکانت شما یافت نشد")
                .Single;


            myAccount.Name = name;

            Image profileImage = null;

            if (myAccount.ProfileImageId.HasValue)
            {
                try
                {
                    profileImage = imageService.GetById(myAccount.ProfileImageId.Value).Single;
                }
                catch (Exception e)
                {
                    _logService.LogFunc(e.Message);
                    profileImage = new Image();
                }
            }
            else
            {
                profileImage = new Image();
            }


            profileImage.Content = image;
            int newId = imageService.Save(profileImage).Single;

            myAccount.ProfileImageId = newId;

            myAccountService.VanillaSave(myAccount);

            return await Task.FromResult(new MyWebSocketResponse
            {

                Name = "saveMyProfileCallback",
                Content = new
                {
                    Image=profileImage.Content,
                    Name=myAccount.Name
                }
            });
        }

        public void Throw(string msg)
        {
            _logService.LogFunc(msg);
            _logService.Save();


            throw new Exception(msg);
        }
    }
}