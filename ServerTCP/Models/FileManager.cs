using System;
using System.IO;
using ServerTCP.Models;
using System.Net.Sockets;
using System.Text;
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
        /// Change The file to become a byte[] type.
        /// </summary>
        /// <param name="fileName">The file Name want to be sent.</param>
        /// <returns>file byte[] type or null</returns>
        public byte[] ChangeFileBeByteArray(string fileName)
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
       
        /// <summary>
        /// Combine file content and filename and 0 mumber to client site
        /// </summary>
        /// <param name="fileName">The file name want to combine after file content</param>
        /// <returns>retun combine file content and filename and 0 byte array data or null</returns>
        public byte[] CombineFileContentAndName(string fileName)
        {
            if (ChangeFileBeByteArray(fileName) == null)
            {
                return null;
            }
            else
            {
                try
                {
                    byte[] fileData = ChangeFileBeByteArray(fileName);
                    //Combine file content and file name to together
                    byte[] buffer = new byte[fileData.Length + fileName.Length + 1];
                    Array.Copy(Encoding.UTF8.GetBytes(fileName), buffer, fileName.Length);
                    buffer[fileName.Length] = 0; // Add 0 after the filName to be a separator
                    Array.Copy(fileData, 0, buffer, fileName.Length + 1, fileData.Length);

                    return buffer;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"SendFile failed: {e.Message}");
                    return null;
                }
            }


        }
    }
}
