using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.Debugger;
using SignalRMVCChat.Areas.Customer;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Init;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.WebSocket
{
    public class FilterHandler : ISocketHandler
    {
        /// <summary>
        /// در این مرحله ما فقط می دانیم که وب سایت چیست ؟ و وب سایت ولیدیت شده است
        /// </summary>
        /// <param name="request"></param>
        /// <param name="currMySocketReq"></param>
        /// <returns></returns>
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            // اگر توکن نداشته باشد
            currMySocketReq.CurrentRequest = MySpecificGlobal.ParseToken(currMySocketReq.Token);

            var customerProviderService = Injector.Inject<CustomerProviderService>();

            if (currMySocketReq.CurrentRequest != null &&
                currMySocketReq.CurrentRequest.customerId.HasValue)
            {
                // اگر بر حسب اتفاقی ، کاستومر حذف شده باشد ، اینجا دوباره ایجاد می شود
                try
                {
                    var customer = customerProviderService.GetById(currMySocketReq.CurrentRequest.customerId.Value)
                        .Single;

                    if (customer.IsBlocked)
                    {
                        throw new NotImplementedException("شما توسط پشتیبانی بلاک شده اید");
                    }
                }
                catch (NotImplementedException e)
                {
                    throw new NotImplementedException("شما توسط پشتیبانی بلاک شده اید");
                }
                catch (Exception e)
                {
                    SignalRMVCChat.Service.LogService.Log(e);

                    throw new FindAndSetExcaption();
                    /*currMySocketReq.CurrentRequest.customerId = customerProviderService.Save(new Customer
                    {
                        Name = " بازدید کننده" + (currMySocketReq?.MySocket?.MyConnectionInfo?.ClientIpAddress ?? DateTime.Now.ToString("HH:mm"))
                    }).Single;*/
                }
            }


            // یعنی بار اولش است نه ادمین است که لوگین کرده و نه کاربر است معلوم نیست
            // در این if توکن ایجاد می شود
            if (currMySocketReq.CurrentRequest == null)
            {
                // اگر ادمین باشد او را لوگین می کنیم
                if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
                {
                    //admin
                    var resp = await new AdminLoginSocketHandler().RegisterAndGenerateToken(request, currMySocketReq);

                    currMySocketReq.MySocket.IsCustomerOrAdmin = MySocketUserType.Admin;
                    currMySocketReq.MySocket.MyAccount = resp.Content;

                    if (currMySocketReq.MySocket.MyAccountId != resp.Content.Id)
                        throw new Exception("کد های ادمین برابر نیست2");
                    currMySocketReq.MySocket.Token = resp.Token;
                }
                else
                {
                    // اگر کاربر باشد او را ثبت نام می کنمیم
                    // customer
                    var rsp = await new CustomerRegisterSocketHandler().RegisterAndGenerateToken(request,
                        currMySocketReq);
                    currMySocketReq.MySocket.IsCustomerOrAdmin = MySocketUserType.Customer;
                    currMySocketReq.MySocket.Customer = rsp.Temp;

                    currMySocketReq.MySocket.Token = rsp.Token;
                }


                currMySocketReq.CurrentRequest = MySpecificGlobal.ParseToken(currMySocketReq.Token);
            }
            else
            {
            }


            onCloseListenerSet(currMySocketReq);


            /// نگه داری کانکشن و اساین فرد به سایت مخصوص خود
            // ما در رم نگه می داریم لذا اینجا ریکوست در رم ثبت می شود
            await WebsiteSingleTon.WebsiteService.AddToOnwSite(currMySocketReq);


            // کد ها به ابکت 
            currMySocketReq = SetObjects(currMySocketReq);

            // یعنی شناخته شده باشد که ادمین است یا کاستومر
            if (currMySocketReq.MySocket.MyAccountId.HasValue || currMySocketReq.MySocket.CustomerId.HasValue)
            {
                /// این بخش مهم است زیرا ابجکت ها را نیز وصل می کند
                if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
                {
                    // خبر دار کردن همه کابران ان سایت از انلاین شدن ادمین جدید
                    await new AnotherSideNewOnlineInformerHandler().InformNewAdminRegistered(
                        currMySocketReq.MySocket.MyAccount,
                        currMySocketReq);
                }
                else
                {
                    // خبر دار کردن همه ادمین های ان سایت از انلاین شدن کاستومر جدید
                    await new AnotherSideNewOnlineInformerHandler()
                        .InformNewCustomerRegistered(currMySocketReq.MySocket.Customer, currMySocketReq);
                }
            }


            //VALIDATION:
            if (!MyGlobal.IsUnitTestEnvirement)
            {
                if (!MyGlobal.IsReactWebTesting)
                {
                    await Validation(request, currMySocketReq);
                }
            }
            //END VALIDATION


            #region Tracknig

            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Customer)
            {
                var _request = MyWebSocketRequest.Deserialize(request);
                if (_request.Name == "Register")
                {
                    // برای هر کاربر فقط یک بار بخوان
                    if (currMySocketReq.MySocket.Customer.LastTrackInfo == null)
                    {
                        try
                        {
                            var ipInfoService = Injector.Inject<IpInfoService>();


                            // اطلاعات کاربر را از سایت شخص ثالث می گیرد
                            var inforByIp =
                                await ipInfoService.GetInforByIp(currMySocketReq.MySocket.Socket.ConnectionInfo
                                    .ClientIpAddress);

                            // اطلاعاتی که خودمان موقع رفرش شدن کاربر از او میگیریم
                            var url = _request.Body.URL;
                            var title = _request.Body.Title;
                            var description = _request.Body.Description;


                            /*header infos:*/

                            var browser = currMySocketReq.MySocket.Socket.ConnectionInfo.Headers["User-Agent"];
                            var language = currMySocketReq.MySocket.Socket.ConnectionInfo.Headers["Accept-Language"];

                            string countryLanguage = "";
                            /*en-US,en;q=0.9*/
                            if (string.IsNullOrEmpty(language) == false)
                            {
                                try
                                {
                                    var parts = language.Split(',')[0].Split('-');

                                    language = parts[0];
                                    countryLanguage = parts[1];
                                }
                                catch (Exception e)
                                {
                                    //ignore
                                }
                            }
                            /*end*/


                            // ذخیره
                            var customerTrackerService = Injector.Inject<CustomerTrackerService>();
                            var track = new CustomerTrackInfo
                            {
                                CustomerId = currMySocketReq.CurrentRequest.customerId.Value,
                                Url = url + "",
                                PageTitle = title + "",
                                Descrition = description + "",
                                CityName = inforByIp.CityName,
                                Region = inforByIp.Region,
                                Time = DateTime.Now.ToString("HH:mm"),
                                TimeDt = DateTime.Now.TimeOfDay,
                                DateTime = DateTime.Now,


                                ip = inforByIp.ip,
                                type = inforByIp.type,
                                continent_code = inforByIp.continent_code,
                                continent_name = inforByIp.continent_name,
                                country_code = inforByIp.country_code,
                                country_name = inforByIp.country_name,
                                region_code = inforByIp.region_code,
                                region_name = inforByIp.region_name,
                                city = inforByIp.city,
                                latitude = inforByIp.latitude,
                                longitude = inforByIp.longitude,

                                UserCity = SystemDataInitService.GetUserCity(inforByIp.city),
                                UserState = SystemDataInitService.GetUserState(inforByIp.region_name),

                                Browser = browser,
                                Language = language,
                                CountryLanguage = countryLanguage
                            };
                            customerTrackerService.Save(track);

                            // چون این اطلاعات در رم است می توانیم آن را به این ابجکت بدهیم و نیازی نیست هر بار از دیتابیس بخوانیم
                            currMySocketReq.MySocket.Customer.LastTrackInfo = track;
                        }
                        catch (Exception e)
                        {
                            SignalRMVCChat.Service.LogService.Log(e);
                            //todo:log
                            //ignore
                        }
                    }
                }
            }

            #endregion


            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        private MyWebSocketRequest SetObjects(MyWebSocketRequest currMySocketReq)
        {
            if (currMySocketReq.MySocket.MyAccount == null && currMySocketReq.MySocket.Customer == null)
            {
                if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
                {
                    currMySocketReq.MySocket.MyAccountId = currMySocketReq.CurrentRequest.myAccountId;
                    var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
                    var myEntityResponse = myAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value);

                    currMySocketReq.MySocket.MyAccount = myEntityResponse.Single;
                }
                else
                {
                    currMySocketReq.MySocket.CustomerId = currMySocketReq.CurrentRequest.customerId;
                    var myAccountProviderService = Injector.Inject<CustomerProviderService>();
                    var myEntityResponse = myAccountProviderService.GetById(currMySocketReq.MySocket.CustomerId.Value);

                    currMySocketReq.MySocket.Customer = myEntityResponse.Single;
                }
            }


            return currMySocketReq;
        }

        private async Task onCloseListenerSet(MyWebSocketRequest currMySocketReq)
        {
            currMySocketReq.MySocket.Socket.OnClose = async () =>
            {
                await currMySocketReq.MySocket.OnSocketClose(currMySocketReq.MyWebsite, currMySocketReq);
            };
        }

        private async Task Validation(string request, MyWebSocketRequest currMySocketReq)
        {
            var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;
            // حساس به آدرس وب سایت استفاده کننده باشد 

            var baseUrl = new Uri(websiteUrl);
            var requestbaseUrl = new Uri(currMySocketReq.MySocket.MyConnectionInfo.Origin);
            if (requestbaseUrl.Authority != baseUrl.Authority)
            {
                throw new Exception(
                    "این وب سایت ثبت نام نشده است لطفا برای استفاده از پلاگین گپ چت ابتدا ثبت نام نموده و سپس پلاگین مخصوص وب سایت دلخواه خود را تعریف نمایید");
            }
        }
    }
}