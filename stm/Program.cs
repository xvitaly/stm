using System;
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

        static string SendPOSTRequest(string APIURL, string APIReq)
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

            if ((HTTPWResp.StatusCode == HttpStatusCode.OK) && HTTPWResp.Headers.Get("X-eresult") == "1")
            {
                using (Stream RespStream = HTTPWResp.GetResponseStream())
                {
                    using (StreamReader StrRead = new StreamReader(RespStream))
                    {
                        Result = StrRead.ReadToEnd();
                    }
                }
            }
            else { throw new UnauthorizedAccessException(HTTPWResp.Headers.Get("X-error_message")); }

            return Result;
        }

        static string SendGETRequest(string StrF)
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

        static bool ValidateAPIKey(string APIKey)
        {
            return Regex.IsMatch(APIKey, "^[0-9A-F]*$");
        }

        static void Route(string[] Args)
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
                case "setmemo": if (Args.Count() >= 3) { APISetMemo(Args[1], Args[2]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                    break;
                default: Console.WriteLine(Properties.Resources.MsgErrUnknownOption);
                    break;
            }
        }

        static void APICreateAccount(string AppID)
        {
            Console.Write(Properties.Resources.MsgGenTokenProgress);
            XmlDocument XMLD = new XmlDocument();
            XMLD.LoadXml(SendPOSTRequest(Properties.Resources.APICreateAccountURI, String.Format(Properties.Resources.APICreateAccountParam, AppID, Properties.Settings.Default.APIKey)));
            Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
            XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
            for (int i = 0; i < XMLNList.Count; i++)
            {
                Console.WriteLine(Properties.Resources.MsgResGenAccount, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, AppID);
            }
        }

        static void APIGetAccountList()
        {
            Console.Write(Properties.Resources.MsgFetchListProgress);
            XmlDocument XMLD = new XmlDocument();
            XMLD.LoadXml(SendGETRequest(String.Format(Properties.Resources.APIGetAccountListURI, Properties.Settings.Default.APIKey)));
            Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
            XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
            for (int i = 0; i < XMLNList.Count; i++)
            {
                Console.WriteLine(Properties.Resources.MsgResGenAccount, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("appid")[i].InnerText);
                try { Console.WriteLine(Properties.Resources.MsgMemoGenAccount, XMLD.GetElementsByTagName("memo")[i].InnerText); } catch { /* Do nothing... */ }
                Console.WriteLine();
            }
        }

        static void APIGetServerSteamIDsByIP(string Rx)
        {
            if (Regex.IsMatch(Rx, Properties.Resources.RegexIPAddress))
            {
                Console.Write(Properties.Resources.MsgGetIDProgress);
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendGETRequest(String.Format(Properties.Resources.APIGetServerSteamIDsByIPURI, Properties.Settings.Default.APIKey, Rx)));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgGetIDResult, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                }
            }
            else { Console.WriteLine(Properties.Resources.MsgIPAddrWrongInput); }
        }

        static void APIResetLoginToken(string ServerID)
        {
            if (Regex.IsMatch(ServerID, Properties.Resources.RegexServerID))
            {
                Console.Write(Properties.Resources.MsgResetRequest, ServerID);
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendPOSTRequest(Properties.Resources.APIResetTokenURI, String.Format(Properties.Resources.APIResetLoginTokenParam, ServerID, Properties.Settings.Default.APIKey)));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgResetResult, XMLD.GetElementsByTagName("login_token")[i].InnerText);
                }
            }
            else { Console.WriteLine(Properties.Resources.MsgServerIDWrongInput); }
        }

        static void APIGetServerIPsBySteamID(string ServerID)
        {
            if (Regex.IsMatch(ServerID, Properties.Resources.RegexServerID))
            {
                Console.Write(Properties.Resources.MsgGetIPProgress);
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendGETRequest(String.Format(Properties.Resources.APIGetServerIPsBySteamIDURI, Properties.Settings.Default.APIKey, ServerID)));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgGetIPResult, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                }
            }
            else { Console.WriteLine(Properties.Resources.MsgServerIDWrongInput); }
        }

        static void APISetMemo(string ServerID, string Memo)
        {
            if (Regex.IsMatch(ServerID, Properties.Resources.RegexServerID))
            {
                Console.Write(Properties.Resources.MsgSetMemoProgress, ServerID);
                SendPOSTRequest(Properties.Resources.APISetMemoURI, String.Format(Properties.Resources.APISetMemoParam, ServerID, Properties.Settings.Default.APIKey, Memo));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
            }
            else { Console.WriteLine(Properties.Resources.MsgServerIDWrongInput); }
        }

        static void Main(string[] Args)
        {
            ConfigureConsole(Properties.Resources.AppName, ConsoleColor.Green);
            try { ShowSplash(Properties.Resources.RFileNameWelcome); } catch (Exception Ex) { Console.WriteLine("{0}{1}", Environment.NewLine, Ex.Message); }

            if (ValidateAPIKey(Properties.Settings.Default.APIKey))
            {
                if (Args.Count() > 0)
                {
                    try { Route(Args); } catch (Exception Ex) { Console.WriteLine(Properties.Resources.MsgGeneralException, Environment.NewLine, Ex.Message); }
                }
                else { ShowSplash(Properties.Resources.RFileNameSyntax); }
            }
            else { Console.WriteLine(Properties.Resources.MsgErrNoKey); }

            #if DEBUG
            Console.ReadKey();
            #endif
        }
    }
}
