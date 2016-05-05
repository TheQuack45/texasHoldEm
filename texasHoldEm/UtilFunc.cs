using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class UtilFunc
    {
        public static string ToSentenceCase(string input)
        {
            char[] inputCharArr = input.ToCharArray();
            char[] outputCharArr = new char[inputCharArr.Length];

            for (int i = 0; i < inputCharArr.Length; i++)
            {
                if (i == 0)
                    outputCharArr[i] = Char.ToUpper(inputCharArr[i]);
                else
                {
                    outputCharArr[i] = inputCharArr[i];
                }
            }

            return new string(outputCharArr);
        }
    }
}
