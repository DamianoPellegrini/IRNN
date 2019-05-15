using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace IRNN {

    public class Network {

        #region -- Properties --

        public double LearnRate {
            get; set;
        }

        public double Momentum {
            get; set;
        }

        public List<Neuron> InputLayer {
            get; set;
        }

        public List<List<Neuron>> HiddenLayers {
            get; set;
        }

        public List<Neuron> OutputLayer {
            get; set;
        }

        #endregion -- Properties --

        #region -- Globals --

        private static readonly Random Random = new Random();

        #endregion -- Globals --

        #region -- Constructor --

        public Network() {
            LearnRate = 0;
            Momentum = 0;
            InputLayer = new List<Neuron>();
            HiddenLayers = new List<List<Neuron>>();
            OutputLayer = new List<Neuron>();
        }

        public Network(int inputSize, int[] hiddenSizes, int outputSize, double? learnRate = null, double? momentum = null) {
            LearnRate = learnRate ?? .4;
            Momentum = momentum ?? .9;
            InputLayer = new List<Neuron>();
            HiddenLayers = new List<List<Neuron>>();
            OutputLayer = new List<Neuron>();

            for (var i = 0; i < inputSize; i++)
                InputLayer.Add(new Neuron());

            var firstHiddenLayer = new List<Neuron>();
            for (var i = 0; i < hiddenSizes[0]; i++)
                firstHiddenLayer.Add(new Neuron(InputLayer));

            HiddenLayers.Add(firstHiddenLayer);

            for (var i = 1; i < hiddenSizes.Length; i++) {
                var hiddenLayer = new List<Neuron>();
                for (var j = 0; j < hiddenSizes[i]; j++)
                    hiddenLayer.Add(new Neuron(HiddenLayers[i - 1]));
                HiddenLayers.Add(hiddenLayer);
            }

            for (var i = 0; i < outputSize; i++)
                OutputLayer.Add(new Neuron(HiddenLayers.Last()));
        }

        #endregion -- Constructor --

        #region -- Training --

        public void Train(List<DataSet> dataSets, int numEpochs) {
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\data.txt", "");
            var errors = new List<double>();
            for (var i = 0; i < numEpochs; i++) {
                foreach (var dataSet in dataSets) {
                    ForwardPropagate(dataSet.Values);
                    BackPropagate(dataSet.Targets);
                    errors.Add(CalculateError(dataSet.Targets));
                }
                WriteErrorOnFile(errors.Average(), i);
                Debug.WriteLine(errors.Average() + "|" + i);
            }
        }

        public void Train(List<DataSet> dataSets, double minimumError) {
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\data.txt", "");
            var error = 1.0;
            var numEpochs = 0;
            var errors = new List<double>();

            while (error > minimumError && numEpochs < int.MaxValue) {
                foreach (var dataSet in dataSets) {
                    ForwardPropagate(dataSet.Values);
                    BackPropagate(dataSet.Targets);
                    errors.Add(CalculateError(dataSet.Targets));
                }
                error = errors.Average();
                numEpochs++;
                WriteErrorOnFile(error, numEpochs);
                Debug.WriteLine(error + "|" + numEpochs);
            }
        }

        private void ForwardPropagate(params double[] inputs) {
            var i = 0;
            InputLayer.ForEach(a => a.Value = inputs[i++]);
            HiddenLayers.ForEach(a => a.ForEach(b => b.CalculateValue()));
            OutputLayer.ForEach(a => a.CalculateValue());
        }

        private void BackPropagate(params double[] targets) {
            var i = 0;
            OutputLayer.ForEach(a => a.CalculateGradient(targets[i + 1]));
            HiddenLayers.Reverse();
            HiddenLayers.ForEach(a => a.ForEach(b => b.CalculateGradient()));
            HiddenLayers.ForEach(a => a.ForEach(b => b.UpdateWeights(LearnRate, Momentum)));
            HiddenLayers.Reverse();
            OutputLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
        }

        public double[] Compute(params double[] inputs) {
            ForwardPropagate(inputs);
            return OutputLayer.Select(a => a.Value).ToArray();
        }

        private double CalculateError(params double[] targets) {
            var i = 0;
            return OutputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i + 1])));
        }

        #endregion -- Training --

        #region -- Helpers --

        public static double GetRandom() {
            return 2 * Random.NextDouble() - 1;
        }

        private void WriteErrorOnFile(double error, int currentEpoch) {
            StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + "\\data.txt");
            sw.WriteLine((currentEpoch) + "|" + error);
            sw.Close();
        }

        public List<List<Neuron>> GetAllLayers() {
            List<List<Neuron>> ret = new List<List<Neuron>>();
            ret.Add(InputLayer);//Input
            foreach (var item in HiddenLayers) {
                ret.Add(item);
            }
            ret.Add(OutputLayer);//Output
            return ret;
        }

        #endregion -- Helpers --
    }

    #region -- Enum --

    public enum TrainingType {
        Epoch,
        MinimumError
    }

    #endregion -- Enum --
}