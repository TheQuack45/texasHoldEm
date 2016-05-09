using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class UtilFunc
    {
        /// <summary>
        /// Converts the input string to sentence case (First character capitalized, rest lowercase
        /// </summary>
        /// <param name="input">String object to convert to sentence case</param>
        /// <returns>String object in sentence case</returns>
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
                    outputCharArr[i] = Char.ToLower(inputCharArr[i]);
                }
            }

            return new string(outputCharArr);
        }

        public static List<Card> Swap(List<Card> list, int iA, int iB)
        {
            Card tmp = list[iA];
            list[iA] = list[iB];
            list[iB] = tmp;

            return list;
        }
    }
}
