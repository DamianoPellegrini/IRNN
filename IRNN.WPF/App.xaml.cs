using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NetworkWindow _netWnd;
        private StatsWindow _statsWnd;
        private ImageCreator _imgWnd;

        public NetworkWindow NetworkWindow
        {
            get { return _netWnd; }
            private set { _netWnd = value; }
        }


        public StatsWindow StatisticsWindow
        {
            get { return _statsWnd; }
            private set { _statsWnd = value; }
        }


        public ImageCreator ImageCreatorWindow
        {
            get { return _imgWnd; }
            set { _imgWnd = value; }
        }
        public App() : base()
        {
            _netWnd = new NetworkWindow();
            _statsWnd = new StatsWindow();
            _imgWnd = new ImageCreator();
        }

    }
}
