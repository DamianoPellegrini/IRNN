using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IRNN.WPF.Utils
{
    public static class GridVisualizer
    {
        private static void CreateRows(Grid grd, int rows, double height, GridUnitType unitType)
        {
            for (int i = 0; i < rows; i++)
            {
                grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height, unitType) });
            }
        }
        private static void CreateColumns(Grid grd, int columns, double width, GridUnitType unitType)
        {
            for (int i = 0; i < columns; i++)
            {
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width, unitType) });
            }
        }

        public static void VisualizeData<T>(Grid grd, T[,] data, Type itemType)
        {
            if (!(itemType == typeof(UIElement))) return;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    UIElement item = new UIElement();
                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);
                    if (itemType == typeof(Label))
                        item = new Label() { Content = data[i, j].ToString() };
                    if (itemType == typeof(Button))
                        item = new Button() { Content = data[i, j].ToString() };
                    if (itemType == typeof(Label))
                        item = new TextBlock() { Text = data[i, j].ToString() };

                    grd.Children.Add(item);
                }
            }
        }
    }
}
