//[Created by Harakian Mikaiel]
using System;

namespace SumOfNumbersInTheExponentialForm
{
    class ExponentialNumber
    {
        private string _exponentialNumberValue;

        public string Value
        {
            get
            {
                return _exponentialNumberValue;
            }
            set
            {
                _exponentialNumberValue = value;
            }
        }

        public ExponentialNumber()
        {
            _exponentialNumberValue = "0";
        }

        public ExponentialNumber(string exponentialNumber)
        {
            _exponentialNumberValue = exponentialNumber;
        }

        public static string operator +(ExponentialNumber object1, ExponentialNumber object2)
        {
            string sumOfExponentialNumbersString = "";

            void AddToSumOfExponentialNumbers(string numberString)
            {
                if (sumOfExponentialNumbersString == "")
                {
                    sumOfExponentialNumbersString += numberString;
                }
                else
                {
                    var countOfSymbolsAfterPointInBasicNumber = 0;
                    var countOfSymbolsAfterPointInAddedNumber = 0;

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
                    int numberOfCurrentCharge = 0;
                    string numberOfCurrentChargeString = "";
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

            return ExponentialCalculator.ReducePrecisionInMantissa(
                      sumOfExponentialNumbersString,
                      ExponentialCalculator.PRECISION
                   );
        }
    }
}
