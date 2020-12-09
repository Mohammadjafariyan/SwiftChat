using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Engine.SysAdmin.Models;
using Fleck;
using Newtonsoft.Json;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Models
{
    public class MySpecificGlobal
    {
        public static string GetBaseUrl(Uri Url)
        {
            return Url.Scheme + "://" + Url.Host + ":" + Url.Port;
        }

        public static string GetBaseUrl(string Url)
        {
            var uri = new Uri(Url);
            string path = uri.GetLeftPart(UriPartial.Path);

            return path;
        }



        public static string GetConnectionString()
        {

            if (MyGlobal.IsUnitTestEnvirement)
            {
                return "DefaultConnectionDebug";

            }


            return
                 SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached ? "DefaultConnectionDebug"
                     : "DefaultConnection";
        }

        public static T Clone<T>(T t)
        {
            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            });

            return JsonConvert.DeserializeObject<T>(json);
        }


        public static string CreateTokenForCustomer(string baseUrl, int mySocketCustomerId, int mySocketWebsiteId)
        {
            return CreateTokenHelper(baseUrl, mySocketCustomerId, "customer", mySocketWebsiteId, 2);
        }
        public static string CreateTokenForAdmin(string baseUrl, int mySocketMyAccountId, int mySocketWebsiteId)
        {
            return CreateTokenHelper(baseUrl, mySocketMyAccountId, "admin", mySocketWebsiteId, 1);
        }

        /// <summary>
        /// admin=1
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="mySocketCustomerId"></param>
        /// <param name="name"></param>
        /// <param name="mySocketWebsiteId"></param>
        /// <param name="adminOrCustomer"></param>
        /// <returns></returns>
        private static string CreateTokenHelper(string baseUrl, int mySocketCustomerId, string name, int mySocketWebsiteId, int adminOrCustomer)
        {
            return EncryptionHelper.Encrypt($"{DateTime.Now}_{baseUrl}_{mySocketCustomerId}_{name}_{mySocketWebsiteId}_{adminOrCustomer}");
        }



        public static int ValidateAdminToken(string adminToken)
        {
            var decrypt = EncryptionHelper.Decrypt(adminToken);

            return 0;

        }

        public static int ValidateToken(string token)
        {
            return 0;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string GenerateWebsiteAdminToken(MyWebsite single)
        {
            var formattableString = $@"{single.Id}_admin_{DateTime.Now}";
            var encrypt = EncryptionHelper.Encrypt(formattableString);
            return Base64Encode(encrypt);
        }



        public static ParsedCustomerTokenViewModel ParseToken(string token)
        {
            try
            {

                var decrypt = EncryptionHelper.Decrypt(token);
                var dt = DateTime.Parse(decrypt.Split('_')[0]);
                var baseUrl = decrypt.Split('_')[1];
                var targetId = int.Parse(decrypt.Split('_')[2]);
                var websiteId = int.Parse(decrypt.Split('_')[4]);
                var type = int.Parse(decrypt.Split('_')[5]) == 1 ? MySocketUserType.Admin : MySocketUserType.Customer;

                return new ParsedCustomerTokenViewModel
                {
                    dt = dt,
                    baseUrl = baseUrl,
                    customerId = type == MySocketUserType.Customer ? targetId : (int?)null,
                    myAccountId = type == MySocketUserType.Admin ? targetId : (int?)null,
                    websiteId = websiteId,
                    IsAdminOrCustomer = type
                };
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                return null;
            }
        }

        public static DateTime ParseDateTime(string value)
        {
            try
            {
                string format = "DD/MM/YYYY HH:MM:SS";

                var datePart = value.Split(' ')[0];
                var timePart = value.Split(' ')[1];


                var day = datePart.Split('/')[0];
                var mon = datePart.Split('/')[1];
                var year = datePart.Split('/')[2];



                var hour = timePart.Split(':')[0];
                var minutes = timePart.Split(':')[1];
                var second = timePart.Split(':')[2];


                return new DateTime(int.Parse(year), int.Parse(mon), int.Parse(day), int.Parse(hour), int.Parse(minutes), int.Parse(second));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("فرم تاریخ صحیح نیست");
            }


        }

        public static TimeSpan ParseTime(string botConditionTimeFrom)
        {
            try
            {
                string format = "2020/11/12 14:36:28";

                botConditionTimeFrom = botConditionTimeFrom.Split(' ')[1];

                /*botConditionTimeFrom=  botConditionTimeFrom.Substring(0
                    , botConditionTimeFrom.IndexOf(" "));*/
                var hour = botConditionTimeFrom.Split(':')[0];
                var minutes = botConditionTimeFrom.Split(':')[1];
                var second = botConditionTimeFrom.Split(':')[2];


                return new TimeSpan(int.Parse(hour), int.Parse(minutes), int.Parse(second));
            }
            catch (Exception e)
            {
                throw new Exception("تایم قابل پردازش نیست");
            }
        }


        public static DateTime ParseDate(string botConditionTimeFrom)
        {
            try
            {
                string format = "2020/11/12 14:36:28";

                if (botConditionTimeFrom.Contains(" "))
                {
                    botConditionTimeFrom = botConditionTimeFrom.Split(' ')[0];

                    /*botConditionTimeFrom=  botConditionTimeFrom.Substring(0
                        , botConditionTimeFrom.IndexOf(" "));*/
                    var year = botConditionTimeFrom.Split('/')[0];
                    var mont = botConditionTimeFrom.Split('/')[1];
                    var day = botConditionTimeFrom.Split('/')[2];


                    return new DateTime(int.Parse(year), int.Parse(mont), int.Parse(day));

                }
                else
                {
                    var year = botConditionTimeFrom.Split('/')[0];
                    var mont = botConditionTimeFrom.Split('/')[1];
                    var day = botConditionTimeFrom.Split('/')[2];


                    return new DateTime(int.Parse(year), int.Parse(mont), int.Parse(day));

                }



            }
            catch (Exception e)
            {
                throw new Exception("تایم قابل پردازش نیست");
            }
        }

        public static void OnControllerException(ExceptionContext filterContext, ViewDataDictionary ViewData)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            // _Logger.Error(filterContext.Exception);
            string msg = MyGlobal.RecursiveExecptionMsg(filterContext.Exception);
            // OR 
            var vm = new ViewDataDictionary(filterContext.Controller.ViewData)
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

        public static string CalculateTimeSpentOnPage(DateTime? dateTime)
        {
            if (dateTime.HasValue == false)
            {
                return null;
            }

            var diff = DateTime.Now.Subtract(dateTime.Value);
            return
                $@"{diff.Hours}:{diff.Minutes}:{diff.Seconds}";
        }

        public static double? CalculateTimeSpentOnPageNum(DateTime? dateTime)
        {
            if (dateTime.HasValue == false)
            {
                return null;
            }

            return Math.Round(DateTime.Now.Subtract(dateTime.Value).TotalMinutes);
        }

        public static string GenerateUploadFileNameUniqPath()
        {
            var fileName = GenerateName(10);

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/upload/" + fileName);
            while (File.Exists(path))
            {
                fileName = GenerateName(10);
                path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/upload/" + fileName);
            }

            return path;
        }
        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }
    }
}