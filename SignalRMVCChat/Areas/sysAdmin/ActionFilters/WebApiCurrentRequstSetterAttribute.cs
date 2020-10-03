﻿using System.Linq;
 using System.Net;
 using System.Net.Http;
 using System.Web.Http.Controllers;
 using System.Web.Mvc;
 using SignalRMVCChat.DependencyInjection;
 using SignalRMVCChat.SysAdmin.Service;
 using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

 namespace TelegramBotsWebApplication.ActionFilters
{
    public class WebApiCurrentRequstSetterFilter :ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var currentRequestService = CurrentRequestSingleton.CurrentRequest;

            
            
            var token = filterContext.Request.Headers.GetValues("access-token").FirstOrDefault();


            if (string.IsNullOrEmpty(token))
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            
            //  var token = filterContext.RequestContext.HttpContext.Request.QueryString.Get("token");
            /*if (string.IsNullOrEmpty(token))
            {
                token = filterContext.RequestContext.HttpContext.Request.QueryString.Get("hashed");
            }*/

            currentRequestService.Token = token;

      //      currentRequestService= MySpecificGlobal.ParseToken(token, currentRequestService);
        }
    }

    

}