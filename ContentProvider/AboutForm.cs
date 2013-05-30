namespace Dabay6.Android.ContentProvider {
    #region USINGS

    using System;
    using System.Reflection;
    using System.Windows.Forms;

    #endregion USINGS

    partial class AboutForm: Form {

        /// <summary>
        /// </summary>
        public AboutForm() {
            InitializeComponent();
            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        /// <summary>
        /// </summary>
        public string AssemblyCompany {
            get {
                var attributes = Assembly.GetExecutingAssembly()
                                         .GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute) attributes[0]).Company;
            }
        }

        /// <summary>
        /// </summary>
        public string AssemblyCopyright {
            get {
                var attributes = Assembly.GetExecutingAssembly()
                                         .GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// </summary>
        public string AssemblyDescription {
            get {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        /// <summary>
        /// </summary>
        public string AssemblyProduct {
            get {
                var attributes = Assembly.GetExecutingAssembly()
                                         .GetCustomAttributes(typeof (AssemblyProductAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute) attributes[0]).Product;
            }
        }

        /// <summary>
        /// </summary>
        public string AssemblyTitle {
            get {
                var attributes = Assembly.GetExecutingAssembly()
                                         .GetCustomAttributes(typeof (AssemblyTitleAttribute), false);

                if (attributes.Length > 0) {
                    var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                    if (titleAttribute.Title != "") {
                        return titleAttribute.Title;
                    }
                }

                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// </summary>
        public string AssemblyVersion {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        #endregion Assembly Attribute Accessors
    }
}