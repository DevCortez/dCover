using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dCover.Geral
{
	class MapParser
	{
		private class FunctionName
		{
			public string name;
			public int offset;
		}

		private static IEnumerable<FunctionName> getFunctionNames(string fileName)
		{
			string mapFile = File.ReadAllText(fileName);

			int namesSection = Regex.Match(mapFile, "Publics by Name").Index;
			mapFile = mapFile.Substring(namesSection, mapFile.Length - namesSection);
			mapFile = mapFile.Remove(Regex.Match(mapFile, "Line numbers for ").Index);

			foreach (Match name in Regex.Matches(mapFile, @"\d{4}\:(.{8})\s*([^\s]*)"))
			{
				FunctionName buffer = new FunctionName();
				buffer.offset = Convert.ToInt32("0x" + name.Groups[1].Value, 16);
				buffer.name = name.Groups[2].Value;

				yield return buffer;
			}
		}

		private static IEnumerable<CoveragePoint> getCoveragePoints(string fileName)
		{
			string mapFile = File.ReadAllText(fileName);

			foreach (Match lines in Regex.Matches(mapFile, @"Line numbers for [^\(]*\(([^\)]*)"))
			{
				int pointsSection = lines.Index + 1;
				string sourceFileName = Regex.Match(lines.Groups[1].Value.ToLower(), @"(.+\\)*(.+\..+)").Groups[2].Value.ToLower();

				string currentSection = mapFile.Substring(pointsSection, mapFile.Length - pointsSection);

				int nextSection = Regex.Match(currentSection, "Line numbers for ").Index;

				if (nextSection > 0)
					currentSection = currentSection.Remove(nextSection);

				foreach (Match point in Regex.Matches(currentSection, @"(\d+) \d{4}\:(.{8})"))
				{
					CoveragePoint buffer = new CoveragePoint();
					buffer.lineNumber = Convert.ToInt32(point.Groups[1].Value);
					buffer.offset = Convert.ToInt32(point.Groups[2].Value, 16);
					buffer.sourceFile = sourceFileName;

					yield return buffer;
				}
			}
		}

		private static void setFunctionNames(List<CoveragePoint> points, List<FunctionName> names)
		{
			// This is a critical procedure to analyze coverage data and needs performanece thus
			// it was coded using simple for to improve performance.

			for(var x = names.Count() - 1; x != 0 ; x--)
				for(var y = 0; y < points.Count; y++)
					if(points[y].routineName == null && points[y].offset >= names[x].offset)
						points[y].routineName = names[x].name;
		}

		public static IEnumerable<CoveragePoint> Parse(string fileName)
		{
			IEnumerable<CoveragePoint> coveragePoints = getCoveragePoints(fileName).ToList();						
			setFunctionNames(coveragePoints.ToList(), getFunctionNames(fileName).OrderBy(x => x.offset).ToList());
			return coveragePoints;
		}
	}
}
