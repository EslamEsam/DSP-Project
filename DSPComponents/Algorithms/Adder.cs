using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int maxSize = 0;
            for (int i = 0; i < InputSignals.Count; i++)
            {
                if (InputSignals[i].Samples.Count > maxSize)
                    maxSize = InputSignals[i].Samples.Count;
            }

            List<float> outputsignals = new List<float>(maxSize);
            for (int i = 0; i < maxSize; i++)
            {
                outputsignals.Add(0f);
            }

            for (int i = 0; i < InputSignals.Count; i++)
            {
                int size = InputSignals[i].Samples.Count;
                for (int j = 0; j < size; j++)
                {
                    outputsignals[j] += InputSignals[i].Samples[j];
                }
            }
            OutputSignal = new Signal(outputsignals, false);


            //List<float> outputsignals = new List<float>();
            //for (int i = 0; i < InputSignals[0].Samples.Count; i++)
            //{

            //    outputsignals.Add(InputSignals[0].Samples[i] + InputSignals[1].Samples[i]);

            //}
            //OutputSignal = new Signal(outputsignals, false);

        }
    }
}
