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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per NetworkWindow.xaml
    /// </summary>
    public partial class NetworkWindow : Window
    {
        private App _main;
        private NeuralNetwork _network;

        public NetworkWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
            _network = _main.Network;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_main.HasClosed)
                return;
            e.Cancel = true;
            this.Hide();
            _main.MenuWindow.Show();
        }

        private void menuEsci(object sender, RoutedEventArgs e)
        {
            //TODO FERMA ALLENAMENTO?
            this.Close();
        }

        private void loadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = "c:\\Users\\simone.lugaresi\\Desktop";
            openfile.Filter = "Portable Bitmap Image(*.pbm)|*.pbm";
            if (openfile.ShowDialog() == true)
            {
                string filePath = openfile.FileName;
                txt_path.Text = filePath;
                applyImage(filePath);
            }
        }

        private void applyImage(string imagePath)
        {
            if (!File.Exists(imagePath)) return;
            img.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        private void ApriStatistiche(object sender, RoutedEventArgs e)
        {
            this.Close();
            _main.StatisticsWindow.Show();
        }

        private void txt_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            applyImage(txt_path.Text);
        }
    }
}
