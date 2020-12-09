using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseMultimediaPmSendSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request
            , MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);


            // اگر ادمین بخواهد پیغامی ارسال بکند چک کن 
            if (_request.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                PlanService.CheckSendMultimedia(currMySocketReq);
            }

            Chat chat;
            try
            {
                chat = JsonConvert.DeserializeObject<Chat>(JsonConvert.SerializeObject(_request.Body));
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                throw new Exception("پیغام شناسایی نشد");
            }

            if (chat.SenderType == ChatSenderType.SaveAsFastAnswering ||
                chat.SenderType == ChatSenderType.SaveAsFastAnsweringForGroup)
            {
                SaveAsTemplate(chat, currMySocketReq);
            }
            else
            {
                ValidateAndSavethenSendToTarget(chat, currMySocketReq);
            }


            return null;
        }

        protected virtual void SaveAsTemplate(Chat chat, MyWebSocketRequest currMySocketReq)
        {
            /// اگر ادمین باشد پس قبلا کد آن شناسایی شده است و یا کاستمور باشد که همینطور
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                chat.MyAccountId = (int) currMySocketReq.CurrentRequest.myAccountId;
            }
            else
            {
                chat.CustomerId = (int) currMySocketReq.CurrentRequest.customerId;
            }


            chat.SendDataTime = DateTime.Now;


            var chatProviderService = Injector.Inject<ChatProviderService>();
            chatProviderService.Save(chat);
        }

        protected virtual async void ValidateAndSavethenSendToTarget(Chat chat, MyWebSocketRequest currMySocketReq)
        {
            if (chat.targetId.HasValue == false && chat.gapFileUniqId.HasValue == false)
            {
                throw new Exception("مخاطب پیغام درست ارسال نشده است");
            }


            var response = new MyWebSocketResponse
            {
                Name = "multimediaPmSendCallback",
                Content = chat,
            };


            MySocket target = null;


            target = GetTarget(target, chat, currMySocketReq);

            if (target == null)
            {
                //throw new Exception("مخاطب پیام شناسایی نشد");
            }


            chat.SenderType = target ==null ? ChatSenderType.CustomerToAccount :
                GetSenderType(
                    target); // بر اساس کاربر مخاطب می فهمیم که چه کسی به چه کسی ارسال کرده است این یک روش دیگر است
            chat.SendDataTime = DateTime.Now;
            chat.SenderMySocketId = target?.Id;

            
           
            
            int totalUnseen=0;

       
             
            

            var chatProviderService = Injector.Inject<ChatProviderService>();
            chatProviderService.Save(chat);
            
            
              
            if (target!=null)
            {
                if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Admin)
                {
                    
                    // ادمین در حال ارسال پیام مولتی مدیا به بازدید کننده است و میگوید که این مقدار پیام جدید هم دارید
                    chat.TotalReceivedMesssages= chatProviderService.GetTotalUnseen(currMySocketReq.MySocket.MyAccountId.Value
                        , target.CustomerId.Value  ,ChatSenderType.AccountToCustomer);

                }
                else
                {
                    chat.TotalReceivedMesssages= chatProviderService.GetTotalUnseen( target.MyAccountId.Value
                        ,currMySocketReq.MySocket.CustomerId.Value ,ChatSenderType.CustomerToAccount);
                }
            }


            if (target != null)
            {
                await Send(target, currMySocketReq, response,chat);

            }
            else
            {
                
            }
            
          


       
        }

        protected virtual async Task Send(MySocket target, MyWebSocketRequest currMySocketReq,
            MyWebSocketResponse response, Chat chat)
        {
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                await MySocketManagerService.SendToCustomer(target.CustomerId.Value, currMySocketReq.MyWebsite.Id,
                    response);


                // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
                await MySocketManagerService.NotifySelf(MySocketUserType.Admin, chat, currMySocketReq.MyWebsite.Id,
                    currMySocketReq);
            }
            else
            {


                await MySocketManagerService.SendToAdmin(target.MyAccountId.Value, currMySocketReq.MyWebsite.Id,
                    response);

                // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
                await MySocketManagerService.NotifySelf(MySocketUserType.Customer, chat, currMySocketReq.MyWebsite.Id,
                    currMySocketReq);
            }
        }

        protected virtual ChatSenderType GetSenderType(MySocket target)
        {
            return target.IsCustomerOrAdmin == MySocketUserType.Admin
                ? ChatSenderType.CustomerToAccount
                : ChatSenderType
                    .AccountToCustomer;
        }

        protected virtual MySocket GetTarget(MySocket target, Chat chat, MyWebSocketRequest currMySocketReq)
        {
            /// اگر ادمین باشد پس قبلا کد آن شناسایی شده است و یا کاستمور باشد که همینطور
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                chat.MyAccountId = (int) currMySocketReq.CurrentRequest.myAccountId;
                target = currMySocketReq.MyWebsite.Customers.FirstOrDefault(f => f.CustomerId == chat.targetId);
                chat.CustomerId = target.CustomerId;
            }
            else
            {
                chat.CustomerId = (int) currMySocketReq.CurrentRequest.customerId;
                target = currMySocketReq.MyWebsite.Admins.FirstOrDefault(f => f.MyAccountId == chat.targetId);

                if (target != null)
                {
                    chat.MyAccountId = target.MyAccountId;
                    
                }
            }

            return target;
        }
    }
}