using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models;
using Newtonsoft.Json;

namespace Client.Service
{
    public class LoginService
    {
        public static Account CheckLogin(string username, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/account/checkLogin/" + username + "/" + password);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    UpdateStatus();
                    var readTask = JsonConvert.DeserializeObject<Account>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        private static void UpdateStatus()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/account/updateStatus");
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
            }
        }
        public static AccountStaff LoginStaff(Account account)
        {
                using (var client = new HttpClient())
            {
                var obj = JsonConvert.SerializeObject(account);
                client.BaseAddress = new Uri("http://localhost:61143/api/account/loginStaff/" + account.id);
                    var responseTask = client.GetAsync(client.BaseAddress);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = JsonConvert.DeserializeObject<AccountStaff>(result.Content.ReadAsStringAsync().Result);
                        return readTask; // nếu return ngay đây sao k return lại method trên luôn
                    }
                    return null;
                }
            
        }
        public static AccountCustomer LoginCustomer(Account account)
        {
            var jsonCustomer = new JavaScriptSerializer().Serialize(account);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/account/loginCustomer/" + account.id);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<AccountCustomer>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
    }
}