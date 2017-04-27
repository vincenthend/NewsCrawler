//Boyer-Moore String checker

using System;
namespace BoyerMooreAlgorithm
{
	class BMA 
	{
		public static int SearchString(string str, string pat)
		{
			List<int> retVal = new List<int>();
			int m = pat.Length;
			int n = str.Length;

			int[] badChar = new int[256];

			BadCharHeuristic(pat, m, ref badChar);

			int s = 0;
			while (s <= (n - m))
			{
				int j = m - 1;

				while (j >= 0 && pat[j] == str[s + j])
					--j;

				if (j < 0)
				{
                    return s;
					s += (s + m < n) ? m - badChar[str[s + m]] : 1;
				}
				else
				{
					s += Math.Max(1, j - badChar[str[s + j]]);
				}
			}

            return -1;
		}

		private static void BadCharHeuristic(string str, int size, ref int[] badChar)
		{
			int i;

			for (i = 0; i < 256; i++)
				badChar[i] = -1;

			for (i = 0; i < size; i++)
				badChar[(int)str[i]] = i;
		}
	}
}