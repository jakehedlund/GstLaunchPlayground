using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GstPlayground
{
    static class Program
    {
        public static string gstPathVar = "GSTREAMER_1_0_ROOT_MINGW_X86_64"; //GSTREAMER_1_0_ROOT_MSVC_X86_64
        public static string gstDebug = "*:2,GST_STATES:2,GST_REFCOUNTING:2,GST_CAPS:2,d3dvideosink:2," +
                "videoscale:2,appsrc:2,jpegenc:2,convertframe:2";

        // "*:4,GST_STATES:5,d3dvideosink:2,videodecoder:2,x264enc:2"
        // "*:4,GST_STATES:2,videodecoder:1,d3dvideosink:2,wasapisrc:2,queue:5,pad:5,probe:5"

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += application_ThreadException;
            System.AppDomain.CurrentDomain.UnhandledException += currentDomain_UnhandledException;
            Application.EnableVisualStyles();
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                Process.GetCurrentProcess().PriorityBoostEnabled = true;
                FindGst(); 
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, gst is a sack of potatoes. " + ex.Message);
                Application.Exit(); 
            }
        }

        private static void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            Console.WriteLine("-- UNHANDLED EXCEPTION -- " + ex?.Message);
            Console.WriteLine(ex?.StackTrace);
        }

        private static void application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Console.WriteLine("-- UNHANDLED THREAD EXCEPTION -- " + e.Exception.Message);
            Console.WriteLine(e.Exception.StackTrace); 
        }

        private static void FindGst()
        {
            var path = Environment.GetEnvironmentVariable("Path");
            var pp = Path.Combine(Environment.GetEnvironmentVariable(gstPathVar), "bin");

            Environment.SetEnvironmentVariable("Path", pp + ";" + path);
            System.Diagnostics.Debug.WriteLine("Path added: " + pp);

            //Environment.SetEnvironmentVariable("GST_DEBUG_NO_COLOR", "1");
            Environment.SetEnvironmentVariable("GST_DEBUG", gstDebug);
        }
    }
}
