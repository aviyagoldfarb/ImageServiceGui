using ImageServiceGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModels
{
    public class MainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private MainWindowModel model;
        public MainWindowViewModel(MainWindowModel model)
        {
            this.model = model;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }
        private bool vm_isConnected;
        public bool VM_IsConnected
        {
            get { return model.IsConnected; }
        }
    }
}
