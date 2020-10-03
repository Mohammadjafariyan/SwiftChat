using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class SaveFormSocketHandler : BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {

            await base.ExecuteAsync(request, currMySocketReq);
        
            //=============================================================================
            _logService.LogFunc("SaveFormSocketHandler");
            //=============================================================================


            var elements = _request.Body?.elements;
            var Name = _request.Body?.Name?.ToString();
            var OnlyMe = _request.Body?.OnlyMe?.ToString();
            var AfterMessage = _request.Body?.AfterMessage?.ToString();
            var Message = _request.Body?.Message?.ToString();
            var idstr = _request.Body?.Id?.ToString();
            
            List<FormElement> formElements = GetElementsFromJson(elements);
            if (formElements == null || formElements?.Count == 0)
            {
                Throw("المت های فرم ها خالی است");
            }

            foreach (var formElement in formElements)
            {
                if (formElement.SubElements!=null)
                {
                    formElement.SubElementsTemp = formElement.SubElements;
                }
            }
            

            if (string.IsNullOrEmpty(Name))
            {
                Throw("نام فرم خالی است");
            }
            
            

            if (string.IsNullOrEmpty(AfterMessage))
            {
                Throw("پیغام بعد از ثبت فرم خالی است");
            }


            bool isOnlyMe = false;
            bool isParsed = bool.TryParse(OnlyMe?.ToString(), out isOnlyMe);
            if (!isParsed)
            {
                Throw("انحصار من باشد ارسال نشده است");
            }

            int id = 0;
            bool idParsed = int.TryParse(idstr?.ToString(), out id);

            var form = new Form
            {
                Id = id,
                OnlyMe = isOnlyMe,
                Name = Name,
                AfterMessage = AfterMessage,
                MyWebsiteId = currMySocketReq.MyWebsite.Id,
                MyAccountId = currMySocketReq.MySocket.MyAccountId.Value,
                Message=Message
                //  Elements = elements
            };


            if (idParsed)
            {
                //VALIDATGION    
                var editingForm= FormService.GetById(form.Id, "فرم وجود ندارد").Single;
               
                //=============================================================================
                _logService.LogFunc("چک کردن دسترسی ادمین درخواست کننده تغییر در فرم");
                _logService.LogFunc("ادمین یا ایجاد کننده است یا ادمین ریشه");
                //=============================================================================


                var myAccont= MyAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value, "ادمین کنونی یافت نشد").Single;

                //=============================================================================
                _logService.LogFunc("اگر دسترسی روت دارد مشکلی ندارد ولی اگر دسترسی اش نرم است فقط فرم های خودش را می تواند ویرایش  کند");
                //=============================================================================

                if (!myAccont.HasRootPrivilages)
                {
                    if (myAccont.Id!=editingForm.MyAccountId)
                    {
                        Throw("به فرم دسترسی ندارید ، یا سازنده آن نیست و یا دسترسی روت ندارید");
                    }

                }
                //=============================================================================
                _logService.LogFunc("end");
                //=============================================================================

                var res = await new GetCreatedFormsSocketHandler().ExecuteAsync(request, currMySocketReq);

                var resp = res.Content as MyDataTableResponse<Form>;


                var f = resp?.EntityList?.FirstOrDefault(e => e.Id == form.Id);
                if (f == null)
                {
                    Throw("به فرم دسترسی ندارید");
                }

                //=============================================================================
                _logService.LogFunc("اتمام چک دسترسی");
                //=============================================================================

            }


            //=============================================================================
            _logService.LogFunc("form.Id==" + form.Id);

            FormService.Save(form);
            _logService.LogFunc("فرم ذخیره شد");


            _logService.LogFunc("formElements.Count==" + formElements?.Count);


            _logService.LogFunc("deleted formElements.Count==" + formElements?.Select(f => f.IsDeleted).Count());
            //=============================================================================


            for (int i = 0; i < formElements.Count; i++)
            {
                formElements[i].FormId = form.Id;
            }


            if (idParsed)
            {
                //=============================================================================
                _logService.LogFunc("فرم ویرایش است");
                //=============================================================================

                var prevFormElements= FormElementService.GetQuery().Where(q => q.FormId == form.Id).ToList();
                
                //=============================================================================
                _logService.LogFunc("prevFormElements.count="+prevFormElements?.Count);
                //=============================================================================

                //=============================================================================
                _logService.LogFunc("کدام حذف شده است ؟");
                //=============================================================================


                var deletedFormElements=  prevFormElements
                    .Where(p =>
                    !formElements.Select(f => f.Id).Contains(p.Id)).ToList();
                
                

              
                //=============================================================================
              _logService.LogFunc("deletedFormElements.count="+deletedFormElements.Count());
              //=============================================================================

              foreach (var deletedFormElement in deletedFormElements)
              {
                  deletedFormElement.IsDeleted = true;
              }
              FormElementService.Save(deletedFormElements);
              
              //=============================================================================
              _logService.LogFunc("ذخیره حذف شده ها"+deletedFormElements.Count());
              //=============================================================================

            }


            FormElementService.Save(formElements);

            //=============================================================================
            _logService.LogFunc("با موفقیت ذخیره شد");
            //=============================================================================


            //=============================================================================
            _logService.Save();
            //=============================================================================

            return new MyWebSocketResponse
            {
                Name = "saveFormCallback",
            };
        }


        /*  MyCaller.Send('SaveForm',{
            elements:this.state.elements,
            Name:this.state.Name,
            AfterMessage:this.state.AfterMessage,
            OnlyMe:this.state.OnlyMe
        });*/
    }
}