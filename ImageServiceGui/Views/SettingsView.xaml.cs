using ImageServiceGui.Communication;
using ImageServiceGui.Models;
using ImageServiceGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGui
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        SettingsViewModel vm;
        public SettingsView()
        {
            InitializeComponent();
            vm = new SettingsViewModel(new SettingsModel(new ServiceTcpClient()));
            DataContext = vm;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
