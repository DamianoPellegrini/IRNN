using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
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
        List<string> classes;
        bool isTrainingFinished;

        public NetworkWindow()
        {
            InitializeComponent();
            isTrainingFinished = false;
            dataSets = new List<DataSet>();
            Loader.Load();
            CreateDataSet();
            network = new Network(Loader.networkInputs, Loader.neuronNumberPerLayer, Loader.outputClasses, Loader.epochMaxNumber, Loader.minimumError);
        }

        private void CreateDataSet()
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Images\\trainingset.cfg");
            classes = new List<string>();
            PBMImage image;
            while (sr.Peek() > 0)
            {
                classes.Add(sr.ReadLine());
            }
            string[] arrTemp = new string[2];

            for (int i = 0; i < classes.Count; i++)
            {
                arrTemp = classes[i].Split('|');
                double[] output = new double[arrTemp[1].Split('.').Length];
                for (int j = 0; j < output.Length; j++)
                {
                    output[j] = Convert.ToDouble(arrTemp[1].Split('.')[j]);
                }
                image = new PBMImage(Directory.GetCurrentDirectory() + "\\Images\\" + arrTemp[0]);
                dataSets.Add(new DataSet(image.ConvertMatToArray(), output));
            }
        }


        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".pbm";
            fileDialog.Filter = "Immagine pbm|.pbm";

            if ((bool)fileDialog.ShowDialog())
            {

                var input = new PBMImage(fileDialog.FileName);
                var results = network.Compute(input.ConvertMatToArray());

                var result = results.Max();

                if (result >= 0.9)
                {
                    var possibleClass = classes[Array.IndexOf(results, result)];
                    MessageBox.Show("The image sent looks like " + possibleClass.Split('|')[0] + "with a value of " + result + ".", "Testing done", MessageBoxButton.OK, MessageBoxImage.Information); 
                }
                if (result <= 0.9 && result >= 0.5)
                {
                    var possibleClass = classes[Array.IndexOf(results, result)];
                    MessageBox.Show("The image sent could be " + possibleClass.Split('|')[0] + ". The neural network gave an output value of " + result + ".", "Testing done", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (result < 0.5)
                {
                    MessageBox.Show("The image sent doesn't look like anything the neural network has ever seen before. Output value: " + result + ".", "Testing done", MessageBoxButton.OK, MessageBoxImage.Information);
                    var AddNewClass = MessageBox.Show("Would you like to add this class to the training set? Keep in mind that you'll have to redo the training.", "Add class", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (AddNewClass == MessageBoxResult.Yes)
                    {                     
                        var className = Path.GetFileName(fileDialog.FileName) + "|" + (Convert.ToString((classes.Count + 1), 2));
                        StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + "\\Images\\trainingset.cfg");
                        sw.Write(className);
                        sw.Close();

                        if (!File.Exists(Directory.GetCurrentDirectory() + "\\Images\\" + Path.GetFileName(fileDialog.FileName) + ".cfg"))
                        {
                            File.Copy(fileDialog.FileName, Directory.GetCurrentDirectory() + "\\Images");
                            MessageBox.Show("Image copied!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("There's already an image with the same name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                
                
            }
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (isTrainingFinished)
            {
                ExportHelper.ExportNetwork(network);
                MessageBox.Show("Neural network has been exported");
            }
            else
                MessageBox.Show("Can't save neural network if no training has been done");
        }

        private void Btn_load_Click(object sender, RoutedEventArgs e)
        {
            network = ImportHelper.ImportNetwork();
            if (network == null)
            {
                MessageBox.Show("No network has been loaded, retry");
            }
        }

        private void btn_training_Click(object sender, RoutedEventArgs e)
        {
            network.Train(dataSets, Loader.epochMaxNumber); 
            isTrainingFinished = true;
            MessageBox.Show("Training Finished");
            //implementare finestra delle statistiche
        }

        private void Btn_trainingError_Click(object sender, RoutedEventArgs e)
        {
            network.Train(dataSets, Loader.minimumError); 
            isTrainingFinished = true;
            MessageBox.Show("Training Finished");
        }
    }
}
