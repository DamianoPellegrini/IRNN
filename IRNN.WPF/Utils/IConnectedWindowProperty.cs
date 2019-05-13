using System.Windows;

namespace IRNN.WPF.Utils {

    /// <summary>
    /// Struttura contente le proprietà basilari di una finestra
    /// </summary>
    public struct WindowProperty {
#pragma warning disable CS0169 // Rimuovi i membri privati inutilizzati
        private double Width;
        private double Height;
        private double Left;
        private double Top;
        private WindowState State;
#pragma warning restore CS0169 // Rimuovi i membri privati inutilizzati
    }

    internal interface IConnectedWindowProperty {

        void Show(WindowProperty prop);

        //void Hide(WindowProperty prop);
    }
}