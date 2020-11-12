using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseAdminSendToCustomerSocketHandler : BaseMySocket
    {
        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            if (currMySocketReq.Name == "AdminSendToCustomer")
            {
                PlanService.CheckChatCount(currMySocketReq);
            }


            var _request = MyWebSocketRequest.Deserialize(request);

            // اگر برای ذخیره پیام ها باشد
            if (_request.Body.targetUserId?.ToString() == "SavedPms")
            {
                _request.Body = new Chat
                {
                    Message = _request.Body.typedMessage,
                    SenderType = ChatSenderType.SaveAsFastAnswering,
                };
                await new MultimediaPmSendSocketHandler().ExecuteAsync(_request.Serialize(), currMySocketReq);
                return null;
            }


            if (_request.Body.targetUserId == null
                || _request.Body.typedMessage == null ||
                _request.Body.uniqId == null)
            {
                throw new Exception("ورودی های اشتباه");
            }


            if (_request.Body.gapFileUniqId == null)
            {
                throw new Exception("gapFileUniqId نال است");
            }

            var gapFileUniqId = int.Parse(_request.Body.gapFileUniqId?.ToString());


            var uniqId = int.Parse(_request.Body.uniqId?.ToString());


            //var adminToken = _request.Body.adminToken?.ToString();
            var targetUserId = int.Parse(_request.Body.targetUserId?.ToString());
            var typedMessage = _request.Body.typedMessage?.ToString();

            //   MySpecificGlobal.ValidateAdminToken(adminToken);

            //todo :send to customer


            Chat chat = await SaveAndSend(targetUserId, typedMessage, uniqId, gapFileUniqId, request, currMySocketReq);


            /*/*#1#
            //=============================================================================
            _logService.LogFunc("CHECK ANOTHER TYPE OF MESSAGE");
            //=============================================================================
            var ChatProviderService = Injector.Inject<ChatProviderService>();


            int? formId = GetParam<int?>("formId", false);

            if (formId.HasValue)
            {
                //=============================================================================
                _logService.LogFunc("is a Form");
                //=============================================================================

                //todo:chat must has is filled flag
                chat.FormId = formId;


                //=============================================================================
                _logService.LogFunc("save chat");
                //=============================================================================

                ChatProviderService.Save(chat);
            }


            //=============================================================================
            _logService.LogFunc("END");
            //=============================================================================

            /*#1#*/
            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        protected virtual async Task<Chat> SaveAndSend(int targetUserId, string typedMessage, int uniqId,
            int gapFileUniqId,
            string request, MyWebSocketRequest currMySocketReq)
        {
            MySocket customer = currMySocketReq.MyWebsite.Customers.FirstOrDefault(c => c.CustomerId == targetUserId);

            if (customer == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

            var chatProviderServices = Injector.Inject<ChatProviderService>();


            MyWebSocketResponse response = new MyWebSocketResponse();

            response.Name = "adminSendToCustomerCallback";
            response.Message = typedMessage;


            int accountId = currMySocketReq.CurrentRequest.myAccountId.Value;

            var chat = chatProviderServices
                .AdminSendToCustomer(accountId
                    , targetUserId, typedMessage, currMySocketReq.MySocket.Id, gapFileUniqId, uniqId);


            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();
            int totalUnseen = chatProviderService.GetTotalUnseen(accountId
                , targetUserId, ChatSenderType.AccountToCustomer);

            var chatObject = chatProviderServices.GetQuery().Where(c=>c.Id==chat.Single).Include(c=>c.Customer)
                .Include(c=>c.MyAccount).SingleOrDefault();

            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            var Account = myAccountProviderService.GetById(accountId).Single;
            response.Content = new AdminSendToCustomerViewModel
            {
                AccountName = Account.Name,
                ProfilePhotoId=Account.ProfileImageId,
                AccountId = accountId,
                Message = typedMessage,
                TotalReceivedMesssages = totalUnseen,
                ChatId = chat.Single,
                Chat = chatObject
            };


            await MySocketManagerService.SendToCustomer(customer.CustomerId.Value, currMySocketReq.MyWebsite.Id,
                response);


            var _ChatObject = chatProviderService.GetById((int) chat.Single).Single;


            var CustomerProviderService = Injector.Inject<CustomerProviderService>();

            var customer_target = CustomerProviderService.GetById(customer.CustomerId.Value, "کاربر یافت نشد").Single;

            if ( currMySocketReq.IsAdminOrCustomer!=(int)MySocketUserType.Customer &&  customer_target.ContactAdmins != null && customer_target.ContactAdmins.Count > 0)
            {
                response.Name = "newSendPMByMeInAnotherPlaceCallback";
                
                foreach (var contactAdmin in customer_target.ContactAdmins)
                {
                    if (contactAdmin.Id!=currMySocketReq.MySocket.MyAccountId)
                    {
                        await MySocketManagerService.SendToAdmin(contactAdmin.Id, currMySocketReq.MyWebsite.Id,
                            response); 
                    }
                    
                }
            }

            


            // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
            await MySocketManagerService.NotifySelf(MySocketUserType.Admin, _ChatObject, currMySocketReq.MyWebsite.Id,
                currMySocketReq);


            return _ChatObject;
        }
    }
}