using IRNN.WPF.Utils;
using Sparrow.Chart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace IRNN.WPF {

    /// <summary>
    /// Logica di interazione per StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window, IConnectedWindowProperty {
        private App _main;
        private Network _network;
        private int NumEpoche = 100;
        private WindowProperty _prop;

        public StatsWindow() {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
            _prop = new WindowProperty();
            var max = Loader.epochMaxNumber;
            epochAxis.MaxValue = max;
            epochAxis.Interval = max / 10;
            //Imposta la linea dell'errore ipotico al coso max
            (spl_general.Points[1] as DoublePoint).Data = max;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _network = _main.Network;
            caricaGrafico();
            List<ListViewItem> itemlist = new List<ListViewItem>();
            var layers = _network.GetAllLayers();
            itemlist.Add(new ListViewItem() { Content = "Input Layer" });
            for (int i = 1; i < layers.Count - 1; i++) {
                itemlist.Add(new ListViewItem() { Content = "Hidden Layer " + i });
            }
            itemlist.Add(new ListViewItem() { Content = "Output Layer" });
            caricaLista(lst_layers, itemlist);
        }

        private void caricaGrafico() {
            spl_error.Points.Clear();
            GC.Collect(); //Diminuisce il quantitativo di memoria utilizzata
            string path = Directory.GetCurrentDirectory() + "\\data.txt";
            List<double[]> DatStringError = LeggiFile(path);
            epochAxis.MaxValue = DatStringError.Count;
            errorAxis.MaxValue = Math.Ceiling(DatStringError[0][1]);
            //foreach (double[] line in DatStringError) {
            spl_error.Points.Add(new DoublePoint() { Data = DatStringError[0][0], Value = DatStringError[0][1] });
            //int increment = (int)((Math.Sqrt(DatStringError.Count-1)) + 1);//MODO 1
            int increment = (int)((Math.Log(DatStringError.Count)*3) + 1);//MODO 2
            for (int i = 1; i < DatStringError.Count - 1; i += increment) {
                double[] line = DatStringError[i];
                spl_error.Points.Add(new DoublePoint() { Data = line[0], Value = line[1] });
                Debug.WriteLine($"{increment}|{i}|{line[0]}|{line[1]}");
            }
            spl_error.Points.Add(new DoublePoint() { Data = DatStringError[DatStringError.Count - 1][0], Value = DatStringError[DatStringError.Count - 1][1] });
            Debug.WriteLine($"{increment}|FINE");
            //}
        }

        private List<double[]> LeggiFile(string path) {
            List<double[]> risultati = new List<double[]>();
            if (File.Exists(path)) {
                StreamReader sr = File.OpenText(path);
                //lettura righe
                string testo = "";
                for (int i = 0; testo != null; i++) {
                    testo = sr.ReadLine();
                    if (testo != null && testo != "") {
                        string[] testo1 = testo.Split('|');
                        risultati.Add(new double[] { double.Parse(testo1[0]), double.Parse(testo1[1]) });
                    }
                }
                sr.Close();
            } else {
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

        private void caricaLista<T>(ListView listView, List<T> list) {
            listView.ItemsSource = list;
            listView.Items.Refresh();
        }

        private void lst_neurons_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lst_neurons.SelectedIndex == -1)
                return;
            if (lst_layers.SelectedIndex == -1)
                return;

            var layers = _network.GetAllLayers();
            if (layers[lst_layers.SelectedIndex] == null)
                return;
            if (layers[lst_layers.SelectedIndex][lst_neurons.SelectedIndex] == null)
                return;

            var neuron = layers[lst_layers.SelectedIndex][lst_neurons.SelectedIndex];
            var labels = grd_neuronInfo.Children;
            (labels[0] as Label).Content = $"ID: {neuron.Id}";
            (labels[1] as Label).Content = $"Bias: {neuron.Bias}";
            (labels[2] as Label).Content = $"Bias Delta: {neuron.BiasDelta}";
            (labels[3] as Label).Content = $"Gradient: {neuron.Gradient}";
            (labels[4] as Label).Content = $"Value: {neuron.Value}";
        }

        private void lst_layers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lst_layers.SelectedIndex == -1)
                return;

            var layers = _network.GetAllLayers();
            if (layers[lst_layers.SelectedIndex] == null)
                return;

            var layer = layers[lst_layers.SelectedIndex];
            caricaLista(lst_neurons, layer);
        }

        public void Show(WindowProperty prop) {
            throw new NotImplementedException();
        }

        private void Window_Activated(object sender, EventArgs e) {
            caricaGrafico();
        }
    }
}