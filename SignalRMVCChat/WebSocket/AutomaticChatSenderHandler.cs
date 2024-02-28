using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AutomaticChatSenderHandler:AbstractAutomaticSendChatsSocketHandler,
        ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            /*// اگر کاربر کنونی نباشد برگرد 
            if (currMySocketReq.IsAdminOrCustomer!=(int)MySocketUserType.Admin)
            {
                return null;
            }*/
            
            /*//در دقایق مثبت فقط چک کند
            if (DateTime.Now.TimeOfDay.Minutes%2!=0)
            {
                return null;
            }*/

            var mySocketService = Injector.Inject<MySocketService>();

            var myaccountId=mySocketService.GetQuery().Where(c => c.AdminWebsiteId == currMySocketReq.CurrentRequest.websiteId)
                .Where(c => c.MyAccountId.HasValue).Select(c=>c.MyAccountId).FirstOrDefault();
            
            // هیچ ادمینی انلاین نیست
            if (myaccountId.HasValue==false)
            {
                return null;
            }

            int? tempMYADMINID = currMySocketReq.CurrentRequest.myAccountId;
            currMySocketReq.CurrentRequest.myAccountId = myaccountId;
           
           // نگه میدارد برای فراخانی متد های دیگر لازم است
            int? holderTemp = currMySocketReq.CurrentRequest.myAccountId;
            
            // درخواست را پارس می کند
            var _request = MyWebSocketRequest.Deserialize(request);


            // ادمین ریسشه این ادمین کنونی را برمی دارد
            int parentId= await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin(_request, currMySocketReq);
            
            // چت های ذخیره شده را بیاور
            var chats= GetAutomaticChats(parentId);
            if (chats==null || chats?.Count<=0)
            {
                currMySocketReq.CurrentRequest.myAccountId = tempMYADMINID;
                return null;
            }
 
            // هنوز آنلاین هستند ، 
            // هنوز چت نکرده اند 
            // از زمان ایجاد انها چند دقیقه گذشته است
            // از زمان ایجاد ان ها بازه زمانی یکی از چت های آماده را دربر می گیرد
            var customers= currMySocketReq.MyWebsite.Customers
                ?.Where(c => HubSingleton.IsAvailable(c.SignalRConnectionId))
                .Where(c=>
                    chats.Any(chat=>DateTime.Now .Subtract(c.CreationDateTime).TotalMinutes >=chat.delay.Value)
                    ).ToList();


            var customerIds = customers.Where(c=>c.CustomerId.HasValue).Select(c => c.CustomerId);

            if (customerIds.Count()==0)
            {
                currMySocketReq.CurrentRequest.myAccountId = tempMYADMINID;
                return null;
            }
            
            // اگر در دیتابیس دیتایی داشته باشند
            var chatService = Injector.Inject<ChatProviderService>();
            
            // کاستومر هایی که چت دریافت کرده اند کرده اند
            var customerChats = chatService.GetQuery()
                .Where(c => c.CustomerId.HasValue)
                .Where(c => customerIds.Contains(c.CustomerId)
                            // اگر چت ی وجود دارد ، آن را کاربر ارسال نکرده است
                            // یعنی اگر کاربر دریافت کرده باشد ملاک نیست
                            && c.SenderType != ChatSenderType.CustomerToAccount).ToList();
                
// کاستومر هایی که چت دریافت کرده اند کرده اند
            var haveChatCustomerIds=customerChats.Select(c => c.CustomerId).ToList();
            
            
            // آن کاستومر هایی که دیتا دارند را حف=ذف کن
            customers = customers.Where(c=>c.CustomerId.HasValue).Where(c =>
                !haveChatCustomerIds.Contains(c.CustomerId)).ToList();

            // امدینی که کمترین چت را دارد بده
            var admin= currMySocketReq.MyWebsite.Admins
                .OrderByDescending(c => c.Chats?.Count)
                .FirstOrDefault();


            if (admin==null)
            {
                currMySocketReq.CurrentRequest.myAccountId = tempMYADMINID;
                //todo:log ادمینی برای این سایت تعریف نشده است
                return null;
            }
            
            currMySocketReq.CurrentRequest.myAccountId = tempMYADMINID;
            foreach (var chat in chats)
            {
              //  var timeDelay= DateTime.Now.AddMinutes(-chat.delay.Value);
                    
                foreach (var customer in customers)
                {
                    if (DateTime.Now .Subtract(customer.CreationDateTime).TotalMinutes <chat.delay.Value)
                    {
                        // وقت آن فرا نرسیده است
                        continue;
                    }

                    #region همان پیغام اوتوماتیک را تکراری نفرست
                    
                    // پیام های این کاربر 
                    var thisCustomerChats=customerChats.Where(c => c.CustomerId == customer.Id);
                   
                    //اگر پیغام قبلا ارسال شده باشد نفرست
                    if (thisCustomerChats.Any(tc=>tc.UniqId==chat.UniqId))
                    {
                        continue;
                    }

                    #endregion
                

                    
                    
                    
                    int? tempMySocketMyAccountId = currMySocketReq.ChatConnection.MyAccountId;
                    int? tempMySocketMyCustomerId = currMySocketReq.ChatConnection.CustomerId;
                    int? tempMyAccountId = currMySocketReq.CurrentRequest.myAccountId;

                    try
                    {
                       
                        currMySocketReq.ChatConnection.MyAccountId = admin.MyAccountId;
                        currMySocketReq.ChatConnection.CustomerId = customer.CustomerId;

                        chat.targetId = customer.CustomerId;

                        if (string.IsNullOrEmpty(chat.MultimediaContent))
                        {
                            currMySocketReq.CurrentRequest.myAccountId = admin.MyAccountId;
                            // از نوع پیغام معمولی
                           await new AdminSendToCustomerSocketHandler()
                                .ExecuteAsync(new MyWebSocketRequest
                                {
                                    Body = new
                                    {
                                        targetUserId=customer.CustomerId,
                                        typedMessage=chat.Message,
                                        uniqId=chat.UniqId,
                                        gapFileUniqId=6161,
                                    }
                                }.Serialize(), currMySocketReq);


                        }
                        else
                        {
                            // چت مولتی مدیا
                            currMySocketReq.CurrentRequest.myAccountId = admin.MyAccountId;

                            // از نوع پیغام معمولی
                            await new MultimediaPmSendSocketHandler()
                                .ExecuteAsync(new MyWebSocketRequest
                                {
                                    Body = chat,
                                    IsAdminOrCustomer =(int) MySocketUserType.Admin,
                                    
                                    
                                }.Serialize(), currMySocketReq);
                        }
                    }
                    catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                        //todo ; logg
                        
                    }
                    
                    currMySocketReq.ChatConnection.MyAccountId= tempMySocketMyAccountId ;
                    currMySocketReq.ChatConnection.CustomerId= tempMySocketMyCustomerId ;
                    currMySocketReq.CurrentRequest.myAccountId = tempMyAccountId;


                }

            }
            currMySocketReq.CurrentRequest.myAccountId = holderTemp;

            return null;
        }
    }
}