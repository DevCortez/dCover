using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dCover.Geral
{
	public class Project
	{		
		public List<CoveragePoint> coveragePointList = new List<CoveragePoint>();
		public List<SourceFolder> sourceFolders = new List<SourceFolder>();
		public List<string> moduleFiles = new List<string>();

		public void SaveToFile(string fileName)
		{
			XDocument fileBuffer = new XDocument();
			fileBuffer.Add(new XElement("coverage"));

			#region Saving source folders
			XElement sourceFoldersNode = new XElement("sourceFolders");
			fileBuffer.Element("coverage").Add(sourceFoldersNode);

			foreach (SourceFolder x in sourceFolders)
			{
				XElement sourceNode = new XElement("source");
				XElement moduleNode = new XElement("module");
				XElement pathNode = new XElement("path");

				moduleNode.Value = x.moduleName;
				pathNode.Value = x.path;

				sourceNode.Add(moduleNode);
				sourceNode.Add(pathNode);
				sourceFoldersNode.Add(sourceNode);
			}			
			#endregion

			#region Saving module paths
			XElement moduleFilesNode = new XElement("moduleFiles");
			fileBuffer.Element("coverage").Add(moduleFilesNode);

			foreach (string x in moduleFiles)
			{
				XElement moduleNode = new XElement("module");
				XElement moduleMD5 = new XElement("hash");

				moduleNode.Value = x;
				moduleFilesNode.Add(moduleNode);
				moduleFilesNode.Add(moduleMD5);
			}
			#endregion

			#region Saving coverage points
			XElement coveragePointsNode = new XElement("coveragePoints");
			fileBuffer.Element("coverage").Add(coveragePointsNode);
			
			foreach(CoveragePoint x in coveragePointList)
			{
				XElement coveragePointNode = new XElement("p");
				coveragePointNode.SetAttributeValue("m", x.moduleName);
				coveragePointNode.SetAttributeValue("l", x.lineNumber);
				coveragePointNode.SetAttributeValue("o", x.offset);
				coveragePointNode.SetAttributeValue("n", x.routineName);
				coveragePointNode.SetAttributeValue("s", x.sourceFile);
				coveragePointNode.SetAttributeValue("c", x.wasCovered);

				coveragePointsNode.Add(coveragePointNode);
			}
			#endregion

			fileBuffer.Save(fileName);
		}

		public void LoadFromFile(string fileName)
		{
			XDocument fileBuffer = XDocument.Load(fileName);

			foreach(XElement x in fileBuffer.Descendants("source"))
			{
				sourceFolders.Add(new SourceFolder(x.Element("module").Value, x.Element("path").Value));
			}

			foreach(XElement x in fileBuffer.Descendants("moduleFiles"))
			{
				moduleFiles.Add(x.Element("module").Value);
			}

			foreach(XElement x in fileBuffer.Descendants("p"))
			{
				CoveragePoint coveragePoint = new CoveragePoint();
				coveragePoint.wasCovered = Convert.ToBoolean(x.Attribute("c").Value);
				coveragePoint.sourceFile = x.Attribute("s").Value;
				coveragePoint.routineName = x.Attribute("n").Value;
				coveragePoint.offset = Convert.ToInt32(x.Attribute("o").Value);
				coveragePoint.lineNumber = Convert.ToInt32(x.Attribute("l").Value);
				coveragePoint.moduleName = x.Attribute("m").Value;

				coveragePointList.Add(coveragePoint);
			}
		}
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
