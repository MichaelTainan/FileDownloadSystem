using ClientTCP.Interfaces;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ClientTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {                
        private ServerInfo serverInfo;
        private IRecordManager recordManager;
        private IConnectManager connectManager;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            serverInfo = new ServerInfo 
            {
                IP = "",
                Port = 0,
                FileName = "",
                SaveAs = "",
                Message = ""
            };
            recordManager = new RecordManager(serverInfo);
            connectManager = new ConnectManager(serverInfo);

            connectManager.ServerConnected += ConnectManager_ServerConnected;
            connectManager.ServerDisconnected += ConnectManager_Disconnected;
            
            InitializeComponent();
            SaveAsTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            recordManager.SaveServerInfoFromClient(serverInfo);
        }
        
        public void ConnectManager_ServerConnected(object sender, ServerConnectedEventArgs e)
        {
            recordManager.SaveServerInfoFromServer(e.ServerInfo.Message);
            SyncServerInfo();
            UpdateMessageTextBox();
        }

        private void ConnectManager_Disconnected(object sender, ServerConnectedEventArgs e)
        {
            recordManager.SaveServerInfoFromServer(e.ServerInfo.Message);
            SyncServerInfo();
            UpdateMessageTextBox();
            connectManager.Disconnect();
        }

        public void TextChangedEvent(object sender, TextChangedEventArgs e)
        {
           SaveRecordToServerInfo();
        }

        public void SaveRecordToServerInfo()
        {
            if (IpTextBox != null) {
                serverInfo.IP = IpTextBox.Text;
            };
            if (PortTextBox != null)
            {
                serverInfo.Port = string.IsNullOrEmpty(PortTextBox.Text) ? 0 : int.Parse(PortTextBox.Text);
            }
            if (FileNameTextBox != null)
            {
                serverInfo.FileName = FileNameTextBox.Text;
            }
            if (SaveAsTextBox != null)
            {
                serverInfo.SaveAs = SaveAsTextBox.Text;
            }
            //serverInfo.Message = MessageTextBox.Text;
            //if (recordManager != null)
            //{
            //    recordManager.SaveServerInfoFromClient(serverInfo);
            //}
            recordManager.SaveServerInfoFromClient(serverInfo);
        }

        private void UpdateMessageTextBox()
        {
            MessageTextBox.Dispatcher.Invoke(() =>
            {
                MessageTextBox.Text = serverInfo.Message;
                MessageTextBox.ScrollToEnd();
            });
        }

        public void ConnectToServerTCP(object sender, RoutedEventArgs e) 
        {
            connectManager.SyncServerInfo(serverInfo);
            connectManager.ConnectAsync();
        }

        public void DownlocadRequestToServerTCP(object sender, RoutedEventArgs e) 
        { 
            connectManager?.SyncServerInfo(serverInfo);
            connectManager?.SendMessage($"Download the file:{serverInfo.FileName}");
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "請選擇下載的資料夾";
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "選擇資料夾";
            dialog.ValidateNames = false;
            dialog.DefaultExt = "選擇資料夾";
            dialog.Filter = "只能選擇資料夾.|*.this.directory.please";

            if (dialog.ShowDialog() == true)
            {
                serverInfo.SaveAs = System.IO.Path.GetDirectoryName(dialog.FileName);
                SaveAsTextBox.Text = serverInfo.SaveAs;
                recordManager.SaveServerInfoFromClient(serverInfo);
            }
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SyncServerInfo() 
        {
            this.serverInfo = recordManager.GetServerInfo();
        }
    }
}
