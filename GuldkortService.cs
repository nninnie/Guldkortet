using Guldkortet2.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guldkortet2
{
    public class GuldkortService
    {
        // Lista med alla loggposter under programmets körning
        private List<Loggpost> loggar = new List<Loggpost>();

        // ConnectionString som pekar på SQL-databasen Kundregister.mdf
        private readonly string kundConnectionString =
    @"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=|DataDirectory|\Kundregister.mdf;
      Integrated Security=True;";

        // ConnectionString som pekar på SQL-databasen Kortregister.mdf
        private readonly string kortConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=|DataDirectory|\Kortregister.mdf;
      Integrated Security=True;";

        // Kollar om kund finns i databasen SQL
        private bool KollaKund(string anvandarNr)
        {
            // Skapar anslutningen med connectionstring
            using (var connection = new SqlConnection(kundConnectionString))
            {
                connection.Open(); // öppnar anslutningen
                
                // SQL-fråga som hämtar kundens data
                string sql = "SELECT AnvändarNr, Namn, Kommun FROM Kunder WHERE AnvändarNr = @AnvändarNr";

                // Skapar nytt SQL kommando
                using (var cmd = new SqlCommand(sql, connection))
                {
                    // Skickar in kundnumret som parameter
                    cmd.Parameters.AddWithValue("@AnvändarNr", anvandarNr);

                    // Kör SELECT och får tillbaka en reader
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Om reader hittar en rad så finns kunden
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }

            // Om ingen rad hittades så finns inte kunden 
            return false;
        }

        // Kollar om kort finns i databasen SQL
        private bool KollaKort(string kortNr)
        {
            // Skapar anslutning med connectionstring
            using (var connection = new SqlConnection(kortConnectionString))
            {
                connection.Open(); // Öppnar anslutning

                // SQL-fråga som hämtar kortets data
                string sql = "SELECT KortNr, KortTyp FROM Kort WHERE KortNr = @KortNr";

                // Skapar nytt SQL kommando
                using (var cmd = new SqlCommand(sql, connection))
                {
                    // Skickar in kortnumret som parameter
                    cmd.Parameters.AddWithValue("@KortNr", kortNr);

                    // Kör SELECT och får tillbaka en reader
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Om reader hittar en rad så finns kortet 
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }

            // Om ingen rad hittades så finns inte kortet 
            return false;
        }

        // Hämtar korttyp fån SQL Databasen
        private string HamtaKortTyp(string kortNr)
        {
            // Skapar anslutning med connectionstring
            using (var conn = new SqlConnection(kortConnectionString))
            {
                conn.Open(); // Öppnar anslutning

                // SQL-fråga som hämtar kortets data
                string sql = "SELECT KortTyp FROM Kort WHERE KortNr = @KortNr";

                // Skapar nytt SQL kommando
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@KortNr", kortNr); // lägger till parameter @kortNr

                    // Kör SQL-frågan
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Returnerar tru om en rad hittas
                        if (reader.Read())
                        {
                            return reader["KortTyp"].ToString(); // Returnerar KortTyp
                        }
                    }
                }
            }

            return null; // Om inget hittas returnerar null
        }



        // Lägg till en loggpost
        public void LaggTillLogg(Loggpost logg)
        {
            loggar.Add(logg);
        }
        // Returnerar alla loggposter som sparats i den generiska listan
        // Form1 hämta alla loggar och skriver ut till textfilen när programmet avslutas eller spara knappen
        public List<Loggpost> HamtaLoggar()
        {
            return loggar;
        }

        // Delar upp koden, kontrollerar att formatet är korrekt, validerar kund/kort, skapar belöning och loggar resultatet
        public string ProcessaKod(string mottagenKod)
        {
            try
            {
                // Validering: Tom kod eller mellanslag
                if (string.IsNullOrWhiteSpace(mottagenKod))
                return "Fel: Koden är tom.";

            // Validering: Saknar bindestreck
            if (!mottagenKod.Contains("-"))
                return "Fel: Koden saknar bindestreck.";

            // Delar upp koden i två delar
            // Validering:Två delad kod
            var delar = mottagenKod.Split('-');
            if (delar.Length != 2)
                return "Fel: Koden måste ha två delar.";

            // Delar koden i 2 delar somm mottaget av NOS_Exxport för att kolla mot databasen
            string anvandarNr = delar[0];
            string kortNr = delar[1];


                // Kollar om kund och kort finns i databasen
                bool kundFinns = KollaKund(anvandarNr);
                bool kortFinns = KollaKort(kortNr);

                // Om kund inte finns
                if (!kundFinns)
                    throw new Exception($"Kunden {anvandarNr} finns inte.");

                // Om kort inte finns
                if (!kortFinns)
                    throw new Exception($"Kortet {kortNr} finns inte.");

                // Dubbelregistrering
                if (loggar.Any(l => l.Kod == mottagenKod))
                    throw new Exception($"Kortet {kortNr} är redan registrerat.");

                // Belöning via arv
                Beloning beloning;

                // KortNr som avgör vilken typ av belöning 
                // Hämtar belöniings beskrivning av subklassernas ToString()
                // Default nekad belöning
                string kortTyp = HamtaKortTyp(kortNr)?.Trim();

                switch (kortTyp)
                {
                    case "Dunderkatt":
                        beloning = new DunderkattBeloning();
                        break;

                    case "Kristallhäst":
                        beloning = new KristallhastBeloning();
                        break;

                    case "Överpanda":
                        beloning = new OverpandaBeloning();
                        break;

                    case "Eldtomat":
                        beloning = new EldtomatBeloning();
                        break;

                    default:
                        beloning = new Beloning { Beskrivning = "⛔ Nekad belöning." };
                        break;
                }


                // Logga godkänd kod
                loggar.Add(new Loggpost
                {
                    Tidstampel = DateTime.Now,
                    Kod = mottagenKod,
                    BeloningsResultat = beloning.Beskrivning
                });

                // Skriver ut belonings beskrivning ToString() till textfil
                return beloning.ToString();
            }
            catch (Exception ex)
            {
                // Logga nekad kod
                loggar.Add(new Loggpost
                {
                    Tidstampel = DateTime.Now,
                    Kod = mottagenKod,
                    BeloningsResultat = ex.Message
                });

                // Returnerar nekad belöning textfil
                return ex.Message;
            }
        }
    }
}