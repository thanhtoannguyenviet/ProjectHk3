using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        private static void UpdateStatusToFinish(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/account/updateStatusToFinish/"+id);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
            }
        }
        public static AccountStaff LoginStaff(Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/account/loginStaff/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = JsonConvert.DeserializeObject<AccountStaff>(response.Content.ReadAsStringAsync().Result);

                    return readTask;
                }
            }
            return null;
        }
        public static AccountCustomer LoginCustomer(Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/account/loginCustomer/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = JsonConvert.DeserializeObject<AccountCustomer>(response.Content.ReadAsStringAsync().Result);
                    UpdateStatusToFinish(account.id);
                    return readTask;
                }
            }
            return null;
        }

        public static List<getCustomerForDetail_Result> GetDetailForStaff(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/account/getDetailForStaff/"+id);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<List<getCustomerForDetail_Result>>(result.Content.ReadAsStringAsync().Result);
                    //ngay chỗ này nó trả về đối tượng trong store
                    // là cái class getCustomerForDetail_Result. e tạo lại đối tượng tương đương ngoài đây để hứng
                    return readTask;
                }
            }
            return null;
        } 
    }
}