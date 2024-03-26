using AutofacPOC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutofacPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {

        private IProxy __proxy; 

        public SampleController(IProxy proxy) {
            __proxy = proxy;
        }

        public string Get()
        {
            var temp = HttpContext.User;
            return __proxy.invoke();
        }
    }
}
