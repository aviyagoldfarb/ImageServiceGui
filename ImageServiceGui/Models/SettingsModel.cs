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
    public class SettingsModel : ISettingsModel
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
            this.stop = false;

            // Testing
            handlers = new ObservableCollection<string>();
            settings = new ObservableCollection<KeyValuePair<string, string>>();

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
            string allInOne;
            string[] configurations;
            string[] keyAndValue;
            string[] handlersList;

            settings.Clear();
            handlers.Clear();

            var uiContext = SynchronizationContext.Current;

            new Thread(delegate () {

                try
                {
                    tcpClient.Write("GetConfigCommand");
                    // the appConfig data in one string
                    allInOne = tcpClient.Read();

                    configurations = allInOne.Split(' ');

                    foreach (string config in configurations)
                    {
                        keyAndValue = config.Split('$');
                        if (keyAndValue[0] == "Handler")
                        {
                            handlersList = keyAndValue[1].Split(';');
                            foreach (string handler in handlersList)
                            {
                                uiContext.Send(x => handlers.Add(handler), null);
                            }
                        }
                        else
                            uiContext.Send(x => Settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1])), null);
                        //Settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]));
                    }
                }
                catch (Exception)
                {

                }
                /*
                tcpClient.Write("GetConfigCommand");
                // the appConfig data in one string
                allInOne = tcpClient.Read();
                    
                configurations = allInOne.Split(' ');

                foreach (string config in configurations)
                {
                    keyAndValue = config.Split('$');
                    if (keyAndValue[0] == "Handler") {
                        handlersList = keyAndValue[1].Split(';');
                        foreach (string handler in handlersList) {
                            uiContext.Send(x => handlers.Add(handler), null);
                        }
                    }
                    else
                        uiContext.Send(x => Settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1])), null);
                    //Settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]));
                }
                */
            }).Start();
        }
    }
}
