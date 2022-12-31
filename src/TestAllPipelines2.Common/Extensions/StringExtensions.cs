using System.Linq;

namespace TestAllPipelines2.Common.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string str) =>
            string.IsNullOrWhiteSpace(str)
                ? str
                : str.First().ToString().ToUpper() + str[1..];
    }
}