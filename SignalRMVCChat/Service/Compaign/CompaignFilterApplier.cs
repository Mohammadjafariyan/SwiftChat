using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Compaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.Service.Compaign
{
    public class CompaignFilterApplier
    {
        private Customer customer;

        public CompaignFilterApplier(Customer customer)
        {
            this.customer = customer;
        }

        public void IsFilterMatch(SignalRMVCChat.Models.Compaign.Compaignfilter filter)
        {

        }

        public bool IsCityMatch(UserCity userCity)
        {
            if (string.IsNullOrEmpty(userCity?.engName?.ToLower()?.Trim()))
            {
                return false;
            }
            return customer.TrackInfos.Any(t => t.UserCity?.engName?.ToLower()?.Trim() == userCity?.engName?.ToLower()?.Trim());
        }

        public bool IsMatchTexts(Compaignfilter compaignfilter)
        {
            //CompanyName = "com",
            //   EmailAddress = "admin@admin.com",
            //   phoneNumber = "09840",
            //   JobTitle = "545",
            //   fullName = "jaf",
            //   JobName = "sdf",


            if (IsMatchSingleText(compaignfilter.CompanyName, customer.CompanyName
                , compaignfilter.selectedFilter))
                return true;

            if (IsMatchSingleText(compaignfilter.EmailAddress, customer.Email
                , compaignfilter.selectedFilter))
                return true;

            if (IsMatchSingleText(compaignfilter.phoneNumber, customer.Phone
           , compaignfilter.selectedFilter))
                return true;


            if (IsMatchSingleText(compaignfilter.JobTitle, customer.JobTitle
           , compaignfilter.selectedFilter))
                return true;

            if (IsMatchSingleText(compaignfilter.fullName, customer.Name
       , compaignfilter.selectedFilter))
                return true;

            if (IsMatchSingleText(compaignfilter.JobName, customer.JobName
, compaignfilter.selectedFilter))
                return true;

            return false;
        }

        public bool IsMatchSingleText(string textToMatch, string text2
            , NameValue selectedFilter)
        {
            if (selectedFilter?.engName == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(textToMatch) ||
                string.IsNullOrEmpty(text2))
            {
                return false;
            }

            if (selectedFilter?.engName == "equals")
            {
                return textToMatch?.ToLower()?.Trim()
                == text2?.ToLower()?.Trim();
            }
            else if (selectedFilter?.engName == "include")
            {
                return textToMatch?.ToLower()?.Trim()?.IndexOfAny(
               text2?.ToLower()?.Trim().ToCharArray()) != -1;
            }
            else if (selectedFilter?.engName == "notEquals")
            {
                return textToMatch?.ToLower()?.Trim()?.IndexOfAny(
             text2?.ToLower()?.Trim().ToCharArray()) == -1;
            }

            return false;
        }

        public bool IsMatchGender(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(customer?.Gender?.engName))
            {
                return false;
            }

            return compaignfilter?.Gender?.engName == customer?.Gender?.engName;
        }

        public bool IsMatchCustomData(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }
            if (customer.CustomerDatas == null || customer.CustomerDatas?.Count == 0)
            {
                return false;
            }


            foreach (var data in customer.CustomerDatas)
            {
                if (data.Key == compaignfilter.CustomData &&
                    data.Value == compaignfilter.CustomDataValue)
                {
                    return true;
                }
            }

            return false;

        }

        internal string ApplyMatch(Compaignfilter filter)
        {
            bool isMatch = filter?.city?.Any(city => IsCityMatch(city)) == true;

            if (isMatch)
                return "شهر ";

            #region 

            isMatch = IsMatchTexts(filter);
            if (isMatch)
                return "اطلاعات و متون";
            #endregion


            #region 

            isMatch = IsMatchDates(filter);
            if (isMatch)
                return "تاریخ";
            #endregion


            #region 

            isMatch = IsMatchGender(filter);
            if (isMatch)
                return "جنسیت";
            #endregion


            #region 

            isMatch = IsMatchCustomData(filter);
            if (isMatch)
                return "داده سفارشی";
            #endregion


            #region 

            isMatch = IsMatchLanguage(filter);
            if (isMatch)
                return "زبان";
            #endregion

            #region 

            isMatch = IsMatchCountry(filter);
            if (isMatch)
                return "کشور";

            #endregion


            #region 

            isMatch = IsMatchWeekDays(filter);
            if (isMatch)
                return "روز هفته";
            #endregion



            #region 

            isMatch = IsMatchStates(filter);
            if (isMatch)
                return "استان";
            #endregion




            #region 

            isMatch = IsMatchVoted(filter);
            if (isMatch)
                return "آیا رای داده است";

            #endregion


            #region 
            isMatch = IsMatchTags(filter);
            if (isMatch)
                return "برچسب ها";
            #endregion


            return isMatch ? "برابر" : null;
        }


        public bool IsMatchLanguage(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }

            var trackInfo = customer.TrackInfos?
                .FirstOrDefault(f => string.IsNullOrEmpty(f.country_name) == false);
            return compaignfilter.country?.Name?.ToLower()?.Trim() == trackInfo?.country_name?.ToLower()?.Trim();
        }

        public bool IsMatchCountry(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }

            var trackInfo = customer.TrackInfos?
                .FirstOrDefault(f => string.IsNullOrEmpty(f.country_name) == false);
            return compaignfilter.country?.Name?.ToLower()?.Trim() == trackInfo?.country_name?.ToLower()?.Trim();

        }

        public bool IsMatchWeekDays(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }

            var today = DateTime.Now.Date;


            var todayWeekDayNum = MyGlobal.WeekNums[today.DayOfWeek];

            return compaignfilter.weekdays?.Any(w => w.code == todayWeekDayNum) == true;


        }

        public bool IsMatchStates(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null ||
                compaignfilter.region == null)
            {
                return false;
            }
            if (compaignfilter.region.Any() == false)
            {
                return false;
            }

            var anyNotNullOnlineTrack = customer.TrackInfos
                .FirstOrDefault(c => c.UserState != null);
            if (anyNotNullOnlineTrack == null || anyNotNullOnlineTrack
                .DateTime.HasValue == false)
            {
                return false;
            }


            return compaignfilter.region?.Any(c => c.engName?.ToLower()?.Trim() ==
            anyNotNullOnlineTrack.UserState?.engName?.ToLower()?.Trim()) == true;

        }

        public bool IsMatchVoted(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }

            if (customer?.RatingCount == null)
            {
                return false;
            }

            return customer?.RatingCount?.Any() == true;
        }

        public bool IsMatchTags(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }

            if (customer?.CustomerTags == null)
            {
                return false;
            }


            return customer.CustomerTags?.Any(
                c => compaignfilter.segments.Any(s => StringMatch(c.Tag?.Name, s?.Name) == true)) == true;

        }

        public static bool StringMatch(string s1, string s2)
        {
            bool isMatch = s1?.ToLower()?.Trim() ==
            s2?.ToLower()?.Trim();

            return isMatch;
        }

        public bool IsMatchDates(Compaignfilter compaignfilter)
        {
            if (compaignfilter?.selectedFilter?.engName == null)
            {
                return false;
            }


            DateTime? lastActiveDate = null;
            if (string.IsNullOrEmpty(compaignfilter.lastActiveDate) == false)
            {
                try
                {
                    lastActiveDate = MySpecificGlobal.ParseDate(compaignfilter.lastActiveDate);
                }
                catch (Exception e)
                {
                    lastActiveDate = null;
                }
            }

            DateTime? creationDate = null;
            if (string.IsNullOrEmpty(compaignfilter.creationDate) == false)
            {
                try
                {
                    creationDate = MySpecificGlobal.ParseDate(compaignfilter.creationDate);
                }
                catch (Exception e)
                {
                    creationDate = null;
                }

            }



            if (lastActiveDate.HasValue)
            {

                var lastOnlineTrack = customer.TrackInfos.OrderByDescending(o => o.Id).FirstOrDefault();
                if (lastOnlineTrack == null || lastOnlineTrack.DateTime.HasValue == false)
                {
                    return false;
                }
                return lastOnlineTrack.DateTime.Value.Date == creationDate.Value.Date;

            }


            if (creationDate.HasValue)
            {
                return customer.CreationDateTime.Date == creationDate.Value.Date;
            }

            return false;
        }
    }
}