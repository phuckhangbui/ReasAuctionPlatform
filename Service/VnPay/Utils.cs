using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Service.VnPay
{
    public class Utils
    {

        public static DateTime ParseDateString(string dateString)
        {
            // Specify the format of the date and time string
            string format = "yyyyMMddHHmmss";

            // Parse the string to a DateTime object
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
                // Handle parsing failure
                throw new ArgumentException("Invalid date and time format");
            }
        }

        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public static string GetRemoteHostIpAddressUsingXForwardedFor(HttpContext httpContext)
        {
            IPAddress? remoteIpAddress = null;
            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim());
                foreach (var ip in ips)
                {
                    if (IPAddress.TryParse(ip, out var address) &&
                        address.AddressFamily is AddressFamily.InterNetwork
                         or AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = address;
                        break;
                    }
                }
            }
            if (remoteIpAddress == null)
            {
                return GetServerIPAddress();
            }
            return remoteIpAddress.ToString();
        }

        public static string GetServerIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }

    }
}
