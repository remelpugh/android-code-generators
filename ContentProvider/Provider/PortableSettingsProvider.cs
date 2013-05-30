namespace Dabay6.Android.ContentProvider.Provider {
    #region USINGS

    using Extensions;
    using Newtonsoft.Json;
    using Properties;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public class PortableSettingsProvider: SettingsProvider {
        private ApplicationSettings _applicationSettings;

        /// <summary>
        /// </summary>
        public override string ApplicationName {
            get {
                //if (!Application.ProductName.IsEmpty()) {
                //    return Application.ProductName;
                //}
                var file = new FileInfo(Application.ExecutablePath);

                return file.Name.Substring(0, file.Name.Length - file.Extension.Length);
            }
            set {
                // do nothing
            }
        }

        /// <summary>
        /// </summary>
        private ApplicationSettings ApplicationSettings {
            get {
                if (_applicationSettings == null) {
                    var filename = Path.Combine(GetAppPath(), GetSettingsFilename());

                    try {
                        var json = File.ReadAllText(filename);

                        _applicationSettings = JsonConvert.DeserializeObject<ApplicationSettings>(json);
                    }
                    catch (Exception) {
                        _applicationSettings = new ApplicationSettings();

                        SaveSettingsFile();
                    }
                }

                return _applicationSettings;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual string GetAppPath() {
            return new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
                                                                          SettingsPropertyCollection collection) {
            // Create a collection of values to return
            var retValues = new SettingsPropertyValueCollection();

            // Create a temporary SettingsPropertyValue to reuse

            // Loop through the list of settings that the application has requested and add them
            // to our collection of return values.
            foreach (var value in from SettingsProperty property in collection
                                  select new SettingsPropertyValue(property) {
                                      IsDirty = false,
                                      SerializedValue = GetSetting(property)
                                  }) {
                retValues.Add(value);
            }
            return retValues;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual string GetSettingsFilename() {
            return ApplicationName + ".json";
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config) {
            base.Initialize(ApplicationName, config);
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection"></param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection) {
            foreach (SettingsPropertyValue value in collection) {
                SetSetting(value);
            }

            try {
                SaveSettingsFile();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Resources.ConfigurationFileError, MessageBoxButtons.OK, MessageBoxIcon.Error);

                Debug.WriteLine("Error writing configuration file to disk: " + ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetSetting(SettingsProperty property) {
            object value;

            try {
                value = ApplicationSettings.GetType().GetProperty(property.Name).GetValue(_applicationSettings);
                //// Search for the specific settings node we are looking for in the configuration file.
                //// If it exists, return the InnerText or InnerXML of its first child node, depending on the setting type.

                //// If the setting is serialized as a string, return the text stored in the config
                //if (property.SerializeAs.ToString() == "String") {
                //    return XMLConfig.SelectSingleNode("//setting[@name='" + property.Name + "']").FirstChild.InnerText;
                //}

                //// If the setting is stored as XML, deserialize it and return the proper object.  This only supports
                //// StringCollections at the moment - I will likely add other types as I use them in applications.
                //else {
                //    string settingType = property.PropertyType.ToString();
                //    string xmlData = XMLConfig.SelectSingleNode("//setting[@name='" + property.Name + "']").FirstChild.InnerXml;
                //    XmlSerializer xs = new XmlSerializer(typeof(string[]));
                //    string[] data = (string[])xs.Deserialize(new XmlTextReader(xmlData, XmlNodeType.Element, null));

                //    switch (settingType) {
                //        case "System.Collections.Specialized.StringCollection":
                //            StringCollection sc = new StringCollection();
                //            sc.AddRange(data);
                //            return sc;
                //        default:
                //            return "";
                //    }
                //}
            }
            catch (Exception) {
                value = null;
                //// Check to see if a default value is defined by the application.
                //// If so, return that value, using the same rules for settings stored as Strings and XML as above
                //if ((property.DefaultValue != null)) {
                //    if (property.SerializeAs.ToString() == "String") {
                //        value = property.DefaultValue.ToString();
                //    }
                //    else {
                //        string settingType = property.PropertyType.ToString();
                //        string xmlData = property.DefaultValue.ToString();
                //        XmlSerializer xs = new XmlSerializer(typeof(string[]));
                //        string[] data = (string[])xs.Deserialize(new XmlTextReader(xmlData, XmlNodeType.Element, null));

                //        switch (settingType) {
                //            case "System.Collections.Specialized.StringCollection":
                //                StringCollection sc = new StringCollection();
                //                sc.AddRange(data);
                //                return sc;

                //            default:
                //                return "";
                //        }
                //    }
                //}
                //else {
                //    value = "";
                //}
            }

            return value;
        }

        private void SaveSettingsFile() {
            var filename = Path.Combine(GetAppPath(), GetSettingsFilename());
            var settings = new JsonSerializerSettings{
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(ApplicationSettings, settings);

            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        private void SetSetting(SettingsPropertyValue property) {
            //// Define the XML path under which we want to write our settings if they do not already exist
            //XmlNode SettingNode = null;

            try {
                var reflected = ApplicationSettings.GetType().GetProperty(property.Name);

                if (reflected != null) {
                    reflected.SetValue(ApplicationSettings, property.SerializedValue);
                }
                //// Search for the specific settings node we want to update.
                //// If it exists, return its first child node, (the <value>data here</value> node)
                //SettingNode = XMLConfig.SelectSingleNode("//setting[@name='" + property.Name + "']").FirstChild;
            }
            catch (Exception) {
                //SettingNode = null;
            }

            //// If we have a pointer to an actual XML node, update the value stored there
            //if ((SettingNode != null)) {
            //    if (property.Property.SerializeAs.ToString() == "String") {
            //        SettingNode.InnerText = property.SerializedValue.ToString();
            //    }
            //    else {
            //        // Write the object to the config serialized as Xml - we must remove the Xml declaration when writing
            //        // the value, otherwise .Net's configuration system complains about the additional declaration.
            //        SettingNode.InnerXml = property.SerializedValue.ToString().Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
            //    }
            //}
            //else {
            //    // If the value did not already exist in this settings file, create a new entry for this setting

            //    // Search for the application settings node (<Appname.Properties.Settings>) and store it.
            //    XmlNode tmpNode = XMLConfig.SelectSingleNode("//" + APPNODE);

            //    // Create a new settings node and assign its name as well as how it will be serialized
            //    XmlElement newSetting = xmlDoc.CreateElement("setting");
            //    newSetting.SetAttribute("name", property.Name);

            //    if (property.Property.SerializeAs.ToString() == "String") {
            //        newSetting.SetAttribute("serializeAs", "String");
            //    }
            //    else {
            //        newSetting.SetAttribute("serializeAs", "Xml");
            //    }

            //    // Append this node to the application settings node (<Appname.Properties.Settings>)
            //    tmpNode.AppendChild(newSetting);

            //    // Create an element under our named settings node, and assign it the value we are trying to save
            //    XmlElement valueElement = xmlDoc.CreateElement("value");
            //    if (property.Property.SerializeAs.ToString() == "String") {
            //        valueElement.InnerText = property.SerializedValue.ToString();
            //    }
            //    else {
            //        // Write the object to the config serialized as Xml - we must remove the Xml declaration when writing
            //        // the value, otherwise .Net's configuration system complains about the additional declaration
            //        valueElement.InnerXml = property.SerializedValue.ToString().Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
            //    }

            //    //Append this new element under the setting node we created above
            //    newSetting.AppendChild(valueElement);
            //}
        }
    }
}