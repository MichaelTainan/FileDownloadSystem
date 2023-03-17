namespace ServerTCP.Models
{
    /// <summary>
    /// Define the client information
    /// </summary>
    public struct ClientInfo
    {
        public string IP { get; set; } // client IP address
        public int Port { get; set; } // client Port number
        public string FileName { get; set; } // The file name client want to download.
    }
}