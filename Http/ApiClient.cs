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
        public ApiClient(string baseAddress = "")
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
        
        string EscapeUriRequest(string uri)
        {
            //if (uri.Contains(":"))
            //    return Uri.EscapeDataString(uri); //HACK: Graph use colon in request path
            //else
                return Uri.EscapeUriString(uri);
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

        public async Task<JObject> GetAsync(string uri = "", params string[] queryParams)
        {
            return GetJson(await GetStringAsync(uri, queryParams));
        }

        public async Task<JArray> GetArrayAsync(string uri = "", params string[] queryParams)
        {
            return GetJsonArray(await GetStringAsync(uri, queryParams));
        }

        public async Task<string> GetStringAsync(string uri = "", params string[] queryParams)
        {
            HttpResponseMessage response;
            var query = string.Join("&", queryParams);
            if (string.IsNullOrEmpty(query))
                response = await Client().GetAsync(EscapeUriRequest(uri));
            else
                response = await Client().GetAsync(EscapeUriRequest(uri) + $"?{query}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<JObject> PostAsync(string uri, JObject json)
        {
            return await PostAsync(uri, new JsonContent(json));
        }

        public async Task<JObject> PostAsync(string uri, object value)
        {
            return await PostAsync(uri, new JsonContent(value));
        }

        public async Task<JObject> PostAsync(string uri, string text)
        {
            return await PostAsync(uri, new StringContent(text));
        }

        public new async Task<JObject> PostAsync(string uri, HttpContent content)
        {
            var response = await Client().PostAsync(EscapeUriRequest(uri), content);
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PostAsync(string uri)
        {
            var response = await Client().PostAsync(EscapeUriRequest(uri), null);
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PutAsync(string uri, object value)
        {
            var response = await Client().PutAsync(EscapeUriRequest(uri), new JsonContent(value));
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PatchAsync(string uri, JObject json)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), EscapeUriRequest(uri))
            {
                Content = new JsonContent(json)
            };

            var response = await Client().SendAsync(request);
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public new async Task DeleteAsync(string uri)
        {
            var response = await Client().DeleteAsync(EscapeUriRequest(uri));
            response.EnsureSuccessStatusCode();
        }
    }
}
