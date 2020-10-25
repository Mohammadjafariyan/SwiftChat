using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using MD.PersianDateTime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalRMVCChat.Areas.security.Models;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Areas.sysAdmin.Service
{
    public static class UrlExtensions
    {
        public static string SetUrlParameter(this string url, string paramName, string value)
        {
            return new Uri(url).SetParameter(paramName, value).ToString();
        }

        public static Uri SetParameter(this Uri url, string paramName, string value)
        {           
            var queryParts = HttpUtility.ParseQueryString(url.Query);
            queryParts[paramName] = value;
            return new Uri(url.AbsoluteUriExcludingQuery() + '?' + queryParts.ToString());
        }

        public static string AbsoluteUriExcludingQuery(this Uri url)
        {
            return url.AbsoluteUri.Split('?').FirstOrDefault() ?? String.Empty;
        }
    }
    public class MyGlobal
    {

        public static bool IsAttached
        {
            get
            {
               // return false;
                return Debugger.IsAttached;
            }
        }

        public static Color ContrastColor(string color)
        {
            Color iColor=Color.FromName(color);   
            ;
            // Calculate the perceptive luminance (aka luma) - human eye favors green color... 
            double luma = ((0.299 * iColor.R) + (0.587 * iColor.G) + (0.114 * iColor.B)) / 255;

            // Return black for bright colors, white for dark colors
            return luma > 0.5 ? Color.Black : Color.White;
        }
        public static object GetPropValue(object src, string propName)
        {
            
            var json=Newtonsoft.Json.JsonConvert.SerializeObject(src);
            if (JObject.Parse(json)?[propName]==null)
            {
                throw new Exception("src.GetType().GetProperty(propName) is null");
            }
            return JObject.Parse(json)[propName];
        }

        public static DateFromToDateViewModel ParseDates(string dateFrom,
            string dateTo)
        {
            DateTime? DateFrom=null;
            DateTime? DateTo=null;
            if (string.IsNullOrEmpty(dateFrom)==false)
            {
                DateFrom=  MyGlobal.ParseIranianDate(dateFrom);
            }
            if (string.IsNullOrEmpty(dateTo)==false)
            {
                DateTo=  MyGlobal.ParseIranianDate(dateTo).AddDays(1);
            }

            return new DateFromToDateViewModel
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
            };
        }
        public static MyDataTableResponse<T> Paging<T>(IQueryable<T> queryable, int? take, int? skip) where T : Entity
        {
            int total = queryable.Count();

            int page = skip ?? 1;
            
            take = take ?? 1;
            if (skip.HasValue && skip > 0)
            {
                skip--;
                queryable = queryable.OrderByDescending(e => e.Id).Skip(skip.Value).Take(take.Value);
            }
            else
            {
                queryable = queryable.OrderByDescending(e => e.Id).Take(take.Value);
            }

            return new MyDataTableResponse<T>
            {
                Total = total,
                EntityList = queryable.ToList(),
                LastTake = take.Value,
                LastSkip = page
            };
        }

        public static readonly Dictionary<DayOfWeek, string> WeekNames = new Dictionary<DayOfWeek, string>
        {
            {DayOfWeek.Saturday, "شنبه"},
            {DayOfWeek.Sunday, "یکشنبه"},
            {DayOfWeek.Monday, "دوشنبه"},
            {DayOfWeek.Tuesday, "سه شنبه"},
            {DayOfWeek.Wednesday, "چهار شنبه"},
            {DayOfWeek.Thursday, "پنج شنبه"},
            {DayOfWeek.Friday, "جمعه"},
        };

        public static string ToIranianDate(DateTime mCdate, bool withDayName = false, bool noYear = false)
        {
            var pc = new PersianCalendar();
            string dayName = "";
            if (withDayName)
            {
                dayName = "  " + WeekNames[pc.GetDayOfWeek(mCdate)];
            }

            if (noYear)
            {
                return $@"{pc.GetMonth(mCdate)}-{pc.GetDayOfMonth(mCdate)}" + dayName;
            }

            var month = pc.GetMonth(mCdate);
            string showMonth = "";
            if (month < 10)
            {
                showMonth += "0" + month;
            }
            else
            {
                showMonth = month + "";
            }

            var day = pc.GetDayOfMonth(mCdate);
            string showDate = "";
            if (day < 10)
            {
                showDate += "0" + day;
            }
            else
            {
                showDate = day + "";
            }

            return $@"{pc.GetYear(mCdate)}-{showMonth}-{showDate}" + dayName;
        }

        public static string ToIranianDateWidthTime(DateTime mCdate)
        {
            var pc = new PersianCalendar();
            var month = pc.GetMonth(mCdate);
            string showMonth = "";
            if (month < 10)
            {
                showMonth += "0" + month;
            }
            else
            {
                showMonth = month + "";
            }

            return
                $@"{mCdate.TimeOfDay.Hours}:{mCdate.TimeOfDay.Minutes}:{mCdate.TimeOfDay.Seconds} {pc.GetYear(mCdate)}-{showMonth}-{pc.GetDayOfMonth(mCdate)}";
        }

        public static DateTime ParseIranianDate(string modelFromDate)
        {
            try
            {
                return PersianDateTime.Parse(modelFromDate).ToDateTime();
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                throw new Exception("فرمت تاریخ اشتباه است و قاoldListل نیست");
            }
        }

        public const string SecurityContextName = "security";

        public static string GetBaseUrl(dynamic Url)
        {
            return Url.Scheme + "://" + Url.Host + ":" + Url.Port;
        }

        /*public static string ExtractChannelIds(IQueryable<SocialChannel> socialChannels)
        {
            string channelIds = string.Join(",", socialChannels
                .Select(s => s.ChatTitle + "_" + s.ChatId + "_" + s.ChannelType + "_" + s.Id).ToList());

            return channelIds;
        }*/
        public static string RecursiveExecptionMsg(Exception e)
        {
            string msg = null;
            Exception e2 = e;
            while (e2 != null)
            {
                msg += e2.Message;
                e2 = e2.InnerException;
            }

            return msg;
        }

        public static string ExtractUniqueNameForHandler(string callbackQueryData)
        {
            return callbackQueryData.Split('_')[0];
        }

        public static string ExtractValueInlineQuery(string idstr)
        {
            return idstr.Split('_')[1];
        }

        public static DateTime CreateDateFromTime(int year, int month, int day, DateTime time)
        {
            return new DateTime(year, month, day, time.Hour, time.Minute, 0);
        }

        public static int ValidateHash(string hash)
        {
            string userIdstr = EncryptionHelper.Decrypt(hash);

            userIdstr = userIdstr.Split('_')[0];

            int userId = int.Parse(userIdstr);
            return userId;
        }

        public static string Encrypt(string txt)
        {
            var now = txt + "_" + DateTime.Now;
            return EncryptionHelper.Encrypt(now);
        }


        public static string SplitAndGetRest(string str, string tosub)
        {
            var i = str.IndexOf(tosub, StringComparison.CurrentCulture);

            var start = i + tosub.Length;
            var length = str.Length - start;
            return str.Substring(start, length);
        }

        public static string GetTelegramChatId(string address)
        {
            return MyGlobal.SplitAndGetRest(address, "t.me/");
        }

        public static void Log(Exception exception)
        {
            try
            {
                /*using (var db=ContextFactory.GetContext(null))
                {
                     db.Logs.Add(new Log
                    {
                        Exception = MyGlobal.RecursiveExecptionMsg(exception)
                    });
                    db.SaveChanges();
                }*/
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                // ignored
            }
        }

        public static bool IsUnitTestEnvirement = false;
        public static bool IsReactWebTesting = true;

        public static T Clone<T>(T feed)
        {
            var json = JsonConvert.SerializeObject(feed,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T2 CloneTo<T, T2>(T feed)
        {
            var json = JsonConvert.SerializeObject(feed,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return JsonConvert.DeserializeObject<T2>(json);
        }

        public static string ShowThreeDotMoney(string price)
        {
            var str = price;

            List<string> splited = new List<string>();
            var c = 0;
            string threeNum = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (c == 3)
                {
                    splited.Add(string.Join("", threeNum.Reverse().ToArray()));
                    c = 0;
                    threeNum = "";
                }

                c++;
                threeNum += str[i];
            }

            if (c > 0)
            {
                splited.Add(string.Join("", threeNum.Reverse().ToArray()));
            }

            splited.Reverse();


            var answer = string.Join(",", splited.ToArray());

            return answer;
        }

        public static List<long> GetChannelIds(string DeliveredChannelIds)
        {
            if (string.IsNullOrEmpty(DeliveredChannelIds))
            {
                throw new Exception("این پست به هیچ کانال ارسال نشده است");
            }

            var strings = DeliveredChannelIds.Split(',');

            var list = strings.Select(s =>
            {
                if (s.Contains("_"))
                {
                    var tmp = s.Split('_')[1];
                    return long.Parse(tmp);
                }
                else
                {
                    return long.Parse(s);
                }
            }).ToList();
            return list;
        }


        public static RegisterViewModel MakeFakeUser()
        {
            List<string> names = new List<string>
            {
                "بهادر", "علی", "اصغر", "سجاد", "محمد", "میلاد", "مسعود", "نرگس", "نفیسه", "مراد", "علی", "عباس"
            };
            List<string> emailNames = new List<string>
            {
                "bahador", "ali", "asgar", "sajad", "mohammad", "milad", "masud", "narges", "nafise", "morad", "aliaba",
                "abbas"
            };

            string password = GetRandom(emailNames) + "@sdfsf";
            return new RegisterViewModel
            {
                Email = GetRandom(emailNames) + "@admin.com",
                Password = password,
                Name = GetRandom(emailNames),
                LastName = GetRandom(emailNames) + "زاده",
                ConfirmPassword = password
            };
        }

        public static T GetRandom<T>(List<T> arr)
        {
            Random random = new Random();
            int start2 = random.Next(0, arr.Count);

            return arr[start2];
        }

        public static string GetSummary(string body)
        {
            string res = "";
            if (string.IsNullOrEmpty(body))
                return "";
            if (body.Length > 50)
            {
                res = body.Substring(0, 50);

                return res + "...";
            }

            return body;
        }


        public static MyDataTableResponse<T> Paging<T>(IQueryable<T> queryable, NameValueCollection requestParams)
            where T : Entity
        {
            int skip = 0;
            int take = 0;
            int.TryParse(requestParams["skip"]?.ToString() ?? "", out skip);
            int.TryParse(requestParams["take"]?.ToString() ?? "", out take);


            int? _skip;
            int? _take;
            if (skip == 0)
            {
                _skip = null;
            }
            else
            {
                _skip = skip;
            }

            if (take == 0)
            {
                _take = null;
            }
            else
            {
                _take = take;
            }

            return Paging<T>(queryable, _take, _skip);
        }

        public static string SplitArr(string data)
        {
           var arr= Enumerable.Range(0, data.Length / 30 + (data.Length % 30 >0 ? 1 : 0))
                .Select(i => data.Substring(i * 30, data.Length >i * 30+30 ? 30 : data.Length -i * 30)).ToList();
          return string.Join("<br/>",arr);
        }

        public static bool IsUnitTestEnvirementNoSeed = false;
        public static string Version = "0.0.1." + (VersionPublishDateTime.HasValue ?  MyGlobal.ToIranianDateWidthTime(VersionPublishDateTime.Value) : "") ;
        public static DateTime? VersionPublishDateTime = null ;
    }

    public class DateFromToDateViewModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }


    public static class MyExtentions
    {
        public static T Convert<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if(converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }
    }
}