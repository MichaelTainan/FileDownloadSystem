namespace ClientTCP.Interfaces
{
    public interface IRecordManager
    {
        ServerInfo GetServerInfo();
        void SaveServerInfoFromClient(ServerInfo fromClientInfo);
        void SaveServerInfoFromServer(string message);
    }
}