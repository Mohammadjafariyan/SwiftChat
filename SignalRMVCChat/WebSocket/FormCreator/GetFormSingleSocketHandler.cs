using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class GetFormSingleSocketHandler : BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {

             base.ExecuteAsync(request, currMySocketReq).GetAwaiter().GetResult();
            //=============================================================================
            _logService.LogFunc("GetFormSingleSocketHandler");
            //=============================================================================

            #region validation

           var formD= ValidateForm();

           var formId = formD.Id;
            #endregion


            //=============================================================================
            _logService.LogFunc("تمامی فرم های تعریف شده و دارای دسترسی");
            //=============================================================================

            
            var res = await new GetCreatedFormsSocketHandler().ExecuteAsync(request, currMySocketReq);


            var resp = res.Content as MyDataTableResponse<Form>;


            var form = resp?.EntityList?.FirstOrDefault(e => e.Id == formId);
            if (form == null)
            {
                Throw("به فرم دسترسی ندارید");
            }


            var list = FormElementService.GetQuery().Where(c => c.FormId == form.Id).ToList();


            //=============================================================================
            _logService.LogFunc("list:" + list?.Count);
            //=============================================================================

            //=============================================================================
            _logService.LogFunc("GET uniqId" );
            //=============================================================================

            
            
            int? uniqId = GetParam<int?>("uniqId", false, "chatId not found");
            //=============================================================================
            _logService.LogFunc("END");
            
            Debug.Assert(form != null, nameof(form) + " != null");
            form.Elements = list;


            //=============================================================================
            _logService.Save();
            //=============================================================================

            return new MyWebSocketResponse
            {
                Name = "getFormSingleCallback",
                Content =  new {form
                ,                    UniqId=uniqId

                }
            };
        }

      
    }
}