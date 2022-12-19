using SSP_1.Exception;
using System;
using System.IO;
using System.Linq;

namespace SSP_1.Services
{
    public class ResourceReader
    {
        public string GetResourcePath(string fileName)
        {
            var runtimeDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            var resourceFolder = runtimeDirectory.Parent.Parent.EnumerateDirectories("Resources").FirstOrDefault();

            if (resourceFolder?.EnumerateFiles(fileName).FirstOrDefault() != null)
            {
                return resourceFolder.EnumerateFiles(fileName).FirstOrDefault()?.FullName;
            }

            throw new DatabaseException("Database doesn't exist");
        }
    }
}
