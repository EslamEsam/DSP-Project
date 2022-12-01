using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
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
                signal2.Add(new Complex(real, imaginary));
            }

            // X1(K) . X2(K)
            List<float> X = new List<float>();
            for (int i = 0; i < N; i++)
            {
                X.Add((float)(signal1[i].Real * signal2[i].Real) + (float)(signal1[i].Imaginary * signal2[i].Imaginary));
            }

            // FD-1 {X1(K) . X2(K)}
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = new Signal(X, false);
            IDFT.Run();

            // 1/N . FD-1 {X1(K) . X2(K)}
            List<float> outputConvolvedSignal = new List<float>();
            for (int j = 0; j < N; j++)
            {
                float r = IDFT.OutputTimeDomainSignal.Samples[j];
                r *= (1 / (float)N);
                outputConvolvedSignal.Add(r);
            }
            OutputConvolvedSignal = new Signal(outputConvolvedSignal, false);
        }
    }
}
