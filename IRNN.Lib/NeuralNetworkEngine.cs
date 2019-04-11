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

        public NeuralNetworkEngine(PBMImage img)
        {
            this.img = img;
        }
    }
}
