namespace Dabay6.Android.ContentProvider.Extensions {
    #region USINGS

    using System;
    using System.Windows.Forms;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public static class ControlExtensions {

        /// <summary>
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void Invoke(this Control control, Action action) {
            if (control.InvokeRequired) {
                control.Invoke(new MethodInvoker(action), null);
            }
            else {
                action.Invoke();
            }
        }
    }
}