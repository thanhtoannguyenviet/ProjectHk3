using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Client.Service
{
    public class StaffService
    {
        #region Staff
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
        public static AccountStaff RegisterService(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/staff/registerService/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountStaff;
            }
            return null;
        }
        public static Service_ UpdateService(Service_ service)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("http://localhost:61143/api/staff/updateStaff/", new StringContent(
                    new JavaScriptSerializer().Serialize(service), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return service;
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