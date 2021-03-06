﻿using System;
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

		private static void updateModuleName(IEnumerable<CoveragePoint> points, string moduleName)
		{
			foreach(CoveragePoint x in points)
				x.moduleName = moduleName;
		}
		
		public static bool LoadNewDelphiProject(Project project, bool silent = false)
		{
			#region Loading map file
			OpenFileDialog mapDialog = new OpenFileDialog();
			mapDialog.CheckFileExists = true;
			mapDialog.Filter = "Detailed map file (.map)|*.map";
			
			if(mapDialog.ShowDialog() != DialogResult.OK)
				return false;

			string rootProjectPath = Regex.Match(mapDialog.FileName, @"(.\:(.+\\)*)(.+\..+)").Groups[1].Value;

			List<CoveragePoint> coveragePoints = MapParser.Parse(mapDialog.FileName).ToList();
			#endregion

			#region Load source and clean up coverage points
			string mainSourceFileName = coveragePoints.Where(x => Regex.IsMatch(x.sourceFile, @"\.dp[rk]")).First().sourceFile;
            string mainSourceFile = FileHelper.recursiveFileSearch(mainSourceFileName, rootProjectPath);

			if(mainSourceFile == null)
			{
				OpenFileDialog sourceDialog = new OpenFileDialog();
				sourceDialog.Filter = "Source file|" + mainSourceFileName;
				sourceDialog.CheckFileExists = true;
				
				if(sourceDialog.ShowDialog() != DialogResult.OK)
					return false;

				mainSourceFile = sourceDialog.FileName;
			}

			rootProjectPath = Regex.Match(mainSourceFile, @"(.\:(.+\\)*)(.+\..+)").Groups[1].Value;
			
			foreach(string currentFile in coveragePoints.Select(x => x.sourceFile).Distinct())
			{
                string currentSourceFile = FileHelper.recursiveFileSearch(currentFile, rootProjectPath, 5);

				if(currentSourceFile == null)
                    currentSourceFile = FileHelper.recursiveFileSearch(currentFile, rootProjectPath + @"..\", 5);

                if (currentSourceFile == null)
                {
                    Console.WriteLine("File not found " + currentFile);

                    coveragePoints = coveragePoints.Where(x => x.sourceFile != currentFile).ToList();
                    continue;
                }

				coveragePoints = SourceParser.FilterCoveragePoints(coveragePoints, currentSourceFile).ToList();
			}
			#endregion

			string moduleFileName = getModuleFileName(mainSourceFile);

			if((from ProjectModule x in project.moduleFiles where x.moduleFile.Contains(moduleFileName) select x).FirstOrDefault() != null)
				return false;

            coveragePoints.Remove(coveragePoints.Where(x => x.sourceFile == mainSourceFileName).OrderBy(x => x.lineNumber).FirstOrDefault());
            
			updateModuleName(coveragePoints, moduleFileName);									
			project.coveragePointList.AddRange(coveragePoints);
            string moduleFile = FileHelper.recursiveFileSearch(moduleFileName, rootProjectPath);

			if(moduleFile != null)
				{
					ProjectModule projectModule = new ProjectModule();
					projectModule.moduleFile = moduleFile;

                    coveragePoints.Select(x => x.sourceFile).ToList().ForEach(x => projectModule.selectedSourceFiles.Add(x));
                    coveragePoints.Select(x => x.routineName).ToList().ForEach(x => projectModule.selectedRoutines.Add(x));

					project.moduleFiles.Add(projectModule);
				}
			
            project.sourceFolders.Add(new SourceFolder(moduleFileName, rootProjectPath));
			return true;
		}
	}
}
