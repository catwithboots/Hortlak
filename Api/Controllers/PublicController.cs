using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Api.Extensions;

namespace Api.Controllers
{
    [RoutePrefix("public")]
    public class PublicController : ApiController
    {
        [Route("WhatIsMyIp")]
        public HttpResponseMessage GetIps()
        {
            var output = Request.GetClientIpAddress();

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }
    }
}
