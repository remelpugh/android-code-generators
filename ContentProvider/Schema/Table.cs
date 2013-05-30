namespace Dabay6.Android.ContentProvider.Schema {
    #region USINGS

    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class Table {
        private static readonly IEqualityComparer<Table> TableComparerInstance = new TableEqualityComparer();

        /// <summary>
        /// </summary>
        public Table() {
            Joins = new List<Join>();
            Version = 1;
            UpgradeFieldMap = new Dictionary<int, List<Field>>();
        }

        public static IEqualityComparer<Table> TableComparer {
            get {
                return TableComparerInstance;
            }
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public string ClassesPrefix {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public String ClassName {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public String ConstantName {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public int DatabaseVersion {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(Order = 2)]
        public List<Field> Fields {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(Order = 3)]
        public List<Join> Joins {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "table_name", Order = 1)]
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public Dictionary<int, List<Field>> UpgradeFieldMap {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty(Order = 4)]
        public int Version {
            get;
            set;
        }

        #region IEquatable<Table> Members

        public bool Equals(Table other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Fields.SequenceEqual(other.Fields, Field.FieldComparer) && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        #endregion IEquatable<Table> Members

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((Table) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = 0;

                if (Fields != null) {
                    Fields.ForEach(x => hashCode ^= x.GetHashCode());
                }

                return (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("Name: {0}, Fields: {1}", Name, Fields.Count);
        }

        #region Nested type: TableEqualityComparer

        private sealed class TableEqualityComparer: IEqualityComparer<Table> {
            #region IEqualityComparer<Table> Members

            public bool Equals(Table x, Table y) {
                if (ReferenceEquals(x, y)) {
                    return true;
                }
                if (ReferenceEquals(x, null)) {
                    return false;
                }
                if (ReferenceEquals(y, null)) {
                    return false;
                }
                if (x.GetType() != y.GetType()) {
                    return false;
                }
                return x.Fields.SequenceEqual(y.Fields, Field.FieldComparer) && string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Table obj) {
                unchecked {
                    var hashCode = 0;

                    if (obj.Fields != null) {
                        obj.Fields.ForEach(x => hashCode ^= x.GetHashCode());
                    }

                    return (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
                }
            }

            #endregion IEqualityComparer<Table> Members
        }

        #endregion Nested type: TableEqualityComparer
    }
}