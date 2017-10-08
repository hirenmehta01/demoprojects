using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBilling.Helper
{
    public class BillingHelper
    {
        public static string UserId { get; set; }
        public static int UserType { get; set; }

        private static string _dbPath;
        public static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_dbPath))
                {
                    _dbPath = Application.StartupPath + "\\DB\\MyBillingSqLiteDB.db";
                }
                return _dbPath;
            }
        }
        private static string _reportPath;
        public static string ReportPath
        {
            get
            {
                if (string.IsNullOrEmpty(_reportPath))
                {
                    _reportPath = Application.StartupPath + "\\Reports\\Bill.rpt";
                }
                return _reportPath;
            }
        }

        //private static string _dbPath;
        //public static string DbPath
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_dbPath))
        //        {
        //            _dbPath = @"D:\Projects\MyBilling\packages" + "\\Db\\MyBillingSqLiteDB.db";
        //        }
        //        return _dbPath;
        //    }
        //}
        //private static string _reportPath;
        //public static string ReportPath
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_reportPath))
        //        {
        //            _reportPath = @"D:\Projects\MyBilling\MyBilling" + "\\Reports\\Bill.rpt";
        //        }
        //        return _reportPath;
        //    }
        //}
    }
 }
