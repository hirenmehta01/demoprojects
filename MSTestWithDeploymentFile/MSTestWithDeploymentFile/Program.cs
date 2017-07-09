using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MSTestWithDeploymentFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculation calc = new Calculation();
            Console.WriteLine($"Sum is {calc.Sum()}");
            Console.ReadLine();
        }
    }
}
