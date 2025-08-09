
namespace ElementalWords
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ElementalWords.ElementalForms("");
            //ElementalWords.ElementalForms("H");
            //ElementalWords.ElementalForms("Be");
            //ElementalWords.ElementalForms("be");
            //ElementalWords.ElementalForms("");
            ElementalWords.ElementalForms("Snack");
        }
    }

    public static class PreLoaded
    {
        public static Dictionary<string, string> ELEMENTS = new()
        {
            { "Na", "Sodium" },
            { "S", "Sulpher" },
            { "Sn", "Tin"},
            { "Ac", "Actinum"},
            { "K", "Potassium"},
            { "C", "Carbon"},
            { "N", "Nitrogen"},
            { "Be", "Beryllium" },
            { "Ti", "Titanium" },
            { "H", "Hydrogen" },
            { "Y", "Yttrium"},
            { "Es", "Einsteinium"},
            { "Pb", "Lead"},
            { "P", "Phosphorus"},
            { "B", "Boron"}
        };
    }
}