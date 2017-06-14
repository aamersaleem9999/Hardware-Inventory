using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PingTest
{
    /// <summary>
    /// Summary description for SnmpGet
    /// </summary>
    public class SnmpGet : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            String ip = context.Request["ip"];
            String oid = context.Request["oid"];

            try
            {
                IList<Lextm.SharpSnmpLib.Variable> result = Messenger.Get(VersionCode.V1,
                               new IPEndPoint(IPAddress.Parse(ip), 161),
                               new Lextm.SharpSnmpLib.OctetString("public"),
                               //new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) },
                               new List<Variable> { new Variable(new ObjectIdentifier(oid)) },
                               1000);

                //context.Response.Write("{'ip':'" + ip + "','data':'" + result.First().Data.ToString() + "'}");
                //ip + ": " + result.First().Data.ToString());
                context.Response.Write(ip + "#" + result.First().Data.ToString());

            }
            catch (Exception ex)
            {
                //context.Response.Write(ip + ": Timedout");
                context.Response.Write("");
            }
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