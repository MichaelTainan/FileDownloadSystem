namespace ServerTCP.Models.Interfaces
{
    public interface IFileManager
    {
        bool FindFile(string fileName);
        string FindFilePath(string fileName);
        byte[] SendFile(string fileName);
    }
}