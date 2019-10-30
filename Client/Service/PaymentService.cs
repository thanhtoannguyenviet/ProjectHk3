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
    public class PaymentService
    {
        public static Detail UpdateDetail(Detail detail)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("http://localhost:61143/api/payment/updateDetail/", new StringContent(
                    new JavaScriptSerializer().Serialize(detail), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = JsonConvert.DeserializeObject<Detail>(response.Content.ReadAsStringAsync().Result);

                    return readTask;
                }
            }
            return null;
        }
    }
}