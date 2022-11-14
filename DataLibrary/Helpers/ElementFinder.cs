namespace DataLibrary.Helpers
{
    public class ElementFinder
    {
        public static bool? DeterminePropertyRedemptionStatus(string stringToMatch, string value)
        {
            if (stringToMatch is null) return null;

            if (stringToMatch.Contains(value)) { return true; }

            return false;
        }
    }
}
