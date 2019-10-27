using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;

namespace Client.Models
{
    public class AccountModel
    {        

        public static List<Account> GetAllAccount()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/account/getall/"+1);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<Account>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
            }
            return null;
        }

    }
}