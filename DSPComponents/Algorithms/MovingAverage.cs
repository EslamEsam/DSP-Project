using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> averagedSamples = new List<float>();
            for(int i = 0; i < InputSignal.Samples.Count - InputWindowSize +1; i++)
            {
                float sum = 0;
                for (int j = i;j < i+InputWindowSize; j++)
                {
                    sum += InputSignal.Samples[j];
                }
                averagedSamples.Add(sum / InputWindowSize);
                Console.WriteLine(averagedSamples[i]);
            }
            OutputAverageSignal = new Signal(averagedSamples, false);
        }
    }
}
