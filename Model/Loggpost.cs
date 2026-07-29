using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2.Model
{
    public class Loggpost
    {
        public DateTime Tidstampel {  get; set; } // Tidstämpel
        public string Kod {  get; set; } // Kod = AnvändarNr-KortNr 
        public string BeloningsResultat { get; set; } // Belönings resulttat

        // Skriver ut loggposten som en rad i textfilen
        public override string ToString()
        {
            return $"{Tidstampel:yyyy-MM-dd HH:mm:ss} | Kod: {Kod} | Resultat: {BeloningsResultat}";
        }
    }
}
