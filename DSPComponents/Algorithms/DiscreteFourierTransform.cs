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

            int count = InputTimeDomainSignal.Samples.Count;

            float freq = (float)Math.Round(((2 * Math.PI) / (count * (1 / InputSamplingFrequency))),4);


            for (int i = 0; i < count; i++)
            {
                number num = new number();
                Complex sum= new Complex(0,0);

                for (int j = 0; j < count; j++)
                {
                    num.cos += InputTimeDomainSignal.Samples[j] * ((float)Math.Cos((2 * Math.PI * i * j) / count));
                    num.sin += -InputTimeDomainSignal.Samples[j] * ((float)Math.Sin((2 * Math.PI * i * j) / count));
                    Complex temp = new Complex(InputTimeDomainSignal.Samples[j] * ((float)Math.Cos((2 * Math.PI * i * j) / count)), -InputTimeDomainSignal.Samples[j] * ((float)Math.Sin((2 * Math.PI * i * j) / count)));
                    sum = new Complex(temp.Real + sum.Real, temp.Imaginary + sum.Imaginary);

                }
                amp.Add((float)(Math.Sqrt(Math.Pow(num.cos, 2) + Math.Pow(num.sin, 2))));
                //amp.Add((float)sum.Magnitude);
                phaseShift.Add((float)Math.Atan2(num.sin,num.cos));
               // phaseShift.Add((float)sum.Phase);
                frequencies.Add((float)Math.Round((freq * i),1,MidpointRounding.AwayFromZero));
            }
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false, frequencies, amp, phaseShift);
        }
    }
}
