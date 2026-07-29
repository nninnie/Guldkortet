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
            tcp.StartaKoppinig(); // Startar kopplingen till servern

            lblStatus.Text = "Lyssnar på NOS-Export..."; // Status ändring för Tpc koppling
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void txtKortnummer_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstKoder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSparaFil_Click(object sender, EventArgs e)
        {

        }

        // Avslutar programet                 
        private void btnAvsluta_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // När programmet avlsutas så skrivs det ut till guldkort2_logg.txt
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
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

        }

        private void lblSvarResultat_Click(object sender, EventArgs e)
        {

        }

        public void HanteraKod(string mottagenKod)
        {


        }



    }
}
