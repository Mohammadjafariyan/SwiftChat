﻿using Newtonsoft.Json;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class MyWebSocketResponse
    {
        public string Message { get; set; }
        public MyWebSocketResponseType Type { get; set; }
        public dynamic Content { get; set; }
        public string Name { get; set; }
        public string TokenAdmin { get; set; }
        public string Token { get; set; }
        public Customer Temp { get; set; }


        public T Deserialize<T>()
        {
           return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Content));
        }
        
        /// <summary>
        ///  اطلاعات گشت و گذار جایگاه کنونی کاربر در سایت 
        /// </summary>
        public CustomerTrackInfo TrackInfo { get; set; }

        public string Serilize()
        {
            string json= Newtonsoft.Json.JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return json;
        }

        public static MyWebSocketResponse Parse(string s)
        {
            return JsonConvert.DeserializeObject<MyWebSocketResponse>(s);
        }
    }
}