using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLBFSDACService.Models
{
    class LoggingImportFile
    {
        public string FileName { get; set; }
        public string Datatype { get; set; }
        public string RetrivedAt { get; set; } // Retrived data from root folder into work folder waiting for translate process 
        public string ProcessAt { get; set; } // Translate process time start from
        public string EndAt { get; set; } // End time of process
        public string OriginData { get; set; }
        public string ReviseData { get; set; }
        public string ProcessResult { get; set; }
    }
}
