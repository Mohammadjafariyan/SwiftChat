namespace SignalRMVCChat.Service
{
    public class IpInfoViewModel
    {
        public string CityName { get; set; }
        public string Region { get; set; }
        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}