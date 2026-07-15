using System.Globalization;
using System.Text.RegularExpressions;

namespace ClashN.Tool
{
    internal static partial class CountryFlagHelper
    {
        private static readonly HashSet<string> AdditionalCountryCodes = new(StringComparer.OrdinalIgnoreCase)
        {
            "AC", "CP", "DG", "EA", "EU", "IC", "TA", "UN", "XK"
        };

        private static readonly (string Code, string[] Names)[] CountryNames =
        {
            ("HK", new[] { "\u9999\u6e2f", "Hong Kong" }),
            ("TW", new[] { "\u53f0\u6e7e", "\u53f0\u7063", "Taiwan" }),
            ("MO", new[] { "\u6fb3\u95e8", "\u6fb3\u9580", "Macau", "Macao" }),
            ("JP", new[] { "\u65e5\u672c", "Japan" }),
            ("SG", new[] { "\u65b0\u52a0\u5761", "Singapore" }),
            ("US", new[] { "\u7f8e\u56fd", "\u7f8e\u570b", "United States", "USA" }),
            ("KR", new[] { "\u97e9\u56fd", "\u97d3\u570b", "South Korea", "Korea" }),
            ("GB", new[] { "\u82f1\u56fd", "\u82f1\u570b", "United Kingdom", "Britain" }),
            ("DE", new[] { "\u5fb7\u56fd", "\u5fb7\u570b", "Germany" }),
            ("FR", new[] { "\u6cd5\u56fd", "\u6cd5\u570b", "France" }),
            ("CA", new[] { "\u52a0\u62ff\u5927", "Canada" }),
            ("AU", new[] { "\u6fb3\u5927\u5229\u4e9a", "\u6fb3\u6d32", "Australia" }),
            ("RU", new[] { "\u4fc4\u7f57\u65af", "\u4fc4\u7f85\u65af", "Russia" }),
            ("IN", new[] { "\u5370\u5ea6", "India" }),
            ("NL", new[] { "\u8377\u5170", "\u8377\u862d", "Netherlands" }),
            ("TR", new[] { "\u571f\u8033\u5176", "Turkey", "Turkiye" }),
            ("PH", new[] { "\u83f2\u5f8b\u5bbe", "\u83f2\u5f8b\u8cd3", "Philippines" }),
            ("TH", new[] { "\u6cf0\u56fd", "\u6cf0\u570b", "Thailand" }),
            ("MY", new[] { "\u9a6c\u6765\u897f\u4e9a", "\u99ac\u4f86\u897f\u4e9e", "Malaysia" }),
            ("ID", new[] { "\u5370\u5ea6\u5c3c\u897f\u4e9a", "\u5370\u5c3c", "Indonesia" }),
            ("VN", new[] { "\u8d8a\u5357", "Vietnam" }),
            ("CN", new[] { "\u4e2d\u56fd", "\u4e2d\u570b", "China" })
        };

        public static string GetCountryCode(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            foreach (var country in CountryNames)
            {
                if (country.Names.Any(value => name.Contains(value, StringComparison.OrdinalIgnoreCase)))
                {
                    return country.Code;
                }
            }

            var match = MatchCountryCodePrefix(name);
            if (!match.Success)
            {
                return string.Empty;
            }

            var code = NormalizeCountryCode(match.Groups["code"].Value);
            return IsCountryCode(code) ? code : string.Empty;
        }

        public static string GetDisplayName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            var match = MatchCountryCodePrefix(name);
            if (!match.Success || !IsCountryCode(NormalizeCountryCode(match.Groups["code"].Value)))
            {
                return name;
            }

            return match.Groups["name"].Value.TrimStart();
        }

        public static string GetAssetName(string countryCode)
        {
            return string.Join('-', countryCode.ToUpperInvariant().Select(character => $"{0x1F1E6 + character - 'A':x}")) + ".png";
        }

        private static Match MatchCountryCodePrefix(string name)
        {
            var exactMatch = ExactCountryCodeRegex().Match(name);
            return exactMatch.Success ? exactMatch : CountryCodePrefixRegex().Match(name);
        }

        private static string NormalizeCountryCode(string countryCode)
        {
            var code = countryCode.ToUpperInvariant();
            return code == "UK" ? "GB" : code;
        }

        private static bool IsCountryCode(string countryCode)
        {
            if (AdditionalCountryCodes.Contains(countryCode))
            {
                return true;
            }

            try
            {
                _ = new RegionInfo(countryCode);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        [GeneratedRegex(@"^\s*(?:\[|\()?(?<code>[A-Za-z]{2})(?:\]|\))?\s*$")]
        private static partial Regex ExactCountryCodeRegex();

        [GeneratedRegex(@"^\s*(?:\[|\()?(?<code>[A-Za-z]{2})(?:\]|\))?(?:(?:\s*[-_|:\u00b7]\s*)|\s+)(?<name>.+)$")]
        private static partial Regex CountryCodePrefixRegex();
    }
}
