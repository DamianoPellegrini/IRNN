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
        private double[,] data;

        /// <summary>
        /// The image matrix.
        /// </summary>
        public double[,] Image
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
                return data.GetLength(1);
            }
        }

        /// <summary>
        /// Height of the image.
        /// </summary>
        public int Height
        {
            get
            {
                return data.GetLength(0);
            }
        }

        /// <summary>
        /// Return the image in a linear array.
        /// </summary>
        public double[] Array
        {
            get
            {
                return ConvertMatToArray();
            }
        }

        /// <summary>
        /// Return the image in a linear list.
        /// </summary>
        public List<double> List
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
        public List<double> ConvertMatToList()
        {
            List<double> list = new List<double>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    list.Add(data[i, j]);
                }
            }

            return list;
        }

        /// <summary>
        /// Convert the matrix to a linear array.
        /// </summary>
        /// <returns>The linear array.</returns>
        public double[] ConvertMatToArray()
        {
            double[] arr = new double[Width * Height];
            int cont = 0;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    arr[cont] = data[i, j];
                    cont++;
                }
            }

            return arr;
        }

        /// <summary>
        /// Convert the matrix to a jagged array.
        /// </summary>
        /// <returns>The jagged array.</returns>
        public double[][] ConvertMatToJaggedArray()
        {
            double[][] jaggedArray = new double[Height][];

            for (int i = 0; i < Height; i++)
            {
                jaggedArray[i] = new double[Width];
                for (int j = 0; j < Width; j++)
                {
                    jaggedArray[i][j] = (double)Image[i, j];
                }
            }

            return jaggedArray;
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
                //Conta i commenti iniziali per poterli gestire in lettura ignorandoli
                if (file[i].StartsWith("#")) {
                    comment++;
                    continue; //end this loop cycle
                }

                string[] line = file[i].Split(' ');
                if (i == (0 + comment)) continue;//tipo del formato pbm
                else if (i == 1 + comment)
                    data = new double[int.Parse(line[1]), int.Parse(line[0])]; //ottiene le dimensioni dell matrice
                else
                {
                    for (int j = 0; j < Width; j++)
                    {
                        data[i - 2 - comment, j] = double.Parse(line[j]);
                    }
                }

            }
        }
    }
}
