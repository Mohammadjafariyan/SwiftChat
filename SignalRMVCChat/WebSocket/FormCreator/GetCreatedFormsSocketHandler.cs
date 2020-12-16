using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class GetCreatedFormsSocketHandler : BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            //=============================================================================
            _logService.LogFunc("GetCreatedFormsSocketHandler");
            //=============================================================================

            await base.ExecuteAsync(request, currMySocketReq);


            var myAccount = MyAccountProviderService
                .GetById(currMySocketReq.MySocket.MyAccountId.Value, "ادمین کنونی وجود ندارد").Single;


            //=============================================================================
            _logService.LogFunc("سایت های دارای دسترسی این ادمین");
            //=============================================================================

            var hasAccessWebsites = MyWebsiteService.LoadAccessWebsites(myAccount);


            //=============================================================================
            _logService.LogFunc("LoadAccessWebsites:" +
                                string.Join("-", hasAccessWebsites.Select(W => W.BaseUrl).ToArray()));
            //=============================================================================


            var formList = FormService.GetQuery()
                .ToList();

            var hasAccessForms = formList.Where(f => hasAccessWebsites.Select(w => w.Id).Contains(f.MyWebsiteId))
                .ToList();


            //=============================================================================
            _logService.LogFunc(
                "اگر دسترسی روت دارد پس همه را می تواند ببیند و الا فقط فرم هایی که خودش تعریف کرده می بیند");
            //=============================================================================


            var myAccont = MyAccountProviderService
                .GetById(currMySocketReq.MySocket.MyAccountId.Value, "ادمین کنونی یافت نشد").Single;

            //=============================================================================
            _logService.LogFunc(
                "اگر دسترسی روت دارد مشکلی ندارد ولی اگر دسترسی اش معولی  است فقط فرم های خودش را می تواند ویرایش  کند");
            //=============================================================================

            if (!myAccont.HasRootPrivilages)
            {
                //hasAccessForms = hasAccessForms.Where(h => h.Id == myAccont.Id).ToList();
            }

            //=============================================================================
            _logService.LogFunc("آنهایی که مخصوص هستند مخصوص من ها فقط باشد");
            //=============================================================================

            var onlyMes = hasAccessForms.Where(h => h.OnlyMe).Where(h => h.MyAccountId == myAccont.Id);

            //=============================================================================
            _logService.LogFunc("آنهایی که برای همه هستند");
            //=============================================================================

            var allpublics = hasAccessForms.Where(h => !h.OnlyMe).ToList();
            allpublics.AddRange(onlyMes);
            
            //=============================================================================
            _logService.LogFunc("تجمیع شد ، آنهایی که مخصوص دیگران بودند حذف گردید");
            //=============================================================================

            hasAccessForms = allpublics;

            //=============================================================================
            _logService.LogFunc("END");
            //=============================================================================

            //=============================================================================
            _logService.LogFunc("فرم های قبلا ارسال شده نباید ارسال بشوند");
            //=============================================================================


            int? customerId = GetParam<int?>("customerId", false, "UniqId not found");


            if (customerId.HasValue)
            {
                //=============================================================================
                _logService.LogFunc("customerId:" + customerId);
                //=============================================================================

                var ChatProviderService = Injector.Inject<ChatProviderService>();
                var usedFormIds = ChatProviderService.GetQuery()
                    .Where(c => c.MyAccountId == currMySocketReq.MySocket.MyAccountId &&
                                c.CustomerId == customerId && c.formId.HasValue)
                    .Select(c => c.formId).ToList();


                //=============================================================================
                _logService.LogFunc("usedForms:" + string.Join("-", usedFormIds));
                //=============================================================================

                hasAccessForms = hasAccessForms.Where(h => !usedFormIds.Contains(h.Id)).ToList();

                //=============================================================================
                _logService.LogFunc("used form names:" + string.Join("-",
                    hasAccessForms.Where(h => usedFormIds.Contains(h.Id))
                        .Select(h => h.Name)));
                //=============================================================================
            }


            //=============================================================================
            _logService.LogFunc("hasAccessForms:" + string.Join("-", hasAccessForms.Select(W => W.Name).ToArray()));

            _logService.Save();
            //=============================================================================


            return new MyWebSocketResponse
            {
                Name = "getCreatedFormsCallback",
                Content = new MyDataTableResponse<Form>
                {
                    EntityList = hasAccessForms
                }
            };
        }
    }
}