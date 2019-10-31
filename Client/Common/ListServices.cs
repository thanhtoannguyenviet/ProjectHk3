using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Common
{
    public class ListServices
    {
        public static Dictionary<int, string> ServiceDic = new Dictionary<int, string>()
        {
            {4500,"In-bound"},
            {6500,"Out-bound"},
            {5500,"Tele Marketing"},
        };
        public ListServices()
        {
            ServiceDic = ServiceDic;
        }
        public static string GetValue(int? TKey)
        {
            return ServiceDic.FirstOrDefault(x => x.Key == TKey).Value;
        }
        public static int GetKey(string TValue)
        {
            return ServiceDic.FirstOrDefault(x => x.Value == TValue).Key;
        }
        public static List<string> LsDepartMent = new List<string>()
        {
            "Staff","HR","Admin"
        };
    }
}