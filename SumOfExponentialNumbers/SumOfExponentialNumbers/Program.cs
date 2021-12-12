//[Created by Harakian Mikaiel]
using System;

namespace SumOfNumbersInTheExponentialForm
{
    class Program
    {
        static void Main(string[] args)
        {
            ExponentialCalculator exponentialCalculator1 = new ExponentialCalculator();
            string tempEnterValue = "";

            Console.WriteLine("Please enter a row of exponential numbers. Enter '=' to complete the input of numbers.");
            while (tempEnterValue != "=")
            {
                tempEnterValue = Console.ReadLine();

                if (tempEnterValue != "=")
                {
                    if ((int)exponentialCalculator1.AddExponentialNumber(tempEnterValue) == 1)
                    {
                        Console.WriteLine("The entered number is not in exponential form. Please enter again.");
                    }
                }
            }
            Console.WriteLine("The sum of exponentional numbers is:");
            Console.WriteLine(exponentialCalculator1.SumExponentialNumbers().Value);
        }
    }
}
