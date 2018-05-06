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

        public SettingsModel(ITcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            stop = false;
           settings = new ObservableCollection<KeyValuePair<string, string>>();
            /** settings.Add(new KeyValuePair<string, string>("Handler", @"C:\Users\hana\Desktop\listened_folder1;C:\Users\hana\Desktop\listened_folder2"));
              settings.Add(new KeyValuePair<string, string>("OutputDir" ,@"C:\Users\hana\Desktop\OutputDir"));
              settings.Add(new KeyValuePair<string, string>("SourceName" ,@"ImageServiceSource"));
              settings.Add(new KeyValuePair<string, string>("LogName" ,@"ImageServiceLog"));
              settings.Add(new KeyValuePair<string, string>("ThumbnailSize" ,@"120"));
           */
            this.Connect("127.0.0.1", 7000);
            this.Start();

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
                   
                    foreach (string config in configurations )
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
