using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> samples = new List<float>();
            List<int> samplesIndcies = new List<int>();
            int max = InputSignal.Samples.Count -1;
            bool flag = InputSignal.Periodic;
            for (int i = max; i >= 0;i-- )
            {
                samples.Add(InputSignal.Samples[i]);
                samplesIndcies.Add(-1 * InputSignal.SamplesIndices[i]);
            }
            
            if (flag)
                flag = false;
            else
                flag = true;
            OutputFoldedSignal = new Signal(samples, samplesIndcies, flag, InputSignal.Frequencies, InputSignal.FrequenciesAmplitudes, InputSignal.FrequenciesPhaseShifts);
        }
    }
}
