using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.WebsocketTests
{
    public abstract class WebsocketTestSuiteBase
    {
        protected PrepareConnectionWebsocketTest prepare;
        protected string adminToken;
        
        
        protected int AccountId { get; set; }
        protected int CustomerId { get; set; }

        public List<Func<MyWebSocketResponse, int>> adminOwnCallbackList = new List<Func<MyWebSocketResponse, int>>();
        public List<Func<MyWebSocketResponse, int>> customerOwnCallbackList = new List<Func<MyWebSocketResponse, int>>();
        protected string customerToken;
        protected dynamic ResponseContentGet(dynamic respContent)
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(respContent));
        }
        public async Task Init()
        {
            prepare = new PrepareConnectionWebsocketTest();
            AccountId=  await prepare.Init();
            await InitAdmins();
            await InitCustomers();

        }
      
        
        
        
        
     

        public virtual async Task  InitAdmins()
        {
           await LoginSingleAdmin();
        }
        
        public virtual async Task  InitCustomers()
        {
            await LoginSingleCustomer();
        }
        
        protected async Task LoginSingleAdmin(string username=null,string pssword=null)
        {

            if (username!=null)
            {
                var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
                myAccountProviderService.Save(new MyAccount
                {
                    Username = username,
                    Password = pssword,
                    ParentId = AccountId
                });

            }
            else
            {
                username = "admin";
                pssword = "admin";
            }
            await prepare.ConnectNewUser("AdminLogin", new
            {
                password = username,
                username = pssword
            }, resp =>
            {


                if ( resp.Name=="adminLoginCallback")
                {
                    adminToken = resp.Token;
                    var CurrentRequest =
                        MySpecificGlobal.ParseToken(adminToken);
                    AccountId = CurrentRequest.myAccountId.Value;
                }

               
               
                Callbacks(resp,adminOwnCallbackList);       
                return -1;
            },true);
        }
        
        protected async Task AdminCall(string name,object body)
        {
            await prepare.ConnectNewUser(name, body, resp =>
            {
                
               
                
                Callbacks(resp,adminOwnCallbackList);       

                return -1;
            },true,adminToken);
        }

        private void Callbacks(MyWebSocketResponse resp, List<Func<MyWebSocketResponse, int>> callbackList)
        {
            foreach (var func in callbackList)
            {
                func(resp);
            }
        }

        protected async Task LoginSingleCustomer()
        {
            await prepare.ConnectNewUser("Register", null, resp =>
            {

                if (resp.Name=="registerCallback")
                {
                    customerToken = resp.Token;
                                  
                                    var CurrentRequest =
                                        MySpecificGlobal.ParseToken(customerToken);
                                    CustomerId = CurrentRequest.customerId.Value;
                }
 
                Callbacks(resp,customerOwnCallbackList);       

                return -1;
            });
        }


        protected async Task CustomerCall(string name,object body)
        {
            await prepare.ConnectNewUser(name, body, resp =>
            {
                
                Callbacks(resp,customerOwnCallbackList);       

                return -1;
            },false,customerToken);
        }
    }
}