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
using IRNN;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per NetworkWindow.xaml
    /// </summary>
    public partial class NetworkWindow : Window
    {

        public NetworkWindow()
        {
            InitializeComponent();
        }

        private void btn_inutile_Click(object sender, RoutedEventArgs e)
        {
            PBMImage pBMImage = new PBMImage(@"C:\Users\antonio.dimeglio\Desktop\immagini\luna.pbm");
            NeuralNetworkEngine neuralNetworkEngine = new NeuralNetworkEngine(pBMImage, NeuralNetworkEngine.ApplicationStatus.Training);
            MessageBox.Show(neuralNetworkEngine.StatusHandler(NeuralNetworkEngine.ApplicationStatus.Training));
        }
    }
}
