﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
			string sourceCode = File.ReadAllText(sourceFile).ToLower();
			List<Tuple<int, int>> validLines = new List<Tuple<int, int>>();
			List<CoveragePoint> filteredPoints = new List<CoveragePoint>();

			foreach(Match x in Regex.Matches(sourceCode, @"[^\/\/\n\{]*(begin|if|except|else|do|repeat)\s{1}"))
			{
				validLines.Add(Tuple.Create(getLineFromPoint(sourceCode, x.Index), getNextTokenLine(sourceCode, x.Index)));
			}

			foreach(CoveragePoint x in points.Where(y => sourceFile.ToLower().Contains(y.sourceFile)))
			{
				if(validLines.Where(y => x.lineNumber >= y.Item1 && x.lineNumber <= y.Item2).Count() > 0)
					filteredPoints.Add(x);
			}

			filteredPoints.AddRange( (from CoveragePoint x in points where !sourceFile.ToLower().Contains(x.sourceFile) select x).ToList() );

			return filteredPoints;
		}
	}
}