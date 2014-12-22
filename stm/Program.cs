using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;

namespace stm
{
    class Program
    {
        static void ConfigureConsole(string Title, ConsoleColor Color)
        {
            Console.Title = Title;
            Console.ForegroundColor = Color;
        }

        static void ShowSplash(string FileName)
        {
            using (Stream Strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName))
            {
                using (StreamReader Reader = new StreamReader(Strm))
                {
                    Console.WriteLine(Reader.ReadToEnd());
                }
            }
        }

        static string GeneratePOSTRequest(string AppID, string APIKey)
        {
            return String.Format("appid={0}&key={1}", AppID, APIKey);
        }

        static string SendAPIReq(string AppID)
        {
            byte[] ByteReqC;
            string Result = "";

            HttpWebRequest WrQ = (HttpWebRequest)WebRequest.Create(Properties.Resources.AddURI);

            WrQ.UserAgent = Properties.Resources.UserAgent;
            WrQ.Method = "POST";
            WrQ.Timeout = 250000;

            ByteReqC = Encoding.UTF8.GetBytes(GeneratePOSTRequest(AppID, Properties.Settings.Default.APIKey));

            WrQ.ContentType = "application/x-www-form-urlencoded";
            WrQ.ContentLength = ByteReqC.Length;

            using (Stream HTTPStreamRq = WrQ.GetRequestStream())
            {
                HTTPStreamRq.Write(ByteReqC, 0, ByteReqC.Length);
                HTTPStreamRq.Close();
            }

            HttpWebResponse HTTPWResp = (HttpWebResponse)WrQ.GetResponse();

            if (HTTPWResp.StatusCode == HttpStatusCode.OK)
            {
                using (Stream RespStream = HTTPWResp.GetResponseStream())
                {
                    using (StreamReader StrRead = new StreamReader(RespStream))
                    {
                        Result = StrRead.ReadToEnd();
                    }
                }
            }
            
            return Result;
        }

        static string FetchStringHTTP(string StrF)
        {
            string DnlStr = "";
            using (WebClient Downloader = new WebClient())
            {
                Downloader.Headers.Add("User-Agent", Properties.Resources.UserAgent);
                DnlStr = Downloader.DownloadString(StrF);
            }
            return DnlStr;
        }

        static void GenerateSrvKey(string AppID)
        {
            Console.Write(Properties.Resources.MsgGn);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendAPIReq(AppID));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.AddRes, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, AppID);
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static void ListSrvKeys()
        {
            Console.Write(Properties.Resources.MsgFt);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(FetchStringHTTP(String.Format(Properties.Resources.FetchURI, Properties.Settings.Default.APIKey)));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.AddRes, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("appid")[i].InnerText);
                    Console.WriteLine();
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static string ValidateAppID(string AppID)
        {
            return Regex.IsMatch(AppID, "^[0-9]*$") ? AppID : Properties.Resources.DefaultAppID;
        }

        static void GetServerIDByIP(string Rx)
        {
            Console.Write(Properties.Resources.MsgAPIFetch);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(FetchStringHTTP(String.Format(Properties.Resources.GetIdByIP, Properties.Settings.Default.APIKey, Rx)));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.SrvByIP, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                    Console.WriteLine();
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static void Main(string[] Args)
        {
            ConfigureConsole(Properties.Resources.AppName, ConsoleColor.Green);
            try { ShowSplash(Properties.Resources.WelcomeFileName); } catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }

            if (Regex.IsMatch(Properties.Settings.Default.APIKey, "^[0-9A-F]*$"))
            {
                if (Args.Count() > 0)
                {
                    switch (Args[0])
                    {
                        case "generate": GenerateSrvKey((Args.Count() >= 2) ? ValidateAppID(Args[1]) : Properties.Resources.DefaultAppID);
                            break;
                        case "list": ListSrvKeys();
                            break;
                        case "version": Console.WriteLine(Properties.Resources.AppVerStr, Properties.Resources.AppName, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                            break;
                        case "getid": GetServerIDByIP(Args[1]);
                            break;
                        default: Console.WriteLine(Properties.Resources.UnknownOpt);
                            break;
                    }
                }
                else { Console.WriteLine(Properties.Resources.WlxMsg); }
            }
            else { Console.WriteLine(Properties.Resources.NoAPIKey); }

            #if DEBUG
            Console.ReadKey();
            #endif
        }
    }
}
