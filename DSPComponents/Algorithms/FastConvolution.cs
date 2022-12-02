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
            int N1 = InputSignal1.Samples.Count;
            int N2 = InputSignal2.Samples.Count;
            int N = N1 + N2 - 1;
            
            for(int i = N1; i < N; i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for(int i = N2; i < N; i++)
            {
                InputSignal2.Samples.Add(0);
            }

            // Converting InputSignal1 to Fourier Transform
            DiscreteFourierTransform DFTSignal1 = new DiscreteFourierTransform();
            DFTSignal1.InputTimeDomainSignal = InputSignal1;
            DFTSignal1.Run();

            // Converting InputSignal2 to Fourier Transform
            DiscreteFourierTransform DFTSignal2 = new DiscreteFourierTransform();
            DFTSignal2.InputTimeDomainSignal = InputSignal2;
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
            List<Complex> X = new List<Complex>();
            for (int i = 0; i < N; i++)
            {
                X.Add(Complex.Multiply(signal1[i], signal2[i]));
            }

            // FD-1 {X1(K) . X2(K)}
            Signal Idft = new Signal(new List<float>(), false);
            Idft.FrequenciesAmplitudes = new List<float>();
            Idft.FrequenciesPhaseShifts = new List<float>();
            for (int i = 0; i < N; i++)
            {
                double realTmp = Math.Pow(X[i].Real, 2);
                double imagTmp = Math.Pow(X[i].Imaginary, 2);
                Idft.FrequenciesAmplitudes.Add((float)Math.Sqrt(realTmp + imagTmp));

                float phaseShiftTmp = (float)Math.Atan2(X[i].Imaginary, X[i].Real);
                Idft.FrequenciesPhaseShifts.Add(phaseShiftTmp);
            }
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = Idft;
            IDFT.Run();
            OutputConvolvedSignal = new Signal(IDFT.OutputTimeDomainSignal.Samples, false);
        }
    }
}
