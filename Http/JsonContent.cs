using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Masticore.Net.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(object value) :
            base(JsonConvert.SerializeObject(value,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }),
                Encoding.UTF8, "application/json")
        { }
    }
}
