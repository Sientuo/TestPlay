﻿using System;
using System.Collections.Generic;
using System.Web;

namespace TestComm.PayBase
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}