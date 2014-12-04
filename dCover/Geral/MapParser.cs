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
			List<FunctionName> functionNames = new List<FunctionName>();

			string mapFile = File.ReadAllText(fileName);

			int namesSection = Regex.Match(mapFile, "Publics by Name").Index;
			mapFile = mapFile.Substring(namesSection, mapFile.Length - namesSection);
			mapFile = mapFile.Remove(Regex.Match(mapFile, "Line numbers for ").Index);

			foreach (Match name in Regex.Matches(mapFile, @"\d{4}\:(.{8})\s*([^\s]*)"))
			{
				FunctionName buffer = new FunctionName();
				buffer.offset = Convert.ToInt32("0x" + name.Groups[1].Value, 16);
				buffer.name = name.Groups[2].Value;

				functionNames.Add(buffer);
			}

			functionNames = functionNames.OrderBy(x => -x.offset).ToList();
			return functionNames;
		}

		private static IEnumerable<CoveragePoint> getCoveragePoints(string fileName)
		{
			List<CoveragePoint> coveragePoints = new List<CoveragePoint>();

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

					coveragePoints.Add(buffer);
				}
			}

			return coveragePoints;
		}

		private static void setFunctionNames(List<CoveragePoint> points, IEnumerable<FunctionName> names)
		{
			points.ForEach(x => x.routineName = names.Where(y => x.offset >= y.offset).Select(y => y.name).First());
		}

		public static IEnumerable<CoveragePoint> Parse(string fileName)
		{
			List<CoveragePoint> coveragePoints = new List<CoveragePoint>();

			coveragePoints = getCoveragePoints(fileName).ToList();
			setFunctionNames(coveragePoints, getFunctionNames(fileName));

			return coveragePoints;
		}
	}
}
