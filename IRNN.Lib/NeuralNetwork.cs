using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRNN
{
    public class NeuralNetwork
    {
        private List<NeuralLayer> _layers;

        public List<NeuralLayer> NeuralLayers
        {
            get { return _layers; }
            set { _layers = value; }
        }

    }
}
