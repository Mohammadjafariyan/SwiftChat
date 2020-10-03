using System;
using System.Web;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.SysAdmin.Service
{
    public class CurrentRequestHolder
    {
        public UserType UserType { get; set; }
        public string Token { get; set; }
        public int? RetailerId { get; set; }
        public int? CustomerId { get; set; }
        public AppLoginViewModel AppLoginViewModel { get; set; }
    }

    public class CurrentRequestSingleton
    {
        private static CurrentRequestHolder _CurrentRequest { get; set; }

        public static CurrentRequestHolder CurrentRequest
        {
            get
            {

                if (MyGlobal.IsUnitTestEnvirement)
                {
                    return Injector.Inject<CurrentRequestHolder>();
                }
                
                _CurrentRequest= HttpContext.Current.Items["currReq"] as CurrentRequestHolder;
                HttpContext.Current.Items["currReq"] = _CurrentRequest;

                return _CurrentRequest;
            }
        }


        /*{
            get => HttpContext.Current.Items["currReq"] as CurrentRequestHolder;
        }

        */
        
        public static void Init(HttpContextBase filterContextHttpContext)
        {
            _CurrentRequest = new CurrentRequestHolder();
            if (filterContextHttpContext.Items.Contains("currReq"))
            {
                filterContextHttpContext.Items["currReq"]=_CurrentRequest ;

            }
            else
            {
                filterContextHttpContext.Items.Add("currReq",_CurrentRequest );

            }
        }
    }


    public enum UserType
    {
        Retailer,
        Customer
    }
}