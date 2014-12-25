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

        static string SendAPIRequest(string APIURL, string APIReq)
        {
            byte[] ByteReqC;
            string Result = "";

            HttpWebRequest WrQ = (HttpWebRequest)WebRequest.Create(APIURL);

            WrQ.UserAgent = Properties.Resources.UserAgent;
            WrQ.Method = "POST";
            WrQ.Timeout = 250000;

            ByteReqC = Encoding.UTF8.GetBytes(APIReq);

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

        static string ValidateAppID(string AppID)
        {
            return Regex.IsMatch(AppID, "^[0-9]*$") ? AppID : Properties.Resources.DefaultAppID;
        }

        static void APICreateAccount(string AppID)
        {
            Console.Write(Properties.Resources.MsgGenTokenProgress);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendAPIRequest(Properties.Resources.APICreateAccountURI, String.Format("appid={0}&key={1}", AppID, Properties.Settings.Default.APIKey)));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgResGenAccount, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, AppID);
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static void APIGetAccountList()
        {
            Console.Write(Properties.Resources.MsgFetchListProgress);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(FetchStringHTTP(String.Format(Properties.Resources.APIGetAccountListURI, Properties.Settings.Default.APIKey)));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgResGenAccount, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("appid")[i].InnerText);
                    Console.WriteLine();
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static void APIGetServerSteamIDsByIP(string Rx)
        {
            if (Regex.IsMatch(Rx, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$"))
            {
                Console.Write(Properties.Resources.MsgGetIDProgress);
                try
                {
                    XmlDocument XMLD = new XmlDocument();
                    XMLD.LoadXml(FetchStringHTTP(String.Format(Properties.Resources.APIGetServerSteamIDsByIPURI, Properties.Settings.Default.APIKey, Rx)));
                    Console.WriteLine(" Done.{0}", Environment.NewLine);
                    XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                    for (int i = 0; i < XMLNList.Count; i++)
                    {
                        Console.WriteLine(Properties.Resources.MsgGetIDResult, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                    }
                }
                catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
            }
            else { Console.WriteLine(Properties.Resources.MsgGetIDWrongInput); }
        }

        static void APIResetLoginToken(string ServerID)
        {
            Console.Write(Properties.Resources.MsgResetRequest, ServerID);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendAPIRequest(Properties.Resources.APIResetTokenURI, String.Format("steamid={0}&key={1}", ServerID, Properties.Settings.Default.APIKey)));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgResetResult, XMLD.GetElementsByTagName("login_token")[i].InnerText);
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static void APIGetServerIPsBySteamID(string ServerID)
        {
            Console.Write(Properties.Resources.MsgGetIPProgress);
            try
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(FetchStringHTTP(String.Format(Properties.Resources.APIGetServerIPsBySteamIDURI, Properties.Settings.Default.APIKey, ServerID)));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgGetIPResult, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                }
            }
            catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }
        }

        static void APISetMemo(string ServerID, string Memo)
        {
            Console.Write(Properties.Resources.MsgSetMemoProgress, ServerID);
            try
            {
                SendAPIRequest(Properties.Resources.APISetMemoURI, String.Format("steamid={0}&key={1}&memo={2}", ServerID, Properties.Settings.Default.APIKey, Memo));
                Console.WriteLine(" Done.{0}", Environment.NewLine);
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
                        case "generate": APICreateAccount((Args.Count() >= 2) ? ValidateAppID(Args[1]) : Properties.Resources.DefaultAppID);
                            break;
                        case "list": APIGetAccountList();
                            break;
                        case "version": Console.WriteLine(Properties.Resources.AppVerStr, Properties.Resources.AppName, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                            break;
                        case "getid": if (Args.Count() >= 2) { APIGetServerSteamIDsByIP(Args[1]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                            break;
                        case "reset": if (Args.Count() >= 2) { APIResetLoginToken(Args[1]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                            break;
                        case "getip": if (Args.Count() >= 2) { APIGetServerIPsBySteamID(Args[1]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                            break;
                        case "comment": if (Args.Count() >= 3) { APISetMemo(Args[1], Args[2]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                            break;
                        default: Console.WriteLine(Properties.Resources.MsgErrUnknownOption);
                            break;
                    }
                }
                else { Console.WriteLine(Properties.Resources.WlxMsg); }
            }
            else { Console.WriteLine(Properties.Resources.MsgErrNoKey); }

            #if DEBUG
            Console.ReadKey();
            #endif
        }
    }
}
