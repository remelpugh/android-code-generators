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
    using Util;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class DataTransferObjectGenerator: IGenerator {

        /// <summary>
        /// </summary>
        /// <param name="schema"></param>
        public DataTransferObjectGenerator(SchemaDescription schema) {
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
                if (Schema.Database.HasDTO) {
                    var contents = new List<String>();
                    var db = Schema.Database;
                    var dto = Resources.dto_class;
                    var getSet = new StringBuilder();
                    var load = Resources.dto_class_load;
                    var loader = new StringBuilder();
                    var member = new StringBuilder();
                    var paths = new List<String>();
                    var properties = Resources.dto_class_getset;
                    var read = new StringBuilder();
                    var tables = Schema.Tables;
                    var values = new StringBuilder();
                    var write = new StringBuilder();

                    try {
                        foreach (var table in tables) {
                            var fields = table.Fields.ToList();
                            var joins = table.Joins.Select(x => x.Field).ToList();
                            var name = table.Name;
                            var packageName = "{0}{1}.{2}Contract.{3}.Columns";

                            joins.ForEach(x => x.IsJoin = true);

                            fields.AddRange(joins);
                            fields.Sort(
                                        (f1, f2) =>
                                        String.Compare(f1.ConstantName, f2.ConstantName, Constants.IgnoreCase));

                            getSet.Clear();
                            loader.Clear();
                            member.Clear();
                            read.Clear();
                            values.Clear();
                            write.Clear();

                            for (int i = 0, n = fields.Count; i < n; i++) {
                                var isNotLast = i != n - 1;
                                var field = fields[i];
                                var constantName = field.ConstantName;
                                var fieldName = constantName.CreateNameFromConstantName();
                                var memberName = fieldName.CreateLowerCamelCaseName();
                                var propertyName = constantName.CreateNameFromConstantName();
                                var type = field.Type;
                                var getter = type.Equals("boolean", Constants.IgnoreCase) ? "is" : "get";
                                var setter = type.Equals("boolean", Constants.IgnoreCase) ? "setIs" : "set";

                                if (type.Equals("boolean", Constants.IgnoreCase)) {
                                    memberName = memberName.CreateBooleanMemberName();
                                }

                                if (!field.IsId && !field.IsJoin) {
                                    values.AppendFormat("{0}name = Columns.{1}.getName();\n", Constants.Tab2,
                                                        constantName);

                                    if (field.IsNullable) {
                                        values.Append(Constants.Tab2);
                                        values.AppendFormat(
                                                            type.Equals("string", Constants.IgnoreCase)
                                                                ? "if (TextUtils.isEmpty({0}.{1}{2}())) {{\n"
                                                                : "if ({0}.{1}{2}() == null) {{\n", name.ToLower(),
                                                            getter, propertyName)
                                              .AppendFormat("{0}values.putNull(name);\n{1}}}\n{1}else {{\n",
                                                            Constants.Tab3, Constants.Tab2);

                                        AddContentValue(values, Constants.Tab3, field, name, getter, propertyName);

                                        values.AppendFormat("{0}}}\n", Constants.Tab2);
                                    }
                                    else {
                                        AddContentValue(values, Constants.Tab2, field, name, getter, propertyName);
                                    }

                                    values.Append("\n");
                                }

                                member.AppendFormat("{0}private {1} {2};\n", Constants.Tab1, type, memberName);

                                var property = string.Format(properties, propertyName, memberName, type, getter, setter);

                                getSet.Append(property);
                                if (isNotLast) {
                                    getSet.Append("\n");
                                }

                                if (type.Equals("boolean", Constants.IgnoreCase)) {
                                    read.AppendFormat("{0}{1} = in.readByte() != 0x00;\n", Constants.Tab2, memberName);

                                    write.AppendFormat("{0}dest.writeByte((byte) ({1} ? 0x01 : 0x00));\n",
                                                       Constants.Tab2, memberName);

                                    loader.AppendFormat("\n{0}index = cursor.getColumnIndex(Columns.{1}.getName());\n",
                                                        Constants.Tab2, constantName)
                                          .AppendFormat("{0}if (index != -1) {{\n", Constants.Tab2)
                                          .AppendFormat("{0}{1}.{2}{3}", Constants.Tab3, name.ToLower(), setter,
                                                        propertyName)
                                          .AppendFormat("(cursor.getInt(Columns.{0}.getIndex()) == 1);\n", constantName)
                                          .AppendFormat("{0}}}\n", Constants.Tab2);
                                }
                                else {
                                    string parcelType;
                                    string loadType;

                                    if (type.Equals("byte[]", Constants.IgnoreCase)) {
                                        loadType = "Blob";
                                        parcelType = "ByteArray";

                                        read.AppendFormat("{0}in.read{1}({2});\n", Constants.Tab2, parcelType,
                                                          memberName);
                                    }
                                    else {
                                        if (type.Equals("integer", Constants.IgnoreCase)) {
                                            loadType = "Int";
                                            parcelType = loadType;
                                        }
                                        else {
                                            loadType = type.CreateProperName();
                                            parcelType = loadType;
                                        }

                                        read.AppendFormat("{0}{1} = in.read{2}();\n", Constants.Tab2, memberName,
                                                          parcelType);
                                    }

                                    loader.AppendFormat(load, constantName, name.ToLower(), setter, propertyName,
                                                        loadType);

                                    write.AppendFormat("{0}dest.write{1}({2});\n", Constants.Tab2, parcelType,
                                                       memberName);
                                }
                            }

                            packageName = string.Format(packageName, db.PackageName,
                                                        db.ProviderFolder.IsEmpty() ? "" : "." + db.ProviderFolder,
                                                        db.ClassesPrefix, table.ClassName);

                            var content = string.Format(dto, db.PackageName, name, member, getSet, read, write,
                                                        packageName, loader, name.ToLower(), values);
                            var filePath = string.Format("{0}{1}DTO.java",
                                                         PathUtils.FilePath(path, db.PackageName, "data.DTO"), name);

                            contents.Add(content);
                            paths.Add(filePath);

                            if (progress != null) {
                                progress.Report(new ProgressResult{
                                    Name = GetType().Name,
                                    Value = 1
                                });
                            }
                        }

                        return new GenerationResult{
                            Content = contents,
                            Path = paths
                        };
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.ToString());
                        return null;
                    }
                }

                return null;
            });
        }

        #endregion IGenerator Members

        /// <summary>
        /// </summary>
        /// <param name="values"></param>
        /// <param name="tab"></param>
        /// <param name="field"></param>
        /// <param name="name"></param>
        /// <param name="getter"></param>
        /// <param name="propertyName"></param>
        private static void AddContentValue(StringBuilder values, string tab, Field field, string name, string getter,
                                            string propertyName) {
            values.Append(tab);
            values.AppendFormat("values.put(name, {0}.{1}{2}()", name.ToLower(), getter, propertyName);

            if (field.Type.Equals("boolean", Constants.IgnoreCase)) {
                values.Append(" ? 1 : 0");
            }

            values.Append(");\n");
        }
    }
}