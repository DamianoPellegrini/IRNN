using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace IRNN
{
    public static class Loader
    {       

        //TODO
        public static int height, width, preambleLength, networkLayersNumber, networkInputs, epochMaxNumber, outputClasses;
        public static int[] neuronNumberPerLayer; //length is based off networkLayersNumber
        public static double minimumError, learningRate, momentum;

        public static void Load()
        {
            StreamReader sr = File.OpenText(@"settings.cfg");
            string[] tempArr = new string[2];
            int cont = 0;
            string tempVar;
            while (sr.Peek() > 0)
            {
                tempVar = sr.ReadLine().Trim().Split('=')[1];
                AllocateVariable(tempVar, cont);
                cont++;
            }
        }

        private static void AllocateVariable(string tempVar, int cont)
        {
            switch (cont)
            {
                case 0:
                    height = int.Parse(tempVar);
                    break;
                case 1:
                    width = int.Parse(tempVar);
                    break;
                case 2:
                    preambleLength = int.Parse(tempVar);
                    break;
                case 3:
                    networkLayersNumber = int.Parse(tempVar);
                    break;
                case 4:
                    networkInputs = int.Parse(tempVar);
                    break;
                case 5:
                    minimumError = double.Parse(tempVar);
                    break;
                case 6:
                    momentum = double.Parse(tempVar);
                    break;
                case 7:
                    learningRate = double.Parse(tempVar);
                    break;
                case 8:
                    epochMaxNumber = int.Parse(tempVar); 
                    break;
                case 9:
                    outputClasses = int.Parse(tempVar);
                    break;
                case 10:
                    neuronNumberPerLayer = new int[networkLayersNumber];
                    for (int i = 0; i < neuronNumberPerLayer.Length; i++)
                    {
                        neuronNumberPerLayer[i] = int.Parse(tempVar.Split('|')[i]);
                    }                  
                    break;
                default:
                    throw new Exception("Invalid cfg file");
            }
        }
    }
}
