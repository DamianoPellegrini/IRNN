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
using IRNN.WPF.Utils;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per ImageCreator.xaml
    /// </summary>
    public partial class ImageCreator : Window
    {
        private App _main;
        public ImageCreator()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_main.HasClosed)
                return;
            e.Cancel = true;
            this.Hide();
            _main.MenuWindow.Show();
        }

        private void menu_salva(object sender, RoutedEventArgs e)
        {
            InkCanvasToPBMConverter.InkCanvasToBitmap(grd_inkWrapper);
        }
    }
}
