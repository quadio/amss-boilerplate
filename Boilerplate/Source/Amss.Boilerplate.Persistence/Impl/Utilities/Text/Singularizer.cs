namespace Amss.Boilerplate.Persistence.Impl.Utilities.Text
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal static class Singularizer
    {
        #region Fields

        private static readonly IDictionary<string, string> Singularizations =
            new Dictionary<string, string>
                {
                    // Start with the rarest cases, and move to the most common
                    { "people", "person" },
                    { "oxen", "ox" },
                    { "children", "child" },
                    { "feet", "foot" },
                    { "teeth", "tooth" },
                    { "geese", "goose" },
                    /*And now the more standard rules.*/
                    { "(.*)ives?", "$1ife" },
                    { "(.*)ves?", "$1f" },
                    /*ie, wolf, wife*/
                    { "(.*)men$", "$1man" },
                    { "(.+[aeiou])ys$", "$1y" },
                    { "(.+[^aeiou])ies$", "$1y" },
                    { "(.+)zes$", "$1" },
                    { "([m|l])ice$", "$1ouse" },
                    { "matrices", @"matrix" },
                    { "indices", @"index" },
                    { "(.*)ices", @"$1ex" },
                    /*ie, Matrix, Index*/
                    { "(octop|vir)i$", "$1us" },
                    { "(.+(s|x|sh|ch))es$", @"$1" },
                    { "(.+)s$", @"$1" }
                };

        private static readonly IList<string> Unpluralizables =
            new List<string>
                {
                    "equipment",
                    "universe",
                    "information",
                    "rice",
                    "money",
                    "species",
                    "series",
                    "fish",
                    "sheep",
                    "deer"
                };

        #endregion

        #region Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "as designed")]
        public static string Singularize(string word)
        {
            if (Unpluralizables.Contains(word.ToLowerInvariant()))
            {
                return word;
            }

            foreach (var singularization in Singularizations)
            {
                if (Regex.IsMatch(word, singularization.Key))
                {
                    return Regex.Replace(word, singularization.Key, singularization.Value);
                }
            }

            return word;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "as designed")]
        public static bool IsPlural(string word)
        {
            if (Unpluralizables.Contains(word.ToLowerInvariant()))
            {
                return true;
            }

            return Singularizations.Any(singularization => Regex.IsMatch(word, singularization.Key));
        }

        #endregion
    }
}