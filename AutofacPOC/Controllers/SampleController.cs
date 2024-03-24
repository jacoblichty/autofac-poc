using AutofacPOC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutofacPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {

        private IClientService __clientService; 

        public SampleController(IClientService clientService) {
            __clientService = clientService;
        }

        public string Get()
        {
            return __clientService.invoke();
        }
    }
}
