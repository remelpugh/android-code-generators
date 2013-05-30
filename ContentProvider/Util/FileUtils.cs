namespace Dabay6.Android.ContentProvider.Util {
    #region USINGS

    using System;
    using System.Collections.Generic;
    using System.IO;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public static class FileUtils {

        /// <summary>
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="contents"></param>
        public static void SaveFile(List<string> paths, List<string> contents) {
            if (paths.Count != contents.Count) {
                throw new ArgumentException("Path count should match content count.");
            }

            var index = 0;

            foreach (var path in paths) {
                var directory = new DirectoryInfo(path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal)));

                if (!directory.Exists) {
                    directory.Create();
                }
                using (var stream = File.CreateText(path)) {
                    stream.Write(contents[index]);
                }

                index += 1;
            }
        }
    }
}