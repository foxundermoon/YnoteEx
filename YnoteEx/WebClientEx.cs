﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YnoteEx
{
    class WebClientEx : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public WebClientEx()
        {
            CookieContainer = new CookieContainer();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if(request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = CookieContainer;
            }
            return request;
        }
    }
}
