using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace TelegramBotsWebApplication.Areas.Admin.MockDatabase
{
    public class MockDb
    {
        
        public static readonly Dictionary<Type,object> Tables =new Dictionary<Type, object>();


        public static List<T> GetTypeTable<T>(List<T> list) where T : class, IEntity, new()
        {
            if (Tables.ContainsKey(typeof(T)))
            {
                return Tables[typeof(T)] as List<T>;
            }
            Tables.Add(typeof(T),list);

            return list;
        }

        public static List<T> MyDb<T>() where T : class, IEntity, new()
        {
            if (Tables.ContainsKey(typeof(T)))
            {
                return Tables[typeof(T)] as List<T>;
            }
            Tables.Add(typeof(T),new List<T>());
            return Tables[typeof(T)] as List<T>;

        }

        public static void SetNewList<T>(List<T> entities) where T : class, IEntity, new()
        {
            if (Tables.ContainsKey(typeof(T)))
            {
                 Tables[typeof(T)]=entities;
                 return;
            }
            Tables.Add(typeof(T),entities);        }
    }
    
   
}