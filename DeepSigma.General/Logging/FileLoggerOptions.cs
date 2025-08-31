using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Logging
{
    public class FileLoggerOptions
    {
        public virtual required string FilePath { get; set; }
        public virtual required string FolderPath { get; set; }
    }
}
