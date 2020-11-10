using AutoMapper;
using PH6_1803.Lib.AutoMapper.Model.Dto;
using PH6_1803.Lib.AutoMapper.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH6_1803.Lib.AutoMapper.CoreApi
{
    public class AutomapperConfig : Profile
    {
        //需要转换的类，在构造函数中配置
        public AutomapperConfig()
        {
            //CreateMap<TSource, TDestination>();

            CreateMap<UserInfoModel, UserInfoDto>();
        }
    }
}
