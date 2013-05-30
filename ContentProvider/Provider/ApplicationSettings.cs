namespace Dabay6.Android.ContentProvider.Provider {
    #region USINGS

    using System.Collections.Generic;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class ApplicationSettings {

        /// <summary>
        /// </summary>
        public bool GenereateDeviceId {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string InputFile {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string JsonDirectory {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public List<string> Login {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string OutputDirectory {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string ServerNames {
            get;
            set;
        }
    }
}