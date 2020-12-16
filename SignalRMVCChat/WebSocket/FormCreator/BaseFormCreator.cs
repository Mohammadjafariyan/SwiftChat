using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public abstract class BaseFormCreatorSocketHandler : BaseMySocket
    {
        protected MyWebsiteService MyWebsiteService = Injector.Inject<MyWebsiteService>();
        protected FormService FormService = Injector.Inject<FormService>();
        protected FormElementService FormElementService = Injector.Inject<FormElementService>();
        protected MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
        protected FormValueService FormValueService = Injector.Inject<FormValueService>();
        protected CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();
        
        
        string MyDictionaryToJson(Dictionary<int, List<int>> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }
        protected Form ValidateForm()
        {
            _logService.LogFunc("_request.Body?.formId:" + _request.Body?.formId);

            var formIdstr = _request.Body?.formId?.ToString();

            int formId = 0;
            bool isParsed = int.TryParse(formIdstr?.ToString(), out formId);
            if (!isParsed)
            {
                Throw("کد فرم باشد ارسال نشده است");
            }

            _logService.LogFunc("_request.Body?.formId:" + _request.Body?.formId);

            var form= FormService.GetById(formId, "فرم یافت نشد").Single;


            return form;
        }
     
        protected List<FormElement> GetElementsFromJson(object elements)
        {
            var json = JsonConvert.SerializeObject(elements,new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            _logService.LogFunc("elements:"+json);

            return JsonConvert.DeserializeObject<List<FormElement>>(json);
        }


        public override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            
            /*if (currMySocketReq.IsAdminOrCustomer!= (int)MySocketUserType.Admin)
            {
                Throw("این عملیات فقط برای ادمین ها مجاز است");
            }*/
            
            
            //if (currMySocketReq.MySocket.MyAccountId.HasValue==false)
            //{
            //    Throw("ادمین ارسال نشده است");
            //}

            //MyAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value, "ادمین یافت نشد");
            
            return base.ExecuteAsync(request, currMySocketReq);
        }
    }


    /*
     *
     * MyCaller.Send('GetFormData', {formId: this.props.formId})

    MyCaller.Send('SaveFormData', {formId: this.props.formId})
 *MyCaller.Send('GetFormSingle',{formId:form.Id});

     * 
     *MyCaller.Send('GetCreatedForms')
     * 
     * MyCaller.Send('DeleteForm',{formId:formId});
     *   MyCaller.Send('SaveForm',{
            elements:this.state.elements,
            Name:this.state.Name,
            AfterMessage:this.state.AfterMessage,
            OnlyMe:this.state.OnlyMe
        });
     */
}