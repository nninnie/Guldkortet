using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2
{
    public partial class TpcKoppling
    {
        private TcpClient klient; // Klienten (NOS_Export) som ansluter                  
        private TcpListener lyssnare; // Servern som lyssnar
        private Form1 form; // Kunna praata med Form1
        private bool running = true; // När running är true fortsätter servern läsa data


        // Konstruktor för tpcKoppling
        public TpcKoppling(Form1 form1)
        {
            this.form = form1; // Form1 obejekt sparas i variabel
        }

        // Startar kopplingen till servern
        public void StartaKoppling()
        {
            // Skapar lyssnare på port 12345, IP: 127.0.0.1
            lyssnare = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
            //Startar lyssnare
            lyssnare.Start();

            // Väntar till Nos eport ansluter  och körs i bakgrundewn så att UI inte låser sig
            Task.Run(async () =>
            {
                try
                {
                    // Väntar asykron på att NOS_Export ansluter
                    klient = await lyssnare.AcceptTcpClientAsync();

                    // TCP kör i bakgrunden och får inte röra UI direkt, annars kraschar programmet.
                    // Invoke låter UI‑tråden göra jobbet och gör det trådsäkert.
                    form.Invoke(new Action(() =>
                    {
                        form.UppdateraStatus("Ansluten till NOS-Export"); // När NOS_Export ansluter ändra status i UI status
                    }));

                }
                // Felhantering 
                catch
                {
                    return;
                }
                // Startar läsningen från NOS_Export, inkomande data
                StartaLasning(klient);
            });
        }

        // Läser inkommande data från NOS_Export
        private async void StartaLasning(TcpClient k)
        {
        
                // Hämta NetworkStream  - strömen där data skickas
                NetworkStream stream = k.GetStream();

            // Loop för att läsa TPC data som komemr in så länmge running är true och klienten är ansluten
            while (running && k.Connected)
            {
                int n;
                byte[] buffer = new byte[1024]; // Skapa buffer för inkomande bytes

                // Om något går fel, fånga felet och stoppa kraschen
                try
                {
                    // Läser datan asykront 
                    n = await stream.ReadAsync(buffer, 0, buffer.Length); 
                }
                catch
                {
                    break; // Avslutar
                }

                // Klienten stängs om inga byte läses in
                if (n <= 0)
                    break;

                // Konverterar bytes till Unicode
                string kod = Encoding.Unicode.GetString(buffer, 0, n).Trim();

                // Tom sträng, resultat fylls i invoke 
                string resultat = "";

                // TCP kör i bakgrunden och får inte röra UI direkt, annars kraschar programmet
                // Invoke låter UI‑tråden göra jobbet och gör det trådsäkert
                                form.Invoke(new Action(() =>
                {
                    resultat = form.HanteraKod(kod); 
                }));

                byte[] data = Encoding.Unicode.GetBytes(resultat); // Svaret kodas som Unicode
                await stream.WriteAsync(data, 0, data.Length); // Svaret skickas tillbaka til NOS_export
            }


            // Om något går fel, fånga felet och stoppa kraschen
            try
            {
                k.Close(); // Stänger ner
            }
            catch
            {

            }
        }

        // Stoppar servern 
        public void StoppaKoppling()
        {
            // Stoppar while-loopen från att läsa mer
            running = false;

            //Om något går fel, fånga felet och stoppa kraschen - Try catch
            try
            {
                // Stänger TCP-lyssnaren
                lyssnare.Stop();
            }
            catch
            {
                // Ignorerar fel vid stopp
            }
        }
    }
} 