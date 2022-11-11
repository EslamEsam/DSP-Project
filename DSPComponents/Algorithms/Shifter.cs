using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public bool shifted = false;

        public override void Run()
        {
            List<int> samplesIndices = new List<int>();
            List<int> samples = new List<int>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Periodic)
                    samplesIndices.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                else
                    samplesIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
               
            }
            
            OutputShiftedSignal = new Signal(InputSignal.Samples, samplesIndices, true, InputSignal.Frequencies, InputSignal.FrequenciesAmplitudes, InputSignal.FrequenciesPhaseShifts);
        }
    }
}
