using ImageServiceGui.Communication;
using ImageServiceGui.Infrastructure;
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
            this.stop = false;

            this.tcpClient.LogUdated += OnLogUpdated;

            logs = new ObservableCollection<KeyValuePair<string, string>>();
            
            this.Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Disconnect()
        {
            stop = true;
            tcpClient.Disconnect();
        }

        public void Start()
        {
            tcpClient.Write("LogCommand");
        }

        public void OnLogUpdated(object sender, MessageEventArgs msg)
        {
            string allInOne = msg.Message;
            string[] entireLog;
            string[] keyAndValue;

            entireLog = allInOne.Split('\n');

            foreach (string entry in entireLog)
            {
                keyAndValue = entry.Split('$');
                string key = "";
                switch (keyAndValue[0])
                {
                    case "Information":
                        key += "INFO";
                        break;
                    case "Warning":
                        key += "WARNING";
                        break;
                    case "FailureAudit":
                        key += "FAIL";
                        break;
                }
                if (key == "")
                    break;
                App.Current.Dispatcher.Invoke((Action)delegate {                    
                    Logs.Add(new KeyValuePair<string, string>(key, keyAndValue[1]));
                });                
            }
        }
    }
}
