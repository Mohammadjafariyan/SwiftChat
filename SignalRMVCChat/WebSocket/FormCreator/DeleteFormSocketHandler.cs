using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class DeleteFormSocketHandler:BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            #region validation

            //=============================================================================
            _logService.LogFunc("DeleteFormSocketHandler");
            //=============================================================================
            await base.ExecuteAsync(request, currMySocketReq);


            int formId = 0;
            bool isParsed= int.TryParse(_request.Body?.formId?.ToString(), out formId);

            if (isParsed==false)
            {
                Throw("کد فرم ارسال نشده است");
                
            }
            
            //=============================================================================
            _logService.LogFunc("formId="+formId);
            //=============================================================================

         

            
            //=============================================================================
            _logService.LogFunc("ایا کد فرم درست است=");
            //=============================================================================


            FormService.GetById(formId, "فرم درخواست شده وجود ندارد");

            var form= FormService.GetQuery().Include(c => c.Elements).FirstOrDefault(f => f.Id == formId);

            if (form==null)
            {
                Throw("معجزه ای رخ داده و فرم وجود ندارد");
            }
            


            var myAccount= MyAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value, "اکانت کنونی وجود ندارد").Single;

            //=============================================================================
            _logService.LogFunc("به چه سایت هایی دسترسی دارد ؟");
            //=============================================================================

          
            //=============================================================================
            var myWebsites= MyWebsiteService.LoadAccessWebsites(myAccount);
            _logService.LogFunc("آیا این ادمین به سایت دسترسی دارد ؟");
            if (!myWebsites.Select(w=>w.Id).Contains(form.MyWebsiteId))
            {
                Throw("شما ایجاد کننده یا ادمین اصلی وب سایت مورد نظر نیست و مجاز به حذف فرم نمی باشید");
            }
            //=============================================================================
            #endregion

            
            

            foreach (var formElement in form.Elements)
            {
                formElement.IsDeleted = true;
            }

            form.IsDeleted = true;

            FormService.Save(form);

            FormElementService.Save(form.Elements);
            
            //=============================================================================
            _logService.LogFunc("فرم بدرستی حذف شد");

            _logService.Save();
            //=============================================================================

            return new MyWebSocketResponse
            {
                Name = "deleteFormCallback"
            };
        }
    }
}