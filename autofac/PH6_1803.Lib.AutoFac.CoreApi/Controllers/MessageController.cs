using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH6_1803.Lib.AutoFac.CoreApi.Services;

namespace PH6_1803.Lib.AutoFac.CoreApi.Controllers
{
    [Route("test")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public IMessage message { get; set; }
        private readonly IMessage _message;

        IAccount _account;

        public MessageController(IMessage message,IAccount account)
        {
            _message = message;

            _account = account;

        }

        [HttpGet("message")]
        public IActionResult GetMessage()
        {
            return Ok(message.Text);
        }

        [HttpGet("account")]
        public IActionResult GetAccount()
        {
            return Ok(_account.Add());
        }
    }
}
