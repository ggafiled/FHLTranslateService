using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHLBFSDACService.Models;
using Dapper;
using System.Text.RegularExpressions;
using static DHLBFSDACService.GlobalConfig;

namespace DHLBFSDACService
{
    enum LogType
    {
        ADD_TO_QUEUE,
        QUEUE_PROCESS
    }

    class Util
    {
        public Util()
        {

        }

        public static bool isDirectoryEmpty(string rootPathIn, string fileExtension)
        {
            return Directory.GetFiles(rootPathIn, fileExtension, SearchOption.TopDirectoryOnly).Length == 0;
        }

        public static void MoveToWorkFolder(string file, string fileName, string workPath, LogType logType = LogType.ADD_TO_QUEUE)
        {
            try
            {
                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }
                File.Move(file, Path.Combine(workPath, fileName));
            }
            catch (Exception ex)
            {
                WriteToFile("ERR:" + ex.Message, logType);
            }
        }

        public static void MoveToBackUp(string file, string fileName, string rootPathBackUp, LogType logType = LogType.ADD_TO_QUEUE)
        {
            try
            {
                string currentDate = DateTime.Now.Date.ToString("dd");
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string path = Path.Combine(rootPathBackUp, currentYear, currentMonth, currentDate);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.Move(file, Path.Combine(path, fileName));
            }
            catch (Exception ex)
            {
                WriteToFile("ERR:" + ex.Message, logType);
            }
        }

        public static void WriteToFile(string Message, LogType logType = LogType.ADD_TO_QUEUE)
        {
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("th-TH");
            string LogPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + (logType == LogType.ADD_TO_QUEUE ? "FileService" : "TaskQueue");
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string filepath = LogPath + "\\ServiceLog_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(localDate.ToString(culture) + " " + Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(localDate.ToString(culture) + " " + Message);
                }
            }
        }

        public static string TranslateFHL(string[] message)
        {
            Regex reg = new Regex(@"[\+\?\*@%&'|,_\(\)<>#:""""=;\~\[\]{}^$!`]");
            var result = new List<string>();

            foreach(var text in message)
            {
                result.Add(Regex.Replace(reg.Replace(text, " ").ToString(), @"\s+", " ").ToUpper().Trim());
            }
            return String.Join(System.Environment.NewLine, result.ToArray());
        }
    }
}
