using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Client.Service
{
    public class StaffService
    {
        #region Staff

        public static List<AccountStaff> FindWithRole(int? id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/staff/findWithRole/" + id);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<List<AccountStaff>>(result.Content.ReadAsStringAsync().Result);
                    //ngay chỗ này nó trả về đối tượng trong store
                    // là cái class getCustomerForDetail_Result. e tạo lại đối tượng tương đương ngoài đây để hứng
                    return readTask;
                }
            }
            return null;

        }
        public static AccountStaff RegisterStaff(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/staff/registerStaff/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountStaff;
            }
            return null;
        }
        public static AccountStaff UpdateStaff(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("http://localhost:61143/api/staff/updateStaff/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountStaff;
            }
            return null;
        }
        public static string RegisterService(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/staff/registerService/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return "Regist Completed";
            }
            return null;
        }
        public static string UpdateService(Service_ account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("http://localhost:61143/api/staff/deleteService/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return "UpdateCompleted";
            }
            return null;
        } 

        #endregion
        #region Img
        public static Img UpdateImg(Img imgStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("http://localhost:61143/api/staff/updateImg/", new StringContent(
                    new JavaScriptSerializer().Serialize(imgStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return imgStaff;
            }
            return null;
        }
        public static Img AddImg(Img imgStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/staff/addImg/", new StringContent(
                    new JavaScriptSerializer().Serialize(imgStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return imgStaff;
            }
            return null;
        }
        #endregion
    }
}