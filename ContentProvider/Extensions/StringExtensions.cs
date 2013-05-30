namespace Dabay6.Android.ContentProvider.Extensions {
    #region USINGS

    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    /// </summary>
    public static class StringExtensions {
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CreateBooleanMemberName(this string s) {
            var temp = s.ToLower();

            if (temp.StartsWith("is")) {
                return s;
            }

            return "is" + s.CreateProperName();
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CreateConstantName(this string s) {
            return Regex.Replace(s, "([a-z])([A-Z])", "$1_$2").ToUpper();
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CreateLowerCamelCaseName(this string s) {
            return s.Substring(0, 1).ToLower() + s.Substring(1);
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CreateNameFromConstantName(this string s) {
            var builder = new StringBuilder();
            var parts = s.Split('_');

            foreach (var part in parts) {
                builder.Append(part.CreateProperName());
            }

            return builder.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CreateProperName(this string s) {
            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string s) {
            return string.IsNullOrWhiteSpace(s);
        }
    }
}