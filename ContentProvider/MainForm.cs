namespace Dabay6.Android.ContentProvider {

    #region USINGS

    using Extensions;
    using Generators;
    using Json;
    using Newtonsoft.Json;
    using Properties;
    using Schema;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Util;

    #endregion USINGS

    /// <summary>
    /// </summary>
    public partial class MainForm: Form {
        private readonly Task<List<String>> _loginTask =
            Task<List<string>>.Factory.StartNew(() => (from x in Settings.Default.Login.Split(',')
                                                       where !x.IsEmpty()
                                                       select x).ToList());
        private readonly Task<List<String>> _serverNameTask =
            Task<List<string>>.Factory.StartNew(() => (from x in Settings.Default.ServerNames.Split('|')
                                                       where !x.IsEmpty()
                                                       select x).ToList());
        private int _progressValue;
        private DataTable _tables;

        /// <summary>
        /// </summary>
        public MainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private SqlConnectionStringBuilder BuildConnectionString() {
            var builder = new SqlConnectionStringBuilder();

            cbAuthentication.Invoke(() => {
                builder.IntegratedSecurity = cbAuthentication.SelectedIndex == 0;

                if (builder.IntegratedSecurity) {
                    return;
                }

                builder.UserID = cbLogin.Text;
                builder.Password = txtPassword.Text;
            });

            cbDatabase.Invoke(() => {
                if (cbDatabase.Text.IsEmpty()) {
                    return;
                }

                builder.InitialCatalog = cbDatabase.Text;
            });

            cbServerName.Invoke(() => {
                builder.DataSource = cbServerName.Text;
            });

            return builder;
        }

        /// <summary>
        /// </summary>
        private void End() {
            if (ShowQuestionMessageBox(Resources.ExitMessage) == DialogResult.Yes) {
                progressBar.Visible = false;
                Close();
            }
        }

        /// <summary>
        /// </summary>
        private async void GenerateCode() {
            var outputPath = txtOutputDirectory.Text;
            var progress = new Progress<ProgressResult>(ReportProgress);
            var schema = await ProcessInputFile();
            var tableCount = schema.Tables.Count;

            progressBar.Maximum = 3 + (3 * tableCount) + (2 * tableCount);

            schema.Tables.ForEach(x => x.Initialize(schema.Database));

            var database = schema.Database;
            if (database.ProviderFolder.IsEmpty()) {
                database.ProviderFolder = Database.DefaultProviderFolder;
            }

            var generatorContent = new ContractClassGenerator(schema);
            var generatorDatabaseHelper = new DatabaseHelperGenerator(schema);
            var generatorDTO = new DataTransferObjectGenerator(schema);
            var generatorMetaData = new MetaDataGenerator(schema);
            var generatorProvider = new ProviderClassGenerator(schema);
            var generatorSelection = new SelectionBuilderGenerator(schema);
            var generatorUriType = new UriTypeGenerator(schema);

            var contentResult = await generatorContent.Generate(outputPath, progress);
            var databaseResult = await generatorDatabaseHelper.Generate(outputPath, progress);
            var dtoResult = await generatorDTO.Generate(outputPath, progress);
            var metaResult = await generatorMetaData.Generate(outputPath, progress);
            var providerResult = await generatorProvider.Generate(outputPath, progress);
            var selectionResult = await generatorSelection.Generate(outputPath, progress);
            var uriResult = await generatorUriType.Generate(outputPath, progress);

            FileUtils.SaveFile(contentResult.Path, contentResult.Content);
            FileUtils.SaveFile(databaseResult.Path, databaseResult.Content);
            FileUtils.SaveFile(dtoResult.Path, dtoResult.Content);
            FileUtils.SaveFile(metaResult.Path, metaResult.Content);
            FileUtils.SaveFile(providerResult.Path, providerResult.Content);
            FileUtils.SaveFile(selectionResult.Path, selectionResult.Content);
            FileUtils.SaveFile(uriResult.Path, uriResult.Content);

            End();
        }

        /// <summary>
        /// </summary>
        private async void GenerateJsonFile() {
            var filename = txtJsonFile.Text;
            var schema = await GenerateSchema(filename);

            File.WriteAllText(filename, JsonConvert.SerializeObject(schema, new JsonSerializerSettings{
                ContractResolver = new LowercaseResolver(),
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            }));

            await UpdateJsonFileSettings();
        }

        /// <summary>
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private Task<SchemaDescription> GenerateSchema(string filename) {
            return Task.Run(() => {
                var builder = BuildConnectionString();
                var schema = new SchemaDescription{
                    Database = new Database(),
                    Tables = new List<Table>()
                };

                using (var connection = new SqlConnection(builder.ConnectionString)) {
                    SchemaDescription previousSchema = null;
                    var generateDeviceId = Settings.Default.GenerateDeviceId;
                    var names = cblTables.CheckedItems;

                    connection.Open();

                    foreach (var name in names) {
                        DataTable keys;
                        var dbName = string.Empty;
                        var file = new FileInfo(filename);
                        var table = new Table();
                        var tableInfo = _tables.Select(string.Format("TABLE_NAME = '{0}'", name)).First();

                        cbDatabase.Invoke(() => {
                            dbName = cbDatabase.Text;
                        });

                        var columns = connection.GetSchema("Columns", new[]{
                            dbName, null, name.ToString()
                        }).Select();

                        if (tableInfo == null || !columns.Any()) {
                            continue;
                        }

                        if (file.Exists) {
                            previousSchema = JsonConvert.DeserializeObject<SchemaDescription>(File.ReadAllText(filename));
                            schema.Database = previousSchema.Database;
                        }
                        else {
                            schema.Database.Name = Path.GetFileNameWithoutExtension(file.Name);
                        }

                        schema.Database.ClassesPrefix = dbName;
                        schema.Database.HasDTO = true;

                        table.Name = name.ToString();
                        table.Fields = new List<Field>();

                        var sql = string.Format("SELECT * FROM [{0}].[{1}]", tableInfo["TABLE_SCHEMA"], name);
                        var command = new SqlCommand{
                            Connection = connection,
                            CommandText = sql
                        };

                        using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo)) {
                            keys = reader.GetSchemaTable();
                        }

                        if (generateDeviceId) {
                            var deviceId = new Field{
                                IsId = true,
                                IsNullable = false,
                                Name = "DeviceId",
                                Type = "long"
                            };

                            table.Fields.Add(deviceId);
                        }

                        foreach (var column in columns) {
                            if (keys == null) {
                                break;
                            }

                            var columnName = column["COLUMN_NAME"];
                            var isNullable = column["IS_NULLABLE"].ToString();
                            //var indexes = connection.GetSchema("IndexColumns", new[]{
                            //    dbName, null, name.ToString(), null, columnName.ToString()
                            //}).Select();

                            //foreach (var index in indexes) {
                            //    if (keys == null) {
                            //        break;
                            //    }

                            // var row = keys.Select(string.Format("ColumnName = '{0}'",
                            // columnName) ).First(); var isIdentity =
                            // Convert.ToBoolean(row["IsIdentity"]); var isKey =
                            // Convert.ToBoolean(row["IsKey"]);

                            // field.IsId = isKey || isIdentity; field.IsUnique =
                            // Convert.ToBoolean(row["IsUnique"]);
                            //}

                            var row = keys.Select(string.Format("ColumnName = '{0}'", columnName)).First();
                            var isIdentity = Convert.ToBoolean(row["IsIdentity"]);
                            var isKey = Convert.ToBoolean(row["IsKey"]);

                            var field = new Field{
                                IsId = !generateDeviceId && (isKey || isIdentity),
                                IsIndex = generateDeviceId && (isKey || isIdentity),
                                IsNullable = isNullable == "YES",
                                IsUnique =
                                    (generateDeviceId && (isKey || isIdentity)) || Convert.ToBoolean(row["IsUnique"]),
                                Name = column["COLUMN_NAME"].ToString(),
                                Type = column["DATA_TYPE"].ToString()
                            };

                            field.ConvertSqlDataType();

                            table.Fields.Add(field);
                        }

                        schema.Tables.Add(table);
                    }

                    if (previousSchema != null && ShowQuestionMessageBox(Resources.UpgradeJsonFile) == DialogResult.Yes) {
                        var tables = schema.Tables;
                        var previousTables = previousSchema.Tables;
                        var except = tables.Except(previousTables, Table.TableComparer).ToList();
                        var intersect = tables.Intersect(previousTables, Table.TableComparer).ToList();
                        var versionIncreased = false;

                        if (intersect.Count > 0) {
                            foreach (var table in intersect) {
                                var previous = (from x in previousTables
                                                where x.Equals(table)
                                                select x).SingleOrDefault();

                                if (previous != null) {
                                    table.Joins = previous.Joins;
                                }
                            }

                            foreach (var fields in from table in intersect
                                                   let previous = (from x in previousTables
                                                                   where x.Equals(table)
                                                                   select x).SingleOrDefault()
                                                   where previous != null
                                                   select
                                                       table.Fields.Except(previous.Fields, Field.FieldComparer)
                                                            .ToList()) {
                                if (fields.Count > 0 && !versionIncreased) {
                                    schema.Database.Version += 1;
                                    versionIncreased = true;
                                }

                                fields.ForEach(x => x.Version = schema.Database.Version);
                            }
                        }

                        if (except.Count > 0) {
                            if (!versionIncreased) {
                                schema.Database.Version += 1;
                            }

                            except.ForEach(x => x.Version = schema.Database.Version);
                        }
                    }
                }

                return schema;
            });
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private Task<List<string>> GetDatabaseNames() {
            return Task.Run(() => {
                var builder = BuildConnectionString();
                var names = new List<string>();

                using (var connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    var table = connection.GetSchema("Databases");
                    var rows = table.Select();

                    names.AddRange(rows.Select(x => x["DATABASE_NAME"].ToString()).OrderBy(x => x));
                }

                return names;
            });
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private Task<List<string>> GetTableNames() {
            return Task.Run(() => {
                var builder = BuildConnectionString();
                var names = new List<string>();

                using (var connection = new SqlConnection(builder.ConnectionString)) {
                    string name = null;

                    cbDatabase.Invoke(() => {
                        name = cbDatabase.Text;
                    });

                    connection.Open();

                    _tables = connection.GetSchema("Tables", new[]{
                        name, null, null, "BASE TABLE"
                    });

                    var rows = _tables.Select();

                    names.AddRange(rows.Select(x => x["TABLE_NAME"].ToString()).Where(x => !x.StartsWith("_")));
                }

                return names;
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleButtonClick(object sender, EventArgs e) {
            var button = sender as Button;

            if (button == btnGenerate) {
                progressBar.Visible = true;
                GenerateCode();
            }
            else if (button == btnGenerateJson) {
                if (ValidateChildren()) {
                    GenerateJsonFile();
                }
            }
            else if (button == btnJsonFile) {
                var directory = Settings.Default.JsonDirectory;

                if (!directory.IsEmpty()) {
                    outputDialog.SelectedPath = directory;
                }

                if (outputDialog.ShowDialog(this) == DialogResult.OK) {
                    var path = outputDialog.SelectedPath;

                    Settings.Default.JsonDirectory = path;

                    txtJsonFile.Text = Path.Combine(path, cbDatabase.Text.ToLower()) + @".json";
                }
            }
            else if (button == btnOpenInput) {
                if (inputDialog.ShowDialog(this) == DialogResult.OK) {
                    txtInputFile.Text = inputDialog.FileName;
                }
            }
            else if (button == btnOpenOutput) {
                if (outputDialog.ShowDialog(this) == DialogResult.OK) {
                    txtOutputDirectory.Text = outputDialog.SelectedPath;
                }
            }
            else if (button == btnSelectAll || button == btnUnselect) {
                var state = button == btnSelectAll;

                for (int i = 0, count = cblTables.Items.Count - 1; i <= count; i++) {
                    cblTables.SetItemChecked(i, state);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleDropDown(object sender, EventArgs e) {
            var control = sender as ComboBox;

            if (control == null) {
                return;
            }

            if (control == cbDatabase) {
                PopulateDatabases();
            }

            if (control == cbLogin) {
                PopulateLoginNames();
            }

            if (control == cbServerName) {
                PopulateServerNames();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandleFormClosing(object sender, FormClosingEventArgs e) {
            var settings = Settings.Default;

            settings.InputFile = txtInputFile.Text;
            settings.OutputDirectory = txtOutputDirectory.Text;

            await UpdateJsonFileSettings(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleFormLoad(object sender, EventArgs e) {
            var settings = Settings.Default;

            cbAuthentication.SelectedIndex = 0;

            txtInputFile.Text = settings.InputFile;
            txtOutputDirectory.Text = settings.OutputDirectory;

            btnGenerate.Enabled = !(txtInputFile.Text.IsEmpty() && txtOutputDirectory.Text.IsEmpty());

            cbServerName.Select();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleLeave(object sender, EventArgs e) {
            var control = sender as ComboBox;

            if (control == null) {
                return;
            }

            if (control == cbLogin && cbLogin.Enabled) {
                if (!control.Text.IsEmpty()) {
                    var index = control.FindString(control.Text);

                    control.SelectedIndex = index;
                }
            }

            if (control == cbServerName) {
                if (!control.Text.IsEmpty()) {
                    var index = control.FindString(control.Text);

                    control.SelectedIndex = index;
                }
                //for (var i = 0; i < cbServerName.Items.Count; i += 1) {
                //    var item = cbServerName.Items[i].ToString();

                // if (String.Equals(item, cbServerName.Text,
                // StringComparison.InvariantCultureIgnoreCase)) { cbServerName.SelectedIndex = i; }
                //}
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMenuItemClick(object sender, EventArgs e) {
            var menuItem = sender as ToolStripMenuItem;

            if (menuItem == aboutToolStripMenuItem) {
                var form = new AboutForm();

                form.ShowDialog(this);
            }
            else if (menuItem == exitToolStripMenuItem) {
                Close();
            }
            else if (menuItem == settingsToolStripMenuItem) {
                var form = new SettingsForm();

                form.ShowDialog(this);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSelectedIndexChanged(object sender, EventArgs e) {
            var control = sender as ComboBox;

            if (control == null) {
                return;
            }

            if (control == cbAuthentication) {
                bool isEnabled;
                string text;

                cbLogin.Items.Clear();
                cbLogin.Text = string.Empty;

                switch (control.SelectedIndex) {
                    case 0: {
                        var identity = WindowsIdentity.GetCurrent();

                        if (identity != null) {
                            cbLogin.Items.Add(identity.Name);
                            cbLogin.SelectedIndex = 0;
                        }

                        isEnabled = false;
                        text = "User name:";
                        break;
                    }
                    default: {
                        isEnabled = true;
                        text = "Login:";
                        break;
                    }
                }

                lblLogin.Enabled = isEnabled;
                lblLogin.Text = text;

                lblPassword.Enabled = isEnabled;

                cbLogin.Enabled = isEnabled;
                txtPassword.Enabled = isEnabled;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTextChanged(object sender, EventArgs e) {
            var control = sender as Control;

            if (control == null) {
                return;
            }

            if (control == cbDatabase) {
                PopulateDatabaseTables();
                return;
            }
            if (control == cbServerName) {
                PopulateServerNames();
                return;
            }

            btnGenerate.Enabled = !txtInputFile.Text.IsEmpty() && !txtOutputDirectory.Text.IsEmpty();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleValidated(object sender, EventArgs e) {
            var control = sender as Control;

            if (control == null) {
                return;
            }

            errorProvider.SetError(control, string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleValidating(object sender, System.ComponentModel.CancelEventArgs e) {
            var cancel = false;
            var control = sender as Control;
            var message = "";

            if (control == null) {
                return;
            }

            if (sender == cbDatabase) {
                if (cbDatabase.Text.IsEmpty()) {
                    cancel = true;
                    message = "Database is required";
                }
            }
            else if (sender == cblTables) {
                if (cblTables.Items.Count == 0 || cblTables.CheckedItems.Count == 0) {
                    cancel = true;
                    message = "Must select at least 1 table";
                }
            }
            else if (sender == cbLogin) {
                if (cbLogin.Text.IsEmpty()) {
                    cancel = true;
                    message = "Login is required";
                }
            }
            else if (sender == cbServerName) {
                if (cbServerName.Text.IsEmpty()) {
                    cancel = true;
                    message = "Server name is required";
                }
            }
            else if (sender == txtJsonFile) {
                if (txtJsonFile.Text.IsEmpty()) {
                    cancel = true;
                    message = "Json file is required";
                }
            }
            else if (sender == txtPassword) {
                if (txtPassword.Text.IsEmpty() && cbAuthentication.SelectedIndex == 1) {
                    cancel = true;
                    message = "Password is required";
                }
            }

            e.Cancel = cancel;

            if (cancel) {
                errorProvider.SetError(control, message);
            }
        }

        /// <summary>
        /// </summary>
        private async void PopulateDatabases() {
            var names = await GetDatabaseNames();

            cbDatabase.Items.Clear();
            foreach (var name in names) {
                cbDatabase.Items.Add(name);
            }
        }

        /// <summary>
        /// </summary>
        private async void PopulateDatabaseTables() {
            var enabled = !cbDatabase.Text.IsEmpty();

            btnJsonFile.Enabled = enabled;
            btnSelectAll.Enabled = enabled;
            btnUnselect.Enabled = enabled;
            cblTables.Enabled = enabled;
            lblJsonFile.Enabled = enabled;
            lblTables.Enabled = enabled;
            txtJsonFile.Enabled = enabled;

            var names = await GetTableNames();

            cblTables.Items.Clear();

            foreach (var name in names) {
                cblTables.Items.Add(name);
            }
        }

        /// <summary>
        /// </summary>
        private async void PopulateLoginNames() {
            var task = Task<List<string>>.Factory.StartNew(() => (from x in Settings.Default.Login.Split(',')
                                                                  where !x.IsEmpty()
                                                                  select x).ToList());

            await task;

            foreach (var x in from name in task.Result
                              let items = cbLogin.Items.Cast<String>()
                              where !items.Contains(name)
                              select name) {
                cbLogin.Items.Add(x);
            }
        }

        /// <summary>
        /// </summary>
        private async void PopulateServerNames() {
            var task = Task<List<string>>.Factory.StartNew(() => (from x in Settings.Default.ServerNames.Split('|')
                                                                  where !x.IsEmpty()
                                                                  select x).ToList());

            await task;

            foreach (var x in from name in task.Result
                              let items = cbServerName.Items.Cast<String>()
                              where !items.Contains(name)
                              select name) {
                cbServerName.Items.Add(x);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private Task<SchemaDescription> ProcessInputFile() {
            var json = File.ReadAllText(txtInputFile.Text);
            var settings = new JsonSerializerSettings();

            return JsonConvert.DeserializeObjectAsync<SchemaDescription>(json, settings);
        }

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        private void ReportProgress(ProgressResult result) {
            _progressValue += result.Value;
            progressBar.Value = _progressValue;
            Debug.WriteLine("{0}: {1}", result.Name, _progressValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private DialogResult ShowQuestionMessageBox(string message) {
            return MessageBox.Show(message, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                   MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// </summary>
        /// <param name="displayConfirmation"></param>
        /// <returns></returns>
        private async Task UpdateJsonFileSettings(bool displayConfirmation = true) {
            var settings = Settings.Default;

            await _serverNameTask;

            var currentName = cbServerName.Text;
            var serverNames = _serverNameTask.Result;

            if (!serverNames.Contains(currentName)) {
                serverNames.Add(currentName);
            }

            settings.ServerNames = string.Join("|", serverNames);

            if (cbLogin.Enabled) {
                await _loginTask;

                var loginNames = _loginTask.Result;

                if (!loginNames.Contains(cbLogin.Text)) {
                    loginNames.Add(cbLogin.Text);
                }

                settings.Login = string.Join(",", loginNames);
            }

            if (displayConfirmation) {
                if (ShowQuestionMessageBox(Resources.GenerateContentProvider) == DialogResult.Yes) {
                    tabStrip.SelectTab(1);
                    txtInputFile.Text = txtJsonFile.Text;
                    btnOpenOutput.PerformClick();
                }
                else {
                    End();
                }
            }
            else {
                settings.Save();
            }
        }
    }
}