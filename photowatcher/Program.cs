using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;


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
                /*(ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "C:\\Windows\\System32\\mspaint.exe";
                startInfo.Arguments = " -p " + e.FullPath;
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }*/
                SendToPrinter(source, e);
            }
            catch
            {
                //error
            }

        }

        private static void SendToPrinter(object source, FileSystemEventArgs e)
        {
            try
            {
                //System.Threading.Thread.Sleep(2000);
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = "Canon SELPHY CP720";
                //pd.DefaultPageSettings.PrinterSettings.PrinterName = "Canon MF240 Series PCL6";
                pd.DefaultPageSettings.Landscape = true; //or false!
                pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); 
                //pd.DefaultPageSettings.PaperSize = new PaperSize("4x5", 320, 960);
                pd.PrintPage += (sender, args) =>
                {
                    Image i = Image.FromFile(e.FullPath);
                    args.Graphics.DrawImage(i, args.MarginBounds);
                };
                pd.Print();
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.ToString());
            }

        }
    }
}
