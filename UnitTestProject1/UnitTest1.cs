using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalRMVCChat.Service;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class IpInfoServiceTests
    {

        [TestMethod]
        public async Task ReadInfoTest()
        {
            var service = new IpInfoService();

           var IpInfoViewModel= await service.ReadDataByIpAsync("46.235.76.7");

            Console.WriteLine(IpInfoViewModel.Region);
            Console.WriteLine(IpInfoViewModel.CityName);

            Assert.IsTrue(IpInfoViewModel.CityName.Contains("Tabriz"));
        }

    }
}
