using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2.Model
{
    //  Basklass för alla belöningarna
    public class Beloning
    {
        public string Beloningstyp {  get; set; } // Vilken typ av belöning
        public string Beskrivning { get; set;  } // Beskrivningen skickas till NOS_Export

        // ToString metod somm skrivs ut till UI
        public override string ToString()
        {
            return Beskrivning;
        }
    }
}
