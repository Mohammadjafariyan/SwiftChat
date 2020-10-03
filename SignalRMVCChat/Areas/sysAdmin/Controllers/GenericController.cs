﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Engine.SysAdmin.Models;
 using SignalRMVCChat.Areas.sysAdmin.Service;
 using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;
namespace TelegramBotsWebApplication.Areas.Admin.Controllers
{
    [CurrentRequstSetterFilter]
    [MyAuthorizeFilter]
    public abstract class GenericController<T> : Controller where T:class,IEntity,new()
    {
        protected IService<T> Service;
        // GET: Admin/Generic
        public virtual ActionResult Index(int? take, int? skip,int? dependId)
        {
            take = take ?? 20;
            skip = skip == 0 ? null : skip;
            MyDataTableResponse<T> response = Service.GetAsPaging(take.Value, skip,dependId);
            return View(response);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
           // _Logger.Error(filterContext.Exception);
           string msg= MyGlobal.RecursiveExecptionMsg(filterContext.Exception);
            // OR 
            var vm= new ViewDataDictionary(filterContext.Controller.ViewData)
            {
                Model = new ErrorViewModel
                {
                    Msg = msg
                } // set the model
            };
            ViewData["Error"] = msg;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = vm
            };
        }



        public virtual ActionResult Detail(int id)
        {
            if (id==0)
            {
                
                return View(new MyEntityResponse<T>
                {
                    Single = new T()
                });
            }
            MyEntityResponse<T> response = Service.GetById(id);

            return View(response);
        }

        [HttpPost]
        public virtual ActionResult Save(T model)
        {
            try
            {
                MyEntityResponse<int> response = Service.Save(model);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                 ModelState.AddModelError("",e.Message);
                                return View("Detail",new MyEntityResponse<T>
                                {
                                    Single = model
                                });
            }
          
        }


        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            MyEntityResponse<bool> response = Service.DeleteById(id);
            return RedirectToAction("Index");
        }

       
    }
}