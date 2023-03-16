using System.IO;
using ServerTCP.Models.Interfaces;

namespace ServerTCP
{
    /// <summary>
    /// This class manages file operations, include about finding the file 
    /// and covering the file to be a byte array type.
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly string uploadDirectory;

        public FileManager()
        {
            uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
        }
        /// <summary>
        /// Change The file that containing file path and file name to be of type byte[].
        /// </summary>
        /// <param name="fileName">The file Name want to be sent.</param>
        /// <returns>file byte[] type or null</returns>
        public byte[] ChangeFileBeByteType(string fileName)
        {
            return FindFile(fileName) ? File.ReadAllBytes(CombineFilePath(fileName)) : null;
        }

        /// <summary>
        /// Find the file if in the filPath.
        /// </summary>
        /// <param name="fileName">The file name want to be finded</param>
        /// <returns>ture = find the filee or false = didn't find the file</returns>
        public bool FindFile(string fileName)
        {
            return (File.Exists(CombineFilePath(fileName)));
        }

        /// <summary>
        /// Comine the path and file name.
        /// </summary>
        /// <param name="fileName">The file name want to be merged.</param>
        /// <returns>file path</returns>
        public string CombineFilePath(string fileName)
        {
            return Path.Combine(uploadDirectory, fileName);
        }
    }
}
