namespace Rollout.Common
{
    public static class RegexHelper
    {
        public static string MatchAnything = @"[\s\S]*";
        public static string MatchNonZeroInteger = @"^[1-9][0-9]*$";
        public static string MatchInteger = @"^[0-9]+$";
        public static string MatchUSADate = @"^(([1]{1}[0-2]{1})|([1-9]{1}))[/.-]{1}(([1-3]{1}[0-9]{1})|([1-9]{1}))[/.-]{1}2[0-9][0-9][[0-9]$"; // Valid from 2000 to 2999
        public static string MatchNonBlank = @"^(?!\s*$).+";
        public static string MatchIntegerOrBlank = @"^(\s*|[0-9]+)$";
        public static string MatchZipCodeUSA = @"^[0-9]{5}$";
        public static string MatchZipCodes = @"^(([0-9]{5})|((([A-Z]|[a-z])[0-9]([A-Z]|[a-z])) ([0-9]([A-Z]|[a-z])[0-9])))$";
        public static string MatchDecimalNumber = @"^[0-9]*[\.]?[0-9]*$";
    }
}
