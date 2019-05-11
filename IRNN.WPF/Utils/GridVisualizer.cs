using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IRNN.WPF.Utils
{
    public static class GridVisualizer
    {
        private static void CreateRows(Grid grd, int rows, double height, GridUnitType unitType)
        {
            grd.RowDefinitions.Clear();
            for (int i = 0; i < rows; i++)
            {
                grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height, unitType) });
            }
        }
        private static void CreateColumns(Grid grd, int columns, double width, GridUnitType unitType)
        {
            grd.ColumnDefinitions.Clear();
            for (int i = 0; i < columns; i++)
            {
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width, unitType) });
            }
        }

        public static void VisualizeData(Grid grd, Byte[,] data, double rowHeight, double columnWidth, GridUnitType unitType)
        {
            grd.Children.Clear();
            CreateRows(grd, data.GetLength(0), rowHeight, unitType);
            CreateColumns(grd, data.GetLength(1), columnWidth, unitType);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    UIElement item = new UIElement();
                    item = new Label() { MinHeight = rowHeight, MinWidth = columnWidth, Background = data[i, j] == 0 ? Brushes.White : Brushes.Black };
                    if (unitType == GridUnitType.Star)
                    {
                        (item as Label).MinHeight = 0b10000000000000;
                        (item as Label).MinWidth = 0b10000000000000; 
                    }
                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);

                    grd.Children.Add(item);
                }
            }
        }
    }
}
