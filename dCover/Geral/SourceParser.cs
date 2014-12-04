using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dCover.Geral
{
	class SourceParser
	{
		private static int getLineFromPoint(string text, int position)
		{
			return Regex.Matches(text.Remove(position), @"\n").Count + 1;
		}

		private static int getNextTokenLine(string code, int position)
		{
			string codeRegion = code.Substring(position, code.Length - position);

			return getLineFromPoint(code, Regex.Match(codeRegion, @"^[^\/\/;\{]*(;)").Groups[1].Index + position);
		}

		public static IEnumerable<CoveragePoint> FilterCoveragePoints(IEnumerable<CoveragePoint> points, string sourceFile)
		{
			string sourceCode = File.ReadAllText(sourceFile);
			List<Tuple<int, int>> validLines = new List<Tuple<int, int>>();
			List<CoveragePoint> filteredPoints = new List<CoveragePoint>();

			foreach (Match x in Regex.Matches(sourceCode, @"\s(begin|\)?then\s*\n?|except|else|do|repeat)()\s", RegexOptions.IgnoreCase))
			{
				validLines.Add(Tuple.Create(getLineFromPoint(sourceCode, x.Groups[2].Index), getNextTokenLine(sourceCode, x.Groups[2].Index)));
			}

			foreach (CoveragePoint x in points.Where(y => sourceFile.ToLower().Contains(y.sourceFile)))
			{
				if (validLines.Where(y => x.lineNumber >= y.Item1 && x.lineNumber <= y.Item2).Count() > 0)
				{
					validLines = validLines.Where(y => !(x.lineNumber >= y.Item1 && x.lineNumber <= y.Item2)).ToList();

					filteredPoints.Add(x);
				}
			}

			filteredPoints.AddRange((from CoveragePoint x
									 in points
									 where !sourceFile.ToLower().Contains(x.sourceFile)
									 select x).ToList());

			return filteredPoints;
		}
	}
}
