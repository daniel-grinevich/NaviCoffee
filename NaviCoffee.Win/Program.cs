using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaviCoffee.Win
{
    static class Program
    {        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread] // Windows won't recgonize the scanner as a device so this is to create a hook that will read it like a keyboard entries 
        static void Main()
        {            
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new BarcodeScannerReady());
        }
    }
}
