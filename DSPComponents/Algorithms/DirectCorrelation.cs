using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            int N = InputSignal1.Samples.Count;

            if (InputSignal2 == null)  // Auto Correlation
            {
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                for (int i = 0; i < N; i++)
                {
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);
                }
            }
            double sum_signal1 = 0, sum_signal2 = 0;
            for (int n = 0; n < N; n++)
            {

                sum_signal1 += (InputSignal1.Samples[n] * InputSignal1.Samples[n]);
                sum_signal2 += (InputSignal2.Samples[n] * InputSignal2.Samples[n]);
            }

            double signals_mult = sum_signal1 * sum_signal2;
            signals_mult = Math.Sqrt(signals_mult);
            signals_mult /= N;

            for (int j = 0; j < N; j++)  // Until rotated signal is the same as the original = number of samples
            {
                double r = 0;
                for (int n = 0; n < N; n++)
                {
                    r += (InputSignal1.Samples[n] * InputSignal2.Samples[n]);

                }

                r /= N;

                double normalize = r / signals_mult;
                
                OutputNonNormalizedCorrelation.Add((float)r);
                if (normalize != 0)
                    OutputNormalizedCorrelation.Add((float)normalize);
                else
                    OutputNormalizedCorrelation.Add(0);

                float shiftedSample = InputSignal2.Samples[0];
                for (int i = 0; i < N - 1; i++)
                {
                    InputSignal2.Samples[i] = InputSignal2.Samples[i + 1];
                }

                if (InputSignal2.Periodic)
                    InputSignal2.Samples[N - 1] = shiftedSample;
                else
                    InputSignal2.Samples[N - 1] = 0;
            }
        }
    }
}