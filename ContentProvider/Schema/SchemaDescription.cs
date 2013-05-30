namespace Dabay6.Android.ContentProvider.Schema {
    #region USINGS

    using Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class SchemaDescription: IEquatable<SchemaDescription> {

        /// <summary>
        /// </summary>
        [JsonProperty(Order = 1)]
        public Database Database {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(Order = 2)]
        public List<Table> Tables {
            get;
            set;
        }

        #region IEquatable<SchemaDescription> Members

        /// <summary>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SchemaDescription other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Tables.Equals(other.Tables);
        }

        #endregion IEquatable<SchemaDescription> Members

        /// <summary>
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsValid(SchemaDescription schema) {
            var db = schema.Database;

            return
                !(db.PackageName.IsEmpty() || db.ClassesPrefix.IsEmpty() || db.Name.IsEmpty() || schema.Tables == null ||
                  schema.Tables.Count == 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SchemaDescription left, SchemaDescription right) {
            return !Equals(left, right);
        }

        /// <summary>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SchemaDescription left, SchemaDescription right) {
            return Equals(left, right);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
            return Equals((SchemaDescription) obj);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return Tables.GetHashCode();
        }
    }
}