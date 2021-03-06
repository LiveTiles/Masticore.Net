﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Masticore.Net.Http
{
    public class ApiClient : HttpClient
    {
        public ApiClient() { }

        public ApiClient(string baseAddress)
        {
            BaseAddress = new Uri(baseAddress);
        }

        protected HttpClient Client()
        {
            return this;
        }

        string EscapeRequest(string request)
        {
            return Uri.EscapeUriString(request);
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

        #region Get
        public virtual async Task<JObject> GetAsync(string request = "", params string[] queryParams)
        {
            return GetJson(await GetStringAsync(request, queryParams));
        }

        public virtual async Task<JObject> GetAsyncNoEscape(string request = "", params string[] queryParams)
        {
            return GetJson(await GetStringAsyncNoEscape(request, queryParams));
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

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetStringAsyncNoEscape(string request = "", params string[] queryParams)
        {
            HttpResponseMessage response;
            var query = string.Join("&", queryParams);
            if (string.IsNullOrEmpty(query))
                response = await Client().GetAsync(request);
            else
                response = await Client().GetAsync(request + $"?{query}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region Post
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

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PostAsync(string request)
        {
            var response = await Client().PostAsync(EscapeRequest(request), null);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> PostAsyncGetString(string request, JObject json)
        {
            return await PostAsyncGetString(request, new JsonContent(json));
        }

        public async Task<string> PostAsyncGetString(string request, object value)
        {
            return await PostAsyncGetString(request, new JsonContent(value));
        }

        public async Task<string> PostAsyncGetString(string request, string text)
        {
            return await PostAsyncGetString(request, new StringContent(text));
        }

        public async Task<string> PostAsyncGetString(string request, HttpContent content)
        {
            var response = await Client().PostAsync(EscapeRequest(request), content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsyncGetString(string request)
        {
            var response = await Client().PostAsync(EscapeRequest(request), null);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region Put
        public async Task<JObject> PutAsync(string request, object value)
        {
            var response = await Client().PutAsync(EscapeRequest(request), new JsonContent(value));
            response.EnsureSuccessStatusCode();
            return GetJson(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region Patch
        public async Task<JObject> PatchAsync(string request, JObject json)
        {
            return await PatchAsync(request, new JsonContent(json));
        }

        public async Task<JObject> PatchAsync(string request, object value)
        {
            return await PatchAsync(request, new JsonContent(value));
        }

        public async Task<JObject> PatchAsync(string request, string text)
        {
            return await PatchAsync(request, new StringContent(text));
        }

        public async Task<JObject> PatchAsync(string request, HttpContent content)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("patch"), EscapeRequest(request))
            {
                Content = content,
            };

            var response = await Client().SendAsync(requestMessage);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

            return GetJson(await response.Content.ReadAsStringAsync());
        }

        public async Task<JObject> PatchAsync(string request)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("patch"), EscapeRequest(request));
            var response = await Client().SendAsync(requestMessage);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

            return GetJson(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region Delete
        public new async Task DeleteAsync(string request)
        {
            var response = await Client().DeleteAsync(EscapeRequest(request));

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                e.Data.Add("HttpStatusCode", response.StatusCode);
                throw e;
            }

        }
        #endregion
    }
}
