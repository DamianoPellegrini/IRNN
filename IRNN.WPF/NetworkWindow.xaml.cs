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
        double[] target1; double[] target2;
        PBMImage pBMImage1;
        PBMImage pBMImage2;
        Network network;
        List<DataSet> dataSets;

        public NetworkWindow(double[] vet1, double[] vet2)
        {
            InitializeComponent();
            dataSets = new List<DataSet>();
            target1 = vet1;
            target2 = vet2;
            pBMImage1 = new PBMImage(@"C:\Users\antonio.dimeglio\Desktop\immagini\luna.pbm");
            pBMImage2 = new PBMImage(@"C:\Users\antonio.dimeglio\Desktop\immagini\casa.pbm");
            Loader.Load();
            network = new Network(Loader.networkInputs, Loader.neuronNumberPerLayer, Loader.outputClasses, Loader.learningRate, Loader.momentum);
            dataSets.Add(new DataSet(pBMImage1.Array, target1));
            dataSets.Add(new DataSet(pBMImage2.Array, target2));
        }

        private void btn_training_Click(object sender, RoutedEventArgs e)
        {
            network.Train(dataSets, Loader.minimumError); //sarà possibile implementare anche l'altra tipologia di allenamento 
            MessageBox.Show("ciao sciro");
        }

        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Primo");
            var inputData = pBMImage1.Array;
            var results = network.Compute(inputData);
            foreach (double d in results)
            {
                MessageBox.Show(d.ToString());
            }

            MessageBox.Show("Secondo");

            inputData = pBMImage2.Array;
            results = network.Compute(inputData);
            foreach (double d in results)
            {
                MessageBox.Show(d.ToString());
            }
            
        }


    }
}
