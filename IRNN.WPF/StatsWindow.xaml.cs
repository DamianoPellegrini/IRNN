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

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        private App _main;
        private NeuralNetwork _network;
        public StatsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _main = Application.Current as App;
            _network = _main.Network;
            caricaLista(lst_layers,-_network.);
        }

        private void caricaLista<T>(ListView listView, List<T> list)
        {
            listView.ItemsSource = list;
            listView.Items.Refresh();
        }

        private void lst_neurons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //TODO gestire la stampa del neurone con il toString
        }

        private void lst_layers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_layers.SelectedIndex == -1) return;
            if (_network.NeuralLayers[lst_layers.SelectedIndex] == null) return;

            var layer = _network.NeuralLayers[lst_layers.SelectedIndex];
            caricaLista(lst_neurons, layer.Neurons);
        }
    }
}
