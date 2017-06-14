using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PingTest
{
    public partial class SnmpTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            //Ping_all(server_ip);
            //if(GetSubnet(server_ip)!=GetSubnet(gateway_ip))
            //{
            //    Ping_all(gateway_ip);
            //}

            //Label1.Text+=GetOS("172.20.11.179");

            //PrinterList();
            //for (int i = 41; i <= 50; i++)
            //{
            //    String ip = "172.20.14." + i;
            //    try
            //    {
            //        IList<Lextm.SharpSnmpLib.Variable> result = Messenger.Get(VersionCode.V1,
            //                       new IPEndPoint(IPAddress.Parse(ip), 161),
            //                       new Lextm.SharpSnmpLib.OctetString("public"),
            //                       new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) },
            //                       1000);
            //        Label1.Text += ip + ": " + result.First().Data.ToString() + "<br/>";
            //    }
            //    catch (Exception ex)
            //    {
            //        Label1.Text += ip + ": Timedout<br/>";
            //    }

            //}


            string ip = "172.19.10.27";

            //var result = Messenger.Get(VersionCode.V1,
            //               new IPEndPoint(IPAddress.Parse(ip), 161),
            //               new Lextm.SharpSnmpLib.OctetString("public"),
            //               new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0")) },
            //               60000);

            //Label1.Text += ip + ": " + result.First().Id + ":" + result.First().Data.ToString() + "<br/>";

            //GetNextRequestMessage message = new GetNextRequestMessage(0,
            //                  VersionCode.V1,
            //                  new Lextm.SharpSnmpLib.OctetString("public"),
            //                  new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0")) });
            //ISnmpMessage response = message.GetResponse(60000, new IPEndPoint(IPAddress.Parse(ip), 161));
            //if (response.Pdu().ErrorStatus.ToInt32() != 0)
            //{

            //}

            //result = response.Pdu().Variables;

            //Label1.Text += ip + ": " + result.First().Id +":"+ result.First().Data.ToString() + "<br/>";

            //Label1.Text += result.Count;


            var result = new List<Variable>();
            Messenger.Walk(VersionCode.V1,
                           new IPEndPoint(IPAddress.Parse(ip), 161),
                           new Lextm.SharpSnmpLib.OctetString("public"),
                           new ObjectIdentifier("1.3.6.1.4.1"),
                           result,
                           1000,
                           WalkMode.WithinSubtree);
            Label1.Text = result.Count.ToString();

            foreach (var item in result)
            {
                Label1.Text += item.Id + ": " + item.Data + "<br/>";
            }

            //var result = new List<Variable>();
            //Messenger.BulkWalk(VersionCode.V2,
            //                   new IPEndPoint(IPAddress.Parse(ip), 161),
            //                   new Lextm.SharpSnmpLib.OctetString("public"),
            //                   new ObjectIdentifier("1.3.6.1.2.1"),
            //                   result,
            //                   60000,
            //                   10,
            //                   WalkMode.WithinSubtree,
            //                   null,
            //                   null);

            //foreach (var item in result)
            //{
            //    Label1.Text += item.Id + ": " + item.Data + "<br/>";
            //}
        }
    }
}