using System;
using System.Collections.Generic;
using System.Linq;

namespace api.Helpers
{
    public static class SearchHelper
    {
        public static string ExtractSearchKeyword(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName)) return companyName;

            var blacklist = new[] { "Inc", "Inc.", "Ltd", "Ltd.", "Corp", "Corp.", "LLC", "Co", "Co.", "PLC", "S.A.", "Class", "A", "B" };

            var words = companyName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                   .Where(word => !blacklist.Contains(word, StringComparer.OrdinalIgnoreCase))
                                   .ToList();

            if (words.Count == 0) return companyName;

            var keyword = companyName.Split(' ')[0];
            return keyword.Trim(',', '.', ';', ' ', '-'); // 这里确保不返回 "Tesla,"
        }
    }
}