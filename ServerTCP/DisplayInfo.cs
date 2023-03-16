namespace ServerTCP
{
    public class DisplayInfo
    {
        private ClientInfo clientInfo;
        private FileManager fileManager;

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