﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wlniao.Net.Dns
{
    /// <summary>
    /// 
    /// </summary>
    public class DnsException : System.Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public DnsException(String message) : base(message)
        {
        }
    }
}
