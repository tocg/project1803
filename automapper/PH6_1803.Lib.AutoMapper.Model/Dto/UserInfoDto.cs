using System;
using System.Collections.Generic;
using System.Text;

namespace PH6_1803.Lib.AutoMapper.Model.Dto
{
    public class UserInfoDto
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Age
        {
            get
            {
                return System.DateTime.Now.Year - (Birthday ?.Year);
            } 
        }
        public string Sex { get; set; }
    }
}
