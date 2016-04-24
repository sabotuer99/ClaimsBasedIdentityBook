using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyCustomTokenHandlerSTS
{
    /// <summary>
    /// Summary description for STSHandler
    /// </summary>
    public class STSHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            String token = File.ReadAllText(@"C:\Users\troy\projects\FederatedDemos\Custom Token Demo\MyCustomTokenHandlerSTS\MyCustomTokenHandlerSTS\Token.xml");
            context.Response.Write(token);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}