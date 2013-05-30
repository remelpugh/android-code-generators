namespace Dabay6.Android.ContentProvider.Generators {
    #region USINGS

    using System;

    #endregion USINGS

    public static class Constants {
        public const string BulkStringValue = "            String value;\n\n";
        public const StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;
        public const string MapToTable = ".mapToTable({0}, \"{1}\")";
        public const string PrimaryKey = " + \", PRIMARY KEY (\" + {0} + \")\"";
        public const string Tab1 = "    ";
        public const string Tab2 = Tab1 + Tab1;
        public const string Tab3 = Tab2 + Tab1;
        public const string Tab4 = Tab3 + Tab1;
        public const string UpgradeAddField = "Add field{2} {0} in table {1}";
        public const string UpgradeAddTable = "Add table {0}";
        public const string UpgradeNoChanges = "No changes";
        public const string UpgradeVersionMulti = "    // Version {0} - {1} : {2}\n";
        public const string UpgradeVersionOther = "    //             {0}\n";
        public const string UpgradeVersionVersion = "    // Version {0} : {1}\n";
        public const string Util = ".util";
        public const string VersionComment = "        // Version {0} : Add field{2} {1}\n";
        public const string VersionCommentNo = "        // Version {0} : No changes\n";
        public const string VersionCommentNoMulti = "        // Version {0} - {1} : No changes\n";
        public const string VersionJumpToLatest = "\n            if (oldVersion < newVersion) {{\n                // No more changes since version {0} so jump to newVersion\n                oldVersion = newVersion;\n            }}";
    }
}