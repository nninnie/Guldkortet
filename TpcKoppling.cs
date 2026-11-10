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
            private bool running = true; // När running är true fortsätter servern läsa data


            // Konstruktor för TpcKoppling
            public TpcKoppling(Form1 form1)
            {
                this.form = form1; // Form1-objekt sparas i variabel
            }

            // Startar kopplingen till servern
            public void StartaKoppling()
            {
                // Skapar lyssnare på port 12345, IP: 127.0.0.1
                lyssnare = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
                // Startar lyssnare
                lyssnare.Start();

                // Väntar till NOS_Export ansluter och körs i bakgrunden så att UI inte låser sig
                Task.Run(async () =>
                {
                    try
                    {
                        // Väntar asynkront på att NOS_Export ansluter
                        klient = await lyssnare.AcceptTcpClientAsync();

                        // TCP kör i bakgrunden och får inte röra UI direkt, annars kraschar programmet
                        // Invoke låter UI‑tråden göra jobbet och gör det trådsäkert
                        // Trådsäkert (thread-safe) betyder att din kod kan köras av flera trådar samtidigt
                        form.Invoke(new Action(() =>
                        {
                            form.UppdateraStatus("Ansluten till NOS-Export"); // Ändrar status i UI när anslutning lyckas
                        }));
                    }
                    // Felhantering vid anslutning
                    catch
                    {
                        return;
                    }

                    // Startar läsningen från NOS_Export för inkommande data
                    StartaLasning(klient);
                });
            }

            // Läser inkommande data från NOS_Export
            private async void StartaLasning(TcpClient klient)
            {
                // Hämta NetworkStream - strömmen där data skickas
                NetworkStream stream = klient.GetStream();

                // Bufferten deklareras utanför loopen för att återanvändas
                byte[] buffer = new byte[1024];

                // Loop för att läsa TCP-data så länge running är true och klienten är ansluten
                while (running && klient.Connected)
                {
                    int lastaBytes;

                    // Om något går fel, fånga felet och stoppa kraschen
                    try
                    {
                    // Läser datan asynkront 
                        lastaBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                    }
                    catch
                    {
                        break; // Avslutar loopen och går till stängning vid nätverksfel
                    }

                    // Klienten stängs om inga bytes läses in 
                    if (lastaBytes <= 0)
                    {
                        break; // Avbryter loopen 
                    }

                    // Konverterar bytes till exakt Unicode och trimmar bort mellanslag
                    string kod = Encoding.Unicode.GetString(buffer, 0, lastaBytes).Trim();

                    // Om strängen blev tom efter rensning hoppar vi till nästa läsning
                    if (string.IsNullOrEmpty(kod))
                    {
                        continue;
                    }

                    // Tom sträng, resultat fylls i genom invoke 
                    string resultat = "";

                    // TCP kör i bakgrunden och får inte röra UI direkt, annars kraschar programmet
                    // Invoke låter UI‑tråden göra jobbet och gör det trådsäkert
                    form.Invoke(new Action(() =>
                    {
                        // Skickar koden till Form1 för ProcceseraKod
                        resultat = form.HanteraKod(kod);
                    }));

                    // Om Form1 returnerar ett svar 
                    if (!string.IsNullOrEmpty(resultat))
                    {
                        // Svaret kodas till råa Unicode-bytes utan extra radbrytningar
                        byte[] data = Encoding.Unicode.GetBytes(resultat.Trim());

                        try
                        {
                            // Svaret skickas tillbaka till NOS_Export asynkront för att trigga deras MessageBox
                            await stream.WriteAsync(data, 0, data.Length);
                        }
                        catch
                        {
                            break; // Avbryt om det inte går att skriva tillbaka till strömmen
                        }
                    }
                }

                // När loopen bryts stängs anslutningen på ett säkert sätt
                try
                {
                    klient.Close();
                }
                catch
                {
                    // Ignorerar fel vid stängning
                }

                // Uppdaterar gränssnittet om frånkopplingen
                if (form != null && !form.IsDisposed)
                {

                // TCP kör i bakgrunden och får inte röra UI direkt, annars kraschar programmet
                // Invoke låter UI‑tråden göra jobbet och gör det trådsäkert
                // Trådsäkerhet = flera trådar får inte ändra samma sak samtidigt, resulterar i att proigrammett kraschar/konstigt betteende 
                form.Invoke(new Action(() =>
                    {
                        form.UppdateraStatus("Frånkopplad från NOS-Export");
                    }));
                }
            }

            // Stoppar servern och rensar resurser
            public void StoppaKoppling()
            {
                // Stoppa huvudloopen i läsningen
                running = false;

                // Om något går fel, fånga felet och stoppa kraschen - Try catch
                try
                {
                    // Stänger TCP-lyssnaren ordentligt
                    if (lyssnare != null)
                    {
                        lyssnare.Stop();
                    }
                }
                catch
                {
                    // Ignorerar fel vid stopp
                }
            }
        }
    }