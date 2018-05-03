using ImageServiceGui.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    class SettingsModel : ISettingsModel
    {
        //INotifyPropertyChanged implementation: 
        public event PropertyChangedEventHandler PropertyChanged;

        private ITcpClient tcpClient;
        private volatile Boolean stop;

        public SettingsModel(ITcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            stop = false;
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // connection to the service 
        public void Connect(string ip, int port)
        {
            tcpClient.Connect(ip, port);
        }

        public void Disconnect()
        {
            stop = true;
            tcpClient.Disconnect();
        }

        public void Start()
        {
            new Thread(delegate () {
                while (!stop)
                {
                    //tcpClient.write("get left sonar");
                    //LeftSonar = Double.Parse(tcpClient.read()); // the same for the other sensors properties
                    //Thread.Sleep(250);// read the data in 4Hz
                }
            }).Start();
        }
    }
}
