using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBilling.DAL;
using System.Reflection;

namespace MyBilling.Test
{
    [TestClass]
    public class LoginTest
    {
        /// <summary>
        /// connection variable.
        /// </summary>
        private SQLiteHelper context;
        /// <summary>
        /// define static db path
        /// </summary>
        private string dbPath;

        /// <summary>
        /// initlize the variable before any test run
        /// </summary>
        [ClassInitialize]
        public void Login_TestInitlize()
        {
            dbPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MyBillingSqLiteDB.db";
            context = new SQLiteHelper(dbPath);
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
