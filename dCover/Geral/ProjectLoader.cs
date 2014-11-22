using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace dCover.Geral
{
	class ProjectLoader
	{
		private static string recursiveFileSearch(string fileName, string filePath, int maxDepth = 2)
		{
			string foundFile = Directory.GetFiles(filePath, fileName).FirstOrDefault();

			if(foundFile != null)
				return foundFile;

			if(maxDepth>0)
				foreach(string currentDirectory in Directory.GetDirectories(filePath))
				{
					foundFile = recursiveFileSearch(fileName, currentDirectory, maxDepth-1);

					if (foundFile != null)
						return foundFile;
				}
			
			return null;
		}
		
		public static IEnumerable<CoveragePoint> LoadProject(bool silent = false)
		{
			#region Loading map file
			OpenFileDialog mapDialog = new OpenFileDialog();
			mapDialog.CheckFileExists = true;
			mapDialog.Filter = "Detailed map file (.map)|*.map";
			
			if(mapDialog.ShowDialog() != DialogResult.OK)
				return null;

			string rootProjectPath = Regex.Match(mapDialog.FileName, @"(.\:(.+\\)*)(.+\..+)").Groups[1].Value;

			List<CoveragePoint> coveragePoints = MapParser.Parse(mapDialog.FileName).ToList();
			#endregion

			#region Load source and clean coverage points
			string mainSourceFileName = coveragePoints.Where(x => Regex.IsMatch(x.sourceFile, @"\.dp[rk]")).First().sourceFile;
			string mainSourceFile = recursiveFileSearch(mainSourceFileName, rootProjectPath);

			if(mainSourceFile == null)
			{
				OpenFileDialog sourceDialog = new OpenFileDialog();
				sourceDialog.Filter = "Source file|" + mainSourceFileName;
				sourceDialog.CheckFileExists = true;
				
				if(sourceDialog.ShowDialog() != DialogResult.OK)
					return null;

				mainSourceFile = sourceDialog.FileName;
			}

			string projectRootPath = Regex.Match(mainSourceFile, @"(.\:(.+\\)*)(.+\..+)").Groups[1].Value;
			
			foreach(string currentFile in coveragePoints.Select(x => x.sourceFile).Distinct())
			{
				string currentSourceFile = recursiveFileSearch(currentFile, projectRootPath, 5);

				if(currentSourceFile == null)
					if(silent)
						return null;
					else
					{
						MessageBox.Show(currentFile + " not found in " + projectRootPath);
						return null;
					}

				coveragePoints = SourceParser.FilterCoveragePoints(coveragePoints, currentSourceFile).ToList();
			}
			#endregion
			
			foreach(CoveragePoint x in coveragePoints)
				Console.WriteLine(x.lineNumber + " " + x.sourceFile);

			return coveragePoints;
		}
	}
}
