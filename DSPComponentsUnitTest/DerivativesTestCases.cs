﻿using DSPAlgorithms.Algorithms;
using DSPAlgorithms.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPComponentsUnitTest
{
    [TestClass]
    public class DerivativesTestCases
    {
        [TestMethod]
        public void DerivativesTestcase()
        {
            Derivatives derivatives = new Derivatives();

            var expectedOutput_first = new Signal(new List<float>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false);
            var expectedOutput_second = new Signal(new List<float> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, false);
            derivatives.InputSignal = new Signal(new List<float>() { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f, 17f, 18f, 19f, 20f, 21f, 22f, 23f, 24f, 25f, 26f, 27f, 28f, 29f, 30f, 31f, 32f, 33f, 34f, 35f, 36f, 37f, 38f, 39f, 40f, 41f, 42f, 43f, 44f, 45f, 46f, 47f, 48f, 49f, 50f, 51f, 52f, 53f, 54f, 55f, 56f, 57f, 58f, 59f, 60f, 61f, 62f, 63f, 64f, 65f, 66f, 67f, 68f, 69f, 70f, 71f, 72f, 73f, 74f, 75f, 76f, 77f, 78f, 79f, 80f, 81f, 82f, 83f, 84f, 85f, 86f, 87f, 88f, 89f, 90f, 91f, 92f, 93f, 94f, 95f, 96f, 97f, 98f, 99f, 100f }, false);

            derivatives.Run();

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput_first.Samples, derivatives.FirstDerivative.Samples) && UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput_second.Samples, derivatives.SecondDerivative.Samples));

        }
    }
}
