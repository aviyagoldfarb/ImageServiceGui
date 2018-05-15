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
    public class LogsModel : ILogsModel
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

        public LogsModel()
        {
            this.tcpClient = ServiceTcpClient.Instance;
            stop = false;
            logs = new ObservableCollection<KeyValuePair<string, string>>();
            logs.Add(new KeyValuePair<string, string>("INFO", "Message1"));
            logs.Add(new KeyValuePair<string, string>("INFO", "Message2"));
            logs.Add(new KeyValuePair<string, string>("FAIL", "Message3"));
            logs.Add(new KeyValuePair<string, string>("WARNING", "Message4"));
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // connection to the service 
        //public void Connect(string ip, int port)
        //{
        //    tcpClient.Connect(ip, port);
        //}

        public void Disconnect()
        {
            stop = true;
            tcpClient.Disconnect();
        }

        public void Start()
        {
            string allInOne;
            string[] configurations;
            string[] keyAndValue;

            logs.Clear();

            var uiContext = SynchronizationContext.Current;

            new Thread(delegate () {

                tcpClient.Write("GetConfigCommand");
                // the appConfig data in one string
                allInOne = tcpClient.Read();

                configurations = allInOne.Split(' ');

                foreach (string config in configurations)
                {
                    keyAndValue = config.Split('$');
                    uiContext.Send(x => Logs.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1])), null);
                    //Settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]));
                }
            }).Start();
        }
    }
}
