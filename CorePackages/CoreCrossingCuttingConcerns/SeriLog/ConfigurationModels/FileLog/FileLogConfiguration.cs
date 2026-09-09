using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCrossingCuttingConcerns.SeriLog.ConfigurationModels.FileLog
{
    public class FileLogConfiguration
    {
        public string FilePath { get; set; }

        public FileLogConfiguration()
        {
            FilePath= string.Empty;
        }

        public FileLogConfiguration(string filePath)
        {
            FilePath = filePath;
        }
    }
}
