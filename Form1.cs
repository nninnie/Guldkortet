using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Guldkortet
{
    public partial class Form1 : Form
    {
        // Filen som loggarna sparas i
        private const string LoggFil = "guldkortet_logg.txt";

        // Affärslogiken (validering, belöningar och loggar)
        private readonly GuldkortService guldkortService = new GuldkortService();

        // TCP-kommunikationen mot NOS_Export
        private readonly TcpKoppling tcp;

        // Hur många loggposter som redan har skrivits till filen (så att inget sparas två gånger)
        private int antalSparadeLoggar = 0;

        public Form1()
        {
            InitializeComponent(); // Skapar alla UI-komponenter

            tcp = new TcpKoppling(this); // Skapar TCP-objektet som lyssnar på NOS_Export
            tcp.StartaKoppling();        // Startar servern

            lblStatus.Text = "Lyssnar på NOS-Export...";
        }

        // Validerar koden som skrivs in manuellt i textrutan
        private void btnValideraKort_Click(object sender, EventArgs e)
        {
            string resultat = HanteraKod(txtKortnummer.Text);

            // Popup med resultatet av validerad kod
            MessageBox.Show(resultat, "Validering", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Anropas både av knappen och av TCP-lyssnaren när en kod tas emot från NOS_Export
        public string HanteraKod(string mottagenKod)
        {
            string resultat = guldkortService.ProcessaKod(mottagenKod);

            lstKoder.Items.Add(mottagenKod); // Visar koden i listan
            lblSvarResultat.Text = resultat; // Visar resultatet

            return resultat;
        }

        // Uppdaterar statusraden: grön när NOS_Export är ansluten, röd när den är frånkopplad
        public void UppdateraStatus(string text, bool ansluten)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = ansluten ? Color.Green : Color.Firebrick;
        }

        // Sparar nya loggposter till textfilen. Returnerar hur många som sparades.
        private int SparaLoggar()
        {
            // Skip hoppar över loggar som redan har sparats tidigare
            var nyaLoggar = guldkortService.HamtaLoggar().Skip(antalSparadeLoggar).ToList();

            if (nyaLoggar.Count == 0)
            {
                return 0;
            }

            // append: true = lägger till i slutet av filen i stället för att skriva över den
            using (var writer = new StreamWriter(LoggFil, append: true))
            {
                foreach (var logg in nyaLoggar)
                {
                    writer.WriteLine(logg.ToString()); // Använder Loggpost.ToString()
                }
            }

            antalSparadeLoggar += nyaLoggar.Count;
            return nyaLoggar.Count;
        }

        // Knappen "Spara till textfil"
        private void btnSparaFil_Click(object sender, EventArgs e)
        {
            int antal = SparaLoggar();

            MessageBox.Show(antal > 0 ? $"{antal} loggar sparade!" : "Inga nya loggar att spara.");
        }

        // Knappen "Avsluta"
        private void btnAvsluta_Click(object sender, EventArgs e)
        {
            Close();
        }

        // När programmet stängs sparas loggarna och TCP-servern stoppas
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SparaLoggar() > 0)
            {
                MessageBox.Show("Loggar sparade!");
            }

            tcp.StoppaKoppling();
        }
    }
}
