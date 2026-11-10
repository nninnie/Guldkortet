using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet
{
    // Sköter TCP-kommunikationen med NOS_Export (klienten som skickar koder)
    public class TcpKoppling
    {
        private TcpClient klient;      // Klienten (NOS_Export) som ansluter
        private TcpListener lyssnare;  // Servern som lyssnar
        private readonly Form1 form;   // Kunna prata med Form1
        private bool running = true;   // När running är true fortsätter servern läsa data

        // Konstruktor för TcpKoppling
        public TcpKoppling(Form1 form1)
        {
            form = form1; // Form1-objekt sparas i variabel
        }

        // Startar kopplingen till servern
        public void StartaKoppling()
        {
            // Skapar lyssnare på port 12345, IP: 127.0.0.1
            lyssnare = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
            lyssnare.Start();

            // Väntar till NOS_Export ansluter och körs i bakgrunden så att UI inte låser sig
            Task.Run(async () =>
            {
                try
                {
                    // Väntar asynkront på att NOS_Export ansluter
                    klient = await lyssnare.AcceptTcpClientAsync();
                }
                catch (Exception)
                {
                    // Lyssnaren stoppades innan någon anslöt (t.ex. när programmet stängs)
                    return;
                }

                UppdateraUI(() => form.UppdateraStatus("Ansluten till NOS-Export", true));

                // Startar läsningen från NOS_Export för inkommande data
                await StartaLasning(klient);
            });
        }

        // Läser inkommande data från NOS_Export
        // async Task (inte async void) så att anroparen kan vänta på metoden och fel inte försvinner
        private async Task StartaLasning(TcpClient klient)
        {
            // Hämta NetworkStream - strömmen där data skickas
            NetworkStream stream = klient.GetStream();

            // Bufferten deklareras utanför loopen för att återanvändas
            byte[] buffer = new byte[1024];

            // Loop för att läsa TCP-data så länge running är true och klienten är ansluten
            while (running && klient.Connected)
            {
                int lastaBytes;

                try
                {
                    // Läser datan asynkront
                    lastaBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch (Exception)
                {
                    break; // Avslutar loopen och går till stängning vid nätverksfel
                }

                // 0 bytes betyder att klienten har stängt anslutningen
                if (lastaBytes <= 0)
                {
                    break;
                }

                // Konverterar bytes till Unicode-text och trimmar bort mellanslag
                string kod = Encoding.Unicode.GetString(buffer, 0, lastaBytes).Trim();

                // Om strängen blev tom efter rensning hoppar vi till nästa läsning
                if (string.IsNullOrEmpty(kod))
                {
                    continue;
                }

                // Resultatet fylls i av Form1 via UI-tråden
                string resultat = "";
                UppdateraUI(() => resultat = form.HanteraKod(kod));

                // Om Form1 returnerar ett svar skickas det tillbaka till NOS_Export
                if (!string.IsNullOrEmpty(resultat))
                {
                    // Svaret kodas till Unicode-bytes utan extra radbrytningar
                    byte[] data = Encoding.Unicode.GetBytes(resultat.Trim());

                    try
                    {
                        // Skickas asynkront för att trigga NOS_Exports MessageBox
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                    catch (Exception)
                    {
                        break; // Avbryt om det inte går att skriva tillbaka till strömmen
                    }
                }
            }

            // När loopen bryts stängs anslutningen
            klient.Close();

            UppdateraUI(() => form.UppdateraStatus("Frånkopplad från NOS-Export", false));
        }

        // TCP kör i bakgrunden och får inte röra UI direkt, annars kraschar programmet.
        // Invoke låter UI-tråden göra jobbet och gör det trådsäkert.
        // Trådsäkerhet = flera trådar får inte ändra samma sak samtidigt, annars kan programmet krascha eller bete sig konstigt.
        private void UppdateraUI(Action action)
        {
            // Formuläret kan redan vara stängt när bakgrundstråden blir klar
            if (form == null || form.IsDisposed || !form.IsHandleCreated)
            {
                return;
            }

            try
            {
                form.Invoke(action);
            }
            catch (ObjectDisposedException)
            {
                // Formuläret stängdes precis innan Invoke – inget att uppdatera
            }
            catch (InvalidOperationException)
            {
                // Fönstrets handle är redan borttaget – inget att uppdatera
            }
        }

        // Stoppar servern och rensar resurser
        public void StoppaKoppling()
        {
            // Stoppa huvudloopen i läsningen
            running = false;

            // Stänger klienten så att en pågående ReadAsync avbryts direkt
            klient?.Close();

            // Stänger TCP-lyssnaren
            lyssnare?.Stop();
        }
    }
}
