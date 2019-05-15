using IRNN.WPF.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IRNN.WPF {

    /// <summary>
    /// Logica di interazione per NetworkWindow.xaml
    /// </summary>
    public partial class NetworkWindow : Window {
        private List<DataSet> dataSets;
        private List<string> classes;
        private bool isTrainingFinished;
        private string TrainFolderPath = Directory.GetCurrentDirectory() + "\\Trainset";
        private App _main;
        private Network _network;

        public NetworkWindow() {
            InitializeComponent();
        }

        private void CreateDataSet() {
            StreamReader sr = new StreamReader(TrainFolderPath + "\\trainingset.cfg");
            classes = new List<string>();
            PBMImage image;
            while (sr.Peek() > 0) {
                classes.Add(sr.ReadLine());
            }
            string[] arrTemp = new string[2];

            for (int i = 0; i < classes.Count; i++) {
                arrTemp = classes[i].Split('|');
                double[] output = new double[arrTemp[1].Split('.').Length];
                for (int j = 0; j < output.Length; j++) {
                    output[j] = Convert.ToDouble(arrTemp[1].Split('.')[j]);
                }
                image = new PBMImage(TrainFolderPath + "\\" + arrTemp[0]);
                dataSets.Add(new DataSet(image.ConvertMatToArray(), output));
            }
        }

        private void saveNetwork(object sender, RoutedEventArgs e) {
            if (isTrainingFinished) {
                ExportHelper.ExportNetwork(_network);
                MessageBox.Show("Neural network has been exported");
            } else
                MessageBox.Show("Can't save neural network if no training has been done");
        }

        private void loadNetwork(object sender, RoutedEventArgs e) {
            isTrainingFinished = true;
            var tmpNet = ImportHelper.ImportNetwork();
            if (tmpNet == null) {
                MessageBox.Show("No network has been loaded, retry");
                isTrainingFinished = false;
                return;
            }
            isTrainingFinished = true;
            _network = tmpNet;
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
            isTrainingFinished = false;
            dataSets = new List<DataSet>();
            CreateDataSet();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _network = _main.Network;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_main.HasClosed)
                return;
            e.Cancel = true;
            this.Hide();
            _main.MenuWindow.Show();
        }

        private void menuEsci(object sender, RoutedEventArgs e) {
            //TODO FERMA ALLENAMENTO?
            this.Close();
        }

        private void ApriStatistiche(object sender, RoutedEventArgs e) {
            if (!isTrainingFinished) {
                MessageBox.Show("Devi avere una rete allenata per vedere le sue statistiche");
                return;
            }
            this.Hide();
            _main.StatisticsWindow.Show();
        }

        #region ImageHandlerMethods

        private void loadImage(object sender, RoutedEventArgs e) {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = Environment.SpecialFolder.DesktopDirectory.ToString();
            openfile.Filter = "Portable Bitmap Image(*.pbm)|*.pbm";
            if (openfile.ShowDialog() == true) {
                string filePath = openfile.FileName;
                txt_path.Text = filePath;
                applyImage(filePath);
            }
        }

        private void applyImage(string imagePath) {
            if (!File.Exists(imagePath))
                return;
            PBMImage pbm = new PBMImage(imagePath);
            bool stretched = cmb_aspect.SelectedIndex == 0 ? true : false;
            GridVisualizer.VisualizeData(grd_img, pbm.Image, stretched ? 1 : sld_scale.Value, stretched ? 1 : sld_scale.Value, stretched ? GridUnitType.Star : GridUnitType.Auto);
        }

        private void txt_path_TextChanged(object sender, TextChangedEventArgs e) {
            grd_img.Children.Clear();
            applyImage(txt_path.Text);
        }

        private void cmb_aspect_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            applyImage(txt_path.Text);
            sld_scale.IsEnabled = cmb_aspect.SelectedIndex == 0 ? false : true;
        }

        private void sld_scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            applyImage(txt_path.Text);
        }

        #endregion ImageHandlerMethods

        private void startRecognition(object sender, RoutedEventArgs e) {
            //OpenFileDialog fileDialog = new OpenFileDialog();
            //fileDialog.DefaultExt = "*.pbm";
            //fileDialog.Filter = "Immagine pbm|*.pbm";

            //if ((bool)fileDialog.ShowDialog()) {
            if (!isTrainingFinished) {
                MessageBox.Show("Devi prima fare un training!");
                return;
            }
            if (txt_path.Text == string.Empty || txt_path == null) {
                MessageBox.Show("Non è stato aperta un'immagine!");
                return;
            }

            var input = new PBMImage(txt_path.Text);
            var results = _network.Compute(input.ConvertMatToArray());

            var result = results.Max();

            if (result >= 0.9) {
                var possibleClass = classes[Array.IndexOf(results, result)];
                MessageBox.Show("The image sent looks like " + possibleClass.Split('|')[0] + " with a value of " + result + ".", "Testing done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (result <= 0.9 && result >= 0.5) {
                var possibleClass = classes[Array.IndexOf(results, result)];
                MessageBox.Show("The image sent could be " + possibleClass.Split('|')[0] + ". The neural network gave an output value of " + result + ".", "Testing done", MessageBoxButton.OK, MessageBoxImage.Information);
            } else if (result < 0.5) {
                MessageBox.Show("The image sent doesn't look like anything the neural network has ever seen before. Output value: " + result + ".", "Testing done", MessageBoxButton.OK, MessageBoxImage.Information);
                var AddNewClass = MessageBox.Show("Would you like to add this class to the training set? Keep in mind that you'll have to redo the training.", "Add class", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (AddNewClass == MessageBoxResult.Yes) {
                    var className = Path.GetFileName(txt_path.Text) + "|" + (Convert.ToString((classes.Count + 1), 2));
                    StreamWriter sw = File.AppendText(TrainFolderPath + "\\trainingset.cfg");
                    sw.Write(className);
                    sw.Close();

                    if (!File.Exists(TrainFolderPath + "\\" + Path.GetFileName(txt_path.Text) + ".cfg")) {
                        File.Copy(txt_path.Text, TrainFolderPath);
                        MessageBox.Show("Image copied!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                    } else {
                        MessageBox.Show("There's already an image with the same name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void startTraining(object sender, RoutedEventArgs e) {
            if ((bool)rdbEpoch.IsChecked)
                _network.Train(dataSets, Loader.epochMaxNumber);
            else
                _network.Train(dataSets, Loader.minimumError);
            isTrainingFinished = true;
            MessageBox.Show("Training Finished");
        }
    }
}