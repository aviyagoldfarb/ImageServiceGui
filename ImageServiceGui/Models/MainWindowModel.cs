using ImageServiceGui.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    public class MainWindowModel
    {
        //INotifyPropertyChanged implementation: 
        public event PropertyChangedEventHandler PropertyChanged;

        private ITcpClient tcpClient;

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        public MainWindowModel()
        {
            this.tcpClient = ServiceTcpClient.Instance;
            isConnected = this.tcpClient.Connected();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
