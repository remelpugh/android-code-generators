namespace Dabay6.Android.ContentProvider.Generators {
    #region USINGS

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Properties;
    using Schema;
    using Util;

    #endregion

    public class DatabaseHelperGenerator: IGenerator {
        /// <summary>
        /// </summary>
        /// <param name="schema"></param>
        public DatabaseHelperGenerator(SchemaDescription schema) {
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
                var content = Resources.database_helper;
                var create = new StringBuilder();
                var db = Schema.Database;
                var imports = new StringBuilder();
                var output = PathUtils.FilePath(path, db.PackageName, db.ProviderFolder);
                var tables = Schema.Tables;
                var upgrade = new StringBuilder();

                tables.Sort((table1, table2) => String.Compare(table1.Name, table2.Name, StringComparison.Ordinal));

                for (int i = 0, tableCount = tables.Count; i < tableCount; i++) {
                    var isNotLast = i != tableCount - 1;
                    var table = tables[i];

                    create.AppendFormat("{0}{1}.createTable(db);\n", Constants.Tab2, table.ClassName);
                    imports.AppendFormat("import {0}.{1}.{2}Contract.{3};\n", db.PackageName, db.ProviderFolder,
                                         db.ClassesPrefix, table.ClassName);
                    upgrade.AppendFormat("{0}{1}.upgradeTable(db, oldVersion, newVersion);\n", Constants.Tab2,
                                         table.ClassName);

                    if (progress != null) {
                        progress.Report(new ProgressResult {
                            Name = this.GetType().Name,
                            Value = 1
                        });
                    }
                }

                content = string.Format(content, db.PackageName, db.ProviderFolder, db.ClassesPrefix, create, upgrade,
                                        imports);

                return new GenerationResult{
                    Content = new List<string>{
                        content
                    },
                    Path = new List<string>{
                        output + "DatabaseHelper.java"
                    }
                };
            });
        }

        #endregion
    }
}