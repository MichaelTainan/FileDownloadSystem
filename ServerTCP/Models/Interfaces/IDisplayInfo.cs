namespace ServerTCP.Models.Interfaces
{
    public interface IDisplayInfo
    {
        string GetClientFileName();
        string GetClientIP();
        int GetClientPort();
        byte[] SendFile();
    }
}