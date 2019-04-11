using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IRNN;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NetworkWindow netWnd;
        public StatsWindow statsWnd;
        public ImageCreator imgWnd;
        private NeuralNetwork _network;

        public NeuralNetwork Network
        {
            get { return _network; }
            set { _network = value; }
        }

    }
}
