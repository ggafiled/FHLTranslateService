using DHLBFSDACService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHLBFSDACService;
using static DHLBFSDACService.GlobalConfig;

namespace DHLBFSDACService
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (!Util.isDirectoryEmpty(rootPath, fileExtension))
            {
                Util.WriteToFile("LOG:---------- Start Process ----------");

                string[] files = Directory.GetFiles(rootPath, fileExtension, SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    try
                    {
                        Util.WriteToFile("LOG: Has file in directory");
                        string fileName = Path.GetFileName(file);
                        Util.WriteToFile("LOG: Processing: " + fileName);
                        File.Move(file, Path.Combine(buildPath, fileName));
                        Util.WriteToFile("LOG: Moved to build folder: " + fileName);
                        Util.WriteToFile("LOG: Moved Successfully: " + fileName);
                    }
                    catch (Exception ex)
                    {
                        Util.WriteToFile("ERR:" + ex.Message);
                    }
                }
                Util.WriteToFile("LOG:---------- End Process ----------");
            }

            if (!Util.isDirectoryEmpty(buildPath, fileExtension))
            {
                Util.WriteToFile("LOG:---------- Start Translate Process ----------");

                string[] files = Directory.GetFiles(buildPath, fileExtension, SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    try
                    {
                        // 1. Logging start timestamp.
                        LoggingImportFile logging = new LoggingImportFile();
                        Util.WriteToFile("LOG: Has file in directory");
                        string fileName = Path.GetFileName(file);

                        // 2. Retreive data from FHL file and store in varible.
                        Util.WriteToFile("LOG: Reading FHL: " + fileName);
                        var fhlData = File.ReadAllLines(file);

                        // 3. Translate FHL data into valid format reference to BFS condition.
                        Util.WriteToFile("LOG: Translating: " + fileName);
                        string new_message = Util.TranslateFHL(fhlData.Skip(2).ToArray());

                        // 4. Move original file into keep folder.
                        Util.WriteToFile("LOG: Moved original file to keep folder: " + fileName);
                        Util.MoveToBackUp(file, fileName, keepPath);

                        // 5. Create new FHL file into work folder with TXT format. (Finish)
                        Util.WriteToFile("LOG: Creating FHL: " + fileName);
                        string workPathFHLData = Path.Combine(workPath, fileName);
                        using (StreamWriter sw = File.CreateText(workPathFHLData))
                        {
                            sw.Write(new_message);
                        }
                        Util.WriteToFile("LOG: Created FHL: " + fileName);

                    }
                    catch (Exception ex)
                    {
                        Util.WriteToFile("ERR:" + ex.Message);
                    }
                }
                Util.WriteToFile("LOG:---------- End Translate Process ----------");
            }
        }
    }
}
