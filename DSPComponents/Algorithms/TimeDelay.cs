using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            // 1) Calculate correlation
            DirectCorrelation dc = new DirectCorrelation();
            dc.InputSignal1 = InputSignal1;
            dc.InputSignal2 = InputSignal2;
            dc.Run();

            // 2) Find the max absolute value
            float maxAbsoluteValue = 0, maxAbsoluteValueIndex = 0;
            for (int i=0; i < dc.OutputNonNormalizedCorrelation.Count; i++)
            {
                if(Math.Abs(dc.OutputNonNormalizedCorrelation[i]) > maxAbsoluteValue)
                {
                    maxAbsoluteValue = Math.Abs(dc.OutputNonNormalizedCorrelation[i]);

                    maxAbsoluteValueIndex = i;   // 3) Save its lag(j)
                }
            }

            OutputTimeDelay = maxAbsoluteValueIndex * InputSamplingPeriod;  // 4) Time delay = j * Ts
        }
    }
}
