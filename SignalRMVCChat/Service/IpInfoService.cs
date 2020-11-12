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

            if (System.Diagnostics.Debugger.IsAttached)
            {
                ip = "84.47.254.12";
            }
            
            var viewModel= await ReadDataByIpAsync(ip);

            return viewModel;
            //return new IpInfoViewModel
            //{
            //    CityName = "تبریز",
            //    Region = "آذربایجان شرقی",
            //};
        }

        /*ip: "74.82.60.191"
type: "ipv4"
continent_code: "NA"
continent_name: "North America"
country_code: "US"
country_name: "United States"
region_code: "CA"
region_name: "California"
city: "Milpitas"
zip: 94539
latitude: 37.52878952026367
longitude: -121.91031646728516*/
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
                           ip=ip,
                           type=cust.type,
                           continent_code=cust.continent_code,
                           continent_name=cust.continent_name,
                           country_code=cust.country_code,
                           country_name=cust.country_name,
                           region_code=cust.region_code,
                           region_name=cust.region_name,
                           city=cust.city,
                           latitude=cust.latitude,
                           longitude=cust.longitude,
                        };
                    }
                    else
                    {
                        // console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
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