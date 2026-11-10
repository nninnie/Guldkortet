namespace Guldkortet.Model
{
    // EldtomatBeloning ärver av Beloning
    public class EldtomatBeloning : Beloning
    {
        public EldtomatBeloning()
        {
            Beloningstyp = "Eldtomat"; // Typ av belöning
            Beskrivning = "🔥 Eldtomat!"; // Skickas till NOS_Export
        }

        // ToString-metod som skriver ut belöningens beskrivning till UI
        public override string ToString()
        {
            return $"Du har fått belöningen: {Beskrivning}";
        }
    }
}
