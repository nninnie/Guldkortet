using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet2.Model
{
    // Klass för ett kort i databasen
    public class Kort
    {
        public string KortNr { get; set; } // Kort nummer
        public string KortTyp { get; set; } /// Kort typ
    }
}
