using System.ComponentModel;
using ServerTCP.Models;

namespace ServerTCP.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly ClientInfo clientInfo;

        public ClientViewModel(ClientInfo clientInfo)
        {
            this.clientInfo = clientInfo;
        }

        public string IP
        {
            get { return clientInfo.IP; }
        }

        public int Port
        {
            get { return clientInfo.Port; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    RaisePropertyChanged("FileName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            //equals if(PropertyChanged !=null){} sentence
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}