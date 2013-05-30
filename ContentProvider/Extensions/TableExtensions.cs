namespace Dabay6.Android.ContentProvider.Extensions {
    #region USINGS

    using System.Collections.Generic;
    using Schema;
    using System;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public static class TableExtensions {

        /// <summary>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="database"></param>
        public static void Initialize(this Table table, Database database) {
            if (table.Version > database.Version) {
                var message = "The table {0} has a version {1} higher than the databaseInfo version {2}";

                message = string.Format(message, table.ClassName, table.Version, database.Version);

                throw new ArgumentException(message);
            }

            table.ClassName = table.ClassesPrefix + table.Name;
            table.ConstantName = table.ClassName.CreateConstantName();
            table.DatabaseVersion = database.Version;

            foreach (var field in table.Fields) {
                List<Field> upgradeList = null;

                field.Initialize(database);

                if (table.UpgradeFieldMap.ContainsKey(field.Version)) {
                    upgradeList = table.UpgradeFieldMap[field.Version];
                }

                if (upgradeList == null) {
                    upgradeList = new List<Field>();

                    table.UpgradeFieldMap.Add(field.Version, upgradeList);
                }

                upgradeList.Add(field);
            }

            var joins = table.Joins;

            foreach (var join in joins) {
                join.Field.Initialize(database);
            }
        }
    }
}