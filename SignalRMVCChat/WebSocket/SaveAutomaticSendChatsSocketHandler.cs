using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class SaveAutomaticSendChatsSocketHandler:AbstractAutomaticSendChatsSocketHandler,ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            // دیتا باید درست باشد
            var chats= this.Validation(request, currMySocketReq);
            
            var _request = MyWebSocketRequest.Deserialize(request);

            // کاربر ریشه این ادمین را بده

            int parentId= await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin( _request, currMySocketReq);

            // نوع را عوض می کند
            foreach (var chat in chats)
            {
                chat.ChatType = ChatType.AutomaticSend;
                chat.MyAccountId = parentId;
            }
            
            var list= GetAutomaticChats(parentId);

            
            /*
            var chatIds= list.Select(c => c.UniqId).ToList();
            
            // چت های قبلی اگر ارسال نشده باشند ، یعنی پاک شده اند
            var deletedChats = list.Where(c => !chats.Select(cc=>cc.UniqId).Contains(c.UniqId)).ToList();
            
            // چت هایی که قبلا وجود نداشته اند فقط برای اضافه کردن بمانند
            chats = chats.Where(c => !list.Select(cc=>cc.UniqId).Contains(c.UniqId)).ToList();
            */

            #region آنهایی که برای ذخیره ارسال شده اند مبادا حذف شوند

            list=list.Where(l =>
                !chats.Select(c => c.UniqId).Contains(l.UniqId)).ToList();

            #endregion
            

            
            if (chats.Count > 15)
            {
                throw new Exception("تعداد پیام های تعریف شده نمی تواند بیشتر از 15 مورد باشد");
            }


            var service = Injector.Inject<AutomaticChatService>();
            service.Delete(list);

            chats = chats.Where(c => c.Id == 0).ToList();
            service.Save(chats);
            
            return  new MyWebSocketResponse
            {
                Name = "successCallback",
                Message = "با موفقیت ثبت گردید"
            };
        }

        

        private List<ChatAutomatic> Validation(string request, MyWebSocketRequest currMySocketReq)
        {
            if (string.IsNullOrEmpty(request))
                        {
                            throw new Exception("درخواست نال است");
                        }
                        
                        var _request = MyWebSocketRequest.Deserialize(request);
            
                        if (_request == null)
                        {
                            throw new Exception("درخواست نال است");
                        }
            
                        if (_request.Body.chats==null)
                        {
                            throw new Exception("هیچ چتی یافت نشد");
                        }
                        
                        
                        var json= JsonConvert.SerializeObject(_request.Body.chats);
                        List<ChatAutomatic> chats= JsonConvert.DeserializeObject<List<ChatAutomatic>>(json);

                        if (chats==null)
                        {
                            throw new Exception("چت ها بصورت لیست ارسال نشده است");
                        }

                        if ((chats is List<ChatAutomatic>) == false)
                        {
                            throw new Exception("نوع چت ها تشخیص داده نشد");

                        }
                        
                        if (chats.Count==0)
                        {
                            throw new Exception("هیچ چتی ارسال نشده است");
                        }

                        foreach (var chat in chats)
                        {
                            if (chat.delay.HasValue==false)
                            {
                                throw new Exception("مقدار زمان تاخیر ارسال نشده است ");
                            }

                            if (chat.delay.Value>60)
                            {
                                throw new Exception("مقدار زمان نمی تواند بیشتر از 60 دقیقه باشد ");
                            }

                            if (chat.delay.Value<=0)
                            {
                                throw new Exception("مقدار زمان نمی تواند کمتر از 1 دقیقه باشد ");
                            }

                            if (string.IsNullOrEmpty(chat.Message) && 
                                string.IsNullOrEmpty(chat.MultimediaContent))
                            {
                                throw new Exception("چت ارسال شده خالی است");
                            }


                        }

                        return chats;
        }
    }
}