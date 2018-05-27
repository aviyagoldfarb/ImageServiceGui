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

        private string selectedHandler;
        public string SelectedHandler
        {
            get { return selectedHandler; }
            set
            {
                selectedHandler = value;
                NotifyPropertyChanged("SelectedHandler");
            }
        }
        /// <summary>
        /// constructor.
        /// </summary>
        public SettingsModel()
        {
            this.tcpClient = ServiceTcpClient.Instance;
            this.stop = false;

            this.tcpClient.ConfigRecieved += OnConfigRecieved;
            this.tcpClient.RemovedHandler += OnRemovedHandler;

            handlers = new ObservableCollection<string>();
            settings = new ObservableCollection<KeyValuePair<string, string>>();

            this.Start();
        }
        /// <summary>
        /// notifying obout a change that happend in property.
        /// </summary>
        /// <param name="propName">the name of the property that has changed.</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        /// <summary>
        /// closing the connection with the service.
        /// </summary>
        public void Disconnect()
        {
            stop = true;
            tcpClient.Disconnect();
        }
        /// <summary>
        /// starting to get settings from the service.
        /// </summary>
        public void Start()
        {
            tcpClient.Write("GetConfigCommand");
        }
        /// <summary>
        /// sending a command to remove a handler.
        /// </summary>
        /// <param name="handlerPath"> the path of the directory that handling by the handler that should to be remove</param>
        public void RemoveHandler(string handlerPath)
        {
            new Thread(delegate () {

                try
                {
                    tcpClient.Write("RemoveHandler" + " " + this.SelectedHandler);
                }
                catch (Exception)
                {

                }
            }).Start();
        }
        /// <summary>
        /// receiving a configuration.
        /// </summary>
        /// <param name="sender">the sender of the massage</param>
        /// <param name="msg">the configurations that reseived </param>
        public void OnConfigRecieved(object sender, MessageEventArgs msg)
        {
            string allInOne = msg.Message;
            string[] configurations;
            string[] keyAndValue;
            string[] handlersList;

            configurations = allInOne.Split('\n');

            foreach (string config in configurations)
            {
                keyAndValue = config.Split('$');
                if (keyAndValue[0] == "Handler")
                {
                    handlersList = keyAndValue[1].Split(';');
                    foreach (string handler in handlersList)
                    {
                        if (handler != "")
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate {
                                Handlers.Add(handler);
                            });
                        }
                    }
                }
                else
                    App.Current.Dispatcher.Invoke((Action)delegate {
                        Settings.Add(new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]));
                    });
            }
        }
        /// <summary>
        /// get and send a message that one handler is removed.
        /// </summary>
        /// <param name="sender">the sender.</param>
        /// <param name="msg">the path of the handler.</param>
        public void OnRemovedHandler(object sender, MessageEventArgs msg)
        {
            string path = msg.Message;
            if (this.Handlers.Contains(path))
            {
                App.Current.Dispatcher.Invoke((Action)delegate {
                    this.Handlers.Remove(path);
                });
                //NotifyPropertyChanged("Handlers");
            }
        }

    }
}
