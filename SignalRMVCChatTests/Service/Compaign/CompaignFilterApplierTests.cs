using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Service.Compaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.Service.Compaign.Tests
{

    [TestClass()]
    public class CompaignFilterApplierTests
    {
        [TestMethod()]
        public void IsFilterMatchTest()
        {

            var customerService = new CustomerProviderService();


            var trackingService = new CustomerTrackerService();

            var customer = new Customer
            {

                Email = "admin@admin.com",
                CreationDateTime = new DateTime(2019, 7, 2),
                Gender = new NameValue
                {
                    engName = "man"
                },
                RatingCount = new Dictionary<int, int>
                 {
                     {0,0 },
                     {1,1 },
                     {2,2 },
                     {3,3 },
                     {4,4 },
                     {5,5 },
                     {6,6 },
                 },
                CustomerDatas = new List<CustomerData>
                 {
                     new CustomerData
                     {
                         Key="color",
                         Value="red"
                     }
                 },


            };

            customer.CustomerTags = new List<CustomerTag>
            {
                new CustomerTag
                {
                    Tag=new Tag
                    {
                        Name="sampleTag"
                    }
                }
            };
            var tracks = new List<CustomerTrackInfo>();

            tracks.Add(new CustomerTrackInfo
            {
                UserCity = new UserCity
                {
                    engName = "Tabriz",
                    name = "تبریز"
                },
                UserState = new UserState
                {
                    engName = "East Azerbaijan",
                    name = "آذربایجان شرقی"

                },
                DateTime = new DateTime(2019, 7, 2),
                country_name = "Afghanistan"
            });

            customer.TrackInfos = tracks;

            var service = new CompaignFilterApplier(customer);

            bool isMatch = service.IsCityMatch(new UserCity
            {
                engName = "Tabriz",
                name = "تبریز"
            });

            Assert.IsTrue(isMatch);

            #region 

            isMatch = service.IsMatchTexts(new Compaignfilter
            {
                CompanyName = "com",
                EmailAddress = "admin@admin.com",
                phoneNumber = "09840",
                JobTitle = "545",
                fullName = "jaf",
                JobName = "sdf",
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion


            #region 

            isMatch = service.IsMatchDates(new Compaignfilter
            {
                creationDate = "2019/07/02 00:00:00",
                lastActiveDate = "2019/07/02",
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion


            #region 

            isMatch = service.IsMatchGender(new Compaignfilter
            {
                Gender = new NameValue()
                {
                    engName = "man"
                },
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion


            #region 

            isMatch = service.IsMatchCustomData(new Compaignfilter
            {
                CustomData = "color",
                CustomDataValue = "red",
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion


            #region 

            isMatch = service.IsMatchLanguage(new Compaignfilter
            {
                country = new Models.HelpDesk.Language
                {
                    Name = "Afghanistan"
                }
                ,
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion

            #region 

            isMatch = service.IsMatchCountry(new Compaignfilter
            {
                country = new Models.HelpDesk.Language
                {
                    Name = "Afghanistan"
                }
                ,
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion


            #region 

            isMatch = service.IsMatchWeekDays(new Compaignfilter
            {
                weekdays = new List<WeekNameCode>
                {
                    new WeekNameCode
                    {
                        code=MyGlobal.WeekNums[DateTime.Now.DayOfWeek]
                    }
                }
                ,
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion



            #region 

            isMatch = service.IsMatchStates(new Compaignfilter
            {
                region = new List<UserState>
                {
                    new UserState
                    {
                          engName = "East Azerbaijan",
                name = "آذربایجان شرقی"
                    }
                }
                ,
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion




            #region 

            isMatch = service.IsMatchVoted(new Compaignfilter
            {
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion


            #region 

            isMatch = service.IsMatchTags(new Compaignfilter
            {
                segments = new List<Tag>
                {
                    new Tag
                    {
                        Name="sampleTag"
                    }
                },
                selectedFilter = new NameValue()
                {
                    engName = "equals"
                }
            });

            Assert.IsTrue(isMatch);
            #endregion
        }



        [TestMethod()]
        public void IsMatchSingleText()
        {
            var customer = new Customer
            {

                Email = "admin@admin.com"
            };

            var service = new CompaignFilterApplier(customer);

            Assert.IsTrue(service.IsMatchSingleText("sdf", "65", new NameValue
            {
                engName = "notEquals"
            }));

            Assert.IsTrue(service.IsMatchSingleText("65", "65", new NameValue
            {
                engName = "equals"
            }));
            Assert.IsTrue(service.IsMatchSingleText("sdf", "vssdfsdfwe", new NameValue
            {
                engName = "include"
            }));

        }
    }
}



//   نام کامل  " role="option" aria-selected="false">
//نام کامل</li><li class="p-dropdown-item" aria-label="
//به کاربران زن یا کاربران مرد  " role="option" aria-selected="false">
//به کاربران زن یا کاربران مرد</li><li class="p-dropdown-item" aria-label="
//با داده های سفارشی  " role="option" aria-selected="false">
//با داده های سفارشی</li><li class="p-dropdown-item" aria-label="
//شماره تلفن " role="option" aria-selected="false">
//شماره تلفن</li><li class="p-dropdown-item" aria-label="
//زبان " role="option" aria-selected="false">
//زبان</li><li class="p-dropdown-item" aria-label="
//با کشور  " role="option" aria-selected="false">
//با کشور</li><li class="p-dropdown-item" aria-label="
//روز های هفته " role="option" aria-selected="false">
//روز های هفته</li><li class="p-dropdown-item" aria-label="
//به کاربران یک یا چند استان  " role="option" aria-selected="false">
//به کاربران یک یا چند استان</li><li class="p-dropdown-item" aria-label="
// به کاربران یک یا چند شهر" role="option" aria-selected="false">
// به کاربران یک یا چند شهر</li><li class="p-dropdown-item" aria-label="
//عنوان شغلی" role="option" aria-selected="false">
//عنوان شغلی</li><li class="p-dropdown-item" aria-label="
//با آخرین تاریخ فعالیت  " role="option" aria-selected="false">
//با آخرین تاریخ فعالیت</li><li class="p-dropdown-item" aria-label="
//تاریخ ایجاد " role="option" aria-selected="false">
//تاریخ ایجاد</li><li class="p-dropdown-item" aria-label="
//امتیاز داده" role="option" aria-selected="false">
//امتیاز داده</li><li class="p-dropdown-item" aria-label="
//برچسب ها " role="option" aria-selected="false">
//برچسب ها</li><li class="p-dropdown-item" aria-label="
//نام شرکت" role="option" aria-selected="false">
//نام شرکت</li></ul>
/// </summary>