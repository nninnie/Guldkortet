using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guldkortet2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();               // Aktiverar moderna Windows-stilar för kontroller
            Application.SetCompatibleTextRenderingDefault(false); // Standardtext-rendering för bättre kompatibilitet
            Application.Run(new Form1());                   // Startar huvudfönstret Form1 – härifrån körs hela programmet
        }
    }
}
