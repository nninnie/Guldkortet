using System;
using System.Collections.Generic;
using System.Linq;
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

        // Startar kopplingen till servern
        public void StartaKoppinig()
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
                }
                // Felhantering 
                catch
                {
                    return;
                }
                // Startar läsningen från NOS_Export, inkomande data
                StartaLäsning(klient);
            });
        }

        // Läser inkommande data från NOS_Export
        private async void StartaLäsning(TcpClient k)
        {
            try
            {
                // Hämta NetworkStream  - strömen där data skcikas
                NetworkStream stream = k.GetStream();

                byte[] buffer = new byte[1024]; // Skapa buffer för inkomande bytes
                int n = await stream.ReadAsync(buffer, 0, buffer.Length); // läser datan asykront 

                // Klienten stängs om inga byte läses in
                if (n <= 0)
                    return;

                // Konverterar bytes till Unicode
                string kod = Encoding.Unicode.GetString(buffer, 0, n);

                string svar = kod; // Svarar och skriver ut till NOS_Export MessageBox
                byte[] data = Encoding.Unicode.GetBytes(svar); // Svaret kodas som Unicode
                await stream.WriteAsync(data, 0, data.Length); // Svaret skickas tillbaka til Nos_xport
            }
            // Felahantering vid error 
            catch 
            {
                return;
            }

            // Rekursiv loop - metoden anropar sig själv, som en while loop
            // Varje läsning är asynkron och metoden anropar sig själv för att fortsätta lyssna
            StartaLäsning(k);
        }

        // Stoppar servern 
        public void StoppaKoppling()
        {
            klient?.Close(); // Stänger klienten
            lyssnare?.Stop(); // Stänger lyssnare
        }
    }
} 