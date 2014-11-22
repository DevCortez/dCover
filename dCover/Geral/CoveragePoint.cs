using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCover.Geral
{
	public class CoveragePoint
	{
		public int lineNumber;
		public int offset;
		public string sourceFile;
		public string routineName;
		public string moduleName;
		public bool wasCovered = false;	

		public byte originalCode;
		public bool isSet = false;
	}
}
