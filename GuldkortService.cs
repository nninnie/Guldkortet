using Guldkortet.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Guldkortet
{
    // Affärslogiken: validerar koder mot databaserna, delar ut belöningar och loggar resultatet
    public class GuldkortService
    {
        // Lista med alla loggposter under programmets körning
        private readonly List<Loggpost> loggar = new List<Loggpost>();

        // Koder som redan har fått en belöning (används för att stoppa dubbelregistrering).
        // HashSet ger snabb sökning och kan inte innehålla dubbletter.
        private readonly HashSet<string> registreradeKoder = new HashSet<string>();

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

        // Kollar om kunden finns i databasen
        private bool KollaKund(string anvandarNr)
        {
            using (var connection = new SqlConnection(kundConnectionString))
            {
                connection.Open();

                // Parametriserad fråga (@AnvändarNr) skyddar mot SQL injection
                string sql = "SELECT AnvändarNr FROM Kunder WHERE AnvändarNr = @AnvändarNr";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@AnvändarNr", anvandarNr);

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Om reader hittar en rad så finns kunden
                        return reader.Read();
                    }
                }
            }
        }

        // Hämtar kortets typ från databasen, eller null om kortet inte finns
        private string HamtaKortTyp(string kortNr)
        {
            using (var connection = new SqlConnection(kortConnectionString))
            {
                connection.Open();

                string sql = "SELECT KortTyp FROM Kort WHERE KortNr = @KortNr";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@KortNr", kortNr);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["KortTyp"].ToString();
                        }
                    }
                }
            }

            return null; // Kortet finns inte
        }

        // Lägg till en loggpost
        public void LaggTillLogg(Loggpost logg)
        {
            loggar.Add(logg);
        }

        // Returnerar alla loggposter som sparats i listan.
        // IReadOnlyList gör att Form1 kan läsa loggarna men inte ändra i listan.
        public IReadOnlyList<Loggpost> HamtaLoggar()
        {
            return loggar;
        }

        // Delar upp koden, kontrollerar formatet, validerar kund/kort, skapar belöning och loggar resultatet
        public string ProcessaKod(string mottagenKod)
        {
            // Validering: tom kod eller bara mellanslag
            if (string.IsNullOrWhiteSpace(mottagenKod))
                return "Fel: Koden är tom.";

            // Validering: saknar bindestreck
            if (!mottagenKod.Contains("-"))
                return "Fel: Koden saknar bindestreck.";

            // Validering: koden måste bestå av exakt två delar (AnvändarNr-KortNr)
            var delar = mottagenKod.Split('-');
            if (delar.Length != 2)
                return "Fel: Koden måste ha två delar.";

            string anvandarNr = delar[0];
            string kortNr = delar[1];

            try
            {
                // Kollar om kund och kort finns i databasen
                if (!KollaKund(anvandarNr))
                    throw new Exception($"Kunden {anvandarNr} finns inte.");

                string kortTyp = HamtaKortTyp(kortNr)?.Trim();
                if (kortTyp == null)
                    throw new Exception($"Kortet {kortNr} finns inte.");

                // Dubbelregistrering: bara koder som redan har fått en belöning räknas
                if (registreradeKoder.Contains(mottagenKod))
                    throw new Exception($"Kortet {kortNr} är redan registrerat.");

                // Belöning via arv: korttypen avgör vilken subklass som skapas
                Beloning beloning;

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

                registreradeKoder.Add(mottagenKod);

                // Loggar godkänd kod
                loggar.Add(new Loggpost
                {
                    Tidstampel = DateTime.Now,
                    Kod = mottagenKod,
                    BeloningsResultat = beloning.Beskrivning
                });

                // Subklassens ToString() ger texten som visas och skickas till NOS_Export (polymorfism)
                return beloning.ToString();
            }
            catch (Exception ex)
            {
                // Loggar nekad kod
                loggar.Add(new Loggpost
                {
                    Tidstampel = DateTime.Now,
                    Kod = mottagenKod,
                    BeloningsResultat = ex.Message
                });

                return ex.Message;
            }
        }
    }
}
