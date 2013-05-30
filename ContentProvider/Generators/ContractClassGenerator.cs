namespace Dabay6.Android.ContentProvider.Generators {
    #region USINGS

    using Extensions;
    using Properties;
    using Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion USINGS

    public class ContractClassGenerator: IGenerator {

        /// <summary>
        /// </summary>
        /// <param name="schema"></param>
        public ContractClassGenerator(SchemaDescription schema) {
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
                var tables = Schema.Tables;
                GenerationResult result;

                tables.Sort((table1, table2) => String.Compare(table1.Name, table2.Name, StringComparison.Ordinal));

                try {
                    var bulkFields = new StringBuilder();
                    var bulkParams = new StringBuilder();
                    var bulkValues = new StringBuilder();
                    var contentClass = Resources.contract_class;
                    var contentSubClass = Resources.contract_subclass;
                    var contentSubClassUpgrade = Resources.contract_subclass_upgrade;
                    var create = new StringBuilder();
                    var createKey = new StringBuilder();
                    var enums = new StringBuilder();
                    var indexes = new StringBuilder();
                    var projection = new StringBuilder();
                    var subClasses = new StringBuilder();
                    var upgrade = new StringBuilder();
                    var upgradeComment = new StringBuilder();
                    var upgradeCommentNew = new StringBuilder();
                    var upgradeInsertFields = new StringBuilder();
                    var upgradeTable = new StringBuilder();
                    var upgradeTableKey = new StringBuilder();

                    foreach (var table in tables) {
                        var fields = table.Fields;

                        enums.Clear();
                        projection.Clear();
                        create.Clear();
                        createKey.Clear();
                        upgradeComment.Clear();
                        upgrade.Clear();
                        indexes.Clear();
                        bulkFields.Clear();
                        bulkParams.Clear();
                        bulkValues.Clear();

                        var hasPreviousPrimaryKey = false;
                        var hasTextField = false;

                        for (int i = 0, n = fields.Count; i < n; i++) {
                            var field = fields[i];
                            var constantName = field.ConstantName;
                            var dbType = field.DatabaseType;
                            var isNotLast = i != n - 1;

                            enums.AppendFormat("{0}{1}(", Constants.Tab3, constantName);

                            if (field.IsPrimaryKey) {
                                enums.Append("BaseColumns._ID");
                            }
                            else {
                                enums.AppendFormat("\"{0}\"", field.Name);
                            }

                            enums.AppendFormat(", \"{0}\")", dbType);

                            projection.AppendFormat("{0}Columns.{1}.getName()", Constants.Tab4, constantName);

                            create.AppendFormat("Columns.{0}.getName() + \" \" + Columns.{0}.getType()", constantName);

                            if (field.IsUnique) {
                                create.Append(" + \" UNIQUE\"");
                            }

                            if (!field.IsNullable) {
                                create.Append(" + \" NOT NULL\"");
                            }

                            if (field.IsPrimaryKey) {
                                if (hasPreviousPrimaryKey) {
                                    createKey.Append(" + \", \" + ");
                                }

                                hasPreviousPrimaryKey = true;

                                createKey.AppendFormat("Columns.{0}.getName()", constantName);
                            }

                            if (field.IsIndex) {
                                indexes.Append(Constants.Tab3)
                                       .Append("db.execSQL(\"CREATE INDEX ")
                                       .Append(table.Name)
                                       .Append("_")
                                       .Append(field.Name)
                                       .Append(" on \" + TABLE_NAME + \"(\" + Columns.")
                                       .Append(constantName)
                                       .Append(".getName() + \");\");\n");
                            }

                            if (!field.IsId) {
                                bulkFields.AppendFormat(".append(Columns.{0}.getName())", constantName);
                                bulkParams.Append("?");

                                if (dbType.Equals("text", Constants.IgnoreCase)) {
                                    hasTextField = true;

                                    bulkValues.AppendFormat("{0}value = values.getAsString(Columns.{1}.getName());\n",
                                                            Constants.Tab3, constantName)
                                              .AppendFormat("{0}stmt.bindString(i++, value != null ? value : \"\");\n",
                                                            Constants.Tab3);
                                }
                                else {
                                    var methodName = string.Empty;

                                    bulkValues.Append(Constants.Tab3);

                                    if (dbType.Equals("byte[]", Constants.IgnoreCase)) {
                                        bulkValues.AppendFormat("stmt.bindBlob(i++, values.getAsByteArray(");
                                    }
                                    else {
                                        if (dbType.Equals("integer", Constants.IgnoreCase)) {
                                            methodName = "Long";
                                        }
                                        else if (dbType.Equals("real", Constants.IgnoreCase)) {
                                            methodName = "Double";
                                        }

                                        bulkValues.AppendFormat("stmt.bind{0}(i++, values.getAs{0}(", methodName);
                                    }

                                    bulkValues.AppendFormat("Columns.{0}.getName()));\n", constantName);
                                }
                            }

                            if (isNotLast) {
                                enums.Append(",\n");
                                projection.Append(",\n");
                                create.Append(" + \", \" + ");
                                if (!field.IsId) {
                                    bulkFields.Append(".append(\", \")");
                                    bulkParams.Append(", ");
                                }
                            }
                        }

                        foreach (var field in table.Joins.Select(x => x.Field)) {
                            var name = field.Name;

                            if (enums.Length > 0) {
                                enums.Append(",\n");
                            }

                            enums.AppendFormat("{0}{1}(\"{2}\"", Constants.Tab3, field.ConstantName, name)
                                 .AppendFormat(", \"{0}\", true)", field.DatabaseType);

                            if (projection.Length > 0) {
                                projection.Append(",\n");
                            }

                            projection.AppendFormat("{0}Columns.{1}.getName()", Constants.Tab4,
                                                    name.CreateConstantName());
                        }

                        // Upgrade management
                        var maxVersion = table.Version;
                        var minVersion = -1;

                        for (int version = table.Version + 1, n = db.Version; version <= n; version++) {
                            if (table.UpgradeFieldMap.ContainsKey(version)) {
                                var upgradeFields = table.UpgradeFieldMap[version];

                                if (upgradeFields == null) {
                                    if (minVersion == -1) {
                                        minVersion = version;
                                    }
                                    continue;
                                }
                                if (minVersion != -1) {
                                    if (minVersion == version - 1) {
                                        // Only one without change
                                        upgradeComment.AppendFormat(Constants.VersionCommentNo, minVersion);
                                    }
                                    else {
                                        // Multiple versions with changes
                                        upgradeComment.AppendFormat(Constants.VersionCommentNoMulti, minVersion,
                                                                    version - 1);
                                    }

                                    minVersion = -1;
                                }

                                maxVersion = version;

                                upgradeCommentNew.Clear();
                                upgradeTable.Clear();
                                upgradeTableKey.Clear();
                                upgradeInsertFields.Clear();

                                var hasPreviousUpgradeElements = false;
                                var hasPreviousInsertFields = false;

                                hasPreviousPrimaryKey = false;

                                foreach (var field in fields) {
                                    var constantName = field.ConstantName;

                                    if (field.Version > version) {
                                        // This field doesn't exist yet in this version
                                        continue;
                                    }

                                    if (hasPreviousUpgradeElements) {
                                        upgradeTable.Append(" + \", \" + ");
                                    }
                                    hasPreviousUpgradeElements = true;

                                    upgradeTable.AppendFormat("Columns.{0}.getName() + \" \" + Columns.{0}.getType()",
                                                              constantName);

                                    if (field.IsPrimaryKey) {
                                        if (hasPreviousPrimaryKey) {
                                            upgradeTableKey.Append(" + \", \" + ");
                                        }

                                        hasPreviousPrimaryKey = true;

                                        upgradeTableKey.AppendFormat("Columns.{0}.getName()", constantName);
                                    }

                                    if (field.Version < version) {
                                        // The field is an old one and is added to the insert list
                                        if (hasPreviousInsertFields) {
                                            upgradeInsertFields.Append(" + \", \" + ");
                                        }
                                        hasPreviousInsertFields = true;

                                        upgradeInsertFields.AppendFormat("Columns.{0}.getName()", constantName);
                                    }
                                    else {
                                        if (hasPreviousInsertFields) {
                                            upgradeInsertFields.Append(" + \", \" + ");
                                        }
                                        hasPreviousInsertFields = true;

                                        upgradeInsertFields.AppendFormat("\"{0}\"", field.DefaultValue());
                                    }
                                }

                                upgrade.AppendFormat(contentSubClassUpgrade, version, upgradeTable,
                                                     hasPreviousPrimaryKey
                                                         ? string.Format(Constants.PrimaryKey, upgradeTableKey) : "",
                                                     upgradeInsertFields);

                                hasPreviousUpgradeElements = false;

                                foreach (var field in upgradeFields) {
                                    if (hasPreviousUpgradeElements) {
                                        upgradeCommentNew.Append(", ");
                                    }
                                    hasPreviousUpgradeElements = true;

                                    upgradeCommentNew.Append(field.ConstantName);
                                }
                                upgradeComment.AppendFormat(Constants.VersionComment, version, upgradeCommentNew,
                                                            upgradeFields.Count > 1 ? "s" : "");
                            }
                        }

                        // No more changes for the last versions so add the code to jump to the latest version
                        if (maxVersion != db.Version) {
                            upgrade.AppendFormat(Constants.VersionJumpToLatest, maxVersion);

                            if (maxVersion == db.Version - 1) {
                                // Only one without change
                                upgradeComment.AppendFormat(Constants.VersionCommentNo, maxVersion + 1);
                            }
                            else {
                                // Multiple versions with changes
                                upgradeComment.AppendFormat(Constants.VersionCommentNoMulti, maxVersion + 1, db.Version);
                            }
                        }

                        subClasses.AppendFormat(contentSubClass, table.ClassName, db.ClassesPrefix, table.Name,
                                                db.ClassesPrefix.ToLower(), table.Name.ToLower(), enums, projection,
                                                create,
                                                hasPreviousPrimaryKey
                                                    ? string.Format(Constants.PrimaryKey, createKey) : "", indexes,
                                                bulkFields, bulkParams, hasTextField ? Constants.BulkStringValue : "",
                                                bulkValues, table.Version, upgradeComment, upgrade);

                        if (progress != null) {
                            progress.Report(new ProgressResult {
                                Name = this.GetType().Name,
                                Value = 1
                            });
                        }
                    }

                    var content = string.Format(contentClass, db.PackageName, db.ClassesPrefix, subClasses,
                                                db.ProviderFolder, db.ProviderFolder + Constants.Util);

                    result = new GenerationResult{
                        Content = new List<string>{
                            content
                        },
                        Path = new List<string>{
                            Database.GeneratePath(db, path) + "Contract.java"
                        }
                    };
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                    result = null;
                }

                return result;
            });
        }

        #endregion IGenerator Members
    }
}