namespace Dabay6.Android.ContentProvider.Generators {
    #region USINGS

    using Schema;
    using System;
    using System.Threading.Tasks;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public interface IGenerator {

        /// <summary>
        /// </summary>
        SchemaDescription Schema {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        Task<GenerationResult> Generate(string path, IProgress<ProgressResult> progress);
    }
}