using System;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string value) => Convert.ToInt32(value);
        public static decimal ToDecimal(this string value) => decimal.TryParse(value, out var _vlrTotal) ? _vlrTotal : 0;
        public static DateTime ToDateTime(this string value) => Convert.ToDateTime(value);
        public static bool IsNullOrEmpty(this string value) => String.IsNullOrEmpty(value);
    }
}
