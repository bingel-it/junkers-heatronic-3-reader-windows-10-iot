using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace BingelIT.MicroWebServerLib
{
    class HttpRequestBuilder 
    {

        /// <summary>
        /// Process the request header
        /// </summary>
        /// <param name="requestData"></param>
        public HttpRequest ProcessRequest(String requestData)
        {
            
            var request = new HttpRequest();

            Debug.WriteLine(requestData);
            var headerLines = requestData.Split('\n');

            // Parse the first line of the request, e.g. "GET /path/ HTTP/1.1"
            string firstLine = headerLines[0];
            string[] words = firstLine.Split(' ');
            var method = words[0];

            request.Url = UnescapeString(words[1]);

            // Has arguments?
            if (words[1].IndexOf('?') >= 0)
            {
                var argumentsString = words[1].Substring(words[1].IndexOf('?') + 1);
                request.Arguments = ParseArguments(argumentsString);
            }

            switch (method)
            {
                case "GET":
                    request.Method = HttpRequestMethod.Get;
                    break;
                case "POST":
                    request.Method = HttpRequestMethod.Post;
                    break;
                case "PUT":
                    request.Method = HttpRequestMethod.Put;
                    break;
                case "DELETE":
                    request.Method = HttpRequestMethod.Delete;
                    break;
                case "OPTIONS":
                    request.Method = HttpRequestMethod.Options;
                    break;
                case "MOVE":
                    request.Method = HttpRequestMethod.Move;
                    break;
                default:
                    request.Method = HttpRequestMethod.Unknown;
                    break;
            }

            // look for further headers in other lines of the request (e.g. User-Agent, Cookie)
            foreach (var line in headerLines)
            {
                if (line.IndexOf(':') < 0)
                    continue;

                var lineParts = line.Split(':');

                var key = UnescapeString(lineParts[0].Trim(' ', '\t'));
                var value = UnescapeString(lineParts[1].Trim(' ', '\t', '\r'));
                if (!request.Header.ContainsKey(key))
                    request.Header.Add(key, value);
            }

            return request;
        }


        private Dictionary<String, String> ParseArguments(string argumentsString)
        {
            Dictionary<String, String> arguments = new Dictionary<String, String>();
            var argumentList = argumentsString.Split(new char[] { '&' });
            foreach (var paramString in argumentList)
            {
                var keyValuePear = paramString.Split(new char[] { '=' });
                if (keyValuePear.Length != 2)
                    continue;

                var key = keyValuePear[0];
                var value = keyValuePear[1];
                arguments.Add(key, value);
            }

            return arguments;
        }

        string UnescapeString(string s)
        {
            int idx = s.IndexOf('%'), beg = 0;
            if (idx < 0) return s;
            var sb = new System.Text.StringBuilder();
            var bytes = new List<byte>();
            while (idx >= 0 && idx < s.Length - 2)
            {
                if (idx > beg) sb.Append(s.Substring(beg, idx - beg));
                do
                {
                    var hex = s.Substring(idx + 1, 2);
                    byte val;
                    if (TryParseHex(hex, out val))
                    {
                        bytes.Add(val);
                        idx += 3;
                    }
                    else
                    {
                        idx++;
                    }
                } while (idx < s.Length - 2 && s[idx] == '%');
                var bs = bytes.ToArray();
                sb.Append(Encoding.UTF8.GetChars(bs));//.GetString(bs));
                bytes.Clear();
                beg = idx;
                idx = s.IndexOf('%', idx);
            }
            if (beg < s.Length) sb.Append(s.Substring(beg));
            return sb.ToString();
        }
        int ParseHex(string s)
        {
            int val = 0;
            int res = 0;
            for (int i = 0; i < s.Length; i++)
            {
                res *= 16;
                char c = s[i];
                if (c >= '0' && c <= '9')
                {
                    val = c - '0';
                }
                else
                {
                    switch (c)
                    {
                        case 'a':
                        case 'A':
                            val = 10;
                            break;
                        case 'b':
                        case 'B':
                            val = 11;
                            break;
                        case 'c':
                        case 'C':
                            val = 12;
                            break;
                        case 'd':
                        case 'D':
                            val = 13;
                            break;
                        case 'e':
                        case 'E':
                            val = 14;
                            break;
                        case 'f':
                        case 'F':
                            val = 15;
                            break;
                        default: throw new ArgumentException("Not a Hex-Char: " + c);
                    }
                }
                res += val;
            }
            return res;
        }
        bool TryParseHex(string s, out byte b)
        {
            int val = 0;
            int res = 0;
            for (int i = 0; i < s.Length; i++)
            {
                res *= 16;
                char c = s[i];
                if (c >= '0' && c <= '9')
                {
                    val = c - '0';
                }
                else if (c >= 'a' && c <= 'f')
                {
                    val = c - 'a' + 10;
                }
                else if (c >= 'A' && c <= 'F')
                {
                    val = c - 'A' + 10;
                }
                else
                {
                    b = 0;
                    return false;
                }
                res += val;
            }
            b = (byte)res;
            return true;
        }
    }
}
