using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace nExtensions
{
    public static class CookieExtended
    {
        public static List<Cookie> GetCookies(this Uri urlString)
        {
            List<Cookie> cookieList = new List<Cookie>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            foreach (Cookie cookie in response.Cookies)
                cookieList.Add(cookie);
            response.Close();

            return cookieList;
        }

        public static string IPAddrToBinary (this IPAddress ipAddress)
        {
            string input = ipAddress.ToString();
            return String.Join(".", ( // join segments
                input.Split('.').Select( // split segments into a string[]

                    // take each element of array, name it "x",
                //   and return binary format string
                    x => Convert.ToString(Int32.Parse(x), 2).PadLeft(8, '0')

                // convert the IEnumerable<string> to string[],
                // which is 2nd parameter of String.Join
                )).ToArray()); 
        }

        public static bool IsAlive (this IPAddress ipAddress)
        {
            Ping pingSender = new Ping ();
            PingReply reply = pingSender.Send(ipAddress);

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }

            return false;
        }

        public static bool IsOpen(this IPAddress ipAddress,int port)
        {
		    TcpClient TcpScan = new TcpClient();	
		    try	
		    {
                TcpScan.Connect(ipAddress, port);
                return true;
		    } 		
		    catch
            { return false; }
        }
    }
}
