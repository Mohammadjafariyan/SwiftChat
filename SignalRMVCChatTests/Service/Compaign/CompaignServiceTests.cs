using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class CompaignServiceTests
    {
        //[TestMethod()]
        //public void CompaignServiceTest()
        //{
        //    var service = new CompaignService();
        //                               IQueryable<SignalRMVCChat.Models.Compaign.Compaign> list = service.GetConfiguredCompagins(1);


        //    Assert.IsTrue(list.Count() > 0);
        //    Assert.IsTrue(list.Any(l => l.IsConfigured && l.IsAutomatic));



        //    var conditions= service.AutomaticCondition(list.ToList(),1);


        //    service.ExecuteCompagins(list);


        //}


        //[TestMethod()]
        //public void ApplyfilterTest()
        //{

        //    MyDependencyResolver.RegisterDependencies();

        //    var service = new CompaignService();


        //    var comp= service.GetQuery().First();

        //    string matchedLog=service.ApplyFilter(comp,1);

        //    Assert.IsNotNull(matchedLog);

        //}


    }
}