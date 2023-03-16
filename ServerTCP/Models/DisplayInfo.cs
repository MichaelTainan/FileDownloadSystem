using ServerTCP.Models;
using ServerTCP.Models.Interfaces;

namespace ServerTCP
{
    public class DisplayInfo : IDisplayInfo
    {
        private ClientInfo clientInfo;
        private IFileManager fileManager;

        public DisplayInfo(ClientInfo client)
        {
            clientInfo = client;
            fileManager = new FileManager();
        }
        public string GetClientIP()
        {
            return clientInfo.IP;
        }

        public int GetClientPort()
        {
            return clientInfo.Port;
        }

        public string GetClientFileName()
        {
            return clientInfo.FileName;
        }

        public byte[] SendFile()
        {
            byte[] file = fileManager.SendFile(clientInfo.FileName);
            return file;

        }
    }
}