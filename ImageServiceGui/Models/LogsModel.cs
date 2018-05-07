using ImageServiceGui.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    class LogsModel : ILogsModel
    {
        //INotifyPropertyChanged implementation: 
        public event PropertyChangedEventHandler PropertyChanged;

        private ITcpClient tcpClient;
        private volatile Boolean stop;

        private ObservableCollection<KeyValuePair<string, string>> logs;
        public ObservableCollection<KeyValuePair<string, string>> Logs
        {
            get { return logs; }
            set
            {
                logs = value;
                NotifyPropertyChanged("Logs");
            }
        }

        public LogsModel(ITcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            stop = false;
            logs = new ObservableCollection<KeyValuePair<string, string>>();
            logs.Add(new KeyValuePair<string, string>("INFO", "Message1"));
            logs.Add(new KeyValuePair<string, string>("INFO", "Message2"));
            logs.Add(new KeyValuePair<string, string>("ERROR ", "Message3"));
            logs.Add(new KeyValuePair<string, string>("WARNING ", "Message4"));
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
