namespace Dabay6.Android.ContentProvider.Json {
    #region USINGS

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Schema;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class LowercaseResolver: DefaultContractResolver {

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            var properties = base.CreateProperties(type, memberSerialization);

            foreach (var property in properties) {
                property.PropertyName = property.PropertyName.ToLower();
            }

            var unordered = properties.Where(p => !p.Order.HasValue).OrderBy(p => p.PropertyName);
            var ordered = properties.Where(p => p.Order.HasValue).OrderBy(p => p.Order);

            return ordered.Concat(unordered).ToList();
        }
    }
}