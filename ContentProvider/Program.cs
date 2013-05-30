namespace Dabay6.Android.ContentProvider {
    #region USINGS

    using System;
    using System.Windows.Forms;

    #endregion USINGS

    internal static class Program {

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}