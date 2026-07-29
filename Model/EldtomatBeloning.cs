using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2.Model
{
    public class EldtomatBeloning : Beloning
    {
        public EldtomatBeloning()
        {
            Beloningstyp = "Eldtomat"; // Typ av belöning
            Beskrivning = "🔥 Eldtomat!"; // Skickas till NOS_Export
        }

        // Tostring mmetod som skriver utt belönings beskrivningen till UI
        public override string ToString()
        {
            return $"Du har fått belöningen: {Beskrivning}";
        }
    }
}
