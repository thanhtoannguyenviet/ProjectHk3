using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models;
using Newtonsoft.Json;
using Client.Models;
namespace Client.Service
{
    public class CustomerService
    {
        #region Customer

        public static List<Detail> GetAllOrder(Customer customer, int pageNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/getAllOrder/"+ customer.id+"/"+pageNumber);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<Detail>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }

        public static int CountAllOrder(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/countAllOrder/" + customer.id);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return -1;
            }
        }
        #endregion
        #region Payment
        public static List<Payment> GetPayment(Customer customer, int pageNumber)
        {
            var jsonCustomer = new JavaScriptSerializer().Serialize(customer);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/getPayment/" +jsonCustomer +"/"+ pageNumber);
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<Payment>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        public static int CountPayment(Customer customer)
        {
            var jsonCustomer = new JavaScriptSerializer().Serialize(customer);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/countPayment/" + jsonCustomer);
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return -1;
            }
        }

        #endregion
        #region Staff
        //HTTP GET
        public static List<AccountStaff> GetStaffCustomer(int pageNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/getStaffCustomer/" + pageNumber);
                
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<AccountStaff>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }

        public static int CountStaffCustomer()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/countStaffCustomer/");
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return -1;
            }
        }

        public static List<Detail> DetailStaff(int idStaff)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61143/api/customer/detailStaff/"+idStaff);
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<Detail>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }

                return null;
            }
        }
        #endregion
        #region Register
        public static AccountCustomer RegisterCustomer(AccountCustomer accountCustomer)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/customer/registerCustomer/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountCustomer), Encoding.UTF8, "application/json")).Result;
                if(response.StatusCode==HttpStatusCode.OK)
                    return accountCustomer;
            }
            return null;
        }
        public static AccountCustomer UpdateCustomer(AccountCustomer accountCustomer)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/customer/updateCustomer/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountCustomer), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountCustomer;
            }
            return null;
        }
        #endregion

        #region Imgage

        public static Img UpdateImg(Img img) => StaffService.UpdateImg(img);

        #endregion
    }
}