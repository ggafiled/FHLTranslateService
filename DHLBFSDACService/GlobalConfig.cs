using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DHLBFSDACService
{
    public static class GlobalConfig
    {
        public static string rootPath = ConfigurationManager.AppSettings["ROOT_PATH"];
        public static string workPath = ConfigurationManager.AppSettings["WORK_PATH"];
        public static string buildPath = ConfigurationManager.AppSettings["BUILD_PATH"];
        public static string keepPath = ConfigurationManager.AppSettings["KEEP_PATH"];
        public static string fileExtension = ConfigurationManager.AppSettings["FILE_EXTENSION"];
    }
}
