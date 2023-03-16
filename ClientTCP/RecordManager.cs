using System;
using ClientTCP.Interfaces;

namespace ClientTCP
{
    public class RecordManager : IRecordManager
    {
        private ServerInfo serverInfo;
        public RecordManager(ServerInfo serverinfo)
        {
            this.serverInfo = serverinfo;
        }

        public ServerInfo GetServerInfo()
        {
            return serverInfo;
        }

        public void SaveServerInfoFromClient(ServerInfo fromClientInfo)
        {
            //serverInfo = fromClientInfo;
            serverInfo.IP = string.IsNullOrEmpty(fromClientInfo.IP) ? "" : fromClientInfo.IP;
            serverInfo.Port = fromClientInfo.Port;
            serverInfo.FileName = string.IsNullOrEmpty(fromClientInfo.FileName) ? "" : fromClientInfo.FileName;
            serverInfo.SaveAs = string.IsNullOrEmpty(fromClientInfo.SaveAs) ? "" : fromClientInfo.SaveAs;
        }

        public void SaveServerInfoFromServer(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                serverInfo.Message += message + "\n";
            }
        }
    }
}