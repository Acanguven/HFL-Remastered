using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace HFL_3._0
{
    public class SocketManager
    {
        public static Bridge luaBridge = new Bridge();

        public SocketManager()
        {
            luaBridge.luaRecieved += messageHandler;
        }

        public void messageHandler(Socket client, LuaMessage msg)
        {
            switch (msg.command)
            {
                case "validation":
                    if (App.gameMask.gameExist(msg.ProcessID))
                    {
                        App.gameMask.workOn(msg.ProcessID);
                        luaBridge.sendAnswer(client, "valid success");
                    }
                    else
                    {
                        luaBridge.sendAnswer(client, "valid fail");
                    }
                    break;
                case "gameended":

                    break;
            }
        }
    }
}
