using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquationToCanonical;

namespace TestProject
{
    /// <summary>
    /// Execures Unit Tests over the Program Core
    /// </summary>
    [TestClass]
    public class ProcessorTest
    {
        [TestMethod]
        public void TransformEquation_AsGivenTestMethod()
        {
            string equation = "x^2 + 3.5xy + y = y^2 - xy + y";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);

            Assert.AreEqual(result, "x^2 - y^2 + 4.5xy = 0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEquationException))]
        public void TransformEquation_InvalidEquationTestMethod()
        {
            string equation = "1 = 5";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);
        }

        [TestMethod]
        public void TransformEquation_Equation1TestMethod()
        {
            string equation = "x^2 + y^2 + 2xy = -2x^2 - 5y^2 + 2xy - 3";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);

            Assert.AreEqual(result, "3x^2 + 6y^2 + 3 = 0");
        }

        [TestMethod]
        public void TransformEquation_Equation2TestMethod()
        {
            string equation = "x^2 + y^2 + 2xy = -2x^3 + 5y^2 + 2xy - 3";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);

            Assert.AreEqual(result, "2x^3 + x^2 - 4y^2 + 3 = 0");
        }

        [TestMethod]
        public void TransformEquation_Equation3TestMethod()
        {
            string equation = "-9 + 2x - x^4 + z = y - 2xy + w^2";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);

            Assert.AreEqual(result, "-x^4 - w^2 - 9 + 2x + z - y + 2xy = 0");
        }

        [TestMethod]
        public void TransformEquation_Equation4TestMethod()
        {
            string equation = "x^2 + (y^2 + 2xy) = -2x^3 + 5y^2 + (2xy - 3)";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);

            Assert.AreEqual(result, "2x^3 + x^2 - 4y^2 + 3 = 0");
        }

        [TestMethod]
        public void TransformEquation_Equation5TestMethod()
        {
            string equation = "-9 + [2x - x^4] + z = (y - 2xy) + w^2";

            Processor pr = new Processor();
            string result = pr.TransformEquation(equation);

            Assert.AreEqual(result, "-x^4 - w^2 - 9 + 2x + z - y + 2xy = 0");
        }
    }
}
