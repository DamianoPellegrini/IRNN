using NeuralNetworkCSharp.Neuron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IRNN
{
    //caricare configurazioni rete neurale
    //impostare modalità applicazione 
    //gestire l'applicazione sulla modalità scelta 
    //se impostato sulla modalità aiuto mostrare cosa fa il programma, alrimenti eseguire routine
    //routine: caricare i parametri, training set 
    //gestire numero neuroni inserito da utente ed epoche
    //implementare apprendimento ed esecuzione 
    public class NeuralNetworkEngine
    {
        PBMImage img;
        SimpleNeuralNetwork simpleNeuralNetwork; 
        public enum ApplicationStatus { Help, Recognition, Training};

        public NeuralNetworkEngine(PBMImage img, ApplicationStatus status)
        {
            this.img = img;
            simpleNeuralNetwork = new SimpleNeuralNetwork();
            simpleNeuralNetwork._pbmImage = img;
        }


        public string StatusHandler(ApplicationStatus status)
        {
            switch (status)
            {
                case ApplicationStatus.Help:
                    return "l'app dovrebbe riconoscere le immagini passate in input";                
                case ApplicationStatus.Training:
                    if (simpleNeuralNetwork.Train(img.ConvertMatToJaggedArray(), Loader.epochMaxNumber))
                    {
                        return "Training eseguito con successo";
                    }
                    else
                    {
                        return "Training fallito, ricalcolare i pesi";
                    }
                case ApplicationStatus.Recognition:
                    string[] classes = CaricaClassi();
                    simpleNeuralNetwork.PushInputValues(img.ConvertMatToArray());
                    List<double> output = simpleNeuralNetwork.GetOutput();
                    int classeOutput = RicercaElementoMax(output);
                    //TODO agggiungere caso in cui non viene assolutamente riconosciuta l'immagine
                    return "L'elemento ottenuto ricorda vagamente forse potrebbe un " + classes[classeOutput] + " .";                    
            }

            return "";
        }

        private string[] CaricaClassi()
        {
            StreamReader sr = new StreamReader("trainingset.cfg");
            string[] o = new string[Loader.outputClasses];

            int i = 0;

            while (sr.Peek() > 0)
            {
                o[i] = sr.ReadLine().Split('.')[0];
                i++;
            }

            return o;
        }

        private int RicercaElementoMax(List<double> output)
        {
            double x = -2;
            int index = 0;

            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] > x)
                {
                    x = output[i];
                    index = i;
                }
            }
            return index;
        }
    }
}
