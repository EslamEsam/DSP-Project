using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public string DecToBin(int num, int levels)
        {
            string result = "";
            while (num >= 1)
            {
                int remainder = num % 2;
                result = Convert.ToString(remainder) + result;
                num /= 2;
            }
            if (levels == 8)
            {
                switch (result.Length)
                {
                    case 1:
                        return "00" + result;
                    case 2:
                        return "0" + result;
                    case 0:
                        return "000";
                }
            }
            if (levels == 4)
            {
                switch (result.Length)
                {
                    case 1:
                        return "0" + result;
                    case 0:
                        return "00";
                }
            }
            return result;
        }
    

        public override void Run()
        {
            List<float> samples = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            if (InputLevel <= 0 || InputNumBits > 0)
            {
                InputLevel = (int)Math.Pow(2, InputNumBits);
            }

            float min = InputSignal.Samples.Min();
            float max = InputSignal.Samples.Max();
            float delta = (max - min) / InputLevel;
            float[] intervals = new float[InputLevel+1];
            intervals[0] = min;
            intervals[InputLevel] = max;
            for (int i = 1; i < InputLevel; i++)
            {
                intervals[i] = intervals[i-1] + delta;
            }
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {

                for (int j = 1; j < InputLevel+1; j++)
                {
                    if (InputSignal.Samples[i] <= intervals[j])
                    {
                        float sig = (float)Math.Round(((Decimal)(intervals[j] + intervals[j - 1]) / 2), 3, MidpointRounding.AwayFromZero);
                        samples.Add(sig);
                        OutputIntervalIndices.Add(j);
                        Console.WriteLine(j);
                        OutputEncodedSignal.Add(DecToBin(j-1,InputLevel));
                        OutputSamplesError.Add(sig - InputSignal.Samples[i]);

                        break;
                    }
                }
            }
            OutputQuantizedSignal = new Signal(samples, false);
        }
    }
}
