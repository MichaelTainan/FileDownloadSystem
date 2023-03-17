namespace ServerTCP.Models.Interfaces
{
    public interface IFileManager
    {
        bool FindFile(string fileName);
        string CombineFilePath(string fileName);
        byte[] ChangeFileBeByteArray(string fileName);
    }
}