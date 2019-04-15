/*
 * Author: Nikola Živković
 * Website: rubikscode.net
 * Year: 2018
 */

using IRNN;
using NeuralNetworkCSharp.Neuron;
using NeuralNetworkCSharp.ActivationFunctions;
using NeuralNetworkCSharp.InputFunctions;
using NeuralNetworkCSharp.Synapses;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NeuralNetworkCSharp.Neuron
{
    /// <summary>
    /// Neural Network implementation.
    /// </summary>
    /// 

        //cambiato tipo di input per PushInputLayer nella firma del suo metodo, stessa cosa per Train per la firma
    public class SimpleNeuralNetwork
    {
        private NeuralLayerFactory _layerFactory;
       

        internal List<NeuralLayer> _layers;
        internal Dictionary<int, double[]> _neuronErrors;
        internal double _learningRate;
        internal double[][] _expectedResult;

        public PBMImage _pbmImage;

        /// <summary>
        /// Constructor of the Neural Network.
        /// Note:
        /// Initialy input layer with defined number of inputs will be created.
        /// </summary>
        /// <param name="numberOfInputNeurons">
        /// Number of neurons in input layer.
        /// </param>
        public SimpleNeuralNetwork() //TODO: aggiungere path immagine selezionata oppure creata da utente 
        {
            Loader.Load();
            _layers = new List<NeuralLayer>();
            _neuronErrors = new Dictionary<int, double[]>();
            _layerFactory = new NeuralLayerFactory();

            _pbmImage = new PBMImage("nuovaImmagine.pbm");  //^todo

            // Create input layer that will collect inputs.
            CreateInputLayer(Loader.height * Loader.width);

            _learningRate = Loader.learningRate;
        }

        /// <summary>
        /// Add layer to the neural network.
        /// Layer will automatically be added as the output layer to the last layer in the neural network.
        /// </summary>
        public void AddLayer(NeuralLayer newLayer)
        {
            if (_layers.Any())
            {
                var lastLayer = _layers.Last();
                newLayer.ConnectLayers(lastLayer);
            }
            
            _layers.Add(newLayer);
            _neuronErrors.Add(_layers.Count - 1, new double[newLayer.Neurons.Count]);
        }

        /// <summary>
        /// Push input values to the neural network.
        /// </summary>
        public void PushInputValues(byte[] inputs)
        {
            _layers.First().Neurons.ForEach(x => x.PushValueOnInput(inputs[_layers.First().Neurons.IndexOf(x)]));
        }

        /// <summary>
        /// Set expected values for the outputs.
        /// </summary>
        public void PushExpectedValues(double[][] expectedOutputs)
        {
            _expectedResult = expectedOutputs;
        }

        /// <summary>
        /// Calculate output of the neural network.
        /// </summary>
        /// <returns></returns>
        public List<double> GetOutput()
        {
            var returnValue = new List<double>();

            _layers.Last().Neurons.ForEach(neuron =>
            {
                 returnValue.Add(neuron.CalculateOutput());
            });

            return returnValue;
        }

        /// <summary>
        /// Train neural network.
        /// </summary>
        /// <param name="inputs">Input values.</param>
        /// <param name="numberOfEpochs">Number of epochs.</param>
        public bool Train(byte[][] inputs, int numberOfEpochs)
        {
            double totalError = 0;

            //this csv file contains all the medium error during all the epoch

            StreamWriter streamWriter = new StreamWriter("data.txt");

            File.WriteAllText("data.txt", String.Empty);

            for (int i = 0; i < numberOfEpochs; i++)
            {
                for(int j = 0; j < inputs.GetLength(0); j ++)
                {
                    PushInputValues(inputs[j]);

                    var outputs = new List<double>();

                    // Get outputs.
                    _layers.Last().Neurons.ForEach(x =>
                    {
                        outputs.Add(x.CalculateOutput());
                    });

                    // Calculate error by summing errors on all output neurons.
                    totalError = CalculateTotalError(outputs, j); 
                    streamWriter.WriteLine((i + 1).ToString() + "|" + totalError);
                    
                    HandleOutputLayer(j);
                    HandleHiddenLayers();
                }
            }
            streamWriter.Close();

            double Error = 0;

            StreamReader streamReader = new StreamReader("data.txt");
            while (streamReader.Peek() > 0)
            {
                Error += (double.Parse(streamReader.ReadLine()));
            }

            if ((Error /= numberOfEpochs) <= Loader.minimumError)
            {
                SaveWeigthOnFile();
                return true;
            }
            else
                return false;
        }



        /// <summary>
        /// Save weight onto an xml file for future recognition 
        /// </summary>
        private void SaveWeigthOnFile()
        {
            File.WriteAllText("Weigth.xml", "");
            XmlDocument xmlDocument = new XmlDocument();

            XmlNode rootNode = xmlDocument.CreateElement("synapses_weigth");
            xmlDocument.AppendChild(rootNode);


            for(int i = 0; i < _layers.Count - 1; i++)
            {
                foreach (Neuron n in _layers[i].Neurons)
                {
                    foreach (Synapse s in n.Outputs)
                    {
                        XmlNode Synapses = xmlDocument.CreateElement("Synapse");
                        rootNode.AppendChild(Synapses);

                        XmlNode weigth = xmlDocument.CreateElement("Weigth");
                        weigth.InnerText = s.Weight.ToString();
                        Synapses.AppendChild(weigth);
                    }
                }
            }
        } //prega dio che funzioni sta merda





        /// <summary>
        /// Helper function that creates input layer of the neural network.
        /// </summary>
        private void CreateInputLayer(int numberOfInputNeurons)
        {
            var inputLayer = _layerFactory.CreateNeuralLayer(numberOfInputNeurons, new SigmoidActivationFunction(1), new WeightedSumFunction());
            inputLayer.Neurons.ForEach(x => x.AddInputSynapse(0));
            this.AddLayer(inputLayer);
        }

        /// <summary>
        /// Hellper function that calculates total error of the neural network.
        /// </summary>
        private double CalculateTotalError(List<double> outputs, int row)
        {
            double totalError = 0;

            outputs.ForEach(output =>
            {
                var error = Math.Pow(output - _expectedResult[row][outputs.IndexOf(output)], 2);
                totalError += error;
            });

            return totalError;
        }
        
        /// <summary>
        /// Hellper function that runs backpropagation algorithm on the output layer of the network.
        /// </summary>
        /// <param name="row">
        /// Input/Expected output row.
        /// </param>
        private void HandleOutputLayer(int row)
        {
            _layers.Last().Neurons.ForEach(neuron =>
            {
                neuron.Inputs.ForEach(connection =>
                {
                    var output = neuron.CalculateOutput();
                    var netInput = connection.GetOutput();

                    var expectedOutput = _expectedResult[row][_layers.Last().Neurons.IndexOf(neuron)];

                    var nodeDelta = (expectedOutput - output) * output * (1 - output);
                    var delta = -1 * netInput * nodeDelta;

                    connection.UpdateWeight(_learningRate, delta);

                    neuron.PreviousPartialDerivate = nodeDelta;
                });
            });
        }

        /// <summary>
        /// Helper function that runs backpropagation algorithm on the hidden layer of the network.
        /// </summary>
        /// <param name="row">
        /// Input/Expected output row.
        /// </param>
        private void HandleHiddenLayers() //reweighting
        {
            for (int k = _layers.Count - 2; k > 0; k--)
            {
                _layers[k].Neurons.ForEach(neuron =>
                {
                    neuron.Inputs.ForEach(connection =>
                    {
                        var output = neuron.CalculateOutput();
                        var netInput = connection.GetOutput();
                        double sumPartial = 0;

                        _layers[k + 1].Neurons
                        .ForEach(outputNeuron =>
                        {
                            outputNeuron.Inputs.Where(i => i.IsFromNeuron(neuron.Id))
                            .ToList()
                            .ForEach(outConnection =>
                            {
                                sumPartial += outConnection.PreviousWeight * outputNeuron.PreviousPartialDerivate;
                            });
                        });

                        var delta = -1 * netInput * sumPartial * output * (1 - output);
                        connection.UpdateWeight(_learningRate, delta);
                    });
                });
            }
        }
    }
}
