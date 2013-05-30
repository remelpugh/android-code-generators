namespace Dabay6.Android.ContentProvider.Schema {
    #region USINGS

    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class Field {
        private static readonly IEqualityComparer<Field> FieldComparerInstance = new FieldEqualityComparer();

        /// <summary>
        /// </summary>
        public Field() {
            IsNullable = true;
            Version = 1;
        }

        public static IEqualityComparer<Field> FieldComparer {
            get {
                return FieldComparerInstance;
            }
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public string ConstantName {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public string DatabaseType {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "is_id")]
        public bool IsId {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "is_index")]
        public bool IsIndex {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public bool IsJoin {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "is_nullable")]
        public bool IsNullable {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "is_primary_key")]
        public bool IsPrimaryKey {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "is_unique")]
        public bool IsUnique {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "map_table")]
        public string MapTable {
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
        [JsonIgnore]
        public string PropertyName {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string Type {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [DefaultValue(1)]
        public int Version {
            get;
            set;
        }

        public bool Equals(Field other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

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
            return Equals((Field) obj);
        }

        public override int GetHashCode() {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("Name: {0}", Name);
        }

        #region Nested type: FieldEqualityComparer

        private sealed class FieldEqualityComparer: IEqualityComparer<Field> {
            #region IEqualityComparer<Field> Members

            public bool Equals(Field x, Field y) {
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
                return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Field obj) {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
            }

            #endregion IEqualityComparer<Field> Members
        }

        #endregion Nested type: FieldEqualityComparer
    }
}