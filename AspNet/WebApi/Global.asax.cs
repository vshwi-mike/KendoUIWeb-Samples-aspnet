using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // http://stackoverflow.com/questions/9614218/how-to-use-console-writeline-in-asp-net-c-during-debug
            Console.SetOut(new LogWriter());
        }

        public class LogWriter : System.IO.TextWriter {
            public override System.Text.Encoding Encoding {
                get { return System.Text.Encoding.UTF8; }
            }

            public override void Write(string value) {
                System.Diagnostics.Debug.Write(value);
            }
        }
    }
}
