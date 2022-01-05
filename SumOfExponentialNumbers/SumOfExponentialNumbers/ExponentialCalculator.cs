//[Created by Harakian Mikaiel]
using System;
using System.Collections.Generic;
using System.Linq;

namespace SumOfNumbersInTheExponentialForm
{
    class ExponentialCalculator
    {
        private List<ExponentialNumber> _exponentialNumberList;

        public const byte PRECISION = 39;

        public ExponentialCalculator()
        {
            _exponentialNumberList = new List<ExponentialNumber>();
        }

        public int CountOfTerms
        {
            get
            {
                return _exponentialNumberList.Count();
            }
        }

        //---------------------------[START] Utility functions-------------------------------
        public static string ReducePrecisionInMantissa(string numberString, byte precision)
        {
            string exponentialPartString = "";

            string DigitAddition(string number)
            {
                int roundingPart = 1;
                string roundingPartString;
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
            int countOfNumbersBeforePoint;

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
            if (numberString.Contains(".") && (char)numberString[0] == '0')
            {
                int exponentialPart = 0;
                numberString = numberString.Replace(".", "");
                for (int j = 0; j < numberString.Length;)
                {
                    if ((char)numberString[j] == '0')
                    {
                        exponentialPart--;
                        numberString = numberString.Remove(0, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return numberString.Insert(1, ".") + "e" + exponentialPart.ToString();
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
                int exponential = numberString.Length - 1;
                numberString = numberString.Insert(1, ".");
                i = numberString.Length - 1;
                while (numberString[i] == '0')
                {
                    numberString = numberString.Remove(i, 1);
                    i--;
                }
                numberString = numberString + "e" + exponential.ToString();
            }
            else if (numberString.Length == 1 || numberString.IndexOf(".") == 1)
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
            if (this._exponentialNumberList.Count == 0)
            {
                return null;
            }
            ExponentialNumber exponentialNumbersSum = _exponentialNumberList[0];
            if (_exponentialNumberList.Count() > 1)
            {
                for (int i = 1; i < _exponentialNumberList.Count(); i++)
                {
                    exponentialNumbersSum = exponentialNumbersSum + _exponentialNumberList[i];
                }
            }
            exponentialNumbersSum.Value = ReducePrecisionInMantissa(
                                             ConvertToExponentialForm(exponentialNumbersSum.Value),
                                             PRECISION
                                          );

            return exponentialNumbersSum;
        }

        public void AddExponentialNumber(string exponentialNumberString)
        {
            try
            {
                ExponentialNumber newExponentialNumber = ExponentialNumber.CheckAndCreateANewNumber(exponentialNumberString);
                _exponentialNumberList.Add(newExponentialNumber);
            }
            catch (ApplicationException)
            {
                throw;
            }
        }
    }
}
