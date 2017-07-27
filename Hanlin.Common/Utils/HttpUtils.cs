using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hanlin.Common.Utils
{
    public class HttpSetting
    {
        public TimeSpan? Timeout { set; get; }
    }

    public class HttpUtils
    {
        private static HttpClient BuildJsonClient(HttpSetting setting = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (setting != null)
            {
                if(setting.Timeout.HasValue) client.Timeout = setting.Timeout.Value;
            }
            return client;
        }

        private static string BuildJsonQuery(object request, Func<HttpClient, StringContent, HttpResponseMessage> exec,
            bool defaultCase = false, HttpSetting setting = null)
        {
            var requestJson = defaultCase ? JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            }) : JsonConvert.SerializeObject(request);
            return BuildJsonQuery(requestJson, exec, setting);
        }

        private static string BuildJsonQuery(string requestJson, Func<HttpClient, StringContent, HttpResponseMessage> exec, HttpSetting setting = null)
        {
            var client = BuildJsonClient(setting);
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = exec(client, httpContent);

            return GetResponseResult(response);
        }

        public static string Get(string url, HttpSetting setting = null)
        {
            var client = BuildJsonClient(setting);
            var response = client.GetAsync(url).Result;
            return GetResponseResult(response);
        }

        private static string GetResponseResult(HttpResponseMessage response)
        {
            var result = response.Content.ReadAsStringAsync().Result;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(result, e);
            }
            return result;
        } 

        public static T GetJson<T>(string url, HttpSetting setting = null)
        {
            return JsonConvert.DeserializeObject<T>(Get(url, setting));
        }

        public static string PostJson(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            return BuildJsonQuery(request, (client, httpContent) => client.PostAsync(url, httpContent).Result, defaultCase, setting);
        }

        public static string PostJson(string url, string jsonBody, HttpSetting setting = null)
        {
            return BuildJsonQuery(jsonBody, (client, httpContent) => client.PostAsync(url, httpContent).Result, setting);
        }

        public static T PostJson<T>(string url, string jsonBody, HttpSetting setting = null)
        {
            var respJson = PostJson(url, jsonBody, setting);
            return JsonConvert.DeserializeObject<T>(respJson);
        }

        public static string PutJson(string url, string jsonBody, HttpSetting setting = null)
        {
            return BuildJsonQuery(jsonBody, (client, httpContent) => client.PutAsync(url, httpContent).Result, setting);
        }

        public static T PutJson<T>(string url, string jsonBody, HttpSetting setting = null)
        {
            var respJson = PutJson(url, jsonBody, setting);
            return JsonConvert.DeserializeObject<T>(respJson);
        }

        public static T PostJson<T>(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            var respJson = PostJson(url, request, defaultCase, setting);
            return JsonConvert.DeserializeObject<T>(respJson);
        }
        
        public static string PutJson(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            return BuildJsonQuery(request, (client, httpContent) => client.PutAsync(url, httpContent).Result, defaultCase, setting);
        }

        public static T PutJson<T>(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            var respJson = PutJson(url, request, defaultCase, setting);
            return JsonConvert.DeserializeObject<T>(respJson);
        }
        
    }
}