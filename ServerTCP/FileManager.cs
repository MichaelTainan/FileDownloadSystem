using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerTCP
{
    public class FileManager
    {
        private byte[] file;
        private string filePath;
        private readonly string uploadDirectory;

        public FileManager()
        {
            uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
        }

        public byte[] SendFile(string fileName)
        {
            if (FindFile(fileName))
            {
                file = File.ReadAllBytes(filePath);
                return file;
            }
            else
            {
                return null;
            }
        }

        public bool FindFile(string fileName)
        {
            filePath = Path.Combine(uploadDirectory, fileName);
            return (File.Exists(filePath));
        }

        public string FindFilePath(string fileName)
        {
            filePath = Path.Combine(uploadDirectory, fileName);
            return (filePath);
        }
    }
}
