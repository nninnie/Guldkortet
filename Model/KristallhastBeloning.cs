namespace Guldkortet.Model
{
    // KristallhastBeloning ärver av Beloning
    public class KristallhastBeloning : Beloning
    {
        public KristallhastBeloning()
        {
            Beloningstyp = "Kristallhäst"; // Typ av belöning
            Beskrivning = "✨ Kristallhäst!"; // Skickas till NOS_Export
        }

        // ToString-metod som skriver ut belöningens beskrivning till UI
        public override string ToString()
        {
            return $"Du har fått belöningen: {Beskrivning}";
        }
    }
}
