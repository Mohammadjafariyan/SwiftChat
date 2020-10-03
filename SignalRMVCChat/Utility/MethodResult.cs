using SignalRMVCChat.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.Utility
{
    public class MethodResult
    {
        private string _message;
        public ResultCode ResultCode { get; set; }

        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                {
                    return ResultCode.GetDisplayName();
                }
                else
                {
                    return _message;
                }
            }
            set => _message = value;
        }



        public object Result { get; set; }


    }
}