﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }


        public override void Run()
        {
            FIR fir = new FIR();
            fir.InputFilterType = FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;

            Signal filteredSignal;

            if (M == 0 && L != 0)  // Up sample by L factor and then apply low pass filter.
            {
                filteredSignal = UpSampling(InputSignal);
                fir.InputTimeDomainSignal = filteredSignal;
                fir.Run();
                OutputSignal = fir.OutputHn;
            }
            else if(M != 0 && L == 0)  // Apply filter first and thereafter down sample by M factor.
            {
                fir.InputTimeDomainSignal = InputSignal;
                fir.Run();
                OutputSignal = DownSampling(fir.OutputHn);
            }
            else if(M != 0 && L != 0)  // This means we want to change sample rate by fraction.
            {
                // Thus, first up sample by L factor, apply low pass filter
                filteredSignal = UpSampling(InputSignal);
                fir.InputTimeDomainSignal = filteredSignal;

                // Then down sample by M factor.
                OutputSignal = DownSampling(filteredSignal);
            }
            else  // Return error message
            {
                Console.WriteLine("Error! Please enter a value for the Decimation and Interpolation Factors (M & L).");
            }
        }

        public Signal UpSampling(Signal signal)
        {
            int N = signal.Samples.Count;
            List<float> samples = new List<float>();
            for(int i=0; i<N; i++)
            {
                samples.Add(signal.Samples[i]);
                for(int j = 0; j < L - 1; j++)
                {
                    samples.Add(0);
                }
            }
            signal.Samples = samples;
            return signal;
        }

        public Signal DownSampling(Signal signal)
        {
            int N = signal.Samples.Count;
            List<float> samples = new List<float>();
            int i = 0;
            while(i < N)
            {
                samples.Add(signal.Samples[i]);
                i += (M - 1);
            }
            signal.Samples = samples;
            return signal;
        }
    }
}