using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRNN
{
    /// <summary>
    /// Defines a PBM image as a class.
    /// </summary>
    public class PBMImage
    {
        //TODO Magari aggiungere un tipo tipo il P1

        
        private byte[,] data;

        /// <summary>
        /// The image matrix.
        /// </summary>
        public byte[,] Image {
            get {
                return data;
            }
            private set {
                data = value;
            }
        }

        /// <summary>
        /// Width of the image.
        /// </summary>
        public int Width
        {
            get
            {
                return data.GetLength(0);
            }
        }

        /// <summary>
        /// Height of the image.
        /// </summary>
        public int Height
        {
            get
            {
                return data.GetLength(1);
            }
        }

        /// <summary>
        /// Return the image in a linear array.
        /// </summary>
        public byte[] Array
        {
            get
            {
                return ConvertMatToArray();
            }
        }

        /// <summary>
        /// Return the image in a linear list.
        /// </summary>
        public List<byte> List
        {
            get
            {
                return ConvertMatToList();
            }
        }

        /// <summary>
        /// Convert the matrix to a linear list.
        /// </summary>
        /// <returns>The linear list.</returns>
        private List<byte> ConvertMatToList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert the matrix to a linear array.
        /// </summary>
        /// <returns>The linear array.</returns>
        private byte[] ConvertMatToArray()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load the image from path
        /// </summary>
        /// <param name="filePath">Path to the image.</param>
        public PBMImage(string filePath)
        {
            //TODO load image to matrix
        }
    }
}
