using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AppleShop.Helpers
{
    /// <summary>
    /// Tạo slug an toàn cho URL từ chuỗi tiếng Việt:
    /// bỏ dấu, thay khoảng trắng bằng "-", loại ký tự đặc biệt.
    /// </summary>
    public static class SlugHelper
    {
        public static string ToSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var slug = RemoveDiacritics(input.Trim().ToLowerInvariant());
            slug = slug.Replace('đ', 'd');
            slug = Regex.Replace(slug, @"\s+", "-");      // khoảng trắng → "-"
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", ""); // loại ký tự đặc biệt
            slug = Regex.Replace(slug, @"-{2,}", "-").Trim('-');
            return slug;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }
            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
