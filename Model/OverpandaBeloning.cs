using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2.Model
{

    // OverpandaBeloning ärver av beloning
    public class OverpandaBeloning : Beloning
    {
        public OverpandaBeloning()
        {
            Beloningstyp = "Överpanda"; // Typ av belöning
            Beskrivning = "🐼 Överpanda!"; // Skickas till NOS_Export
        }

        // Tostring mmetod som skriver utt belönings beskrivningen till UI
        public override string ToString()
        {
            return $"Du har fått belöningen: {Beskrivning}";     
        }
    }
}
