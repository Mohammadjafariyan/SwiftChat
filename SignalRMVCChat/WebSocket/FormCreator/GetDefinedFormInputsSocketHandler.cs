using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.WebSocket.FormCreator
{
    public class GetDefinedFormInputsSocketHandler : ListSocketHandler<FormElement, FormElementService>
    {
        public GetDefinedFormInputsSocketHandler() : base("getDefinedFormInputsCallback")
        {
        }



        protected override IQueryable<FormElement> FilterAccess(IQueryable<FormElement> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {

            getQuery = getQuery.Include(c => c.Form)
                .Where(c => c.Form.MyWebsiteId == currMySocketReq.MyWebsite.Id);
            return base.FilterAccess(getQuery, request, currMySocketReq);
        }
    }
}