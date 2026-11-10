using System;
using System.Windows.Forms;

namespace Guldkortet
{
    internal static class Program
    {
        // Startpunkten för programmet
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();               // Aktiverar moderna Windows-stilar för kontroller
            Application.SetCompatibleTextRenderingDefault(false); // Standardtext-rendering för bättre kompatibilitet
            Application.Run(new Form1());                   // Startar huvudfönstret Form1 – härifrån körs hela programmet
        }
    }
}
