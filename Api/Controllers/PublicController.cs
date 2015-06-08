using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.Cors;
using Api.Extensions;
using Api.Objects;
using Newtonsoft.Json.Linq;

namespace Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("public")]
    public class PublicController : ApiController
    {
        [Route("WhatIsMyIp")]
        public HttpResponseMessage GetIps()
        {
            Console.WriteLine("WhatIsMyIp called");
            var output = Request.GetClientIpAddress();

            PublicObjects.KV kvobject = new PublicObjects.KV
            {
                Key = "IP",
                Value = output
            };

            JObject ob = (JObject)JToken.FromObject(kvobject);

            return Request.CreateResponse(HttpStatusCode.OK, ob);
        }
        
        [Route("WhereIsHortlak")]
        public HttpResponseMessage GetHostName()
        {
            Console.WriteLine("WhereIsHortlak called");

            PublicObjects.KV kvobject = new PublicObjects.KV
            {
                Key = "Host",
                Value = Environment.MachineName
            };

            JObject ob = (JObject) JToken.FromObject(kvobject);

            //JArray array = new JArray {ob};

            //JObject o = new JObject();
            //o["hosts"] = array;

            return Request.CreateResponse<JObject>(HttpStatusCode.OK, ob);
        }
    }
}
