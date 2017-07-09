using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestWithDeploymentFile;

namespace DeploymentItemTest
{
    [TestClass]
    public class AdditionTest
    {
        static Calculation calc;
        /// <summary>
        /// initlize the test variables.
        /// </summary>
        /// <param name="context"></param>
        [ClassInitialize]
        public static void Initlize_Variables(TestContext context)
        {
            calc = new Calculation();
        }
        [TestMethod]
        [DeploymentItem("Resources\\api.json")]
        public void TestMethod1()
        {
            int expectedValue = 30;
            Assert.AreEqual(expectedValue, calc.Sum());
        }
    }
}
