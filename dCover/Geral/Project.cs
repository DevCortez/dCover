using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCover.Geral
{
	public class Project
	{		
		public List<CoveragePoint> coveragePointList = new List<CoveragePoint>();
		public List<SourceFolder> sourceFolders = new List<SourceFolder>();
		public List<string> moduleFiles = new List<string>();
	}

	public class SourceFolder
	{
		public string moduleName;
		public string path;

		public SourceFolder(string _moduleName, string _path)
		{
			moduleName = _moduleName;
			path = _path;
		}
	}
}
