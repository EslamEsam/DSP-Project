using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public struct number
        {
            public float cos;
            public float sin;
        };

        public override void Run()
        {
            
            List<float> phaseShift = new List<float>();
            List<float> amp = new List<float>();
            List<float> frequencies = new List<float>();

            float freq = (int)((2 * Math.PI) / (InputTimeDomainSignal.Samples.Count * (1 / InputSamplingFrequency)));

            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                number num = new number();

                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
                {
                    num.cos += InputTimeDomainSignal.Samples[j] * ((float)Math.Cos((2 * Math.PI * i * j) / InputTimeDomainSignal.Samples.Count));
                    num.sin += -1*InputTimeDomainSignal.Samples[j] * ((float)Math.Sin((2 * Math.PI * i * j) / InputTimeDomainSignal.Samples.Count));

                }
                amp.Add((float)(Math.Sqrt(Math.Pow(num.cos, 2) + Math.Pow(num.sin, 2))));
                phaseShift.Add((float)Math.Atan2(num.sin,num.cos));
                frequencies.Add(freq * (i + 1));
            }
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false, frequencies, amp, phaseShift);
        }
    }
}
