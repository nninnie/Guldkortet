using Guldkortet2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guldkortet2
{
    public partial class Form1 : Form
    {
        // Fält för tillgång till guldkorservice klassen
        private GuldkortService guldkortService = new GuldkortService();

        // Fält för TCP‑kommunikationen mot NOS_Export
        private TpcKoppling tcp;

        public Form1()
        {
            InitializeComponent(); // Skapar alla UI‑komponenter

            tcp = new TpcKoppling(this); // Skapar Tpc objektet som lyssnar på NOS_Export
            tcp.StartaKoppling(); // Startar kopplingen till servern

            lblStatus.Text = "Lyssnar på NOS-Export..."; // Status ändring för Tpc koppling
        }

        private void txtKortnummer_TextChanged(object sender, EventArgs e)
        {
            // Ingen funktion – kunden skriver in kortnummer
        }

        private void lstKoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ingen funktion – list används bara för att visa resultat från koder
        }

        // Sparar loggpost till guldkort2_logg.txt
        private void btnSparaFil_Click(object sender, EventArgs e)
        {
            using (var writer = new StreamWriter("guldkort2_logg.txt", append: true))
            {
                foreach (var logg in guldkortService.HamtaLoggar())
                {
                    writer.WriteLine(logg.ToString()); // Skriver ut loggpost via ToString()
                }
            }

            MessageBox.Show("Loggar sparade!"); // Bekräftelse till användaren
        }

        // Avslutar programet                 
        private void btnAvsluta_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // När programmet avlsutas så skrivs det ut till guldkort2_logg.txt
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            // Skriver ut logg till guldkort2_logg.txt
            using (var writer = new StreamWriter("guldkort2_logg.txt", append: true))
            {
                foreach (var logg in guldkortService.HamtaLoggar())
                {
                    writer.WriteLine(logg.ToString());   // Använder Loggpost ToString()
                }
            }

            tcp.StoppaKoppling(); // Stoppar lyssnaren när programmet avslutas
        }


        private void lblStatus_Click(object sender, EventArgs e)
        {
            // Ingen funktion – label används bara för att visa status
        }

        private void lblSvarResultat_Click(object sender, EventArgs e)
        {
            // Ingen funktion – label används bara för att visa resultat
        }

        // Validerar koden och skickar till ProcessaKod
        private void btnValideraKort_Click(object sender, EventArgs e)
        {
            string kod = txtKortnummer.Text; // Hämtar inmatad kod 

            string resultat = guldkortService.ProcessaKod(kod); // Processar koden

            lstKoder.Items.Add(kod); // Visar kod i listan
            lblSvarResultat.Text = resultat; // Visar resultat
        }

        // Metod som anropas av TCP‑lyssnaren när en kod tas emot från NOS_Export
        public void HanteraKod(string mottagenKod)
        {
            // Använder metoden ProcessaKod via GuldkortService för koden
            string resultat = guldkortService.ProcessaKod(mottagenKod);

            // Visa mottagen kod i listan
            lstKoder.Items.Add(mottagenKod);

            // Visa resultatet i UI
            lblSvarResultat.Text = resultat;
        }
    }
}