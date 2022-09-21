using Client.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        public ChatController()
        {

        }

        [HttpGet]
        public string Get(string useremail)
        {
            SessionQueService ss = new();
            ss.PublishMessage(useremail);
            return "Chat Client connected";
        }
    }
}
