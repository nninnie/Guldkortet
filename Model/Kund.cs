using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2.Model
{
    // Klass för en kund i databasen
    public class Kund
    {
        //  Properties
        public string AnvändarNr { get; set; } // Användar nummer för kund
        public string Namn { get; set; } // Namn på kunden
        public string Kommun { get; set; } // Kundens Kommun
    }
}
