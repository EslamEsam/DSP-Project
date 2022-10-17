using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            float min = InputSignal.Samples.Min();
            float max = InputSignal.Samples.Max();
            List<float> outputsignals = new List<float>();
            //for (int i = 0; i < InputSignal.Samples.Count; i++)
            //{
            //    outputsignals.Add(((InputSignal.Samples[i] - min) / max - min  ) * ((InputMaxRange - InputMinRange) + InputMinRange ));

            //}
            if (InputMinRange < 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    outputsignals.Add((2 * ((InputSignal.Samples[i] - min ) / (max - min ))) - 1);
                }
            }
            else
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    outputsignals.Add ( (InputSignal.Samples[i] - min) / (max - min) );
                }
            }
            OutputNormalizedSignal= new Signal(outputsignals, false);
        }
    }
}
