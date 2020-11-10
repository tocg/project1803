using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH6_1803.Lib.AutoFac.CoreApi.Services
{
    public class Services
    {
    }


    public interface IAccount {
        string Add();
    }

    public interface ITool { }

    public interface IMessage
    {
        public string Text { get; set; }
    }
    public class Account : IAccount {
        public string Add() { return "131646496"; }
    }

    public class Message : IMessage
    {
        public string Text { get; set; }

        public Message()
        {
            Text = "Hello Message";

        }

    }
    public class Tool : ITool { }
}
