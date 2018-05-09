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
    class SettingsModel : ISettingsModel
    {
        //INotifyPropertyChanged implementation: 
        public event PropertyChangedEventHandler PropertyChanged;

        private ITcpClient tcpClient;
        private volatile Boolean stop;

        private ObservableCollection<KeyValuePair<string,string>> settings;
        public ObservableCollection<KeyValuePair<string, string>> Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                NotifyPropertyChanged("Settings");
            }
        }

        private ObservableCollection<string> handlers;
        public ObservableCollection<string> Handlers
        {
            get { return handlers; }
            set
            {
                handlers = value;
                NotifyPropertyChanged("Handlers");
            }
        }

        public SettingsModel()
        {
            this.tcpClient = ServiceTcpClient.Instance;
            stop = false;

            // Testing
            handlers = new ObservableCollection<string>();
            settings = new ObservableCollection<KeyValuePair<string, string>>();

            handlers.Add((@"C:\Users\hana\Desktop\listened_folder1"));
            handlers.Add((@"C:\Users\hana\Desktop\listened_folder2"));

            settings.Add(new KeyValuePair<string, string>("OutputDir" ,@"C:\Users\hana\Desktop\OutputDir"));
            settings.Add(new KeyValuePair<string, string>("SourceName" ,@"ImageServiceSource"));
            settings.Add(new KeyValuePair<string, string>("LogName" ,@"ImageServiceLog"));
            settings.Add(new KeyValuePair<string, string>("ThumbnailSize" ,@"120"));

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

            settings.Clear();

            new Thread(delegate () {
                while (!stop)
                {
                    tcpClient.Write("1 GetConfigCommand");
                    allInOne = tcpClient.Read(); // the appConfig data in one string
                    Thread.Sleep(250);// read the data in 4Hz

                    configurations = allInOne.Split(' ');

                    foreach (string config in configurations)
                    {
                        keyAndValue = config.Split('$');
                        settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]));
                    }
                    continue;
                }
            }).Start();
        }
    }
}
