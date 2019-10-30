using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public static class PayPalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static PayPalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            var config = new Dictionary<string, string>();
            config.Add("mode", "sandbox");
            config.Add("connectionTimeout", "360000");
            config.Add("requestRetries", "1");
            config.Add("clientId", "AQOC5jnO2ACbaauHw6W-tCBxlKBvXgwW9lGYCSO3vbPkq3Zt7W4uCiGjg06hATlIukN3o2kIcfPrK6zv");
            config.Add("clientSecret", "EEV7ij0kHkFhmX56WNHWhnnr3C0GdPsAFi2UJJ_w5IZ_F00L0zC4K_aX3mx8t7MtT7sSsspZyRJlRT4x");
            return config;
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string accessToken = "", string requestID = "")
        {
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken, string.IsNullOrEmpty(requestID) ? Guid.NewGuid().ToString() : requestID);
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}