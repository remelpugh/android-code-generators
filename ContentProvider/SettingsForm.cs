namespace Dabay6.Android.ContentProvider {
    #region USINGS

    using Properties;
    using System;
    using System.Windows.Forms;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public partial class SettingsForm: Form {

        /// <summary>
        /// </summary>
        public SettingsForm() {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleButtonClick(object sender, EventArgs e) {
            var button = sender as Button;

            if (button == null) {
                return;
            }

            if (button == btnCancel) {
                Close();
            }
            else if (button == btnOk) {
                SaveSettings();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleFormLoad(object sender, EventArgs e) {
            var settings = Settings.Default;

            chkGenerateDeviceId.Checked = settings.GenerateDeviceId;
        }

        /// <summary>
        /// </summary>
        private void SaveSettings() {
            var settings = Settings.Default;

            settings.GenerateDeviceId = chkGenerateDeviceId.Checked;
            settings.Save();

            Close();
        }
    }
}