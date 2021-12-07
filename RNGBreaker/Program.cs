using System;
using System.Collections.Generic;

namespace RNGBreaker
{
    class Program
    {
        static void Main(string[] args)
        {
            bool success = false;
            while (!success) {
                try
                {
                    var values = new List<long>();
                    for (int i = 0; i < 3; i++)
                    {
                        var value = LCGGenerator.Next();
                        values.Add(value);
                    }
                    var a = (values[2] - values[1]) * ((values[1] - values[0]).ModInversion(LCGGenerator.m)) % LCGGenerator.m;
                    var b = values[1] - (values[0] * a % LCGGenerator.m);
                    Console.WriteLine(a);
                    Console.WriteLine(b);
                    success = true;
                }
                catch (Exception ex)
                {
                    continue;
                } 
            }
        }
    }
}
