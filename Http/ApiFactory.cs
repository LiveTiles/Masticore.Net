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

        public async Task<JObject> GetAsync(string uri = "", string query = "")
        {
            return await Client().GetAsync(uri, query);
        }

        public async Task<string> GetStringAsync(string uri = "", string query = "")
        {
            return await Client().GetStringAsync(uri, query);
        }

        public async Task<JArray> GetArrayAsync(string uri = "", string query = "")
        {
            return await Client().GetArrayAsync(uri, query);
        }
        
        public async Task<JObject> PostAsync(string uri, JObject json)
        {
            return await Client().PostAsync(uri, json);
        }

        public async Task<JObject> PostAsync(string uri, string text)
        {
            return await Client().PostAsync(uri, text);
        }

        public async Task<JObject> PostAsync(string uri, HttpContent content)
        {
            return await Client().PostAsync(uri, content);
        }

        public async Task<JObject> PatchAsync(string uri, JObject json)
        {
            return await Client().PatchAsync(uri, json);
        }

        public async Task DeleteAsync(string uri)
        {
            await Client().DeleteAsync(uri);
        }
    }
}
