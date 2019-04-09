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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRNN.WPF
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();

            /*====================TEST CODE TO REMOVE=======================*/
            StatsWindow wndtest = new StatsWindow();
            //wndtest.Show();
            //foreach (Window w in Application.Current.Windows)
            //    if(!(w is StatsWindow))
            //    w.Show();
            /*====================END TEST CODE=============================*/
        }
    }
}
