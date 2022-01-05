//[Created by Harakian Mikaiel]
using System;

namespace SumOfNumbersInTheExponentialForm
{
    class Program
    {
        static void Main(string[] args)
        {
            ExponentialCalculator exponentialCalculator1 = new ExponentialCalculator();
            ExponentialNumber resultingExponentialNumber;
            string tempEnterValue = "";

            Console.WriteLine("Please, enter a row of exponential numbers. Enter '=' to complete the input.");
            while (tempEnterValue != "=")
            {
                tempEnterValue = Console.ReadLine();

                if (tempEnterValue != "=")
                {
                    try
                    {
                        exponentialCalculator1.AddExponentialNumber(tempEnterValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The entered number {0}. Please, enter again.", e.Message);
                    }
                }
            }
            resultingExponentialNumber = exponentialCalculator1.SumExponentialNumbers();
            if (resultingExponentialNumber != null)
            {
                Console.WriteLine("The sum of exponentional numbers is equal to:");
                Console.WriteLine(resultingExponentialNumber.ToString());
            }
            else
            {
                Console.WriteLine("No numbers have been entered!");
            }
        }
    }
}
