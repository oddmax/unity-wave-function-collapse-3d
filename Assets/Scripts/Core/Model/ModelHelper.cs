﻿/*
The MIT License(MIT)
Copyright(c) mxgmn 2016.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
The software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the software or the use or other dealings in the software.
*/

using System.Linq;

namespace Core.Model
{
	static class ModelHelper
	{
		public static int Random(this double[] a, double r)
		{
			double sum = a.Sum();

			if (sum == 0)
			{
				for (int j = 0; j < a.Count(); j++) a[j] = 1;
				sum = a.Sum();
			}

			for (int j = 0; j < a.Count(); j++) a[j] /= sum;

			int i = 0;
			double x = 0;

			while (i < a.Count())
			{
				x += a[i];
				if (r <= x) return i;
				i++;
			}

			return 0;
		}

		public static long Power(int a, int n)
		{
			long product = 1;
			for (int i = 0; i < n; i++) product *= a;
			return product;
		}
	}
}
