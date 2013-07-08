namespace Dabay6.Android.ContentProvider.Extensions {
    #region USINGS

    using System;
    using System.Data;
    using Schema;

    #endregion

    /// <summary>
    /// </summary>
    public static class FieldExtensions {
        /// <summary>
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static void ConvertSqlDataType(this Field field) {
            DbType dbType;

            if (!Enum.TryParse(field.Type, true, out dbType)) {
                var type = field.Type;

                if (type == "int" && field.IsId) {
                    dbType = DbType.Int64;
                }
                else {
                    switch (type) {
                        case "bit": {
                            dbType = DbType.Boolean;
                            break;
                        }
                        case "int": {
                            dbType = DbType.Int32;
                            break;
                        }
                        case "money": {
                            dbType = DbType.Currency;
                            break;
                        }
                        case "varbinary": {
                            dbType = DbType.Binary;
                            break;
                        }
                        default: {
                            dbType = DbType.String;
                            break;
                        }
                    }
                }
            }

            if (field.IsId) {
                switch (dbType) {
                    case DbType.Int16:
                    case DbType.Int32:
                    case DbType.Int64: {
                        field.Type = "long";
                        break;
                    }
                }
            }
            else {
                switch (dbType) {
                    case DbType.Int16: {
                        field.Type = "integer";
                        break;
                    }
                    case DbType.Binary: {
                        field.Type = "blob";
                        break;
                    }
                    case DbType.Boolean: {
                        field.Type = "boolean";
                        break;
                    }
                    case DbType.Currency:
                    case DbType.Decimal: {
                        field.Type = "float";
                        break;
                    }
                    case DbType.DateTime:
                    case DbType.Int32:
                    case DbType.Int64: {
                        field.Type = "long";
                        break;
                    }
                    default: {
                        field.Type = "text";
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string DefaultValue(this Field field) {
            if (field.IsNullable) {
                return "NULL";
            }
            else {
                var type = field.DatabaseType;

                if (string.Equals(type, "string", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(type, "text", StringComparison.OrdinalIgnoreCase)) {
                    return "''";
                }
                else {
                    return (-1).ToString();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="databaseVersion"></param>
        public static void Initialize(this Field field, Database database) {
            if (field.Version > database.Version) {
                var message = "The field {0} has a version {1} higher than the database version {2}";

                message = string.Format(message, field.Name, field.Version, database.Version);

                throw new ArgumentException(message);
            }

            field.ConstantName = field.Name.CreateConstantName();
            field.PropertyName = field.ConstantName.CreateNameFromConstantName();
            if (field.IsId) {
                field.Name = "_id";
            }

            if (field.Type.Equals("blob", StringComparison.OrdinalIgnoreCase)) {
                field.Type = "byte[]";
                field.DatabaseType = field.Type;
            }
            else if (field.Type.Equals("boolean", StringComparison.OrdinalIgnoreCase)) {
                field.Type = "Boolean";
                field.DatabaseType = "integer";
            }
            else if (field.Type.Equals("double", StringComparison.OrdinalIgnoreCase)) {
                field.Type = "Double";
                field.DatabaseType = "real";
            }
            else if (field.Type.Equals("int", StringComparison.OrdinalIgnoreCase) ||
                     field.Type.Equals("integer", StringComparison.OrdinalIgnoreCase) ||
                     field.Type.Equals("date", StringComparison.OrdinalIgnoreCase)) {
                field.DatabaseType = "integer";
                field.Type = field.DatabaseType.CreateProperName();
            }
            else if (field.Type.Equals("float", StringComparison.OrdinalIgnoreCase) ||
                     field.Type.Equals("real", StringComparison.OrdinalIgnoreCase)) {
                field.Type = "Float";
                field.DatabaseType = "real";
            }
            else if (field.Type.Equals("long", StringComparison.OrdinalIgnoreCase)) {
                field.Type = "Long";
                field.DatabaseType = "integer";
            }
            else if (field.Type.Equals("string", StringComparison.OrdinalIgnoreCase) ||
                     field.Type.Equals("text", StringComparison.OrdinalIgnoreCase)) {
                field.Type = "String";
                field.DatabaseType = "text";
            }
        }
    }
}