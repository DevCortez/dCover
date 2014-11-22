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
		private static string getModuleFileName(string mainSourceFile)
		{
			if(mainSourceFile.ToLower().Contains(".dpk"))
				return Path.ChangeExtension(Path.GetFileName(mainSourceFile), ".bpl");

			string sourceCode = File.ReadAllText(mainSourceFile).ToLower();
			sourceCode = sourceCode.Remove( Regex.Match(sourceCode, @"\;").Index );

			if (sourceCode.Contains("library"))
				return Path.ChangeExtension(Path.GetFileName(mainSourceFile), ".dll");

			if (sourceCode.Contains("program"))
				return Path.ChangeExtension(Path.GetFileName(mainSourceFile), ".exe");

			return null;
		}
		
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

		private static void updateModuleName(IEnumerable<CoveragePoint> points, string moduleName)
		{
			foreach(CoveragePoint x in points)
				x.moduleName = moduleName;
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

			#region Load source and clean up coverage points
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

			updateModuleName(coveragePoints, getModuleFileName(mainSourceFile));			
			
			foreach(CoveragePoint x in coveragePoints.OrderBy(y => y.lineNumber).ToList())
				Console.WriteLine(x.lineNumber + " " + x.sourceFile);

			return coveragePoints;
		}
	}
}
