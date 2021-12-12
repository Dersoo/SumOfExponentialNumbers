//[Created by Harakian Mikaiel]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SumOfNumbersInTheExponentialForm
{
    enum ResponsesFromClass
    {
        IsCorrect,
        NonExponentialFormat,
    }


    class ExponentialCalculator
    {
        private List<ExponentialNumber> _exponentialNumberList;

        //Template to checking exponential number
        private static Regex _exponentialFormPattern = new Regex("-?[\\d.]+(?:e[+-]?\\d)+?", RegexOptions.IgnoreCase);

        //Exponential number separation pattern
        private static Regex _exponentialNumberSeparationRegex = new Regex(@"(?<numericPart>[^-]+)e(?<exponentialPart>.+)", RegexOptions.IgnoreCase);

        public const byte PRECISION = 39;

        public ExponentialCalculator()
        {
            _exponentialNumberList = new List<ExponentialNumber>();
        }

        //---------------------------[START] Utility functions-------------------------------
        private static bool IsNumberMatchesThePattern(string exponentialNumberString)
        {
            //Checking to see is the entered number matches with template
            return _exponentialFormPattern.IsMatch(exponentialNumberString);
        }

        public static string ReducePrecisionInMantissa(string numberString, byte precision)
        {
            string exponentialPartString = "";

            string DigitAddition(string number)
            {
                int roundingPart = 1;
                string roundingPartString = "";
                string newNumber = "";

                for (int i = precision + 1; i != -1; i--)
                {
                    if (numberString[i] == '.')
                    {
                        newNumber = "." + newNumber;
                        continue;
                    }
                    roundingPart = (int)Char.GetNumericValue(numberString[i]) + roundingPart;
                    roundingPartString = roundingPart.ToString();
                    if (i != 0 && roundingPartString.Length > 1)
                    {
                        if (roundingPartString[roundingPartString.Length - 1] != '0')
                        {
                            newNumber = roundingPartString[roundingPartString.Length - 1] + newNumber;
                        }

                        roundingPartString = roundingPartString.Remove(roundingPartString.Length - 1, 1);

                        roundingPart = Int32.Parse(roundingPartString);
                    }
                    else
                    {
                        newNumber = roundingPartString + newNumber;

                        roundingPart = 0;
                    }
                }

                return newNumber;
            }

            if ((numberString.IndexOf("e") - numberString.IndexOf(".")) - 1 > precision)
            {
                exponentialPartString = numberString.Substring(numberString.IndexOf("e"), numberString.Length - numberString.IndexOf("e"));
                numberString = numberString.Remove(numberString.IndexOf("e"), numberString.Length - numberString.IndexOf("e"));
                if ((int)Char.GetNumericValue(numberString[numberString.IndexOf(".") + precision + 1]) > 4)
                {
                    numberString = DigitAddition(numberString);
                }
                else
                {
                    numberString = numberString.Remove(numberString.IndexOf(".") + precision + 1);
                }
            }
            if ((numberString.IndexOf(".") != -1) && (numberString.IndexOf(".") == numberString.Length - 1))
            {
                numberString = numberString.Replace(".", "");
            }

            return numberString + exponentialPartString;
        }

        private static string ConvertToExponentialForm(string numberString)
        {
            int i = numberString.Length - 1;
            int countOfNumbersBeforePoint = 0;

            if (numberString.Contains("."))
            {
                while (numberString[i] == '0')
                {
                    numberString = numberString.Remove(i, 1);
                    i--;
                }
            }
            if (numberString[i] == '.')
            {
                numberString = numberString.Replace(".", "");
            }
            if (numberString.Contains(".") && numberString.IndexOf(".") != 1)
            {
                countOfNumbersBeforePoint = numberString.IndexOf(".");
                if (countOfNumbersBeforePoint > 1)
                {
                    numberString = numberString.Replace(".", "");
                    numberString = numberString.Insert(1, ".");
                    numberString = numberString + "e" + (countOfNumbersBeforePoint - 1).ToString();
                }
                else if (countOfNumbersBeforePoint == 1)
                {
                    numberString = numberString + "e0";
                }
            }
            else if (!numberString.Contains(".") && numberString.Length > 1)
            {
                int exponential = 0;

                exponential = numberString.Length - 1;
                numberString = numberString.Insert(1, ".");
                i = numberString.Length - 1;
                while (numberString[i] == '0')
                {
                    numberString = numberString.Remove(i, 1);
                    i--;
                }
                numberString = numberString + "e" + exponential.ToString();
            }
            else if (numberString.Length == 1)
            {
                return numberString + "e0";
            }

            return numberString;
        }
        //---------------------------[END] Utility functions----------------------------------

        public List<ExponentialNumber> GetExponentialNumberList()
        {
            return _exponentialNumberList;
        }

        public ExponentialNumber SumExponentialNumbers()
        {
            ExponentialNumber exponentialNumbersSum = new ExponentialNumber();
            ExponentialNumber exponentialNumbersSumTemp = new ExponentialNumber();

            for (int i = 0; i < _exponentialNumberList.Count(); i += 2)
            {
                if ((i + 1) > (_exponentialNumberList.Count() - 1))
                {
                    exponentialNumbersSum.Value = _exponentialNumberList[i] + exponentialNumbersSum;
                }
                else
                {
                    exponentialNumbersSumTemp.Value = (_exponentialNumberList[i] + _exponentialNumberList[i + 1]);
                    exponentialNumbersSum.Value = exponentialNumbersSumTemp + exponentialNumbersSum;
                }
            }
            exponentialNumbersSum.Value = ReducePrecisionInMantissa(
                                            ConvertToExponentialForm(exponentialNumbersSum.Value),
                                            PRECISION
                                          );

            return exponentialNumbersSum;
        }
        public ResponsesFromClass AddExponentialNumber(string exponentialNumberString)
        {
            string numericPart = "";
            int exponentialPart = 0;

            if (exponentialNumberString.Contains("+"))
            {
                exponentialNumberString.Replace("+", "");
            }
            //Checking to see is the entered number matches with template
            if (IsNumberMatchesThePattern(exponentialNumberString))
            {
                exponentialNumberString = ReducePrecisionInMantissa(exponentialNumberString, PRECISION);
                Match exponentialMatch = _exponentialNumberSeparationRegex.Match(exponentialNumberString);
                if (exponentialMatch.Success)
                {
                    numericPart = exponentialMatch.Groups["numericPart"].Value;
                    exponentialPart = Int32.Parse(exponentialMatch.Groups["exponentialPart"].Value);
                }
                if (exponentialPart == 0)
                {
                    _exponentialNumberList.Add(
                        new ExponentialNumber(numericPart)
                    );
                }
                else
                {
                    int countOfNumbersAfterPoint = 0;
                    countOfNumbersAfterPoint = (numericPart.Length - numericPart.IndexOf(".")) - 1;
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
                    _exponentialNumberList.Add(
                        new ExponentialNumber(numericPart)
                    );
                }
            }
            else
            {
                return ResponsesFromClass.NonExponentialFormat;
            }

            return ResponsesFromClass.IsCorrect;
        }
    }
}
