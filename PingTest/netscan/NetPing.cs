using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;

namespace PingTest.netscan
{
    public class NetPing
    {
        public static string GetNetworkGateway()
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

        public static List<String> GetAllSubnetIPs()
        {
            List<String> IPs = new List<String>();
            string GatewayIP = GetNetworkGateway();

            string[] array = GatewayIP.Split('.');

            for (int i = 2; i <= 255; i++)
            {

                string temp_ip = array[0] + "." + array[1] + "." + array[2] + "." + i;

                IPs.Add(temp_ip);

            }

            return IPs;
        }

        public static List<DeviceInfo> GetDeviceInfoByPing(List<String> IPs)
        {
            List<DeviceInfo> deviceInfoList = new List<DeviceInfo>();
            foreach (String ip in IPs)
            {
                DeviceInfo temp = GetDeviceInfoByPing(ip);
                if(temp!=null)
                {
                    deviceInfoList.Add(temp);
                }
            }
            return deviceInfoList;
        }

        public static DeviceInfo GetDeviceInfoByPing(String IP)
        {
            Ping ping = new Ping();
            PingReply Reply = ping.Send(IP);
            if (Reply != null && Reply.Status == IPStatus.Success)
            {
                string hostname = GetHostName(IP);
                string macaddres = GetMacAddress(IP);

                DeviceInfo deviceInfo = new DeviceInfo();
                deviceInfo.DeviceName = hostname;
                deviceInfo.IPAddress = IP;
                deviceInfo.MACAddress = macaddres;

                return deviceInfo;
            }
            else
            {
                return null;
            }
        }

        public static string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException)
            {
                // MessageBox.Show(e.Message.ToString());
            }

            return null;
        }


        //Get MAC address
        public static string GetMacAddress(string ipAddress)
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

    }
}