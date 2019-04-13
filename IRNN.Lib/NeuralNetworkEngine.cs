using NeuralNetworkCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRNN
{
    //caricare configurazioni rete neurale
    //impostare modalità applicazione 
    //gestire l'applicazione sulla modalità scelta 
    //se impostato sulla modalità aiuto mostrare cosa fa il programma, alrimenti eseguire routine
    //routine: caricare i parametri, training set 
    //gestire numero neuroni inserito da utente ed epoche
    //implementare apprendimento ed esecuzione 
    class NeuralNetworkEngine
    {
        PBMImage img;
        SimpleNeuralNetwork simpleNeuralNetwork; 
        public enum ApplicationStatus { Help, Recognition, Training};

        public NeuralNetworkEngine(PBMImage img, ApplicationStatus status)
        {
            this.img = img;
            simpleNeuralNetwork = new SimpleNeuralNetwork();
            StatusHandler(status);
            
        }


        private void StatusHandler(ApplicationStatus status)
        {
            switch (status)
            {
                case ApplicationStatus.Help:
                    //DAMIANO
                    break;
                case ApplicationStatus.Training:
                    simpleNeuralNetwork.Train(img.Image, Loader.epochMaxNumber);
                    break;
                case ApplicationStatus.Recognition:
                    simpleNeuralNetwork.PushInputValues();
                    break;
            }
        }

    }
}
