using IRNN.WPF.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IRNN.WPF {

    /// <summary>
    /// Logica di interazione per ImageCreator.xaml
    /// </summary>
    public partial class ImageCreator : Window {
        private App _main;

        public ImageCreator() {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _main = Application.Current as App;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_main.HasClosed)
                return;
            e.Cancel = true;
            this.Hide();
            _main.MenuWindow.Show();
        }

        private void menu_salva(object sender, RoutedEventArgs e) {
            //TODO: SAVE FILE DIALOG CON PATH
            TransformedBitmap bitmap = InkCanvasToPBMConverter.InkCanvasToBitmap(ink_drawingBoard);
        }

        private void menu_pulisci(object sender, RoutedEventArgs e) {
            ink_drawingBoard.Strokes.Clear();
        }

        private void menu_esci(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void switchTool(object sender, SelectionChangedEventArgs e) {
            if (chb_tool.SelectedItem == chb_tool.Items[0])
                ink_drawingBoard.EditingMode = InkCanvasEditingMode.Ink;
            if (chb_tool.SelectedItem == chb_tool.Items[1])
                ink_drawingBoard.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void switchSize(object sender, SelectionChangedEventArgs e) {
            //TODO: Se il programma funziona scoprire perchè non va
            //ComboBoxItem item = sender as ComboBoxItem;
            //if (item == null)
            //    return;

            //double size = double.Parse(item.Content.ToString());
            //ink_drawingBoard.DefaultDrawingAttributes = new System.Windows.Ink.DrawingAttributes() {
            //    Width = size,
            //    Height = size,
            //    Color = System.Windows.Media.Color.FromRgb(0,0,0),
            //    FitToCurve = false,
            //    IgnorePressure = false,
            //    IsHighlighter = false,
            //    StylusTip = System.Windows.Ink.StylusTip.Ellipse,
            //    StylusTipTransform = System.Windows.Media.Matrix.Identity
            //};
            //ink_drawingBoard.UpdateDefaultStyle();
            //ink_drawingBoard.UpdateLayout();
        }
    }
}