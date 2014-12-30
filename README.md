Intro
===
Simple Server Manager can be used to generate, reset or view tokens for Steam game servers. This tool is completely free and open-source.

Getting started
===
Open `stm.exe.config` in text editor and enter your Steam Web API key. You can find or get it on this page: http://steamcommunity.com/dev/apikey.

Syntax
===
 * [generate] - generate a new pair of ServerID and token;
 * [list] - list all registered servers on current Steam account;
 * [version] - show application version information;
 * [getid IP] - resolve ServerID from IP address (example: 127.0.0.1:27015);
 * [getip ServerID] - resolve IP address from ServerID;
 * [reset ServerID] - generate new token for selected ServerID;
 * [setmemo ServerID "Message"] - set small comment for selected ServerID.

License
===
This program is licensed under the terms of General Public License version 3 (GNU GPLv3). Copy can be found in `COPYING.txt` file.
