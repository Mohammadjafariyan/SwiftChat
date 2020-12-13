using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class UploadController:Controller
    {

        private ImageService _service = Injector.Inject<ImageService>();
        public ActionResult Upload(int id)
        {
            try
            {
                
                string imgContent= _service.GetById(id).Single.Content;

                int i = imgContent.IndexOf(",");
                imgContent = imgContent.Substring(i+1,imgContent.Length-i-1);
                byte[] bytes = Convert.FromBase64String(imgContent);  
  
               
                return File(bytes, "image/jpeg");


            }
            catch (Exception e)
            {
                var allBytes= System.IO.File.ReadAllBytes(Server.MapPath("~/Content/upload/notFound.png"));
                return File(allBytes, "image/png");
            }   
        }
    }
}