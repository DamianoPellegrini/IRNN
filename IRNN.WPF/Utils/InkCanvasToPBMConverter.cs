using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IRNN.WPF.Utils
{
    internal class InkCanvasToPBMConverter
    {
        internal static BitmapImage InkCanvasToBitmap(Grid canvasWrapper)
        {
            int width = (int)canvasWrapper.ActualWidth;
            int height = (int)canvasWrapper.ActualHeight;
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(width, height, 300d, 300d, PixelFormats.Default);
            renderBitmap.Render(canvasWrapper);
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            //MemoryStream stream = new MemoryStream();
            //encoder.Save(stream);
#if DEBUG
            FileStream fs = File.Create("test.bmp");
            encoder.Save(fs);
            fs.Close();
#endif

            BitmapImage ret = new BitmapImage();
            //ret.StreamSource = stream;

            //stream.Close();

            return ret;
        }
        internal static Color[,] BitmapToColorMatrix(BitmapImage image)
        {
            throw new NotImplementedException();
        }
    }
}
