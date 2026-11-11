namespace Guldkortet.Model
{

    // OverpandaBeloning ärver av Beloning
    public class OverpandaBeloning : Beloning
    {
        public OverpandaBeloning()
        {
            Beloningstyp = "Överpanda"; // Typ av belöning
            Beskrivning = "🐼 Överpanda!"; // Skickas till NOS_Export
        }

        // ToString-metod som skriver ut belöningens beskrivning till UI
        public override string ToString()
        {
            return $"Du har fått belöningen: {Beskrivning}";
        }
    }
}
