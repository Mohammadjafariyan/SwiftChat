using NUnit.Framework;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace SignalRMVCChat.Service
{
    public class IpInfoService
    {
        public async Task<IpInfoViewModel> GetInforByIp(string ip)
        {

            var viewModel= await ReadDataByIpAsync(ip);

            return viewModel;
            //return new IpInfoViewModel
            //{
            //    CityName = "تبریز",
            //    Region = "آذربایجان شرقی",
            //};
        }

        public async Task<IpInfoViewModel>  ReadDataByIpAsync(string ip)
        {
            var baseAddress = $@"http://api.ipstack.com/{ip}?access_key=8978667595104e040c446369ef71e1f9&language=fa&output=json";
            using (var client = new HttpClient())
            {
                using (var response = client.GetAsync(baseAddress).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var customerJsonString = await response.Content.ReadAsStringAsync();
                        var cust = JsonConvert.DeserializeObject<IPInfoJsonViewModel>(customerJsonString);

                        return new IpInfoViewModel
                        {
                           Region= cust.country_name+ " "+ cust.region_name,
                           CityName= cust.city,
                        };
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }

            return null;
        }
    }


    public class IPInfoJsonViewModel
    {
        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        //public object? zip { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }

    }

}