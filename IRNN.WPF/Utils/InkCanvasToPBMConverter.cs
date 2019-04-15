using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace IRNN.WPF.Utils
{
    internal class InkCanvasToPBMConverter
    {
        internal static BitmapImage InkCanvasToBitmap(InkCanvas canvas)
        {
            //RENDERING
            int width = (int)canvas.ActualWidth;
            int height = (int)canvas.ActualHeight;
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            renderBitmap.Render(canvas);

            //ENCODING
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            //SAVING TO BITMAP OBJECT
            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);
            BitmapImage ret = new BitmapImage();
            ret.BeginInit();
            stream.Position = 0;
            ret.StreamSource = new MemoryStream();
            stream.CopyTo(ret.StreamSource);
            ret.DecodePixelWidth = width;
            ret.DecodePixelHeight = height;
            //TODO: non va un bidone di nulla
            ret.EndInit();
            stream.Close();
            return ret;
        }

        internal static Color[,] BitmapToColorMatrix(BitmapImage image)
        {
            throw new NotImplementedException();
        }

        internal static object/*TODO: Substitute to PBMImage on merge*/ ColorMatrixToPBMImage(Color[,] image) {
            throw new NotImplementedException();
        }
    }
}
