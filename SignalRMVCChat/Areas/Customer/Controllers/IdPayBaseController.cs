using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TelegramBotsWebApplication.Idpay;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class IdPayBaseController:Controller 
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        public virtual string GetCallbackUrl()
        {
             throw new NotImplementedException();
        }
        
        public virtual string GetUsername()
        {
            throw new NotImplementedException();

        }
        
        public virtual string GetDescription()
        {
            throw new NotImplementedException();

        }
        
        [HttpGet]
        public async Task<ActionResult> Payment(string Amount,int OrderId)
        {
            string Link = "", Message = "";
            try
            {
                var payment = new Payment();
                var obj = new Payment.Request(OrderId+"");
                obj.amount = decimal.Parse(Amount);
                obj.name =GetUsername();
                obj.phone = "09141167864";
                obj.mail = "";
                obj.desc = GetDescription();
                obj.callback = GetCallbackUrl();

                var res = await payment.RequestPayment(obj);
                if (res is Payment.RequestRespons_Success)
                    Link = ((Payment.RequestRespons_Success)res).link;
                else
                    Message = ((Payment.RequestRespons_Fail)res).error_message;
            }
            catch(Exception ex)
            {
                
            }
            return View("Payment",new PaymentViewModel
            {
                PaymentUrl = Link, Message = Message
                ,Amount=Amount
            });
        }

        
        public async Task<ActionResult> AfterPayment()
        {
            string Message = "";
            try
            {
                var payment = new Payment();
                var obj = new Payment.ResultPayment();
                obj.status = int.Parse(Request.Params["status"]);
                obj.track_id = Request.Params["track_id"];
                obj.id = Request.Params["id"];
                obj.order_id = Request.Params["order_id"];
                obj.amount = decimal.Parse(Request.Params["amount"]);
                obj.card_no = Request.Params["card_no"];
                obj.hashed_card_no = Request.Params["hashed_card_no"];
                obj.date = double.Parse(Request.Params["date"]);




                if (!obj.IsOK)
                {
                    ViewBag.ID = obj.id;
                    ViewBag.OrderID = obj.order_id;
                    Message = obj.Message;
                }
                else
                {


                    // تایید تراکنش
                    var res = await payment.VerifyPayment(obj);
                    
                    if (res is Payment.PaymentInfo)
                    {
                        var tmp = (Payment.PaymentInfo)res;
                        Message = tmp.Message;
                        ViewBag.ID = tmp.id;
                        ViewBag.OrderID = tmp.order_id;

                        if (tmp.status == 100)
                        {
                            PaymentSuccessfulDoBusiness(tmp);
                        }
                        else
                        {
                            PaymentSuccessfulReCheck(tmp);
                        }



                    }
                    else
                    {
                        Message = ((Payment.RequestRespons_Fail)res).error_message;
                        PaymentError(((Payment.RequestRespons_Fail) res));
                    }
                    
                    
                    
                }
                
                

            }
            catch(Exception ex)
            {
                Message = ex.Message;
            }
            ViewBag.Message = Message;
            return View();
        }

        public virtual void PaymentError(Payment.RequestRespons_Fail tmp)
        {
            throw new NotImplementedException();

        }
        public virtual void PaymentSuccessfulReCheck(Payment.PaymentInfo tmp)
        {
            throw new NotImplementedException();

        }
        public virtual void PaymentSuccessfulDoBusiness(Payment.PaymentInfo tmp)
        {
            throw new NotImplementedException();

        }

        [HttpPost]
        public async Task<JsonResult> Inquiry(string ID, string OrderID)
        {
            string Message = "";
            try
            {
                var payment = new Payment();
                var res = await payment.InquiryPayment(ID, OrderID);
                if (res is Payment.PaymentStatus)
                {
                    var tmp = (Payment.PaymentStatus)res;
                    Message = tmp.Message;
                }
                else
                    Message = ((Payment.RequestRespons_Fail)res).error_message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            return Json(new { Message = Message }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult BeforeBank(int prise,int OrderId)
        {
            return View("BeforeBank", new BeforeBankViewModel
            {
                price=prise,
                OrderId=OrderId
            });
        }
    }

    public class PaymentViewModel 
    {
        public string PaymentUrl { get; set; }
        public string Message { get; set; }
        public string Amount { get; set; }
    }
}