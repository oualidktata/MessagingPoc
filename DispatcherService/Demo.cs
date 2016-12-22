using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DispatcherService
{
    public class DemoController:ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Hello World");
        }
    }
}
