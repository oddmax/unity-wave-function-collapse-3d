/*
The MIT License(MIT)
Copyright(c) mxgmn 2016.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
The software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the software or the use or other dealings in the software.
*/

using System;
using System.Collections;
using Core.Data;
using UnityEngine;
using Random = System.Random;

namespace Core.Model
{
	public abstract class Model<PARAM> : IModel where PARAM : WaveFunctionCollapseModelParams
	{
		protected bool[][] wave;
		protected double[] stationary;
		protected int[] observed;

		protected bool[] changes;
		protected int[] stack;
		protected int stacksize;

		protected System.Random random;
		protected int FMX, FMY, T;
		
		/// <summary>
		/// Whether the input data wraps around an axis.
		/// </summary>
		protected bool periodic;

		private double[] logProb;
		private double logT;
		private PARAM modelParam;

		/// <summary>
		/// Amount of all possible patterns
		/// </summary>
		public int PatternsAmount
		{
			get { return T; }
		}
		
		public WaveFunctionCollapseModelParams ModelParam
		{
			get { return modelParam; }
		}
		
		protected Model(PARAM modelParam)
		{
			this.modelParam = modelParam;
			FMX = modelParam.Width;
			FMY = modelParam.Depth;

			wave = new bool[FMX * FMY][];
			changes = new bool[FMX * FMY];

			stack = new int[FMX * FMY];
			stacksize = 0;
		}

		
		/// <summary>
		/// Run model to generate output
		/// </summary>
		/// <param name="seed"> All internal random values are derived from this seed, providing 0 results in a random number. </param>
		/// <param name="limit"> How many iterations to run, providing 0 will run until completion or a contradiction. </param>
		/// <returns></returns>
		public bool Run(int seed, int limit)
		{
			Init(seed);

			for (int l = 0; l < limit || limit == 0; l++)
			{
				bool? result = Observe();
				if (result != null) return result.Value;
				Propagate();
			}

			return true;
		}

		/// <summary>
		/// Run model as Enumerator to generate output
		/// </summary>
		/// <param name="seed"> All internal random values are derived from this seed, providing 0 results in a random number. </param>
		/// <param name="limit"> How many iterations to run, providing 0 will run until completion or a contradiction. </param>
		/// <returns></returns>
		public IEnumerator RunViaEnumerator(int seed, int limit, Action<bool> resultCallback, Action<bool[][]> iterationCallback)
		{
			Init(seed);

			for (int iteration = 0; iteration < limit || limit == 0; iteration++)
			{
				bool? result = Observe();
				if (result != null)
				{
					resultCallback(result.Value);
					break;
				}
				Debug.Log("Propagate, iteration: " + iteration);
				Propagate();
				iterationCallback(wave);
				yield return null;
			}
		}
		
		private void Init(int seed)
		{
			logT = Math.Log(T);
			logProb = new double[T];
			for (int t = 0; t < T; t++)
			{
				logProb[t] = Math.Log(stationary[t]);
			}

			Clear();
			
			random = new Random(seed);
			if (seed == 0)
			{
				random = new Random();
			} 
		}

		protected abstract void Propagate();
		
		protected void InitWave()
		{
			for (var i = 0; i < wave.Length; i++) wave[i] = new bool[T];
		}

		bool? Observe()
		{
			Debug.Log("Observe state");
			int? indexWithLowestEntropy = FindCellWithLowestEntropy();
			
			//There is the cell with no possible values which means that we found a contradiction
			if (indexWithLowestEntropy == null) return false;
			
			Debug.Log("Cell with lowest entropy : " + indexWithLowestEntropy);
			// All values has collapsed, fill result in observed function
			if (indexWithLowestEntropy == -1)
			{
				FillGeneratedResult();
				return true;
			}

			//collapse cell value to one of possiblities randomly based on their weight
			double[] distribution = new double[T];
			for (int t = 0; t < T; t++)
			{
				distribution[t] = wave[indexWithLowestEntropy.Value][t] ? stationary[t] : 0;
			}
			int r = distribution.Random(random.NextDouble());
			for (int t = 0; t < T; t++)
			{
				wave[indexWithLowestEntropy.Value][t] = t == r;
			}
			Change(indexWithLowestEntropy.Value);

			return null;
		}

		private int? FindCellWithLowestEntropy()
		{
			double min = 1E+3;
			int indexWithLowestEntropy = -1;

			for (int i = 0; i < wave.Length; i++)
			{
				if (OnBoundary(i)) continue;

				bool[] waveValuesForCurrentIndex = wave[i];
				int amount = 0;
				double sum = 0;

				for (int t = 0; t < T; t++)
				{
					if (waveValuesForCurrentIndex[t] == false) continue;
					amount += 1;
					sum += stationary[t];
				}

				if (sum == 0)
				{
					return null;
				}

				double noise = 1E-6 * random.NextDouble();

				double entropy = CalculateEntropy(amount, sum, waveValuesForCurrentIndex);

				if (entropy > 0 && entropy + noise < min)
				{
					min = entropy + noise;
					indexWithLowestEntropy = i;
				}
			}
			
			return indexWithLowestEntropy;
		}

		private void FillGeneratedResult()
		{
			observed = new int[FMX * FMY];
			for (int i = 0; i < wave.Length; i++)
			{
				for (int t = 0; t < T; t++)
				{
					if (wave[i][t])
					{
						observed[i] = t;
						break;
					}
				}
			}
		}

		public abstract CellState GetCellStateAt(int x, int y);

		protected void CalculateEntropyAndPatternIdAt(int x, int y, out float entropy, out int? patternId)
		{
			int indexInWave = x + y * FMX;
			int amount = 0;
			patternId = null;
			var possiblePatternsFlags = wave[indexInWave];
			for (int t = 0; t < T; t++)
			{
				if (possiblePatternsFlags[t])
				{
					amount += 1;
					patternId = t;
				}
			}

			if (amount != 1)
			{
				patternId = null;
			}
			
			entropy = (float)amount / T;
		}

		private double CalculateEntropy(int amount, double sum, bool[] waveValuesForCurrentIndex)
		{
			if (amount == 1)
			{
				return 0;
			}
			if (amount == T)
			{
				return logT;
			}
			
			double mainSum = 0;
			double logSum = Math.Log(sum);
			for (int t = 0; t < T; t++)
			{
				if (waveValuesForCurrentIndex[t])
				{
					mainSum += stationary[t] * logProb[t];
				}
			}

			return logSum - mainSum / sum;
		}

		protected void Change(int i)
		{
			if (changes[i]) return;

			stack[stacksize] = i;
			stacksize++;
			changes[i] = true;
		}

		protected virtual void Clear()
		{
			for (int i = 0; i < wave.Length; i++)
			{
				for (int t = 0; t < T; t++) wave[i][t] = true;
				changes[i] = false;
			}
		}

		public abstract bool OnBoundary(int i);
 	}
}
