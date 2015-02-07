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
            Console.Clear();
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
            return Regex.IsMatch(AppID, Properties.Resources.RegexAppID) ? AppID : Properties.Resources.DefaultAppID;
        }

        static bool ValidateAPIKey(string APIKey)
        {
            return Regex.IsMatch(APIKey, Properties.Resources.RegexAPIKey);
        }

        static void Route(string[] Args)
        {
            switch (Args[0])
            {
                case "generate": APICreateAccount(Properties.Settings.Default.APIKey, (Args.Count() >= 2) ? ValidateAppID(Args[1]) : Properties.Resources.DefaultAppID);
                    break;
                case "list": APIGetAccountList(Properties.Settings.Default.APIKey);
                    break;
                case "version": Console.WriteLine(Properties.Resources.AppVerStr, Properties.Resources.AppName, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    break;
                case "getid": if (Args.Count() >= 2) { APIGetServerSteamIDsByIP(Properties.Settings.Default.APIKey, Args[1]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                    break;
                case "reset": if (Args.Count() >= 2) { APIResetLoginToken(Properties.Settings.Default.APIKey, Args[1]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                    break;
                case "getip": if (Args.Count() >= 2) { APIGetServerIPsBySteamID(Properties.Settings.Default.APIKey, Args[1]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                    break;
                case "setmemo": if (Args.Count() >= 3) { APISetMemo(Properties.Settings.Default.APIKey, Args[1], Args[2]); } else { Console.WriteLine(Properties.Resources.MsgErrNotEnough); }
                    break;
                default: Console.WriteLine(Properties.Resources.MsgErrUnknownOption);
                    break;
            }
        }

        static void APICreateAccount(string APIKey, string AppID)
        {
            if (!ValidateAPIKey(APIKey)) { throw new ArgumentException(Properties.Resources.MsgErrNoKey); }
            Console.Write(Properties.Resources.MsgGenTokenProgress);
            XmlDocument XMLD = new XmlDocument();
            XMLD.LoadXml(SendPOSTRequest(Properties.Resources.APICreateAccountURI, String.Format(Properties.Resources.APICreateAccountParam, AppID, APIKey)));
            Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
            XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
            for (int i = 0; i < XMLNList.Count; i++)
            {
                Console.WriteLine(Properties.Resources.MsgResGenAccount, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, AppID);
            }
        }

        static void APIGetAccountList(string APIKey)
        {
            if (!ValidateAPIKey(APIKey)) { throw new ArgumentException(Properties.Resources.MsgErrNoKey); }
            Console.Write(Properties.Resources.MsgFetchListProgress);
            XmlDocument XMLD = new XmlDocument();
            XMLD.LoadXml(SendGETRequest(String.Format(Properties.Resources.APIGetAccountListURI, APIKey)));
            Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
            XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
            for (int i = 0; i < XMLNList.Count; i++)
            {
                Console.WriteLine(Properties.Resources.MsgResGenAccount, XMLD.GetElementsByTagName("steamid")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("login_token")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("appid")[i].InnerText);
                try { Console.WriteLine(Properties.Resources.MsgMemoGenAccount, XMLD.GetElementsByTagName("memo")[i].InnerText); } catch { /* Do nothing... */ }
                Console.WriteLine();
            }
        }

        static void APIGetServerSteamIDsByIP(string APIKey, string Rx)
        {
            if (!ValidateAPIKey(APIKey)) { throw new ArgumentException(Properties.Resources.MsgErrNoKey); }
            if (Regex.IsMatch(Rx, Properties.Resources.RegexIPAddress))
            {
                Console.Write(Properties.Resources.MsgGetIDProgress);
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendGETRequest(String.Format(Properties.Resources.APIGetServerSteamIDsByIPURI, APIKey, Rx)));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgGetIDResult, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                }
            }
            else { Console.WriteLine(Properties.Resources.MsgIPAddrWrongInput); }
        }

        static void APIResetLoginToken(string APIKey, string ServerID)
        {
            if (!ValidateAPIKey(APIKey)) { throw new ArgumentException(Properties.Resources.MsgErrNoKey); }
            if (Regex.IsMatch(ServerID, Properties.Resources.RegexServerID))
            {
                Console.Write(Properties.Resources.MsgResetRequest, ServerID);
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendPOSTRequest(Properties.Resources.APIResetTokenURI, String.Format(Properties.Resources.APIResetLoginTokenParam, ServerID, APIKey)));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("response");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgResetResult, XMLD.GetElementsByTagName("login_token")[i].InnerText);
                }
            }
            else { Console.WriteLine(Properties.Resources.MsgServerIDWrongInput); }
        }

        static void APIGetServerIPsBySteamID(string APIKey, string ServerID)
        {
            if (!ValidateAPIKey(APIKey)) { throw new ArgumentException(Properties.Resources.MsgErrNoKey); }
            if (Regex.IsMatch(ServerID, Properties.Resources.RegexServerID))
            {
                Console.Write(Properties.Resources.MsgGetIPProgress);
                XmlDocument XMLD = new XmlDocument();
                XMLD.LoadXml(SendGETRequest(String.Format(Properties.Resources.APIGetServerIPsBySteamIDURI, APIKey, ServerID)));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("message");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    Console.WriteLine(Properties.Resources.MsgGetIPResult, XMLD.GetElementsByTagName("addr")[i].InnerText, Environment.NewLine, XMLD.GetElementsByTagName("steamid")[i].InnerText);
                }
            }
            else { Console.WriteLine(Properties.Resources.MsgServerIDWrongInput); }
        }

        static void APISetMemo(string APIKey, string ServerID, string Memo)
        {
            if (!ValidateAPIKey(APIKey)) { throw new ArgumentException(Properties.Resources.MsgErrNoKey); }
            if (Regex.IsMatch(ServerID, Properties.Resources.RegexServerID))
            {
                Console.Write(Properties.Resources.MsgSetMemoProgress, ServerID);
                SendPOSTRequest(Properties.Resources.APISetMemoURI, String.Format(Properties.Resources.APISetMemoParam, ServerID, APIKey, Memo));
                Console.WriteLine(Properties.Resources.MsgResDone, Environment.NewLine);
            }
            else { Console.WriteLine(Properties.Resources.MsgServerIDWrongInput); }
        }

        static void Main(string[] Args)
        {
            try { ConfigureConsole(Properties.Resources.AppName, ConsoleColor.Green); ShowSplash(Properties.Resources.RFileNameWelcome); } catch (Exception Ex) { Console.WriteLine(Properties.Resources.MsgGeneralException, Environment.NewLine, Ex.Message); }

            if (Args.Count() > 0)
            {
                try { Route(Args); }
                catch (Exception Ex) { Console.WriteLine(Properties.Resources.MsgGeneralException, Environment.NewLine, Ex.Message); }
            }
            else { ShowSplash(Properties.Resources.RFileNameSyntax); }

            #if DEBUG
            Console.ReadKey();
            #endif
        }
    }
}
