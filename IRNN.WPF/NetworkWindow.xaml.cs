using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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

        Network network;
        List<DataSet> dataSets;

        public NetworkWindow(double[] vet1, double[] vet2)
        {
            InitializeComponent();
            dataSets = new List<DataSet>();
            Loader.Load();
            CreateDataSet();
            network = new Network(Loader.networkInputs, Loader.neuronNumberPerLayer, Loader.outputClasses, null, null);
        }

        private void CreateDataSet()
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Images\\trainingset.cfg");
            List<string> temp = new List<string>();
            PBMImage image;
            while (sr.Peek() > 0)
            {
                temp.Add(sr.ReadLine());
            }
            string[] arrTemp = new string[2];

            for (int i = 0; i < temp.Count; i++)
            {
                arrTemp = temp[i].Split('|');
                double[] output = new double[arrTemp[1].Split('.').Length];
                for (int j = 0; j < output.Length; j++)
                {
                    output[j] = Convert.ToDouble(arrTemp[1].Split('.')[j]);
                }
                image = new PBMImage(Directory.GetCurrentDirectory() + "\\Images\\" + arrTemp[0]);
                dataSets.Add(new DataSet(image.ConvertMatToArray(), output));
            }
        }

        private void btn_training_Click(object sender, RoutedEventArgs e)
        {
            network.Train(dataSets, Loader.minimumError); //sarà possibile implementare anche l'altra tipologia di allenamento 
            MessageBox.Show("ciao sciro");
        }

        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
