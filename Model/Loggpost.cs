using System;

namespace Guldkortet.Model
{
    public class Loggpost
    {
        public DateTime Tidstampel { get; set; } // Tidstämpel
        public string Kod { get; set; } // Kod = AnvändarNr-KortNr
        public string BeloningsResultat { get; set; } // Belöningens resultat

        // Skriver ut loggposten som en rad i textfilen
        public override string ToString()
        {
            return $"{Tidstampel:yyyy-MM-dd HH:mm:ss} | Kod: {Kod} | Resultat: {BeloningsResultat}";
        }
    }
}
