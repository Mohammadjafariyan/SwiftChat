﻿using System.IO;
using System.Web;
using System.Web.Mvc;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.Utils
{
   public static class RazorViewToString
   {
       public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
       {
           controller.ViewData.Model = model;
           
           
           controller.ControllerContext= MySpecificGlobal.Clone(SocketSingleton.ExampleControllerContext);
           System.Web.HttpContext.Current = MySpecificGlobal.Clone(SocketSingleton.ExampleHttpContext);

           
           using (var sw = new StringWriter())
           {
               var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
               var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
               viewResult.View.Render(viewContext, sw);
               viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
               return sw.GetStringBuilder().ToString();
           }
       }
   }
}