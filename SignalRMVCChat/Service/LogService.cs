using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.CompilerServices;
using Engine.SysAdmin.Service;
using NUnit.Framework;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class LogService:GenericService<Log>
    {

        private List<Log> CurrentSessionLogs = new List<Log>();
        
        public LogService() : base(null)
        {
        }
        public  void Save( )
        {
            this.Impl.Save(CurrentSessionLogs);
            
        }

       
        public  void LogFunc( string msg,  [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null,[CallerFilePath] string file=null)
        {
            var log = new Log(CurrentSessionLogs.Count+1, msg,lineNumber,file+"->"+caller);

            if (CurrentSessionLogs.Count==0)
            {
                SessionDateTime = DateTime.Now;
                log.SessionDateTime = DateTime.Now;
            }
            else
            {
                log.SessionDateTime = SessionDateTime;


            }
            
            
            CurrentSessionLogs.Add(log);
        }

        public DateTime SessionDateTime { get; set; }

        public static void Log(Exception e, string data=null)
        {
            var log = new Log(e,data);
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                db.Logs.Add(log);


                var queryForDelete = db.Logs
                    .Where(c => EntityFunctions.DiffDays(c.CreationDateTime, DateTime.Today) > 0);

                db.Logs.RemoveRange(queryForDelete);
                db.SaveChanges();
            }
        }
    }
}