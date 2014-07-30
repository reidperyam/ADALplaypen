using Owin;
using Microsoft.Owin.Extensions;
using System;
using System.Net;

namespace Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            new Api.Startup().Configuration(app);
            new Core.Startup().Configuration(app);
        }
    }
}
