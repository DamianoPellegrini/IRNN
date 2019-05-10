using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRNN
{
    public class NeuralLayer
    {
        private List<Neuron> _neurons;

        public List<Neuron> Neurons
        {
            get { return _neurons; }
            set { _neurons = value; }
        }
    }
}
