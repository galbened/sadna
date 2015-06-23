﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Interfaces;
using Driver;

namespace WebCommunicationClientSoftware.Controllers
{
    [RoutePrefix("api/forumsnames")]
    public class ForumsNamesController : ApiController
    {
        //private static IApplicationBridge driver = new BridgeReal();

         public ForumsNamesController()
        {
        }

        [Route("getForumsNames")]
        public List<string> Get()
        {
            //List<string> names = driver.GetForumTopics();
            List<string> names = new List<string>();
            names.Add("third");
            names.Add("forth");
            return names;
        }
    }
}