using System;

namespace TravelAgency.Extensions;

public static class StringExtensions
{
    public static bool IsValidPESEL(this string input)
    {
        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        bool result = false;
        if (input.Length == 11)
        {
            int controlSum = CalculateControlSum(input, weights);
            int controlNum = controlSum % 10;
            controlNum = 10 - controlNum;
            if (controlNum == 10)
            {
                controlNum = 0;
            }
            int lastDigit = int.Parse(input[input.Length - 1].ToString());
            result = controlNum == lastDigit;
        }
        return result;
    }

    private static int CalculateControlSum(string input, int[] weights, int offset = 0)
    {
        int controlSum = 0;
        for (int i = 0; i < input.Length - 1; i++)
        {
            controlSum += weights[i + offset] * int.Parse(input[i].ToString());
        }
        return controlSum;
    }
}
