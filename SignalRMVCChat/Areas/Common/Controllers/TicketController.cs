using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using OpenQA.Selenium;
using SignalRMVCChat.Areas.Common.Service;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Areas.Common.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter]
    public class TicketController : GenericController<Ticket>
    {
        public TicketService TicketService { get; }

        private MyFileService _myfileService;

        public TicketController(TicketService service, MyFileService myfileService)
        {
            Service = service;
            TicketService = service;
            _myfileService = myfileService;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }

        public override ActionResult Index(int? take, int? skip, int? dependId)
        {
            return base.Index(take, skip, dependId);
        }


        [ChildActionOnly]
        public PartialViewResult PartialList(int take, int? skip, string detailUrl)
        {
            skip = skip <= 0 ? null : skip;
            var response = Service.GetAsPaging(take, skip, null);

            ViewBag.detailUrl = detailUrl;
            return PartialView("TicketListPartial", response);
        }


        [ChildActionOnly]
        public PartialViewResult PartialDetail(int ticketId, string returnUrl, TicketStatus status)
        {

           


            return PartialView("TicketDetailPartial", new MyEntityResponse<Ticket>
            {
                Single = new Ticket
                {
                    ReturnUrl = returnUrl,
                    Status = status
                }
            });

        }

        public override ActionResult Save(Ticket model)
        {
            if (model.ReturnUrl == null)
            {
                throw new Exception("model.ReturnUrl is null");
            }

            List<MyFile> files = new List<MyFile>();
            if (model.files.Length > 0)
            {
                foreach (HttpPostedFileBase @file in model.files)
                {

                    if (file == null || @file.ContentType.ToLower() == "application/octet-stream")
                        continue;

                    if (!@file.ContentType.ToLower().Contains("pdf") &&
                        !@file.ContentType.ToLower().Contains("image") &&
                        !@file.ContentType.ToLower().Contains("zip"))
                        throw new Exception("این نوع فایل پشتیبانی نمی شود" + @file.ContentType);

                    var byteArr = FileToByTeArray(@file);

                    files.Add(new MyFile
                    {
                        FileContent = byteArr,
                        FileExtention = @file.ContentType,
                        FileName = @file.FileName
                    });
                }
            }

            model.ChangeDateTime = DateTime.Now;

            //  model.MyFiles = files;


            base.Save(model);
            foreach (var f in files)
            {
                f.TicketId = model.Id;
            }

            _myfileService.Save(files);

            if (model.ParentId.HasValue)
            {
                var parent = Service.GetById(model.ParentId.Value).Single;
                parent.ChangeDateTime = DateTime.Now;
                parent.Status = model.Status;
                base.Save(parent);
            }
            try
            {
                Uri myUri = new Uri( MyGlobal.GetBaseUrl(Request.Url )+ model.ReturnUrl ?? "");
                string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("ticketId");

                if (string.IsNullOrEmpty(param1))
                {
                    model.ReturnUrl += model.Id;
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + "\n"+ "پارت اول " + Request.Url.Authority + model.ReturnUrl ?? "");
            }
            try
            {
                Uri serverUri = new Uri(MyGlobal.GetBaseUrl(Request.Url));

                // needs UriKind arg, or UriFormatException is thrown
                Uri relativeUri = new Uri(model.ReturnUrl, UriKind.Relative);

                // Uri(Uri, Uri) is the preferred constructor in this case
                Uri fullUri = new Uri(serverUri, relativeUri);

                return Redirect(fullUri.ToString());
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + "\n" + "پارت دوم  ");
            }
          
        }

        private byte[] FileToByTeArray(HttpPostedFileBase file)
        {
            byte[] data;
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }

                data = memoryStream.ToArray();
            }

            return data;
        }


        //[ChildActionOnly]
        //public PartialViewResult UnSeenTicketsCount()
        //{

        //    var QUERY = TicketService.
        //        GetQuery();

        //    if (CurrentRequestSingleton.CurrentRequest?.AppLoginViewModel?.IsAdmin)
        //    {
        //        QUERY = QUERY.Where(c => c.TicketSeenStatus == TicketSeenStatus.NotSeen
        //        && c.AppAdminId == CurrentRequestSingleton.CurrentRequest.AppLoginViewModel.AppUserId);
        //    }
        //    else
        //    {
        //        QUERY = QUERY.Where(c => c.TicketSeenStatus == TicketSeenStatus.NotSeen
        //         && c.AppUserId == CurrentRequestSingleton.CurrentRequest.AppLoginViewModel.AppUserId);
        //    }

        //    return PartialView("UnSeenTicketsCount", QUERY.Count());

        //}
    }
}