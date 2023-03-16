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
        private IListenManager listenManager;
        private ObservableCollection<ClientViewModel> clients;

        public event PropertyChangedEventHandler PropertyChanged;

        public ServerViewModel(IListenManager listenManager)
        {
            this.listenManager = listenManager;
            clients = new ObservableCollection<ClientViewModel>();
            StartCommand = new RelayCommand(Start, CanStart);
            StopCommand = new RelayCommand(Stop, CanStop);
            listenManager.ClientConnected += ListenManager_ClientConnected;
            listenManager.ClientDisconnected += ListenManager_Disconnected;
        }

        public void ListenManager_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            AddClient(e.ClientInfo);
            UpdateClient(e.ClientInfo);
        }

        private void ListenManager_Disconnected(object sender, ClientConnectedEventArgs e)
        {
            var clientToRemove = clients.FirstOrDefault
                             (c => c.IP == e.ClientInfo.IP && c.Port == e.ClientInfo.Port);
            if (clientToRemove != null)
            {
                //clients.Remove(clientToRemove);
                RemoveClient(e.ClientInfo);
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
                foreach (var client in clients)
                {
                    if (client.IP == clientInfo.IP && client.Port == clientInfo.Port)
                    {
                        return;
                    }
                }
                clients.Add(new ClientViewModel(clientInfo));
            });
        }

        private void UpdateClient(ClientInfo clientInfo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var client in clients)
                {
                    if (client.IP == clientInfo.IP && client.Port == clientInfo.Port)
                    {
                        client.FileName = clientInfo.FileName;
                        break;
                    }
                }
            });
        }

        private void RemoveClient(ClientInfo clientInfo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var client in clients)
                {
                    if (client.IP == clientInfo.IP && client.Port == clientInfo.Port)
                    {
                        clients.Remove(client);
                        break;
                    }
                }
            });
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //public class ClientInfoEventArgs : EventArgs
    //{
    //    public ClientInfo ClientInfo { get; set; }
    //    public ClientInfoEventArgs(ClientInfo clientInfo)
    //    {
    //        ClientInfo = clientInfo;
    //    }
    //}

    internal class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
