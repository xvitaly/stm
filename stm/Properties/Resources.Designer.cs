﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1022
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stm.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("stm.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to appid={0}&amp;key={1}.
        /// </summary>
        internal static string APICreateAccountParam {
            get {
                return ResourceManager.GetString("APICreateAccountParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.steampowered.com/IGameServersService/CreateAccount/v0001/?format=xml.
        /// </summary>
        internal static string APICreateAccountURI {
            get {
                return ResourceManager.GetString("APICreateAccountURI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.steampowered.com/IGameServersService/GetAccountList/v0001/?key={0}&amp;format=xml.
        /// </summary>
        internal static string APIGetAccountListURI {
            get {
                return ResourceManager.GetString("APIGetAccountListURI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.steampowered.com/IGameServersService/GetServerIPsBySteamID/v0001/?key={0}&amp;server_steamids[0]={1}&amp;format=xml.
        /// </summary>
        internal static string APIGetServerIPsBySteamIDURI {
            get {
                return ResourceManager.GetString("APIGetServerIPsBySteamIDURI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.steampowered.com/IGameServersService/GetServerSteamIDsByIP/v0001/?key={0}&amp;server_ips[0]={1}&amp;format=xml.
        /// </summary>
        internal static string APIGetServerSteamIDsByIPURI {
            get {
                return ResourceManager.GetString("APIGetServerSteamIDsByIPURI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to steamid={0}&amp;key={1}.
        /// </summary>
        internal static string APIResetLoginTokenParam {
            get {
                return ResourceManager.GetString("APIResetLoginTokenParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.steampowered.com/IGameServersService/ResetLoginToken/v0001/?format=xml.
        /// </summary>
        internal static string APIResetTokenURI {
            get {
                return ResourceManager.GetString("APIResetTokenURI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to steamid={0}&amp;key={1}&amp;memo={2}.
        /// </summary>
        internal static string APISetMemoParam {
            get {
                return ResourceManager.GetString("APISetMemoParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.steampowered.com/IGameServersService/SetMemo/v0001/.
        /// </summary>
        internal static string APISetMemoURI {
            get {
                return ResourceManager.GetString("APISetMemoURI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Simple Server Manager by EasyCoding Team.
        /// </summary>
        internal static string AppName {
            get {
                return ResourceManager.GetString("AppName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}. Version: {1}..
        /// </summary>
        internal static string AppVerStr {
            get {
                return ResourceManager.GetString("AppVerStr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 440.
        /// </summary>
        internal static string DefaultAppID {
            get {
                return ResourceManager.GetString("DefaultAppID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Steam API key entered in config file. Please open stm.exe.config in text editor and enter your API key. You can find it one on this page: http://steamcommunity.com/dev/apikey..
        /// </summary>
        internal static string MsgErrNoKey {
            get {
                return ResourceManager.GetString("MsgErrNoKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: not enough options for selected method!.
        /// </summary>
        internal static string MsgErrNotEnough {
            get {
                return ResourceManager.GetString("MsgErrNotEnough", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown option. Start program without any options to view help..
        /// </summary>
        internal static string MsgErrUnknownOption {
            get {
                return ResourceManager.GetString("MsgErrUnknownOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fetching list of registered servers....
        /// </summary>
        internal static string MsgFetchListProgress {
            get {
                return ResourceManager.GetString("MsgFetchListProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}Exception detected: {1}.
        /// </summary>
        internal static string MsgGeneralException {
            get {
                return ResourceManager.GetString("MsgGeneralException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generating new ServerID and login token pair....
        /// </summary>
        internal static string MsgGenTokenProgress {
            get {
                return ResourceManager.GetString("MsgGenTokenProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to get ServerID from requested IP....
        /// </summary>
        internal static string MsgGetIDProgress {
            get {
                return ResourceManager.GetString("MsgGetIDProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IP: {0}{1}ServerID: {2}.
        /// </summary>
        internal static string MsgGetIDResult {
            get {
                return ResourceManager.GetString("MsgGetIDResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to get IP address from requested ServerID....
        /// </summary>
        internal static string MsgGetIPProgress {
            get {
                return ResourceManager.GetString("MsgGetIPProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IP: {0}{1}ServerID: {2}.
        /// </summary>
        internal static string MsgGetIPResult {
            get {
                return ResourceManager.GetString("MsgGetIPResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong input. The second argument must be a valid IP address with port. Example: 127.0.0.1:27015..
        /// </summary>
        internal static string MsgIPAddrWrongInput {
            get {
                return ResourceManager.GetString("MsgIPAddrWrongInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Memo: {0}.
        /// </summary>
        internal static string MsgMemoGenAccount {
            get {
                return ResourceManager.GetString("MsgMemoGenAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  Done.{0}.
        /// </summary>
        internal static string MsgResDone {
            get {
                return ResourceManager.GetString("MsgResDone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Requesting new login token for {0}....
        /// </summary>
        internal static string MsgResetRequest {
            get {
                return ResourceManager.GetString("MsgResetRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New login token: {0}.
        /// </summary>
        internal static string MsgResetResult {
            get {
                return ResourceManager.GetString("MsgResetResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ServerID: {0}{1}Login token: {2}{3}AppID: {4}.
        /// </summary>
        internal static string MsgResGenAccount {
            get {
                return ResourceManager.GetString("MsgResGenAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong input. The second argument must be a valid ServerID. Example: 012345678901234567..
        /// </summary>
        internal static string MsgServerIDWrongInput {
            get {
                return ResourceManager.GetString("MsgServerIDWrongInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Setting new memo for server {0}....
        /// </summary>
        internal static string MsgSetMemoProgress {
            get {
                return ResourceManager.GetString("MsgSetMemoProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[0-9A-F]{32}$.
        /// </summary>
        internal static string RegexAPIKey {
            get {
                return ResourceManager.GetString("RegexAPIKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[0-9]*$.
        /// </summary>
        internal static string RegexAppID {
            get {
                return ResourceManager.GetString("RegexAppID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$.
        /// </summary>
        internal static string RegexIPAddress {
            get {
                return ResourceManager.GetString("RegexIPAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[0-9]{17}$.
        /// </summary>
        internal static string RegexServerID {
            get {
                return ResourceManager.GetString("RegexServerID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to stm.Resources.Syntax.txt.
        /// </summary>
        internal static string RFileNameSyntax {
            get {
                return ResourceManager.GetString("RFileNameSyntax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to stm.Resources.WelcomeMsg.txt.
        /// </summary>
        internal static string RFileNameWelcome {
            get {
                return ResourceManager.GetString("RFileNameWelcome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Syntax:
        ///  [generate] - generate a new pair of ServerID and token;
        ///  [list] - list all registered servers on current Steam account;
        ///  [version] - show application version information;
        ///  [getid IP] - resolve ServerID from IP address (example: 127.0.0.1:27015);
        ///  [getip ServerID] - resolve IP address from ServerID;
        ///  [reset ServerID] - generate new token for selected ServerID;
        ///  [setmemo ServerID &quot;Message&quot;] - set small comment for selected ServerID..
        /// </summary>
        internal static string Syntax {
            get {
                return ResourceManager.GetString("Syntax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mozilla/5.0 (Windows NT 6.1; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0.
        /// </summary>
        internal static string UserAgent {
            get {
                return ResourceManager.GetString("UserAgent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ###########################################################################
        ///#                    WELCOME TO SIMPLE SERVER MANAGER!                    #
        ///#         This console program will fetch or list tokens via API.         #
        ///#                                                                         #
        ///#          (C) 2005 - 2015 EasyCoding Team. All rights reserved.          #
        ///#             Original author: V1TSK (vitaly@easycoding.org).             #
        ///#                                                  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WelcomeMsg {
            get {
                return ResourceManager.GetString("WelcomeMsg", resourceCulture);
            }
        }
    }
}
