namespace Dabay6.Android.ContentProvider.Generators {
    #region USINGS

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Extensions;
    using Properties;
    using Schema;

    #endregion

    /// <summary>
    /// </summary>
    public class ProviderClassGenerator: IGenerator {
        /// <summary>
        /// </summary>
        /// <param name="schema"></param>
        public ProviderClassGenerator(SchemaDescription schema) {
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
                var bulk = new StringBuilder();
                var caseId = new StringBuilder();
                var caseNoId = new StringBuilder();
                var codeIdentifier = 10000;
                var codes = new StringBuilder();
                //var create = new StringBuilder();
                var db = Schema.Database;
                var imports = new StringBuilder();
                var tables = Schema.Tables;
                var uri = new StringBuilder();
                //var upgrade = new StringBuilder();
                var upgradeComment = new StringBuilder();
                var upgradeCommentFields = new StringBuilder();

                tables.Sort((table1, table2) => String.Compare(table1.Name, table2.Name, StringComparison.Ordinal));

                for (int i = 0, tableCount = tables.Count; i < tableCount; i++) {
                    var isNotLast = i != tableCount - 1;
                    var table = tables[i];

                    imports.AppendFormat("import {0}.{1}.{2}Contract.{3};\n", db.PackageName, db.ProviderFolder,
                                         db.ClassesPrefix, table.ClassName);

                    var field = table.Fields.FirstOrDefault(x => x.IsId);
                    var idFieldName = (field == null)
                                          ? ""
                                          : string.Format("{0}.Columns.{1}.getName()", table.ClassName,
                                                          field.ConstantName);

                    codes.Append(Constants.Tab1)
                         .AppendFormat("private static final int {0} = 0x{1};\n", table.ConstantName, codeIdentifier);
                    uri.Append(Constants.Tab2)
                       .AppendFormat(
                                     "uriTypes.put({0}, new UriType({0}, {1}.TABLE_NAME, {1}.TABLE_NAME, {1}.TYPE_ELEM_TYPE, {2}));\n",
                                     table.ConstantName, table.ClassName, idFieldName);

                    codes.Append(Constants.Tab1)
                         .AppendFormat("private static final int {0}_ID = 0x{1};", table.ConstantName, ++codeIdentifier);
                    uri.Append(Constants.Tab2)
                       .AppendFormat(
                                     "uriTypes.put({0}_ID, new UriType({0}_ID, {1}.TABLE_NAME + \"/#\", {1}.TABLE_NAME, {1}.TYPE_DIR_TYPE, {2}));",
                                     table.ConstantName, table.ClassName, idFieldName);

                    if (isNotLast) {
                        codes.Append("\n");
                        uri.Append("\n");
                    }

                    //create.AppendFormat("{0}{1}.createTable(db);\n", Constants.Tab3, table.ClassName);

                    //upgrade.AppendFormat("{0}{1}.upgradeTable(db, oldVersion, newVersion);\n", Constants.Tab3,
                    //                     table.ClassName);

                    caseId.AppendFormat("{0}case {1}_ID:", Constants.Tab3, table.ConstantName);
                    if (isNotLast) {
                        caseId.Append("\n");
                    }

                    caseNoId.AppendFormat("{0}case {1}:", Constants.Tab3, table.ConstantName);
                    if (isNotLast) {
                        caseNoId.Append("\n");
                    }

                    bulk.AppendFormat(Resources.provider_bulk, table.ConstantName, table.ClassName);

                    codeIdentifier++;

                    if (progress != null) {
                        progress.Report(new ProgressResult {
                            Name = this.GetType().Name,
                            Value = 1
                        });
                    }
                }

                var saveCaseId = caseId.ToString();
                var saveCaseNoId = caseNoId.ToString();
                // Upgrade comments in the provider
                var minUpgradeWithoutChanges = -1;

                for (var version = 2; version <= db.Version; version++) {
                    var first = true;

                    upgradeCommentFields.Clear();

                    foreach (var table in tables) {
                        if (table.Version == version) {
                            UpgradeDbComment(upgradeComment, first, minUpgradeWithoutChanges, version,
                                             string.Format(Constants.UpgradeAddTable, table.ClassName));
                            first = false;
                            minUpgradeWithoutChanges = -1;
                        }

                        if (table.UpgradeFieldMap.ContainsKey(version)) {
                            var upgradeFields = table.UpgradeFieldMap[version];

                            if (upgradeFields != null) {
                                var firstField = true;

                                foreach (var field in upgradeFields) {
                                    if (!firstField) {
                                        upgradeCommentFields.Append(", ");
                                    }
                                    firstField = false;
                                    upgradeCommentFields.Append(field.ConstantName);
                                }

                                UpgradeDbComment(upgradeComment, first, minUpgradeWithoutChanges, version,
                                                 string.Format(Constants.UpgradeAddField, upgradeCommentFields,
                                                               table.ClassName, upgradeFields.Count > 1 ? "s" : ""));
                                first = false;
                                minUpgradeWithoutChanges = -1;
                            }
                        }
                    }

                    // No changes in this version
                    if (first && minUpgradeWithoutChanges == -1) {
                        minUpgradeWithoutChanges = version;
                    }
                }

                if (minUpgradeWithoutChanges != -1) {
                    if (minUpgradeWithoutChanges == db.Version) {
                        // Only one without change
                        upgradeComment.AppendFormat(Constants.UpgradeVersionVersion, minUpgradeWithoutChanges,
                                                    Constants.UpgradeNoChanges);
                    }
                    else {
                        // Multiple versions with changes
                        upgradeComment.AppendFormat(Constants.UpgradeVersionMulti, minUpgradeWithoutChanges, db.Version,
                                                    Constants.UpgradeNoChanges);
                    }
                }

                var joins = (from x in tables
                             where x.Joins != null && x.Joins.Count > 0
                             select x).ToList();

                tables = (from x in tables
                          where x.Joins == null || x.Joins.Count == 0
                          select x).ToList();

                caseId.Clear();
                caseNoId.Clear();

                for (int i = 0, tableCount = tables.Count; i < tableCount; i++) {
                    var isNotLast = i != tableCount - 1;
                    var table = tables[i];

                    caseId.AppendFormat("{0}case {1}_ID:", Constants.Tab3, table.ConstantName);
                    if (isNotLast) {
                        caseId.Append("\n");
                    }

                    caseNoId.AppendFormat("{0}case {1}:", Constants.Tab3, table.ConstantName);
                    if (isNotLast) {
                        caseNoId.Append("\n");
                    }

                    if (progress != null) {
                        progress.Report(new ProgressResult {
                            Name = this.GetType().Name,
                            Value = 1
                        });
                    }
                }

                var joinId = new StringBuilder();
                var joinNoId = new StringBuilder();

                foreach (var table in joins) {
                    var joinTables = table.Joins;
                    var tableJoin = new StringBuilder();

                    tableJoin.AppendFormat("\"{0} ", table.Name);

                    for (int i = 0, joinCount = joinTables.Count; i < joinCount; i++) {
                        var isNotFirst = i != 0;
                        var join = joinTables[i];

                        if (isNotFirst) {
                            tableJoin.Append(" ");
                        }
                        tableJoin.AppendFormat("LEFT OUTER JOIN {0} ON {1}.{2} = {0}._id", join.TableName, table.Name,
                                               join.ForeignKey);
                    }

                    tableJoin.Append("\"");

                    var mappings = new StringBuilder();
                    var mappedFields = (from x in table.Fields
                                        where !x.MapTable.IsEmpty()
                                        select x).ToList();

                    foreach (var mappendField in mappedFields) {
                        mappings.AppendFormat(Constants.MapToTable, "\"" + mappendField.Name + "\"",
                                              mappendField.MapTable);
                    }

                    foreach (var join in joinTables) {
                        var field = join.Field;

                        mappings.AppendFormat(Constants.MapToTable, "\"" + join.ForeignKey + "\"", table.Name);

                        if (!field.MapTable.IsEmpty()) {
                            mappings.AppendFormat(Constants.MapToTable, "\"" + field.Name + "\"", field.MapTable);
                        }
                    }

                    var code = string.Format(Resources.expanded_selection_id, tableJoin,
                                             string.Format("{0}{1}",
                                                           string.Format(Constants.MapToTable,
                                                                         "uriType.getIdColumnName()", table.Name),
                                                           mappings), table.Name);

                    joinId.AppendFormat("{0}case {1}_ID: {{\n", Constants.Tab3, table.ConstantName);
                    joinId.AppendFormat("{0}{1}}}\n", code, Constants.Tab3);

                    code = string.Format(Resources.expanded_selection, tableJoin,
                                         string.Format("{0}{1}",
                                                       string.Format(Constants.MapToTable, "uriType.getIdColumnName()",
                                                                     table.Name), mappings));

                    joinNoId.AppendFormat("{0}case {1}: {{\n", Constants.Tab3, table.ConstantName);
                    joinNoId.AppendFormat("{0}{1}}}\n", code, Constants.Tab3);

                    if (progress != null) {
                        progress.Report(new ProgressResult {
                            Name = this.GetType().Name,
                            Value = 1
                        });
                    }
                }

                var content = string.Format(Resources.provider, db.PackageName, imports, db.ClassesPrefix,
                                            db.AuthorityPackage, codes, saveCaseId, saveCaseNoId, bulk,
                                            db.ProviderFolder, db.Version, upgradeComment, db.Name,
                                            db.ProviderFolder + Constants.Util, caseId, caseNoId, joinId, joinNoId, uri);

                return new GenerationResult{
                    Content = new List<string>{
                        content
                    },
                    Path = new List<string>{
                        Database.GeneratePath(db, path) + "Provider.java"
                    }
                };
            });
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="first"></param>
        /// <param name="minVersion"></param>
        /// <param name="version"></param>
        /// <param name="content"></param>
        private void UpgradeDbComment(StringBuilder sb, bool first, int minVersion, int version, string content) {
            if (minVersion != -1) {
                if (minVersion == version - 1) {
                    sb.Append(string.Format(Constants.UpgradeVersionVersion, version - 1, Constants.UpgradeNoChanges));
                }
                else {
                    sb.Append(string.Format(Constants.UpgradeVersionMulti, minVersion, version - 1,
                                            Constants.UpgradeNoChanges));
                }
            }

            sb.Append(first
                          ? string.Format(Constants.UpgradeVersionVersion, version, content)
                          : string.Format(Constants.UpgradeVersionOther, content));
        }
    }
}