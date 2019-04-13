/*
 * Author: Nikola Živković
 * Website: rubikscode.net
 * Year: 2018
 */

using System;

namespace NeuralNetworkCSharp.ActivationFunctions
{
    /// <summary>
    /// Implementation of Sigmoid Activation Function.
    /// </summary>
    public class SigmoidActivationFunction : IActivationFunction
    {
        private double _coefficient;

        public SigmoidActivationFunction(double coefficient)
        {
            _coefficient = coefficient;
        }

        public double CalculateOutput(double input)
        {
            return (1 / (1 + Math.Exp(-input * _coefficient)));
        }
    }
}
