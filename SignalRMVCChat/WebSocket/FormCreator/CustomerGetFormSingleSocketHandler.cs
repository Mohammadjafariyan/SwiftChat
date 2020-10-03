using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class CustomerGetFormSingleSocketHandler : BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            _logService.LogFunc(request);

            _request = MyWebSocketRequest.Deserialize(request);

            if (_request == null)
            {
                Throw("درخواست نال است");
            }


            //=============================================================================
            _logService.LogFunc("CustomerGetFormSingleSocketHandler");
            //=============================================================================

            #region validation

            var form = ValidateForm();

            #endregion


            //=============================================================================
            _logService.LogFunc("تمامی فرم های تعریف شده و دارای دسترسی");
            //=============================================================================


            var list = FormElementService.GetQuery().Where(c => c.FormId == form.Id).ToList();


            //=============================================================================
            _logService.LogFunc("list:" + list?.Count);
            //=============================================================================


            Debug.Assert(form != null, nameof(form) + " != null");
            form.Elements = list;


            //=============================================================================
            _logService.LogFunc("read chat:");
            //=============================================================================


            int chatId = GetParam<int>("chatId", true, "chatId not found");
            var ChatProviderService = Injector.Inject<ChatProviderService>();

            var chat = ChatProviderService.GetById(chatId, "چت یافت نشد از نوع فرم").Single;
            //=============================================================================
            _logService.LogFunc("END");
            //=============================================================================


            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                //=============================================================================
                _logService.LogFunc(
                    "اگر این متد از سمت ادمین درخواست شده باشد ، یعنی اطلاعاتی که کاربر ارسال کرده است را می خواهد بنابراین لازم است همه اطلاعات را داشته باشیم " +
                    "و مقدار هایی که کاربر ارسال کرده را در بیاوریم و برگردانیم");
                //=============================================================================

                //=============================================================================
                _logService.LogFunc("get customerId");
                //=============================================================================

                int CustomerId = GetParam<int>("customerId", true, "کد کاربر یافت نشد");

                var formValues = FormValueService.GetQuery()
                    .Where(f => f.ChatId == chatId && f.FormId == form.Id &&
                                f.CustomerId == CustomerId).ToList();


                //=============================================================================
                _logService.LogFunc("SET form.Elements VALUES");
                //=============================================================================

                if (formValues.Any())
                {
                    foreach (var element in form.Elements)
                    {
                        var i = formValues.FindIndex(s => s.FormElementId == element.Id);

                        if (i == -1)
                        {
                            //=============================================================================
                            _logService.LogFunc(
                                "اینجا اکسپشن ندادیم ، چون احتمالا ساختار فرم ویرایش شود و اینجا نفهمیم" +
                                "البته deletesafe است و باید ویرایش را نیز پشتیبانی کند ، ولی احتیاط می کنیم");
                            //=============================================================================

                            continue;
                        }

                        element.Value = formValues[i].Value;
                    }
                }
            }


            //=============================================================================
            _logService.Save();
            //=============================================================================


            return new MyWebSocketResponse
            {
                Name = "customerGetFormSingleCallback",
                Content = new
                {
                    form,
                    chatId = chat.Id,
                    chat,
                    Id = form.Id,
                    UniqId = chat.UniqId
                }
            };
        }
    }
}