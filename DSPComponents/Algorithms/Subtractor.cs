using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            MultiplySignalByConstant msc = new MultiplySignalByConstant();
            msc.InputSignal = InputSignal2;
            msc.InputConstant = -1;
            msc.Run();

            Adder adder = new Adder();
            adder.InputSignals = new List<Signal>();
            adder.InputSignals.Add(InputSignal1);
            adder.InputSignals.Add(msc.OutputMultipliedSignal);
            adder.Run();

            OutputSignal = adder.OutputSignal;

        }
    }
}