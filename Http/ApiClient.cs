using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Masticore.Net.Http
{
    public class ApiClient : HttpClient
    {
        public ApiClient(string baseAddress)
        {
            BaseAddress = new Uri(baseAddress);
        }

        //public new string BaseAddress { get; set; }

        protected HttpClient Client()
        {
            //base.BaseAddress = new Uri(BaseAddress);
            return this;
        }

        //public HttpClient Clone()
        //{
        //    return new HttpClient(BaseAddress);
        //}
        
        string EscapeRequest(string request)
        {
            //if (request.Contains(":"))
            //    return Uri.EscapeDataString(request); //HACK: Graph use colon in request path
            //else
            return Uri.EscapeDataString(request);
        }

        JObject GetJson(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new JObject();
            else
                return JObject.Parse(value);
        }

        JArray GetJsonArray(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new JArray();
            else
                return JArray.Parse(value);
        }

        public async Task<JObject> GetAsync(string request = "", params string[] queryParams)
        {
            return GetJson(await GetStringAsync(request, queryParams));
        }

        public async Task<JArray> GetArrayAsync(string request = "", params string[] queryParams)
        {
            return GetJsonArray(await GetStringAsync(request, queryParams));
        }

        public async Task<string> GetStringAsync(string request = "", params string[] queryParams)
        {
            HttpResponseMessage response;
            var query = string.Join("&", queryParams);
            if (string.IsNullOrEmpty(query))
                response = await Client().GetAsync(EscapeRequest(request));
            else
                response = await Client().GetAsync(EscapeRequest(request) + $"?{query}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<JObject> PostAsync(string request, JObject json)
        {
            return await PostAsync(request, new JsonContent(json));
        }

        public async Task<JObject> PostAsync(string request, object value)
        {
            return await PostAsync(request, new JsonContent(value));
        }

        public async Task<JObject> PostAsync(string request, string text)
        {
            return await PostAsync(request, new StringContent(text));
        }

        public new async Task<JObject> PostAsync(string request, HttpContent content)
        {
            var response = await Client().PostAsync(EscapeRequest(request), content);
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PostAsync(string request)
        {
            var response = await Client().PostAsync(EscapeRequest(request), null);
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PutAsync(string request, object value)
        {
            var response = await Client().PutAsync(EscapeRequest(request), new JsonContent(value));
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PatchAsync(string request, JObject json)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), EscapeRequest(request))
            {
                Content = new JsonContent(json)
            };

            var response = await Client().SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public new async Task DeleteAsync(string request)
        {
            var response = await Client().DeleteAsync(EscapeRequest(request));
            response.EnsureSuccessStatusCode();
        }
    }
}
