using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseFastFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public void IFFT(ref List<Complex> complexes, int count)
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
            IFFT(ref even_list, new_count);
            IFFT(ref odd_list, new_count);

            //combine
            for (int i = 0; i < new_count; i++)
            {
                float real = (float)Math.Cos((2 * Math.PI * i) / count);
                float img = (float)Math.Sin((2 * Math.PI * i) / count);
                Complex temp = new Complex(real, img) * odd_list[i];
                complexes[i] = even_list[i] + temp;
                complexes[i + new_count] = even_list[i] - temp;

            }

        }

        public override void Run()
        {
            List<float> samples = new List<float>();

            List<Complex> complexes = new List<Complex>();

            int count = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            for (int i = 0; i < count; i++)
            {
                float amp = InputFreqDomainSignal.FrequenciesAmplitudes[i];
                float phase = InputFreqDomainSignal.FrequenciesPhaseShifts[i];

                complexes.Add(new Complex((amp * Math.Cos(phase)), (amp * Math.Sin(phase))));
            }

            IFFT(ref complexes, count);


            for (int i = 0; i < count; i++)
            {

                samples.Add((float)(complexes[i].Real + complexes[i].Imaginary) / count);
            }
            OutputTimeDomainSignal = new Signal(samples, false);
        }
    }
}
