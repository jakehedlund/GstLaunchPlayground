using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GstPlayground
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeConsole();


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);


        //public delegate bool EventHandler(CtrlType sig);
        //[DllImport("Kernel32.dll")]
        //public static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        public static void ShowConsole()
        {
            //
            // Show console window for debugging purpose.
            //
            if (!NativeMethods.AllocConsole())
            {
                MessageBox.Show("Show console window failed");
                NativeMethods.FreeConsole();
            }

            try
            {
                //MessageBox.Show("Last error: " + Marshal.GetLastWin32Error()); 
                Console.BufferWidth = Console.WindowWidth = 160;
                Console.WindowHeight = 20;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Title = "GstTest";

                //Debug.Listeners.Add(new TextWriterTraceListener(System.Console.Out));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring console. (win32 error: {Marshal.GetLastWin32Error()}) {ex.Message}");
            }

            Console.WriteLine("Console logging activated.");
            Debug.WriteLine("Debug log active."); 
            //NativeMethods.SetConsoleCtrlHandler(ConsoleHandler, true);
        }
    }
}
