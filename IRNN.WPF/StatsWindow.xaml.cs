using Sparrow.Chart;
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
using IRNN.WPF.Utils;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window, IConnectedWindowProperty
    {
        private App _main;
        
        private NeuralNetwork _network;
        private int NumEpoche=100;
        private WindowProperty _prop;
        public StatsWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
            _prop = new WindowProperty();
            //TODO prendere dal file (di dime) settings.cfg il numero delle epoche(line 8) salvarno in NumEpoche
            //TODO il 450 sostituirlo con il numero di epoche
            //(spl_general.Points[1] as DoublePoint).Data = NumEpoche;
            //TODO cambiare il maxvalue con il numepoche
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _network = _main.Network;
            caricaGrafico();
            caricaLista(lst_layers, _network.NeuralLayers);
        }

        private void caricaGrafico()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\prova.txt";//TODO mettere la path corretta
            List<double[]> DatStringError = LeggiFile(path);
            foreach (double[] line in DatStringError)
            {
                spl_error.Points.Add(new DoublePoint() { Data = line[0], Value = line[1] });
            }
        }

        private List<double[]> LeggiFile(string path)
        {
            List<double[]> risultati = new List<double[]>();
            if (File.Exists(path))
            {
                StreamReader sr = File.OpenText(path);
                //lettura righe
                string testo = "";
                for (int i = 0; testo != null; i++)
                {
                    testo = sr.ReadLine();
                    if(testo!=null&&testo!="")
                    {
                        string[] testo1 = testo.Split('|');
                        risultati.Add(new double[] { double.Parse(testo1[0]), double.Parse(testo1[1]) });
                    }
                }
                sr.Close();
            }
            else
            {
                MessageBox.Show("Errore interno mancata creazione file degli errori globali", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return risultati;
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_main.HasClosed)
                return;
            e.Cancel = true;
            this.Hide();
            _main.NetworkWindow.Show();
        }

        private void caricaLista<T>(ListView listView, List<T> list)
        {
            listView.ItemsSource = list;
            listView.Items.Refresh();
        }

        private void lst_neurons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_neurons.SelectedIndex == -1) return;
            if (lst_layers.SelectedIndex == -1)
                return;
            if (_network.NeuralLayers[lst_layers.SelectedIndex] == null) return;
            if (_network.NeuralLayers[lst_layers.SelectedIndex].Neurons[lst_neurons.SelectedIndex] == null)
                return;

            var neuron = _network.NeuralLayers[lst_layers.SelectedIndex].Neurons[lst_neurons.SelectedIndex];
            //TODO gestire la stampa del neurone con il toString
            
        }

        private void lst_layers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_layers.SelectedIndex == -1) return;
            if (_network.NeuralLayers[lst_layers.SelectedIndex] == null) return;

            var layer = _network.NeuralLayers[lst_layers.SelectedIndex];
            caricaLista(lst_neurons, layer.Neurons);
        }

        public void Show(WindowProperty prop)
        {
            throw new NotImplementedException();
        }
    }
}
