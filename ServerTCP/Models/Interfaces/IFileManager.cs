﻿namespace ServerTCP.Models.Interfaces
{
    public interface IFileManager
    {
        bool FindFile(string fileName);
        string CombineFilePath(string fileName);
        byte[] ChangeFileBeByteType(string fileName);
    }
}