namespace Dabay6.Android.ContentProvider.Schema {
    #region USINGS

    using Newtonsoft.Json;
    using System.ComponentModel;
    using Util;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class Database {
        public const string DefaultProviderFolder = "data.provider";

        /// <summary>
        /// </summary>
        public Database() {
            ProviderFolder = DefaultProviderFolder;
            Version = 1;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "authority_package")]
        public string AuthorityPackage {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "classes_prefix")]
        public string ClassesPrefix {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "content_classes_prefix")]
        public string ContentClassesPrefix {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "generate_dto")]
        public bool HasDTO {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(Order = 1)]
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "package")]
        public string PackageName {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [DefaultValue("data.provider")]
        [JsonProperty(PropertyName = "provider_folder")]
        public string ProviderFolder {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public int Version {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GeneratePath(Database database, string fileName) {
            var path = PathUtils.FilePath(fileName, database.PackageName, database.ProviderFolder);

            return path + database.ClassesPrefix;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("Name: {0}, , Version: {1}", Name, Version);
        }
    }
}