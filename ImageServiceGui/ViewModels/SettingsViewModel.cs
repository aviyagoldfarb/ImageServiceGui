using ImageServiceGui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace ImageServiceGui.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ISettingsModel model;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="model">the settings model</param>
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.PropertyChanged += RemovePropertyChange;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            
        }

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<KeyValuePair<string, string>> vm_settings;
        public ObservableCollection<KeyValuePair<string, string>> VM_Settings
        {
            get { return model.Settings; }
        }
        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<string> vm_handlers;
        public ObservableCollection<string> VM_Handlers
        {
            get { return model.Handlers; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string vm_selectedHandler;
        public string VM_SelectedHandler
        {
            get { return model.SelectedHandler; }
            set
            {
                model.SelectedHandler = value;
                NotifyPropertyChanged("VM_SelectedHandler");
            }
        }
        
        private void RemovePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove(object obj)
        {
            this.model.RemoveHandler(this.VM_SelectedHandler);
        }

        private bool CanRemove(object obj)
        {
            if (string.IsNullOrEmpty(this.VM_SelectedHandler))
            {
                return false;
            }
            return true;
        }
    }
}
