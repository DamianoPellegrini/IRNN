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
        private MenuWindow _menuWnd;
        private NetworkWindow _netWnd;
        private StatsWindow _statsWnd;
        private ImageCreator _imgWnd;
        private Network _network;

        /// <summary>
        /// If the main window is closing this, if true, will prevent other windows from not closing.
        /// </summary>
        public bool HasClosed {
            get; set;
        }

        /// <summary>
        /// Main menu.
        /// </summary>
        public MenuWindow MenuWindow {
            get {
                return _menuWnd;
            }
            set {
                _menuWnd = value;
            }
        }

        /// <summary>
        /// It commands the network.
        /// </summary>
        public NetworkWindow NetworkWindow
        {
            get { return _netWnd; }
            set { _netWnd = value; }
        }

        /// <summary>
        /// It shows the statistic of the network.
        /// </summary>
        public StatsWindow StatisticsWindow
        {
            get { return _statsWnd; }
            set { _statsWnd = value; }
        }

        /// <summary>
        /// It handles the .pbm creation.
        /// </summary>
        public ImageCreator ImageCreatorWindow
        {
            get { return _imgWnd; }
            set { _imgWnd = value; }
        }

        /// <summary>
        /// Neural Network that handle everything.
        /// </summary>
        public Network Network {
            get {
                return _network;
            }
            set {
                _network = value;
            }
        }

        public App() : base()
        {
            Loader.Load();
            _menuWnd = MainWindow as MenuWindow;
            _netWnd = new NetworkWindow();
            _statsWnd = new StatsWindow();
            _imgWnd = new ImageCreator();
            _network = new Network(Loader.networkInputs, Loader.neuronNumberPerLayer, Loader.outputClasses, Loader.epochMaxNumber, Loader.minimumError);

            HasClosed = false;
        }

    }
}
