using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Common
{
    public class Role
    {
        public static Dictionary<int,string> ROLE_= new Dictionary<int,string>()
        {
            {100,"Admin"},
            {75,"HR"},
            {50,"KeToan"},//chua lam gi, doi sang tieng anh
            {25,"Trainer"},
            {20,"NhanVien"},
            {1,"Customer" }
        };
        public Role()
        {
            ROLE_=ROLE_;
        }
        public static string GetValue(int? TKey)
        {
            return ROLE_.FirstOrDefault(x => x.Key == TKey).Value;
        }
            public static int GetKey(string TValue)
        {
            return ROLE_.FirstOrDefault(x => x.Value == TValue).Key;
        }
    }
}