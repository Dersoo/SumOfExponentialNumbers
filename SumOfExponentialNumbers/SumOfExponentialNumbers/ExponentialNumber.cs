//[Created by Harakian Mikaiel]
using System;
using System.Text.RegularExpressions;

namespace SumOfNumbersInTheExponentialForm
{
    class ExponentialNumber
    {
        public string Value { get; set; }

        //Template to checking exponential number
        private static readonly Regex _exponentialFormPattern = new Regex("-?[\\d.]+(?:e[+-]?\\d)+?", RegexOptions.IgnoreCase);

        //Exponential number separation pattern
        private static readonly Regex _exponentialNumberSeparationRegex = new Regex(@"(?<numericPart>[^-]+)e(?<exponentialPart>.+)", RegexOptions.IgnoreCase);

        public ExponentialNumber()
        {
            Value = "0";
        }

        public ExponentialNumber(string exponentialNumber)
        {
            Value = exponentialNumber;
        }

        //---------------------------[START] Utility functions-------------------------------
        private static bool IsNumberMatchesThePattern(string exponentialNumberString)
        {
            //Checking to see is the entered number matches with template
            return _exponentialFormPattern.IsMatch(exponentialNumberString);
        }

        private static bool IsZeroValue(string numberString)
        {
            return numberString.Trim().Equals("0", StringComparison.InvariantCultureIgnoreCase) ||
                   numberString.Trim().Equals("0.0e0", StringComparison.InvariantCultureIgnoreCase) ||
                   numberString.Trim().Equals("0.0e+0", StringComparison.InvariantCultureIgnoreCase);
        }
        //---------------------------[END] Utility functions---------------------------------

        public override string ToString()
        {
            return Value;
        }

        public static ExponentialNumber CheckAndCreateANewNumber(string exponentialNumberString)
        {
            string numericPart = "";
            if (!IsZeroValue(exponentialNumberString))
            {
                int exponentialPart = 0;

                if (exponentialNumberString.Contains("+"))
                {
                    exponentialNumberString = exponentialNumberString.Replace("+", "");
                }
                //Checking to see is the entered number matches with template
                if (IsNumberMatchesThePattern(exponentialNumberString))
                {
                    exponentialNumberString = ExponentialCalculator.ReducePrecisionInMantissa(exponentialNumberString, ExponentialCalculator.PRECISION);
                    Match exponentialMatch = _exponentialNumberSeparationRegex.Match(exponentialNumberString);
                    if (exponentialMatch.Success)
                    {
                        numericPart = exponentialMatch.Groups["numericPart"].Value;
                        exponentialPart = Int32.Parse(exponentialMatch.Groups["exponentialPart"].Value);
                    }
                    if (numericPart.IndexOf(".") == -1 || (numericPart.IndexOf(".") == 1 && (char)numericPart[0] != '0'))
                    {
                        if (exponentialPart != 0)
                        {
                            int countOfNumbersAfterPoint = (numericPart.Length - numericPart.IndexOf(".")) - 1;
                            numericPart = numericPart.Replace(".", "");
                            if (exponentialPart > 0)
                            {
                                for (int i = 0; i < (exponentialPart - countOfNumbersAfterPoint); i++)
                                {
                                    numericPart += "0";
                                }
                            }
                            else if (0 > exponentialPart)
                            {
                                for (int i = 0; i < (Math.Abs(exponentialPart) - 1); i++)
                                {
                                    numericPart = "0" + numericPart;
                                }
                                numericPart = "0." + numericPart;
                            }
                        }
                        return new ExponentialNumber(numericPart);
                    }
                    else
                    {
                        throw new ApplicationException("is not in normalized form");
                    }
                }
                else
                {
                    throw new ApplicationException("is not in exponential form");
                }
            }
            return new ExponentialNumber("0");
        }

        public static ExponentialNumber operator +(ExponentialNumber object1, ExponentialNumber object2)
        {
            string sumOfExponentialNumbersString = "";
            if (object1.Value != "0" && object2.Value != "0")
            {
                void AddToSumOfExponentialNumbers(string numberString)
                {
                    if (sumOfExponentialNumbersString == "")
                    {
                        sumOfExponentialNumbersString += numberString;
                    }
                    else
                    {
                        int countOfSymbolsAfterPointInBasicNumber;
                        int countOfSymbolsAfterPointInAddedNumber;

                        if (numberString.Contains("."))
                        {
                            countOfSymbolsAfterPointInAddedNumber = numberString.Length - numberString.IndexOf(".") - 1;
                            if (!sumOfExponentialNumbersString.Contains("."))
                            {
                                sumOfExponentialNumbersString += ".";
                            }
                            countOfSymbolsAfterPointInBasicNumber = sumOfExponentialNumbersString.Length - sumOfExponentialNumbersString.IndexOf(".") - 1;
                            if (countOfSymbolsAfterPointInAddedNumber > countOfSymbolsAfterPointInBasicNumber)
                            {
                                for (int i = 0; i < countOfSymbolsAfterPointInAddedNumber - countOfSymbolsAfterPointInBasicNumber; i++)
                                {
                                    sumOfExponentialNumbersString += "0";
                                }
                            }
                        }

                        if (sumOfExponentialNumbersString.Contains(".") && !numberString.Contains("."))
                        {
                            countOfSymbolsAfterPointInBasicNumber = sumOfExponentialNumbersString.Length - sumOfExponentialNumbersString.IndexOf(".") - 1;
                            if (!numberString.Contains("."))
                            {
                                numberString += ".";
                            }
                            countOfSymbolsAfterPointInAddedNumber = numberString.Length - numberString.IndexOf(".") - 1;
                            if (countOfSymbolsAfterPointInBasicNumber > countOfSymbolsAfterPointInAddedNumber)
                            {
                                for (int i = 0; i < countOfSymbolsAfterPointInBasicNumber - countOfSymbolsAfterPointInAddedNumber; i++)
                                {
                                    numberString += "0";
                                }
                            }
                        }

                        int largestOfTwoNumbersLength = (sumOfExponentialNumbersString.Length > numberString.Length) ?
                                                                sumOfExponentialNumbersString.Length :
                                                                numberString.Length;
                        string largestOfTwoNumbersString = (sumOfExponentialNumbersString.Length > numberString.Length) ?
                                                                sumOfExponentialNumbersString :
                                                                numberString;

                        string smallestOfTwoNumbersString = (sumOfExponentialNumbersString.Length > numberString.Length) ?
                                                                numberString :
                                                                sumOfExponentialNumbersString;
                        int numberFromPreviousCharge = 0;
                        int numberOfCurrentCharge;
                        string numberOfCurrentChargeString;
                        string newNumberString = "";

                        for (int i = 0; i < largestOfTwoNumbersLength; i++)
                        {
                            bool isNotOutOfLine = (smallestOfTwoNumbersString.Length - 1) > i || (smallestOfTwoNumbersString.Length - 1) == i;

                            if (largestOfTwoNumbersString[largestOfTwoNumbersString.Length - 1 - i] != '.' &&
                                (isNotOutOfLine ? smallestOfTwoNumbersString[smallestOfTwoNumbersString.Length - 1 - i] != '.' : true))
                            {
                                if (isNotOutOfLine)
                                {
                                    numberOfCurrentCharge = (int)Char.GetNumericValue(largestOfTwoNumbersString[largestOfTwoNumbersString.Length - 1 - i]) +
                                                            (int)Char.GetNumericValue(smallestOfTwoNumbersString[smallestOfTwoNumbersString.Length - 1 - i]) +
                                                            numberFromPreviousCharge;
                                    numberFromPreviousCharge = 0;
                                }
                                else
                                {
                                    numberOfCurrentCharge = (int)Char.GetNumericValue(largestOfTwoNumbersString[largestOfTwoNumbersString.Length - 1 - i]) +
                                                            numberFromPreviousCharge;
                                    numberFromPreviousCharge = 0;
                                }

                                if (numberOfCurrentCharge > 9)
                                {
                                    numberOfCurrentChargeString = numberOfCurrentCharge.ToString();
                                    numberOfCurrentCharge = (int)Char.GetNumericValue(numberOfCurrentChargeString[numberOfCurrentChargeString.Length - 1]);
                                    newNumberString = numberOfCurrentCharge.ToString() + newNumberString;
                                    if (numberOfCurrentChargeString.Length > 1)
                                    {
                                        numberFromPreviousCharge = Convert.ToInt32(numberOfCurrentChargeString.Remove(numberOfCurrentChargeString.Length - 1));
                                    }
                                    else
                                    {
                                        numberFromPreviousCharge = 0;
                                    }
                                }
                                else
                                {
                                    newNumberString = numberOfCurrentCharge + newNumberString;
                                }
                            }
                            else
                            {
                                newNumberString = "." + newNumberString;
                            }
                            if ((i == largestOfTwoNumbersLength - 1) && numberFromPreviousCharge != 0)
                            {
                                newNumberString = numberFromPreviousCharge.ToString() + newNumberString;
                            }
                        }
                        sumOfExponentialNumbersString = newNumberString;
                    }
                }

                AddToSumOfExponentialNumbers(object1.Value);
                AddToSumOfExponentialNumbers(object2.Value);
            }
            else if (object1.Value == "0")
            {
                sumOfExponentialNumbersString = object2.Value;
            }
            else
            {
                sumOfExponentialNumbersString = object1.Value;
            }

            return new ExponentialNumber(
               ExponentialCalculator.ReducePrecisionInMantissa(
                  sumOfExponentialNumbersString,
                  ExponentialCalculator.PRECISION
               )
           );
        }
    }
}
