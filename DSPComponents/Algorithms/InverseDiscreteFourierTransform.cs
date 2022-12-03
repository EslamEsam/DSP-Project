using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public struct number
        {
            public float cos;
            public float sin;
        };

        public override void Run()
        {
            List<float> samples = new List<float>();

            int count = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            for (int i = 0; i < count; i++)
            {
                number num = new number();

                for (int j = 0; j < count; j++)
                {
                    //(amp*phase)*(2*pi*n*k / N)
                    num.cos += (InputFreqDomainSignal.FrequenciesAmplitudes[j] * (float)Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[j])) * ((float)Math.Cos((2 * Math.PI * i * j) / count));
                    num.sin += -1 * (InputFreqDomainSignal.FrequenciesAmplitudes[j] * (float)Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[j])) * ((float)Math.Sin((2 * Math.PI * i * j) / count));
                }
                
                samples.Add((num.cos + num.sin) / count);
            }
            OutputTimeDomainSignal = new Signal(samples, false);
        }
    }
}
