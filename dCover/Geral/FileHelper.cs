using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCover.Geral
{
    class FileHelper
    {
        public static string recursiveFileSearch(string fileName, string filePath, int maxDepth = 2)
        {
            string foundFile = Directory.GetFiles(filePath, fileName).FirstOrDefault();

            if (foundFile != null)
                return foundFile;

            if (maxDepth > 0)
                foreach (string currentDirectory in Directory.GetDirectories(filePath))
                {
                    foundFile = recursiveFileSearch(fileName, currentDirectory, maxDepth - 1);

                    if (foundFile != null)
                        return foundFile;
                }

            return null;
        }
    }
}
