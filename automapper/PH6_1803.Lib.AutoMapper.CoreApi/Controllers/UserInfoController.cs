using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH6_1803.Lib.AutoMapper.Model.Dto;
using PH6_1803.Lib.AutoMapper.Model.Models;

namespace PH6_1803.Lib.AutoMapper.CoreApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UserInfoController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("get")]
        public IActionResult GetUser()
        {
            UserInfoModel userInfo = new UserInfoModel
            {
                ID = "1",
                Birthday = Convert.ToDateTime("2010-10-4 12:12:12"),
                Name = "name",
                Sex = "男"
            };

            var userDto = _mapper.Map<UserInfoDto>(userInfo);

            return Ok(userDto);
        }
    }
}
