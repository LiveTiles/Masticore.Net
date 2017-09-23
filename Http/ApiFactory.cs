using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Masticore.Net.Http
{
    [Serializable]
    public abstract class ApiFactory
    {
        public string BaseAddress;

        public ApiFactory(string path = "")
        {
            BaseAddress += path;
        }
        
        protected virtual ApiClient Client(string path = "")
        {
            return new ApiClient(BaseAddress + path);   
        }

        public async Task<JObject> GetAsync(string request = "", string query = "")
        {
            return await Client().GetAsync(request, query);
        }

        public async Task<string> GetStringAsync(string request = "", string query = "")
        {
            return await Client().GetStringAsync(request, query);
        }

        public async Task<JArray> GetArrayAsync(string request = "", string query = "")
        {
            return await Client().GetArrayAsync(request, query);
        }
        
        public async Task<JObject> PostAsync(string request, JObject json)
        {
            return await Client().PostAsync(request, json);
        }

        public async Task<JObject> PostAsync(string request, object value)
        {
            return await Client().PostAsync(request, value);
        }

        public async Task<JObject> PostAsync(string request, string text)
        {
            return await Client().PostAsync(request, text);
        }

        public async Task<JObject> PostAsync(string request, HttpContent content)
        {
            return await Client().PostAsync(request, content);
        }

        public async Task<JObject> PatchAsync(string request, JObject json)
        {
            return await Client().PatchAsync(request, json);
        }

        public async Task DeleteAsync(string request)
        {
            await Client().DeleteAsync(request);
        }
    }
}
