using System;
using System.Diagnostics;
using SignalRMVCChat.Areas.sysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Log:Entity
    {

        public Log()
        {
            
        }
        public Log(Exception e,string data=null)
        {
            CreationDateTime = DateTime.Now;
            SessionDateTime = DateTime.Now;
            Exception = MyGlobal.RecursiveExecptionMsg(e);

            Data = data;
            
            var trace = new StackTrace(e, true);

            // Get the top stack frame
            var frame = trace.GetFrame(0);

            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();

            File=frame.GetFileName();
            LineNumber=line+"";
        }

        public Log(int count, string msg, int lineNumber, string caller)
        {
            this.Data = msg;
            this.LineNumber = lineNumber.ToString();
            this.File = caller;
            this.Step = count;
        }

        public int Step { get; set; }

        public string Data { get; set; }

        public DateTime? CreationDateTime { get; set; }


        public string Exception { get; set; }
        public string File { get; set; }

        public string LineNumber { get; set; }
        public DateTime? SessionDateTime { get; set; }
    }
}