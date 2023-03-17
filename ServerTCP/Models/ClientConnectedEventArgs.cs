using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP.Models
{
    /// <summary>
    /// When a new client connect success then create the event data.
    /// </summary>
    public class ClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Get set new clinet information.
        /// </summary>
        public ClientInfo ClientInfo { get; set; }

        /// <summary>
        /// initialize ClientConnectedEventArgs object and set up new clinet info.
        /// </summary>
        /// <param name="clientInfo"></param>
        public ClientConnectedEventArgs(ClientInfo clientInfo)
        {
            this.ClientInfo = clientInfo;
        }
    }
}
