using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.Debugger;
using SignalRMVCChat.Areas.Customer;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Init;
using SignalRMVCChat.Service.MyWSetting;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket
{
    public class FilterHandler : ISocketHandler
    {

        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();

        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();

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
                    var customer = customerProviderService
                        .GetById(currMySocketReq.CurrentRequest.customerId.Value)
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
                catch (NotFoundExeption e)
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
                if (currMySocketReq.IsAdminOrCustomer == (int)MySocketUserType.Admin)
                {
                    //admin
                    var resp = await new AdminLoginSocketHandler().RegisterAndGenerateToken(request, currMySocketReq);

                    currMySocketReq.ChatConnection.IsCustomerOrAdmin = MySocketUserType.Admin;
                    currMySocketReq.ChatConnection.MyAccount = resp.Content;

                    if (currMySocketReq.ChatConnection.MyAccountId != resp.Content.Id)
                        throw new Exception("کد های ادمین برابر نیست2");
                    currMySocketReq.ChatConnection.Token = resp.Token;

                    //currMySocketReq.CurrentRequest = new ParsedCustomerTokenViewModel
                    //{
                    //    baseUrl = currMySocketReq.MyWebsite.BaseUrl,
                    //    myAccountId = currMySocketReq.MySocket.MyAccount.Id,
                    //    websiteId = currMySocketReq.MyWebsite.Id,
                    //    IsAdminOrCustomer = MySocketUserType.Admin,
                    //    dt = DateTime.Now,
                    //};


                }
                else
                {
                    // اگر کاربر باشد او را ثبت نام می کنمیم
                    // customer
                    var rsp = await new CustomerRegisterSocketHandler().RegisterAndGenerateToken(request,
                        currMySocketReq);
                    currMySocketReq.ChatConnection.IsCustomerOrAdmin = MySocketUserType.Customer;
                    currMySocketReq.ChatConnection.Customer = rsp.Temp;

                    currMySocketReq.ChatConnection.Token = rsp.Token;
                }


                currMySocketReq.CurrentRequest = MySpecificGlobal.ParseToken(currMySocketReq.Token);
            }
            else
            {
            }


            onCloseListenerSet( currMySocketReq);


            /// نگه داری کانکشن و اساین فرد به سایت مخصوص خود
            // ما در رم نگه می داریم لذا اینجا ریکوست در رم ثبت می شود
            await WebsiteSingleTon.WebsiteService.AddToOnwSite(currMySocketReq);


            // کد ها به ابکت 
            currMySocketReq = SetObjects(currMySocketReq);

            // یعنی شناخته شده باشد که ادمین است یا کاستومر
            if (currMySocketReq.ChatConnection.MyAccountId.HasValue || currMySocketReq.ChatConnection.CustomerId.HasValue)
            {
                /// این بخش مهم است زیرا ابجکت ها را نیز وصل می کند
                if (currMySocketReq.IsAdminOrCustomer == (int)MySocketUserType.Admin)
                {

                    // ----------------------- online status -----------------------------
                    var myaccount = MyAccountProviderService.GetById(currMySocketReq.ChatConnection.MyAccountId.Value).Single;
                    myaccount.OnlineStatus = OnlineStatus.Online;
                    MyAccountProviderService.VanillaSave(myaccount);
                    // ----------------------- END -----------------------------


                    // خبر دار کردن همه کابران ان سایت از انلاین شدن ادمین جدید
                    await new AnotherSideNewOnlineInformerHandler().InformNewAdminRegistered(
                        currMySocketReq.ChatConnection.MyAccount,
                        currMySocketReq);

                }
                else
                {

                    // ----------------------- online status -----------------------------
                    var customer = currMySocketReq.ChatConnection.Customer;
                    customer.OnlineStatus = OnlineStatus.Online;
                    customerProviderService.Save(customer);
                    // ----------------------- END -----------------------------


                    // خبر دار کردن همه ادمین های ان سایت از انلاین شدن کاستومر جدید
                    await new AnotherSideNewOnlineInformerHandler()
                        .InformNewCustomerRegistered(currMySocketReq.ChatConnection.Customer, currMySocketReq);
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

            if (currMySocketReq.IsAdminOrCustomer == (int)MySocketUserType.Customer)
            {
                var _request = MyWebSocketRequest.Deserialize(request);
                if (_request.Name == "Register")
                {
                    // برای هر کاربر فقط یک بار بخوان

                    try
                    {
                        var ipInfoService = Injector.Inject<IpInfoService>();

                        IpInfoViewModel inforByIp = null;


                        CustomerTrackInfoType trackInfoType = CustomerTrackInfoType.NotDetect;
                        if (currMySocketReq.ChatConnection.Customer.LastTrackInfo == null)
                        {
                            trackInfoType = CustomerTrackInfoType.EnterWebsite;
                        }

                        // اگر به تازگی اطلاعاتش را دریافت نکرده باشیم و ایا اینکه ایپی کاربر تغییر کرده باشد مجددا بخواند
                        if (currMySocketReq.ChatConnection.Customer.LastTrackInfo == null
                            || currMySocketReq.ChatConnection.Customer.LastTrackInfo?.ip != currMySocketReq.ChatConnection?.MyConnectionInfo?.ClientIpAddress)
                        {
                            if (!MyGlobal.IsAttached)
                            {
                                trackInfoType = CustomerTrackInfoType.ChangeIp;

                            }

                            // اطلاعات کاربر را از سایت شخص ثالث می گیرد
                            inforByIp =
                                await ipInfoService.GetInforByIp(currMySocketReq.ChatConnection?.MyConnectionInfo?.ClientIpAddress);
                        }
                        else
                        {
                            if (currMySocketReq.ChatConnection.Customer.LastTrackInfo?.CustomerTrackInfoType ==
                                CustomerTrackInfoType.ExitWebsite)
                            {
                                trackInfoType = CustomerTrackInfoType.ComeBack;
                            }
                            else if (currMySocketReq.ChatConnection.Customer.LastTrackInfo?.Url != _request.Body.URL)
                            {
                                /*------------------------------ is page changed----------------------------*/
                                trackInfoType = CustomerTrackInfoType.PageChange;

                            }
                            else
                            {
                                trackInfoType = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.CustomerTrackInfoType ?? CustomerTrackInfoType.NoChange;
                            }

                            inforByIp = new IpInfoViewModel
                            {

                                city = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.city,
                                continent_code = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.continent_code,
                                continent_name = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.continent_name,
                                ip = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.ip,
                                latitude = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.latitude,
                                longitude = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.longitude,
                                country_code = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.country_code,
                                country_name = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.country_name,
                                region_code = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.region_code,
                                region_name = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.region_name,
                                type = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.type,
                                Region = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.Region,
                                CityName = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.CityName,
                            };
                        }

                        // اطلاعاتی که خودمان موقع رفرش شدن کاربر از او میگیریم
                        var url = _request.Body.URL;
                        var title = _request.Body.Title;
                        var description = _request.Body.Description;


                        /*header infos:*/

                        var browser = currMySocketReq.ChatConnection?.MyConnectionInfo?.Headers["User-Agent"];
                        var language = currMySocketReq.ChatConnection?.MyConnectionInfo?.Headers["Accept-Language"];

                        string countryLanguage = "";
                        /*en-US,en;q=0.9*/
                        if (string.IsNullOrEmpty(language) == false)
                        {
                            try
                            {
                                var parts = language.Split(',').Length > 1
                                    ? language.Split(',')[1].Split('-')
                                    : new[] { language, language };

                                language = parts.ElementAtOrDefault(0);
                                countryLanguage = parts.ElementAtOrDefault(1);
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
                            PrevTrackInfoId = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.Id,
                            PrevTrackInfoDateTime = currMySocketReq.ChatConnection.Customer.LastTrackInfo?.DateTime,

                            TimeSpent = MySpecificGlobal.CalculateTimeSpentOnPage(currMySocketReq.ChatConnection.Customer.LastTrackInfo?.DateTime),
                            TimeSpentNum = MySpecificGlobal.CalculateTimeSpentOnPageNum(currMySocketReq.ChatConnection.Customer.LastTrackInfo?.DateTime),
                            CustomerTrackInfoType = trackInfoType,
                            CustomerId = currMySocketReq.CurrentRequest.customerId.Value,
                            Url = url + "",
                            PageTitle = title + "",
                            Descrition = description + "",
                            CityName = inforByIp.CityName,
                            Region = inforByIp.Region,
                            Time = DateTime.Now.ToString("HH:mm"),
                            TimeDt = DateTime.Now.TimeOfDay,
                            DateTime = DateTime.Now,

                            Id = 0,
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

                        if (currMySocketReq.ChatConnection.Customer.Name?.Contains(" کاربر آنلاین جدید ") == true)
                        {
                            currMySocketReq.ChatConnection.Customer.Name = track.Address;

                           await  customerProviderService.SaveAsync(currMySocketReq.ChatConnection.Customer);
                        }
                        // چون این اطلاعات در رم است می توانیم آن را به این ابجکت بدهیم و نیازی نیست هر بار از دیتابیس بخوانیم
                        currMySocketReq.ChatConnection.Customer.LastTrackInfo = track;
                    }
                    catch (Exception e)
                    {
                        SignalRMVCChat.Service.LogService.Log(e);
                        //todo:log
                        //ignore
                    }
                }
            }

            #endregion


            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        private MyWebSocketRequest SetObjects(MyWebSocketRequest currMySocketReq)
        {
            if (currMySocketReq.ChatConnection.MyAccount == null && currMySocketReq.ChatConnection.Customer == null)
            {
                if (currMySocketReq.IsAdminOrCustomer == (int)MySocketUserType.Admin)
                {
                    currMySocketReq.ChatConnection.MyAccountId = currMySocketReq.CurrentRequest.myAccountId;
                    var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
                    var myEntityResponse = myAccountProviderService.GetById(currMySocketReq.ChatConnection.MyAccountId.Value);

                    currMySocketReq.ChatConnection.MyAccount = myEntityResponse.Single;
                }
                else
                {
                    currMySocketReq.ChatConnection.CustomerId = currMySocketReq.CurrentRequest.customerId;
                    var myAccountProviderService = Injector.Inject<CustomerProviderService>();
                    var myEntityResponse = myAccountProviderService.GetById(currMySocketReq.ChatConnection.CustomerId.Value);

                    currMySocketReq.ChatConnection.Customer = myEntityResponse.Single;
                }
            }


            return currMySocketReq;
        }

        private async Task onCloseListenerSet(MyWebSocketRequest currMySocketReq)
        {

            currMySocketReq.ChatConnection.Hub.OnDisconnect += async (string connectionId) =>
            {
                if (connectionId==currMySocketReq.ChatConnection.SignalRConnectionId)
                {
                    await currMySocketReq.ChatConnection.OnSocketClose(currMySocketReq.MyWebsite, currMySocketReq);
                }
            };
        }

        private async Task Validation(string request, MyWebSocketRequest currMySocketReq)
        {

            var myWebsiteSettingService = Injector.Inject<MyWebsiteSettingService>();

            var myWebsiteSetting = myWebsiteSettingService
                .GetQuery().FirstOrDefault(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);

            // default is true
            bool isLock = myWebsiteSetting?.IsLockToUrl ?? true;
            if (isLock)
            {

                #region url security 
                var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;
                // حساس به آدرس وب سایت استفاده کننده باشد 

                var baseUrl = new Uri(websiteUrl);
                var requestbaseUrl = new Uri(currMySocketReq.ChatConnection.MyConnectionInfo.Origin);
                if (requestbaseUrl.Authority != baseUrl.Authority)
                {
                    throw new Exception(
                        "این وب سایت ثبت نام نشده است لطفا برای استفاده از پلاگین سوئیفت چت ابتدا ثبت نام نموده و سپس پلاگین مخصوص وب سایت دلخواه خود را تعریف نمایید");
                }

                #endregion
            }

        }
    }
}