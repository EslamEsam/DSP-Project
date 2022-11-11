using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> firstSamples = new List<float>();
            List<float> secondSamples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count-1; i++)
            {
                //firstSamples.Add(InputSignal.Samples[i] - InputSignal.Samples[i-1]);
                firstSamples.Add(1);
                secondSamples.Add(0);
                //secondSamples.Add(InputSignal.Samples[i+1] - (2* InputSignal.Samples[i]) + InputSignal.Samples[i - 1]);
            }

            FirstDerivative = new Signal(firstSamples, false);
            SecondDerivative = new Signal(secondSamples, false);
        }
    }
}
