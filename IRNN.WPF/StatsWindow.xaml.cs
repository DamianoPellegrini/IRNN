using IRNN.WPF.Utils;
using Sparrow.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace IRNN.WPF {

    /// <summary>
    /// Logica di interazione per StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window, IConnectedWindowProperty {
        private App _main;
        //TODO: BUG?: le epoche max non sono giuste se il file data.txt è stato creato con errore minimo perhce potrebbero essere di più o di meno
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
            //TODO sistema
            caricaLista(lst_layers, _network.GetAllLayers());
        }

        private void caricaGrafico() {
            spl_error.Points.Clear();
            GC.Collect(); //Diminuisce il quantitativo di memoria utilizzata
            string path = Directory.GetCurrentDirectory() + "\\data.txt";
            List<double[]> DatStringError = LeggiFile(path);
            epochAxis.MaxValue = DatStringError.Count;
            //foreach (double[] line in DatStringError) {
            //TODO: Reimplementa il coso con incrementi ogni 10
            spl_error.Points.Add(new DoublePoint() { Data = DatStringError[0][0], Value = DatStringError[0][1] });
            for (int i = 1; i < DatStringError.Count - 1; i += 10) {
                double[] line = DatStringError[i];
                spl_error.Points.Add(new DoublePoint() { Data = line[0], Value = line[1] });
            }
            spl_error.Points.Add(new DoublePoint() { Data = DatStringError[DatStringError.Count - 1][0], Value = DatStringError[DatStringError.Count - 1][1] });
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
            //TODO sistema
            //TODO: DOPO AVER SISTEMATO: non stampa nelle listbox il nome/numero del layer, mettere il tostring del neurone, e mettee le label su cui stampare i dati del neurone
            var layers = _network.GetAllLayers();
            if (layers[lst_layers.SelectedIndex] == null) return;
            if (layers[lst_layers.SelectedIndex][lst_neurons.SelectedIndex] == null)
                return;

            var neuron = layers[lst_layers.SelectedIndex][lst_neurons.SelectedIndex];
            //TODO gestire la stampa del neurone con il toString
        }

        private void lst_layers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lst_layers.SelectedIndex == -1)
                return;
            //TODO sistema
            var layers = _network.GetAllLayers();
            if (layers[lst_layers.SelectedIndex] == null) return;

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