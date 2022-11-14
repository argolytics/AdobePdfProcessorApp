namespace DataLibrary.Helpers
{
    public class StringTrimmer
    {
        public static string? Trimmer(string stringToTrim)
        {
            if (stringToTrim is null) return null;
            var result = stringToTrim.Replace(" ", "");
            return result;
        }
    }
}
