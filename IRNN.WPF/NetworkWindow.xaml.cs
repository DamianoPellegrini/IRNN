using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using IRNN.WPF.Utils;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per NetworkWindow.xaml
    /// </summary>
    public partial class NetworkWindow : Window
    {
        List<DataSet> dataSets;
        List<string> classes;
        bool isTrainingFinished;

        private App _main;
        private Network _network;

        public NetworkWindow()
        {
            InitializeComponent();
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

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (isTrainingFinished)
            {
                ExportHelper.ExportNetwork(_network);
                MessageBox.Show("Neural network has been exported");
            }
            else
                MessageBox.Show("Can't save neural network if no training has been done");
        }

        private void Btn_load_Click(object sender, RoutedEventArgs e)
        {
            _network = ImportHelper.ImportNetwork();
            if (_network == null)
            {
                MessageBox.Show("No network has been loaded, retry");
            }
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            _main = Application.Current as App;
            isTrainingFinished = false;
            dataSets = new List<DataSet>();
            CreateDataSet();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void ApriStatistiche(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _main.StatisticsWindow.Show();
        }

        #region ImageHandlerMethods
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
        #endregion

        private void startRecognition(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".pbm";
            fileDialog.Filter = "Immagine pbm|.pbm";

            if ((bool)fileDialog.ShowDialog())
            {

                var input = new PBMImage(fileDialog.FileName);
                var results = _network.Compute(input.ConvertMatToArray());

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

        private void startTraining(object sender, RoutedEventArgs e)
        {
            if ((bool)rdbEpoch.IsChecked)
                _network.Train(dataSets, Loader.epochMaxNumber);
            else
                _network.Train(dataSets, Loader.minimumError);
            isTrainingFinished = true;
            MessageBox.Show("Training Finished");
        }
    }
}
