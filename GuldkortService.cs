using Guldkortet2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2
{
    public class GuldkortService
    {
        // Lista med alla loggposter under programmets körning
        private List<Loggpost> loggar = new List<Loggpost>();

        // Lägg till en loggpost
        public void LaggTillLogg(Loggpost logg)
        {
            loggar.Add(logg);
        }

        // Hämta alla loggar
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

            // Validering:Saknar bindestreck
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
                bool kundFinns = true;
                bool kortFinns = true;

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
                switch (kortNr)
                {
                    case "1": beloning = new DunderkattBeloning(); break;
                    case "2": beloning = new KristallhastBeloning(); break;
                    case "3": beloning = new OverpandaBeloning(); break;
                    case "4": beloning = new EldtomatBeloning(); break;
                    default: beloning = new Beloning { Beskrivning = "⛔ Nekad belöning." }; break;
                }


                // Logga godkänd kod
                loggar.Add(new Loggpost
                {
                    Tidstampel = DateTime.Now,
                    Kod = mottagenKod,
                    BeloningsResultat = beloning.Beskrivning
                });

                // Skriver ut belonings beskrivning till textfil
                return beloning.Beskrivning;
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
