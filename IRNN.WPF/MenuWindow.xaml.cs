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

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private App _main;
        public MenuWindow()
        {
            InitializeComponent();
            NetworkWindow networkWindow = new NetworkWindow();
            networkWindow.Show();
            this.Close();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
            _main.MenuWindow = this;
        }

        private void Window_Closed(object sender, EventArgs e) {
            _main.HasClosed = true;
            _main.Shutdown();
        }

        private void openWindow(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;

            string window = btn.Name.Substring(4);
            this.Hide();
            (window == "network" ? _main.NetworkWindow as Window : _main.ImageCreatorWindow as Window).Show();
        }
    }
}
