namespace Dabay6.Android.ContentProvider.Schema {
    #region USINGS

    using Newtonsoft.Json;
    using System;

    #endregion USINGS

    public class Join: IEquatable<Join> {

        /// <summary>
        /// </summary>
        public Field Field {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "foreign_key")]
        public string ForeignKey {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "table_name", Order = 1)]
        public string TableName {
            get;
            set;
        }

        #region IEquatable<Join> Members

        /// <summary>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Join other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(TableName, other.TableName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(ForeignKey, other.ForeignKey, StringComparison.OrdinalIgnoreCase) &&
                   Field.Equals(other.Field);
        }

        #endregion IEquatable<Join> Members

        /// <summary>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Join left, Join right) {
            return !Equals(left, right);
        }

        /// <summary>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Join left, Join right) {
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
            return Equals((Join) obj);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            unchecked {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(TableName);

                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(ForeignKey);
                hashCode = (hashCode * 397) ^ Field.GetHashCode();

                return hashCode;
            }
        }
    }
}