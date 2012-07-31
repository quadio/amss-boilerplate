namespace Amss.Boilerplate.Common.Utils
{
    using System.Globalization;

    public static class StringUtils
    {
        public static string Fmt(this string format, params object[] args)
        {
            return string.IsNullOrEmpty(format) ? string.Empty : string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}