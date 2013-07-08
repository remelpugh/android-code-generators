namespace Dabay6.Android.ContentProvider
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnGenerate = new System.Windows.Forms.Button();
            this.inputDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnOpenInput = new System.Windows.Forms.Button();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.outputDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabStrip = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnUnselect = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnJsonFile = new System.Windows.Forms.Button();
            this.lblJsonFile = new System.Windows.Forms.Label();
            this.cblTables = new System.Windows.Forms.CheckedListBox();
            this.txtJsonFile = new System.Windows.Forms.TextBox();
            this.lblTables = new System.Windows.Forms.Label();
            this.cbDatabase = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cbLogin = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.cbAuthentication = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbServerName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerateJson = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabStrip.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            resources.ApplyResources(this.btnGenerate, "btnGenerate");
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // inputDialog
            // 
            this.inputDialog.CheckFileExists = false;
            resources.ApplyResources(this.inputDialog, "inputDialog");
            this.inputDialog.Multiselect = true;
            // 
            // txtInputFile
            // 
            resources.ApplyResources(this.txtInputFile, "txtInputFile");
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.ReadOnly = true;
            this.txtInputFile.TextChanged += new System.EventHandler(this.HandleTextChanged);
            // 
            // btnOpenInput
            // 
            resources.ApplyResources(this.btnOpenInput, "btnOpenInput");
            this.btnOpenInput.Name = "btnOpenInput";
            this.btnOpenInput.UseVisualStyleBackColor = true;
            this.btnOpenInput.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // btnOpenOutput
            // 
            resources.ApplyResources(this.btnOpenOutput, "btnOpenOutput");
            this.btnOpenOutput.Name = "btnOpenOutput";
            this.btnOpenOutput.UseVisualStyleBackColor = true;
            this.btnOpenOutput.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // outputDialog
            // 
            this.outputDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // txtOutputDirectory
            // 
            resources.ApplyResources(this.txtOutputDirectory, "txtOutputDirectory");
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.ReadOnly = true;
            this.txtOutputDirectory.TextChanged += new System.EventHandler(this.HandleTextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tabStrip
            // 
            resources.ApplyResources(this.tabStrip, "tabStrip");
            this.tabStrip.Controls.Add(this.tabPage1);
            this.tabStrip.Controls.Add(this.tabPage2);
            this.tabStrip.Name = "tabStrip";
            this.tabStrip.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUnselect);
            this.tabPage1.Controls.Add(this.btnSelectAll);
            this.tabPage1.Controls.Add(this.btnJsonFile);
            this.tabPage1.Controls.Add(this.lblJsonFile);
            this.tabPage1.Controls.Add(this.cblTables);
            this.tabPage1.Controls.Add(this.txtJsonFile);
            this.tabPage1.Controls.Add(this.lblTables);
            this.tabPage1.Controls.Add(this.cbDatabase);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.cbLogin);
            this.tabPage1.Controls.Add(this.lblPassword);
            this.tabPage1.Controls.Add(this.lblLogin);
            this.tabPage1.Controls.Add(this.cbAuthentication);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cbServerName);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.btnGenerateJson);
            this.tabPage1.Controls.Add(this.label5);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnUnselect
            // 
            resources.ApplyResources(this.btnUnselect, "btnUnselect");
            this.btnUnselect.Name = "btnUnselect";
            this.btnUnselect.UseVisualStyleBackColor = true;
            this.btnUnselect.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // btnJsonFile
            // 
            resources.ApplyResources(this.btnJsonFile, "btnJsonFile");
            this.btnJsonFile.Name = "btnJsonFile";
            this.btnJsonFile.UseVisualStyleBackColor = true;
            this.btnJsonFile.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // lblJsonFile
            // 
            resources.ApplyResources(this.lblJsonFile, "lblJsonFile");
            this.lblJsonFile.Name = "lblJsonFile";
            // 
            // cblTables
            // 
            resources.ApplyResources(this.cblTables, "cblTables");
            this.cblTables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cblTables.CheckOnClick = true;
            this.cblTables.FormattingEnabled = true;
            this.cblTables.Name = "cblTables";
            this.cblTables.Sorted = true;
            this.cblTables.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidating);
            this.cblTables.Validated += new System.EventHandler(this.HandleValidated);
            // 
            // txtJsonFile
            // 
            resources.ApplyResources(this.txtJsonFile, "txtJsonFile");
            this.txtJsonFile.Name = "txtJsonFile";
            this.txtJsonFile.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidating);
            this.txtJsonFile.Validated += new System.EventHandler(this.HandleValidated);
            // 
            // lblTables
            // 
            resources.ApplyResources(this.lblTables, "lblTables");
            this.lblTables.Name = "lblTables";
            // 
            // cbDatabase
            // 
            resources.ApplyResources(this.cbDatabase, "cbDatabase");
            this.cbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.DropDown += new System.EventHandler(this.HandleDropDown);
            this.cbDatabase.TextChanged += new System.EventHandler(this.HandleTextChanged);
            this.cbDatabase.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidating);
            this.cbDatabase.Validated += new System.EventHandler(this.HandleValidated);
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidating);
            this.txtPassword.Validated += new System.EventHandler(this.HandleValidated);
            // 
            // cbLogin
            // 
            resources.ApplyResources(this.cbLogin, "cbLogin");
            this.cbLogin.FormattingEnabled = true;
            this.cbLogin.Name = "cbLogin";
            this.cbLogin.DropDown += new System.EventHandler(this.HandleDropDown);
            this.cbLogin.Leave += new System.EventHandler(this.HandleLeave);
            this.cbLogin.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidating);
            this.cbLogin.Validated += new System.EventHandler(this.HandleValidated);
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            // 
            // lblLogin
            // 
            resources.ApplyResources(this.lblLogin, "lblLogin");
            this.lblLogin.Name = "lblLogin";
            // 
            // cbAuthentication
            // 
            resources.ApplyResources(this.cbAuthentication, "cbAuthentication");
            this.cbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuthentication.FormattingEnabled = true;
            this.cbAuthentication.Items.AddRange(new object[] {
            resources.GetString("cbAuthentication.Items"),
            resources.GetString("cbAuthentication.Items1")});
            this.cbAuthentication.Name = "cbAuthentication";
            this.cbAuthentication.SelectedIndexChanged += new System.EventHandler(this.HandleSelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbServerName
            // 
            resources.ApplyResources(this.cbServerName, "cbServerName");
            this.cbServerName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbServerName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbServerName.FormattingEnabled = true;
            this.cbServerName.Name = "cbServerName";
            this.cbServerName.DropDown += new System.EventHandler(this.HandleDropDown);
            this.cbServerName.SelectedIndexChanged += new System.EventHandler(this.HandleSelectedIndexChanged);
            this.cbServerName.TextChanged += new System.EventHandler(this.HandleTextChanged);
            this.cbServerName.Leave += new System.EventHandler(this.HandleLeave);
            this.cbServerName.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidating);
            this.cbServerName.Validated += new System.EventHandler(this.HandleValidated);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnGenerateJson
            // 
            resources.ApplyResources(this.btnGenerateJson, "btnGenerateJson");
            this.btnGenerateJson.Name = "btnGenerateJson";
            this.btnGenerateJson.UseVisualStyleBackColor = true;
            this.btnGenerateJson.Click += new System.EventHandler(this.HandleButtonClick);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnOpenInput);
            this.tabPage2.Controls.Add(this.progressBar);
            this.tabPage2.Controls.Add(this.txtInputFile);
            this.tabPage2.Controls.Add(this.btnGenerate);
            this.tabPage2.Controls.Add(this.btnOpenOutput);
            this.tabPage2.Controls.Add(this.txtOutputDirectory);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.HandleMenuItemClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.HandleMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.HandleMenuItemClick);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnGenerate;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.tabStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.HandleFormLoad);
            this.tabStrip.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.OpenFileDialog inputDialog;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnOpenInput;
        private System.Windows.Forms.Button btnOpenOutput;
        private System.Windows.Forms.FolderBrowserDialog outputDialog;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TabControl tabStrip;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cbDatabase;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cbLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.ComboBox cbAuthentication;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbServerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGenerateJson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckedListBox cblTables;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.TextBox txtJsonFile;
        private System.Windows.Forms.Label lblJsonFile;
        private System.Windows.Forms.Button btnJsonFile;
        private System.Windows.Forms.Button btnUnselect;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

