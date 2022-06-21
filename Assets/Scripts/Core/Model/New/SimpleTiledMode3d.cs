/*
The MIT License(MIT)
Copyright(c) mxgmn 2016.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
The software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the software or the use or other dealings in the software.
*/

using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Core.Data;
using Core.Data.SimpleTiledModel;
using UnityEngine;

public class SimpleTiledMode3d : Model3d<SimpleTiledModelParams>
{
	List<SimpleTiledModelTile> tiles;
	List<string> tilenames;
	int tilesize;
	bool black;

	public SimpleTiledMode3d(InputSimpleTiledModelData inputData, SimpleTiledModelParams modelParams) : base(modelParams)
	{
		this.periodic = modelParams.Periodic;
		this.black = modelParams.Black;

		tilesize = 1;

		List<string> subset = null;
		
		tiles = new List<SimpleTiledModelTile>();
		tilenames = new List<string>();
		var tempStationary = new List<double>();

		List<int[]> action = new List<int[]>();
		Dictionary<string, int> firstOccurrence = new Dictionary<string, int>();

		foreach (var tileConfig in inputData.TileConfigData.ConfigsList)
		{
			if (subset != null && !subset.Contains(tileConfig.Id)) continue;

			Func<int, int> a, b;
			int cardinality;

			var sym = tileConfig.Symmetry;
			if (sym == SymmetryType.L)
			{
				cardinality = 4;
				a = i => (i + 1) % 4;
				b = i => i % 2 == 0 ? i + 1 : i - 1;
			}
			else if (sym == SymmetryType.T)
			{
				cardinality = 4;
				a = i => (i + 1) % 4;
				b = i => i % 2 == 0 ? i : 4 - i;
			}
			else if (sym == SymmetryType.I)
			{
				cardinality = 2;
				a = i => 1 - i;
				b = i => i;
			}
			else if (sym == SymmetryType.Slash)
			{
				cardinality = 2;
				a = i => 1 - i;
				b = i => 1 - i;
			}
			else
			{
				cardinality = 1;
				a = i => i;
				b = i => i;
			}

			T = action.Count;
			firstOccurrence.Add(tileConfig.Id, T);
			
			int[][] map = new int[cardinality][];
			for (int t = 0; t < cardinality; t++)
			{
				map[t] = new int[8];

				map[t][0] = t;
				map[t][1] = a(t);
				map[t][2] = a(a(t));
				map[t][3] = a(a(a(t)));
				map[t][4] = b(t);
				map[t][5] = b(a(t));
				map[t][6] = b(a(a(t)));
				map[t][7] = b(a(a(a(t))));

				for (int s = 0; s < 8; s++) map[t][s] += T;

				action.Add(map[t]);
			}

			
			tiles.Add(new SimpleTiledModelTile(tileConfig, 0));
			tilenames.Add(tileConfig.Id + " 0");

			for (int t = 1; t < cardinality; t++)
			{
				//tiles.Add(rotate(tiles[T + t - 1]));
				//tilenames.Add($"{tilename} {t}");
				tiles.Add(new SimpleTiledModelTile(tileConfig, t));
				tilenames.Add(tileConfig.Id + " " + t);
			}


			for (int t = 0; t < cardinality; t++)
			{
				tempStationary.Add(tileConfig.Weight);
			}
		}

		T = action.Count;
		weights = tempStationary.ToArray();

		propagator = new int[DIRECTIONS_AMOUNT][][];
		var tempPropagator = new bool[DIRECTIONS_AMOUNT][][];
		for (int d = 0; d < DIRECTIONS_AMOUNT; d++)
		{
			tempPropagator[d] = new bool[T][];
			propagator[d] = new int[T][];
			for (int t = 0; t < T; t++)
			{
				tempPropagator[d][t] = new bool[T];
			}
		}

		foreach (NeighborData neighborData in inputData.NeighborDatas)
		{
			var leftNeighbor = neighborData.LeftNeighborConfig.Id;
			var rightNeighbor = neighborData.RightNeighborConfig.Id;
			if (subset != null && (!subset.Contains(leftNeighbor) || !subset.Contains(rightNeighbor))) continue;

			int L = action[firstOccurrence[leftNeighbor]][neighborData.LeftRotation], D = action[L][1];
			int R = action[firstOccurrence[rightNeighbor]][neighborData.RightRotation], U = action[R][1];

			if (neighborData.Horizontal)
			{
				tempPropagator[0][R][L] = true;
				tempPropagator[0][action[R][6]][action[L][6]] = true;
				tempPropagator[0][action[L][4]][action[R][4]] = true;
				tempPropagator[0][action[L][2]][action[R][2]] = true;

				tempPropagator[1][U][D] = true;
				tempPropagator[1][action[D][6]][action[U][6]] = true;
				tempPropagator[1][action[U][4]][action[D][4]] = true;
				tempPropagator[1][action[D][2]][action[U][2]] = true;
			}
			else
			{
				// for vertical neighbours
				for (int g = 0; g < 8; g++)
				{
					tempPropagator[4][action[L][g]][action[R][g]] = true;
				}
			}
		}

		for (int t2 = 0; t2 < T; t2++) 
		for (int t1 = 0; t1 < T; t1++)
			{
				tempPropagator[2][t2][t1] = tempPropagator[0][t1][t2];
				tempPropagator[3][t2][t1] = tempPropagator[1][t1][t2];
				tempPropagator[5][t2][t1] = tempPropagator[4][t1][t2];
			}

		List<int>[][] sparsePropagator = new List<int>[DIRECTIONS_AMOUNT][];
		for (int d = 0; d < DIRECTIONS_AMOUNT; d++)
		{
			sparsePropagator[d] = new List<int>[T];
			for (int t = 0; t < T; t++)
			{
				sparsePropagator[d][t] = new List<int>();
			}
		}

		for (int d = 0; d < DIRECTIONS_AMOUNT; d++)
		{
			for (int t1 = 0; t1 < T; t1++)
			{
				List<int> sp = sparsePropagator[d][t1];
				bool[] tp = tempPropagator[d][t1];

				for (int t2 = 0; t2 < T; t2++)
				{
					if (tp[t2]) sp.Add(t2);
				}

				int ST = sp.Count;
				propagator[d][t1] = new int[ST];
				for (int st = 0; st < ST; st++)
				{
					propagator[d][t1][st] = sp[st];
				}
			}
		}
	}

	protected override bool OnBoundary(int x, int y, int z)
	{
		return !periodic && (x < 0 || y < 0 || z < 0 || x >= FMX || y >= FMY || z >= FMZ);
	}

	public override CellState GetCellStateAt(int x, int y, int z)
	{
		bool[] a = wave[To1D(x,y,z)];
			
		double entropy;
		int? collapsedPatternId;
		CalculateEntropyAndPatternIdAt(x, y, z, out entropy, out collapsedPatternId);

		ITile tile = null;
		if (collapsedPatternId != null)
		{
			tile = tiles[collapsedPatternId.Value];
		}

		return new CellState(entropy, tile);
	}

	/*
	public override Bitmap Graphics()
	{
		Bitmap result = new Bitmap(FMX * tilesize, FMY * tilesize);
		int[] bitmapData = new int[result.Height * result.Width];

		if (observed != null)
		{
			for (int x = 0; x < FMX; x++) for (int y = 0; y < FMY; y++)
					{
						Color[] tile = tiles[observed[x + y * FMX]];
						for (int yt = 0; yt < tilesize; yt++) for (int xt = 0; xt < tilesize; xt++)
							{
								Color c = tile[xt + yt * tilesize];
								bitmapData[x * tilesize + xt + (y * tilesize + yt) * FMX * tilesize] =
									unchecked((int)0xff000000 | (c.R << 16) | (c.G << 8) | c.B);
							}
					}
		}
		else
		{
			for (int x = 0; x < FMX; x++) for (int y = 0; y < FMY; y++)
				{
					bool[] a = wave[x + y * FMX];
					int amount = (from b in a where b select 1).Sum();
					double lambda = 1.0 / (from t in Enumerable.Range(0, T) where a[t] select weights[t]).Sum();

					for (int yt = 0; yt < tilesize; yt++) for (int xt = 0; xt < tilesize; xt++)
						{
							if (black && amount == T) bitmapData[x * tilesize + xt + (y * tilesize + yt) * FMX * tilesize] = unchecked((int)0xff000000);
							else
							{
								double r = 0, g = 0, b = 0;
								for (int t = 0; t < T; t++) if (wave[x + y * FMX][t])
									{
										Color c = tiles[t][xt + yt * tilesize];
										r += (double)c.R * weights[t] * lambda;
										g += (double)c.G * weights[t] * lambda;
										b += (double)c.B * weights[t] * lambda;
									}

								bitmapData[x * tilesize + xt + (y * tilesize + yt) * FMX * tilesize] =
									unchecked((int)0xff000000 | ((int)r << 16) | ((int)g << 8) | (int)b);
							}
						}
				}
		}

		var bits = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		System.Runtime.InteropServices.Marshal.Copy(bitmapData, 0, bits.Scan0, bitmapData.Length);
		result.UnlockBits(bits);

		return result;
	}
	

	public string TextOutput()
	{
		var result = new System.Text.StringBuilder();

		for (int y = 0; y < FMY; y++)
		{
			for (int x = 0; x < FMX; x++) result.Append($"{tilenames[observed[x + y * FMX]]}, ");
			result.Append(Environment.NewLine);
		}

		return result.ToString();
	}
	*/
}
