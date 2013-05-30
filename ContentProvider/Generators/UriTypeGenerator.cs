﻿namespace Dabay6.Android.ContentProvider.Generators {
    #region USINGS

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Properties;
    using Schema;
    using Util;

    #endregion

    public class UriTypeGenerator: IGenerator {
        /// <summary>
        /// </summary>
        /// <param name="schema"></param>
        public UriTypeGenerator(SchemaDescription schema) {
            Schema = schema;
        }

        #region IGenerator Members

        /// <summary>
        /// </summary>
        public SchemaDescription Schema {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public Task<GenerationResult> Generate(string path, IProgress<ProgressResult> progress) {
            return Task.Run(() => {
                var db = Schema.Database;
                var content = Resources.uri_type;
                var output = PathUtils.FilePath(path, db.PackageName, db.ProviderFolder + Constants.Util);

                content = string.Format(content, db.PackageName, db.ProviderFolder + Constants.Util);

                if (progress != null) {
                    progress.Report(new ProgressResult {
                        Name = this.GetType().Name,
                        Value = 1
                    });
                }

                return new GenerationResult{
                    Content = new List<string>{
                        content
                    },
                    Path = new List<string>{
                        output + "UriType.java"
                    }
                };
            });
        }

        #endregion
    }
}