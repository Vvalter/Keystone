using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSInterface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
	    //ImageHelper.ProcessImage(@"C:\NewCards", @"C:\ProcessedCards");
            Console.WriteLine(KSDllWrapper.InitializeDatabase(@"C:\ProcessedCards").ToString());
            Console.WriteLine(KSDllWrapper.RecognizeImage(@"C:\test\blar.bmp").ToString());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
