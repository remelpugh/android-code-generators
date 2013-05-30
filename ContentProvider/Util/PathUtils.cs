namespace Dabay6.Android.ContentProvider.Util {
    /// <summary>
    /// </summary>
    public static class PathUtils {
        /// <summary>
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="classPackage"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FilePath(string filename, string classPackage, string path) {
            return filename + "\\" + classPackage.Replace(".", "\\") + "\\" + path.Replace(".", "\\") + "\\";
        }
    }
}