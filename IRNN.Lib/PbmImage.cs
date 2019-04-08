using System;
using System.Collections.Generic;
using System.IO;
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
        public byte[,] Image
        {
            get
            {
                return data;
            }
            private set
            {
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
            string[] file = File.ReadAllLines(filePath);
            int comment = 0;
            for (int i = 0; i < file.Length; i++)
            {
                //if (file[i].StartsWith("#"))
                //    comment++;

                string[] line = file[i].Split(' ');
                if (i == (0 + comment)) continue;//tipo
                else if (i == 1 + comment)
                    data = new byte[int.Parse(line[1]), int.Parse(line[0])];
                else
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //FIX loading
                        data[i - 2 - comment, j] = byte.Parse(line[j]);
                    }
                }

            }
        }
    }
}
