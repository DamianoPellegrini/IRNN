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
using IRNN.WPF.Utils;

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

        private void Window_Initialized(object sender, EventArgs e)
        {
            _main = Application.Current as App;
            _network = _main.Network;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
            openfile.InitialDirectory = Environment.SpecialFolder.DesktopDirectory.ToString();
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
            PBMImage pbm = new PBMImage(imagePath);
            bool stretched = cmb_aspect.SelectedIndex == 0 ? true : false;
            GridVisualizer.VisualizeData(grd_img, pbm.Image, stretched ? 1 : sld_scale.Value, stretched ? 1 : sld_scale.Value, stretched ? GridUnitType.Star : GridUnitType.Auto);
        }

        private void ApriStatistiche(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _main.StatisticsWindow.Show();
        }

        private void txt_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            applyImage(txt_path.Text);
        }

        private void cmb_aspect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            applyImage(txt_path.Text);
            sld_scale.IsEnabled = cmb_aspect.SelectedIndex == 0 ? false : true;
        }

        private void sld_scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            applyImage(txt_path.Text);
        }
    }
}
