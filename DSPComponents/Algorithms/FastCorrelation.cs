using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
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

            // Converting InputSignal1 to Fourier Transform
            DiscreteFourierTransform DFTSignal1 = new DiscreteFourierTransform();
            DFTSignal1.InputTimeDomainSignal = InputSignal1;
            DFTSignal1.InputSamplingFrequency = 4;
            DFTSignal1.Run();

            // Converting InputSignal2 to Fourier Transform
            DiscreteFourierTransform DFTSignal2 = new DiscreteFourierTransform();
            DFTSignal2.InputTimeDomainSignal = InputSignal2;
            DFTSignal2.InputSamplingFrequency = 4;
            DFTSignal2.Run();

            List<Complex> signal1 = new List<Complex>();
            List<Complex> signal2 = new List<Complex>();
            float A, phaseShift, real, imaginary;

            for (int i = 0; i < N; i++)
            {
                A = DFTSignal1.OutputFreqDomainSignal.FrequenciesAmplitudes[i];
                phaseShift = DFTSignal1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i];
                real = A * (float)Math.Cos(phaseShift);
                imaginary = A * (float)Math.Sin(phaseShift);
                signal1.Add(new Complex(real, imaginary));

                A = DFTSignal2.OutputFreqDomainSignal.FrequenciesAmplitudes[i];
                phaseShift = DFTSignal2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i];
                real = A * (float)Math.Cos(phaseShift);
                imaginary = A * (float)Math.Sin(phaseShift);
                signal2.Add(new Complex(real, -1 * imaginary));   // X1*(K)
            }

            // X1*(K) . X2(K)
            List<float> X = new List<float>();
            for (int i = 0; i < N; i++)
            {
                X.Add((float)(signal1[i].Real * signal2[i].Real) + (float)(signal1[i].Imaginary * signal2[i].Imaginary));
            }

            // FD-1 {X1*(K) . X2(K)}
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = new Signal(X, false);
            IDFT.Run();

            // 1/N . FD-1 {X1*(K) . X2(K)}  + Normalization
            float sum_signal1 = 0, sum_signal2 = 0;
            for (int n = 0; n < N; n++)
            {
                sum_signal1 += (InputSignal1.Samples[n] * InputSignal1.Samples[n]);
                sum_signal2 += (InputSignal2.Samples[n] * InputSignal2.Samples[n]);
            }
            float normalize = (1 / (float)N) * (float)(Math.Sqrt(sum_signal1 * sum_signal2));

            for (int j = 0; j < N; j++)
            {
                float r = IDFT.OutputTimeDomainSignal.Samples[j];
                r *= (1 / (float)N);

                OutputNonNormalizedCorrelation.Add(r);
                OutputNormalizedCorrelation.Add(r / normalize);
            }
        }
    }
}
