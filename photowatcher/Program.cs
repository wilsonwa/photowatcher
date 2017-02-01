using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Diagnostics;

namespace photowatcher
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Watching folder " + args[0].ToString());
            Console.WriteLine("Press q to quit");
            Run();
            while (Console.Read() != 'q') ;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            FileSystemWatcher filewatcher = new FileSystemWatcher();
            if (args[1] == "Pictures")
                filewatcher.Path = "C:\\Users\\wilsonwa\\Pictures\\";
            else
                filewatcher.Path = args[1];
            filewatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            filewatcher.Created += new FileSystemEventHandler(OnCreated);
            //filewatcher.Changed += new FileSystemEventHandler(OnCreated);
            filewatcher.Filter = "*.bmp";

            filewatcher.EnableRaisingEvents = true;
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            try
            {
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "C:\\Windows\\System32\\mspaint.exe";
                startInfo.Arguments = " -p " + e.FullPath;
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                //error
            } 

        }
    }
}
