using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List <float> samples = new List<float>();
            List<int> samplesIndices = new List<int>();
            int total = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            for (int i = 0; i < total; i++)
            {
                float x = 0, h = 0, sum = 0;
                for (int j = 0; j <= i; j++)
                {
                    x = (i - j < InputSignal1.Samples.Count)? x = InputSignal1.Samples[i - j]: x = 0;
                    h = (j < InputSignal2.Samples.Count) ? h = InputSignal2.Samples[j]: h = 0;
                    
                    sum += x * h;
                    
                       
                }
                if (i == total - 1 && sum == 0)
                    continue;
                samples.Add(sum);
            }
            int min = Math.Min(InputSignal1.SamplesIndices.Min(), InputSignal2.SamplesIndices.Min());
            int max = Math.Max(InputSignal1.SamplesIndices.Max(), InputSignal2.SamplesIndices.Max());
            if (InputSignal1.Samples.Count > InputSignal2.Samples.Count)
            {
                for (int i = min-1; i <= max ; i++)
                {
                    samplesIndices.Add(i);
                }
            }
            else
            {
                for (int i = min ; i <= max+1; i++)
                {
                    samplesIndices.Add(i);
                }
            }
            
            OutputConvolvedSignal = new Signal(samples,samplesIndices, false);
        }
    }
}
