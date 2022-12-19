using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public void FFT(ref List<Complex> complexes , int count)
        {
            // base case
            if (count == 1)
                return;
            
            // splitting even and odd
            List<Complex> even_list = new List<Complex>();
            List<Complex> odd_list = new List<Complex>();

            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                    even_list.Add(complexes[i]);
                else
                    odd_list.Add(complexes[i]);
            }
            int new_count = count / 2;

            // recursion part
            FFT(ref even_list,new_count);
            FFT(ref odd_list, new_count);

            //combine
            for (int i = 0; i < new_count; i++)
            {
                float real = (float)Math.Cos((2 * Math.PI * i ) / count);
                float img = (float)Math.Sin((-2 * Math.PI * i ) / count);
                Complex temp = new Complex(real, img) * odd_list[i];
                complexes[i] = even_list[i] + temp;
                complexes[i + new_count] = even_list[i] - temp;

            }

        }

        public override void Run()
        {
            List<float> phaseShift = new List<float>();
            List<float> amp = new List<float>();
            List<float> frequencies = new List<float>();
            List<Complex> complexes= new List<Complex>();

            int count = InputTimeDomainSignal.Samples.Count;

            //float freq = (int)((2 * Math.PI) / (count * (1 / InputSamplingFrequency)));
            float freq = (float)Math.Round(((2 * Math.PI) / (count * (1 / InputSamplingFrequency))), 4);

            for (int i = 0; i < count;i++)
                complexes.Add(new Complex(InputTimeDomainSignal.Samples[i],0));

            FFT(ref complexes, count);

            for (int i = 0; i < count; i++)
            {
                amp.Add((float)(Math.Sqrt(Math.Pow(complexes[i].Real, 2) + Math.Pow(complexes[i].Imaginary, 2))));
                phaseShift.Add((float)Math.Atan2(complexes[i].Imaginary, complexes[i].Real));
                frequencies.Add((float)Math.Round((freq * i), 1, MidpointRounding.AwayFromZero));
            }
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false, frequencies, amp, phaseShift);
        }
    }
}
