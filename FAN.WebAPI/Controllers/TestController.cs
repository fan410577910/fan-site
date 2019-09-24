using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace FAN.WebAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage AsyncIndex()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
            });
            return new HttpResponseMessage { StatusCode=HttpStatusCode.OK};
        }
        [HttpGet]
        public HttpResponseMessage SyncIndex()
        {
            Thread.Sleep(5000);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK};
        }
    }
}
