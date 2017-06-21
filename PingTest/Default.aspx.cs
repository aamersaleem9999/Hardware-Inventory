using Lextm.SharpSnmpLib.Messaging;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PingTest
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // exampleBody.InnerHtml = "";
          

            String ServerIP = GetServerIP();
            String GatewayIP = GetNetworkGatewayIP();
            


            BulkPing(ServerIP);
            if (GetSubnet(ServerIP) != GetSubnet(ServerIP))
            {
                BulkPing(GatewayIP);
            }

            //Label1.Text+=GetOS("172.20.11.179");

        }

        public string GetSubnet(String IP)
        {
            String[] array = IP.Split('.');
            return array[2];
        }

        public static string GetServerIP()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in ipHostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address.ToString();
            }

            return string.Empty;
        }


        static string GetNetworkGatewayIP()
        {
            string ip = null;

            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (f.OperationalStatus == OperationalStatus.Up)
                {                    
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        ip = d.Address.ToString();
                    }
                }
            }

            return ip;
        }
        public void BulkPing(String IP)
        {           
            string[] array = IP.Split('.');

            for (int i = 2; i <= 255; i++)
            {

                string HOST_IP = array[0] + "." + array[1] + "." + array[2] + "." + i;

                PingHOST(HOST_IP, 1,4000);

            }

        }

        public void PingHOST(string HOST_IP, int count, int timeout)
        {
            //Label1.Text += "ping:" + host + "<br/>";
            for (int i = 0; i < count; i++)
            {
                new Thread(delegate()
                {
                    try
                    {
                        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                        ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        ping.SendAsync(HOST_IP, timeout, HOST_IP);
                    }
                    catch(Exception ex)
                    {
                        // handle exception
                    }
                }).Start();
            }
        }
        private void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string IP = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                string DomainName = GetDomainName(IP);
                string HostName = GetHostName(IP);
                string MACAddress = GetMacAddress(IP);
                string[] arr = new string[3];
                string Vendor = HostName != null ? (HostName.IndexOf("565") > 0 ? "Virtual Machine" : (HostName.IndexOf("s114d") == 0 ? "Apple Inc." : "Dell")) : "";
                string OS = HostName != null ? (HostName.IndexOf("s114d") == 0 ? "Mac OS and Windows OS (Dual boot)" : (HostName.IndexOf("osx.") > 0 ? "Mac OS and Windows (Dual boot)" : (HostName.IndexOf("linux.") > 0 ? "Linux" : "Windows OS"))) : "";               
                                

                // add new row
                //HtmlTableRow row = new HtmlTableRow();
                //row.Cells.Add(new HtmlTableCell(IP));
                //row.Cells.Add(new HtmlTableCell(HostName));
                //row.Cells.Add(new HtmlTableCell(MACAddress));
                //row.Cells.Add(new HtmlTableCell("PC"));
                //row.Cells.Add(new HtmlTableCell(Vendor));
                //row.Cells.Add(new HtmlTableCell(OS));

                //example.Rows.Add(row);


                HiddenField1.Value += @"<tr>
                                <td>" + DomainName + @"</td>
                                <td>" + IP + @"</td>
                                <td>" + HostName + @"</td>
                                <td>" + MACAddress + @"</td>
                                <td>PC</td>
                                <td>" + Vendor + @"</td>
                                <td>" + OS + @"</td>                                
                            </tr>";
            }
            else
            {
                // MessageBox.Show(e.Reply.Status.ToString());
            }
        }

        public string GetDomainName(string ipAddress)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                String hostName = hostEntry.HostName;
                //String ipAddress = hostEntry.
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (hostEntry != null)
                {
                    char[] separating = { '.' };
                    String realhostname1 = hostName;
                    String[] s1 = new string[5];
                    int i = 0;
                    bool contains = false;
                    string[] words = realhostname1.Split(separating);
                    foreach (string s in words)
                    {
                        if (contains = words.Contains("cs"))
                        {

                            s1[i] = s;
                            i++;
                        }
                    }
                    String s2;
                    for (int j = 1; j < 5; j++)
                    {
                        sb.AppendLine(s1[j]);
                    }
                    s2 = sb.ToString();

                    return s2;

                }
            }
            catch (SocketException ex)
            {
                //Label1.Text += ex.Message;
                // MessageBox.Show(e.Message.ToString());
            }

            return null;
        }



        public string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                String hostName = hostEntry.HostName;
                //String ipAddress = hostEntry.

                if (hostEntry != null)
                {
                    return hostEntry.HostName;
                    
                }
            }
            catch (SocketException ex)
            {
                //Label1.Text += ex.Message;
                // MessageBox.Show(e.Message.ToString());
            }

            return null;
        }


        //Get MAC address
        public string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process Process = new System.Diagnostics.Process();
            Process.StartInfo.FileName = "arp";
            Process.StartInfo.Arguments = "-a " + ipAddress;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.CreateNoWindow = true;
            Process.Start();
            string strOutput = Process.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "OWN Machine";
            }
        }

        public String GetOS(String IP)
        {
            ManagementNamedValueCollection mContext = new ManagementNamedValueCollection();
            mContext.Add("__ProviderArchitecture", 64);
            mContext.Add("__RequiredArchitecture", false);
            ConnectionOptions options = new ConnectionOptions();           
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.EnablePrivileges = false;
            options.Context = mContext;
            string strIP = IP;
            string sOperatingSystem ="";
            string sComputerName ="";
            //try
            {
                ManagementScope ManagementScope1 = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", strIP), options);
                ManagementScope1.Connect();
                ObjectGetOptions objectGetOptions = new ObjectGetOptions();
              //  try
                {
                    ManagementPath mpWin32_OperatingSystem = new ManagementPath("Win32_OperatingSystem");
                    ManagementClass mcWin32_OperatingSystem = new ManagementClass(ManagementScope1, mpWin32_OperatingSystem, objectGetOptions);
                    foreach (ManagementObject moWin32_OperatingSystem in mcWin32_OperatingSystem.GetInstances())
                    {
                        sOperatingSystem = moWin32_OperatingSystem["Caption"].ToString();                        
                        sComputerName = moWin32_OperatingSystem["csname"].ToString();                        
                    }
                }
                //catch (Exception ex)
                {
              //      Label1.Text = ex.ToString();
                }
            }
            //catch(Exception ex)
            {
            //    Label1.Text = ex.ToString();
            }

            return sOperatingSystem;
        }      
 
    }
}
