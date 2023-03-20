using ServerTCP.Models;
using ServerTCP.Models.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ServerTCP.ViewModels
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        private readonly IListenManager listenManager;
        private readonly IClientManager clientManager;
        private ObservableCollection<ClientViewModel> clients;

        public event PropertyChangedEventHandler PropertyChanged;

        public ServerViewModel(IListenManager listenManager, IClientManager clientManager)
        {
            this.listenManager = listenManager;
            this.clientManager = clientManager;
            clients = new ObservableCollection<ClientViewModel>();
            StartCommand = new RelayCommand(Start, CanStart);
            StopCommand = new RelayCommand(Stop, CanStop);
            clientManager.AddClientInfo += ListenManager_ClientConnected;
            clientManager.updateClientInfo += ListenManager_ClientConnected;
            clientManager.RemoveClientInfo += ListenManager_Disconnected;
        }

        public void ListenManager_ClientConnected(object sender, ClientInfo e)
        {
            AddClient(e);
            UpdateClient(e);
        }

        private void ListenManager_Disconnected(object sender, ClientInfo e)
        {
            if (!string.IsNullOrEmpty(e.IP) && e.Port != 0)
            {
                RemoveClient(e);
            }
        }

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        public ObservableCollection<ClientViewModel> Clients
        {
            get { return clients; }
        }

        private bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    RaisePropertyChanged("IsRunning");
                }
            }
        }

        private void Start()
        {
            if (!isRunning)
            {
                listenManager.Start();
                IsRunning = true;
            }
        }

        private bool CanStart()
        {
            return !IsRunning;
        }

        private void Stop()
        {
            if (isRunning)
            {
                listenManager.Close();
                IsRunning = false;
            }
        }

        private bool CanStop()
        {
            return IsRunning;
        }

        private void AddClient(ClientInfo clientInfo)
        {
            if (clientInfo.FileName == null)
            {
                clientInfo.FileName = "";
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!clients.Any(x => x.IP == clientInfo.IP && x.Port == clientInfo.Port))
                {
                    clients.Add(new ClientViewModel(clientInfo));
                }
            });
        }

        private void UpdateClient(ClientInfo clientInfo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var client = clients.FirstOrDefault(x => x.IP == clientInfo.IP && x.Port == clientInfo.Port);
                if (client != null) 
                {
                    client.FileName = clientInfo.FileName;
                }
            });
        }

        private void RemoveClient(ClientInfo clientInfo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var client = clients.FirstOrDefault(x => x.IP == clientInfo.IP && x.Port == clientInfo.Port);
                if (client != null) 
                {
                    clients.Remove(client);
                }
            });
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
