using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public float windowFn(int type , int n , int N)
        {
            float result;

            if (type == 1) {
                result = 1;
            }
            else if (type == 2)
            {
                result = (float)(0.5 + (0.5 * Math.Cos((2 * Math.PI * n) / N)));
            }
            else if (type == 3)
            {
                result = (float)(0.54 + (0.46 * Math.Cos((2 * Math.PI * n) / N)));
            }
            else
            {
                float temp1 = (float)(0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)));
                float temp2 = (float)(0.08 * Math.Cos((4 * Math.PI * n) / (N - 1)));
                result = (float)(0.42 + temp1 + temp2);
            }

            return result;
        }

        public override void Run()
        {
            List<float> Hn = new List<float>();
            List<float> Yn = new List<float>();
            List<int> indcies = new List<int>();

            float normTW = InputTransitionBand / InputFS;
            int N , type;
            float res;
            //delta f
            if (InputStopBandAttenuation <= 21)
            {
                res = (int)(0.9 / normTW);
                type = 1;
            }
            else if (InputStopBandAttenuation <= 44) 
            {
                res = (int)(3.1 / normTW);
                type = 2;
            }
            else if (InputStopBandAttenuation <= 53)
            {
                res = (int)(3.3 / normTW);
                type = 3;
            }
            else
            {
                res = (int)(5.5 / normTW);
                type = 4;
            }
            // calculate N
            if (res % 2 == 0)
            {
                N = (int)(res + 1);
            }
            else
            {
                N = (int)(res + 2);
            }
            // cutoff freq
            float fc1 = 0,fc2 = 0;
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                fc1 = (float)(InputCutOffFrequency + (InputTransitionBand / 2));
                fc1 /= InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                fc1 = (float)(InputCutOffFrequency - (InputTransitionBand / 2));
                fc1 /= InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                fc1 = (float)(InputF1 - (InputTransitionBand / 2));
                fc2 = (float)(InputF2 + (InputTransitionBand / 2));
                fc1 /= InputFS;
                fc2 /= InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                fc1 = (float)(InputF1 + (InputTransitionBand / 2));
                fc2 = (float)(InputF2 - (InputTransitionBand / 2));
                fc1 /= InputFS;
                fc2 /= InputFS;
            }

            for (int i = -(int)(N / 2); i <= (N/2); i++)
            {
                indcies.Add(i);
            }

            // calculate Hn
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                for (int i = 0; i <N; i++)
                {
                    int n = (int)Math.Abs(indcies[i]);
                    if (n == 0)
                    {
                        float hn = (float)(2 * fc1);
                        float wn = windowFn(type, n, N);
                        Hn.Add(hn * wn);
                    }
                    else
                    {
                        float wc = (float)(2 * Math.PI * fc1 * n);
                        float hn = (float)((2 * fc1 * Math.Sin(wc)) / wc);
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                for (int i = 0; i < N; i++)
                {
                    int n = (int)Math.Abs(indcies[i]);
                    if (n == 0)
                    {
                        float hn = (float) (2 * fc1);
                        hn = 1 - hn;
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                    else
                    {
                        float wc = (float)(2 * Math.PI * fc1 * n);
                        float hn = (float)((-2 * fc1 * Math.Sin(wc)) / wc);
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                for (int i = 0; i < N; i++)
                {
                    int n = (int)Math.Abs(indcies[i]);
                    if (n == 0)
                    {
                        float hn = (float)(2 * (fc2 - fc1));
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                    else
                    {
                        float wc1 = (float)(2 * Math.PI * fc1 * n);
                        float wc2 = (float)(2 * Math.PI * fc2 * n);
                        float hn = (float)((2 * fc2 * (Math.Sin(wc2) / wc2)) - (float)((2 * fc1 * ((Math.Sin(wc1)) / wc1))));
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    int n = (int)Math.Abs(indcies[i]);
                    if (n == 0)
                    {
                        float hn = (float)(2 * (fc2 - fc1));
                        hn = 1 - hn;
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                    else
                    {
                        float wc1 = (float) (2 * Math.PI * fc1 * n);
                        float wc2 = (float)(2 * Math.PI * fc2 * n);
                        float hn = (float)((2 * fc1 * Math.Sin(wc1)) / wc1) - (float)((2 * fc2 * Math.Sin(wc2)) / wc2);
                        float wn = windowFn(type, n, N);
                        Hn.Add((hn * wn));
                    }
                }
            }

            OutputHn = new Signal(Hn, indcies, false);
            List<float> copy = Hn.ToList();
            // calculate Yn 
            DirectConvolution fc = new DirectConvolution();
            fc.InputSignal1 = InputTimeDomainSignal;
            fc.InputSignal2 = new Signal(copy, indcies, false);
            fc.Run();
            OutputYn = fc.OutputConvolvedSignal;
        }
    }
}
