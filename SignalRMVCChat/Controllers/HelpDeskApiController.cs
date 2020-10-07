using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SignalRMVCChat.Controllers
{
    
    public class HelpDeskApiController:Controller
    {


        public ActionResult Search(string searchTerm)
        {
            
            List<dynamic> data=new List<dynamic>
            {
                
            };
            for (int i = 0; i < 100; i++)
            {
                data.Add(new {Id=i++,title="این یک لینک نمونه است ؟ ",link="#" + new Random().Next(0,99),});
            }




            return Json(new
            {
                array=data, 
                emptyText= "پیغام در صورت خالی بودن"

            }, JsonRequestBehavior.AllowGet);
        }
    }
}