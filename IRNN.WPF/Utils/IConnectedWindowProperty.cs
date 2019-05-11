using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IRNN.WPF.Utils
{
    public struct WindowProperty
    {
        double Width;
        double Height;
        double Left;
        double Top;
        WindowState State;
    }
    interface IConnectedWindowProperty
    {
        void Show(WindowProperty prop);
        //void Hide(WindowProperty prop);
    }
}
