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
        private Form1 form; // Kunna prata med Form1

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
        private async void StartaLasning(TcpClient klient)
        {
            // Hämta NetworkStream – strömmen där data skickas
            NetworkStream stream = klient.GetStream();

            try
            {
                byte[] buffer = new byte[1024]; // Buffer för inkommande bytes

                // Läser inkommande data asynkront
                int antalBytes = await stream.ReadAsync(buffer, 0, buffer.Length);

                // Avslutar om inga byte läses in 
                if (antalBytes <= 0)
                {
                    klient.Close(); // Stänger anslutningen
                    return;
                }

                // Konverterar mottagna byte till Unicode-text
                string kod = Encoding.Unicode.GetString(buffer, 0, antalBytes).Trim();

                string resultat = ""; // Tom sträng som fylls i via Invoke

                // TCP kör i bakgrunden och får inte uppdatera UI direkt
                // Invoke låter UI‑tråden hantera resultatet trådsäkert
                form.Invoke(new Action(() =>
                {
                    resultat = form.HanteraKod(kod); // Hanterar koden i Form1
                }));

                // Kodar svaret som Unicode och skickar tillbaka till NOS_Export
                byte[] data = Encoding.Unicode.GetBytes(resultat);
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch
            {
                // Avslutar vid fel 
                try { klient.Close(); } catch { }
                return;
            }

            // Anropa StartaLasning igen för att fortsätta läsa
            StartaLasning(klient);
        }


        // Stoppar servern 
        public void StoppaKoppling()
        {
            try
            {
                klient.Close(); // Stänger anslutningen till klient
            }
            catch
            {
                // Ignorerar fel
            }

            try
            {
                lyssnare.Stop(); // Stänger lyssnaren till servern som tar emot anslutningar från NOS_Export
            }
            catch
            {
                // Ignorerar fel
            }
        }
    }
} 