using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> outputSignal = new List<float>();
            int N = InputSignal.Samples.Count;

            float summation;
            for(int k = 0; k < N; k++)
            {
                summation = 0;
                for (int n = 0; n < N; n++)
                {
                    float cos = (float)Math.Cos((float)Math.PI/(4*(float)N) * (2*n-1) * (2*k-1));
                    summation += InputSignal.Samples[n] * cos;
                }
                outputSignal.Add((float)Math.Sqrt(2/(float)N) * summation);
            }

            OutputSignal = new Signal(outputSignal, false);
        }
    }
}
