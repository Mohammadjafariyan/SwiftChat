using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using Newtonsoft.Json;
using NUnit.Framework;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    /*MyCaller.Send('GetFormData', {formId: this.props.formId})
*/
    public class GetFormDataSocketHandler : BaseFormCreatorSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            //=============================================================================
            _logService.LogFunc("GetFormDataSocketHandler");
            //=============================================================================

            #region validation

            var form = ValidateForm();

            //=============================================================================
            _logService.LogFunc("_request.Body?.Page:" + _request.Body?.Page);
            //=============================================================================

            var Pagestr = _request.Body?.Page?.ToString();

            int? Page = null;
            int PageTemp = 0;
            bool isParsed = int.TryParse(Pagestr?.ToString(), out PageTemp);
            if (!isParsed)
            {
            }
            else
            {
                Page = PageTemp;
            }

            #endregion


            Page = Page ?? 1;
            Page = Page <= 0 ? 1 : Page;


            var query = FormValueService.GetQuery().Where(q => q.FormId == form.Id)
                .Include(f => f.FormElement)
                .OrderByDescending(o => o.CreationDateTime).AsQueryable();

            //=============================================================================
            _logService.LogFunc("Page:" + Page);
            //=============================================================================


           // var total = query.Count();
            if (Page == 1)
            {
                query = query.Take(20);
            }
            else
            {
                Page--;
                query = query.Skip(Page.Value * 20).Take(20);
            }

            //=============================================================================
            _logService.LogFunc("لود المنت های فرم:");
            //=============================================================================

            var formElement = FormElementService.GetQuery()
                .Where(c => c.FormId == form.Id).OrderByDescending(o=>o.Id).ToList();

            form.Elements = formElement;

            //=============================================================================
            _logService.LogFunc("لود المنت های فرم:" + form.Elements?.Count);
            //=============================================================================

            /*
        let formStructure = res.Content.Form;
        let formData = res.Content.FormData;
        let Page = res.Content.Page;
        let total = res.Content.Total;*/


            /*  let str = {
            elements: this.state.formStructure.elements,
            Name: this.state.formStructure.Name,
            AfterMessage: this.state.formStructure.AfterMessage,
            OnlyMe: this.state.formStructure.OnlyMe,
            Id: this.state.formStructure.Id
        };*/


            var list = query.ToList();
            var formData = list.Select(q =>
                new KeyValuePair<string, string>(q.FormElement.FieldName, q.Value)
            );


            List<List<FormElement>> table = new List<List<FormElement>>();


            var grouped = list.GroupBy(l => l.ChatId);

            foreach (var col in grouped)
            {
                List<FormElement> row = new List<FormElement>();
                foreach (var element in form.Elements)
                {
                    FormValue FormValue = null;
                    FormElement formelementClone = element.Clone();
                    try
                    {
                        FormValue = col.ToList().Single(c => c.FormElementId == element.Id);
                    }
                    catch (Exception e)
                    {
                        Throw("ارایه بیشتر از یک مقدار برای یک المنت فرم ثبت شده است");
                        throw;
                    }

                    formelementClone.Value = FormValue.Value;
                    
                    row.Add(formelementClone);
                }

                row=row.OrderByDescending(o => o.Id).ToList();
                table.Add(row);
            }


            return new MyWebSocketResponse
            {
                Name = "getFormDataCallback",
                Content = new
                {
                    Form = form,
                    Page = Page,
                    total = table.Count,
                    formData = table
                }
            };
        }
    }
}