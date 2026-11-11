namespace Guldkortet.Model
{
    // DunderkattBeloning ärver av Beloning
    public class DunderkattBeloning : Beloning
    {
        public DunderkattBeloning()
        {
            Beloningstyp = "Dunderkatt"; // Typ av belöning
            Beskrivning = "🎉 Dunderkatt!"; // Skickas till NOS_Export
        }

        // ToString-metod som skriver ut belöningens beskrivning till UI
        public override string ToString()
        {
            return $"Du har fått belöningen: {Beskrivning}";
        }
    }
}
