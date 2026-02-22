using System.Drawing;
using System.Windows.Forms;
using DBClip.Models;
using DBClip.Services;

namespace DBClip.Views;

public class DatabaseSettingsDialog : Form
{
    private readonly List<DatabaseSettings> _databases;
    private readonly IDatabaseService _databaseService;
    private ComboBox cmbDatabase;
    private Button btnAdd;
    private Button btnRemove;
    private ComboBox cmbProvider;
    private TextBox txtConnectionString;
    private Button btnTest;
    private Button btnOK;
    private Button btnCancel;
    private Label lblDatabase;
    private Label lblProvider;
    private Label lblConnectionString;
    private Label lblStatus;
    private TableLayoutPanel mainTable;
    private FlowLayoutPanel buttonPanel;
    private FlowLayoutPanel dbButtonPanel;

    public DatabaseSettingsDialog(List<DatabaseSettings> databases)
    {
        _databases = databases;
        _databaseService = new DatabaseService();
        InitializeComponent();
        LoadDatabases();
    }

    private void InitializeComponent()
    {
        this.cmbDatabase = new System.Windows.Forms.ComboBox();
        this.btnAdd = new System.Windows.Forms.Button();
        this.btnRemove = new System.Windows.Forms.Button();
        this.cmbProvider = new System.Windows.Forms.ComboBox();
        this.txtConnectionString = new System.Windows.Forms.TextBox();
        this.btnTest = new System.Windows.Forms.Button();
        this.btnOK = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblDatabase = new System.Windows.Forms.Label();
        this.lblProvider = new System.Windows.Forms.Label();
        this.lblConnectionString = new System.Windows.Forms.Label();
        this.lblStatus = new System.Windows.Forms.Label();
        this.mainTable = new System.Windows.Forms.TableLayoutPanel();
        this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
        this.dbButtonPanel = new System.Windows.Forms.FlowLayoutPanel();

        this.SuspendLayout();

        // 
        // mainTable
        // 
        this.mainTable.ColumnCount = 2;
        this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
        this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mainTable.Location = new System.Drawing.Point(12, 12);
        this.mainTable.Name = "mainTable";
        this.mainTable.RowCount = 5;
        this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
        this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
        this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
        this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
        this.mainTable.Size = new System.Drawing.Size(570, 280);
        this.mainTable.TabIndex = 0;

        // 
        // lblDatabase
        // 
        this.lblDatabase.AutoSize = true;
        this.lblDatabase.Location = new System.Drawing.Point(3, 8);
        this.lblDatabase.Name = "lblDatabase";
        this.lblDatabase.Size = new System.Drawing.Size(75, 20);
        this.lblDatabase.Text = "Database:";
        this.lblDatabase.Anchor = AnchorStyles.Left;

        // 
        // cmbDatabase
        // 
        this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
        this.cmbDatabase.FormattingEnabled = true;
        this.cmbDatabase.Location = new System.Drawing.Point(120, 5);
        this.cmbDatabase.Name = "cmbDatabase";
        this.cmbDatabase.Size = new System.Drawing.Size(250, 28);
        this.cmbDatabase.TabIndex = 0;
        this.cmbDatabase.TextChanged += new System.EventHandler(this.cmbDatabase_TextChanged);

        // 
        // dbButtonPanel
        // 
        this.dbButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
        this.dbButtonPanel.Location = new System.Drawing.Point(380, 3);
        this.dbButtonPanel.Name = "dbButtonPanel";
        this.dbButtonPanel.Size = new System.Drawing.Size(180, 30);
        this.dbButtonPanel.TabIndex = 0;

        // 
        // btnAdd
        // 
        this.btnAdd.Location = new System.Drawing.Point(3, 3);
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size(80, 25);
        this.btnAdd.TabIndex = 0;
        this.btnAdd.Text = "Add New";
        this.btnAdd.UseVisualStyleBackColor = true;
        this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

        // 
        // btnRemove
        // 
        this.btnRemove.Location = new System.Drawing.Point(89, 3);
        this.btnRemove.Name = "btnRemove";
        this.btnRemove.Size = new System.Drawing.Size(80, 25);
        this.btnRemove.TabIndex = 1;
        this.btnRemove.Text = "Remove";
        this.btnRemove.UseVisualStyleBackColor = true;
        this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);

        dbButtonPanel.Controls.Add(btnAdd);
        dbButtonPanel.Controls.Add(btnRemove);

        // 
        // lblProvider
        // 
        this.lblProvider.AutoSize = true;
        this.lblProvider.Location = new System.Drawing.Point(3, 40);
        this.lblProvider.Name = "lblProvider";
        this.lblProvider.Size = new System.Drawing.Size(68, 20);
        this.lblProvider.Text = "Provider:";
        this.lblProvider.Anchor = AnchorStyles.Left;

        // 
        // cmbProvider
        // 
        this.cmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbProvider.FormattingEnabled = true;
        this.cmbProvider.Items.AddRange(new object[] { "SqlServer", "PostgreSQL", "MySQL", "SQLite", "Sybase" });
        this.cmbProvider.Location = new System.Drawing.Point(120, 37);
        this.cmbProvider.Name = "cmbProvider";
        this.cmbProvider.Size = new System.Drawing.Size(350, 28);
        this.cmbProvider.TabIndex = 1;
        this.cmbProvider.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        // 
        // lblConnectionString
        // 
        this.lblConnectionString.AutoSize = true;
        this.lblConnectionString.Location = new System.Drawing.Point(3, 70);
        this.lblConnectionString.Name = "lblConnectionString";
        this.lblConnectionString.Size = new System.Drawing.Size(130, 20);
        this.lblConnectionString.Text = "Connection String:";
        this.lblConnectionString.Anchor = AnchorStyles.Left;

        // 
        // txtConnectionString
        // 
        this.txtConnectionString.Font = new System.Drawing.Font("Consolas", 9F);
        this.txtConnectionString.Location = new System.Drawing.Point(120, 67);
        this.txtConnectionString.Multiline = true;
        this.txtConnectionString.Name = "txtConnectionString";
        this.txtConnectionString.Size = new System.Drawing.Size(440, 150);
        this.txtConnectionString.TabIndex = 2;
        this.txtConnectionString.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(3, 0);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(0, 20);

        // 
        // buttonPanel
        // 
        this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
        this.buttonPanel.Location = new System.Drawing.Point(3, 0);
        this.buttonPanel.Name = "buttonPanel";
        this.buttonPanel.Size = new System.Drawing.Size(564, 50);
        this.buttonPanel.TabIndex = 0;
        this.buttonPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        // 
        // btnTest
        // 
        this.btnTest.Location = new System.Drawing.Point(3, 3);
        this.btnTest.Name = "btnTest";
        this.btnTest.Size = new System.Drawing.Size(120, 32);
        this.btnTest.TabIndex = 0;
        this.btnTest.Text = "Test Connection";
        this.btnTest.UseVisualStyleBackColor = true;
        this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

        // 
        // btnOK
        // 
        this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOK.Location = new System.Drawing.Point(3, 3);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(90, 32);
        this.btnOK.TabIndex = 1;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

        // 
        // btnCancel
        // 
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(3, 3);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(90, 32);
        this.btnCancel.TabIndex = 2;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;

        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(btnOK);
        buttonPanel.Controls.Add(btnTest);

        mainTable.Controls.Add(lblDatabase, 0, 0);
        mainTable.Controls.Add(cmbDatabase, 1, 0);
        mainTable.Controls.Add(dbButtonPanel, 1, 0);
        mainTable.Controls.Add(lblProvider, 0, 1);
        mainTable.Controls.Add(cmbProvider, 1, 1);
        mainTable.Controls.Add(lblConnectionString, 0, 2);
        mainTable.Controls.Add(txtConnectionString, 1, 2);
        mainTable.Controls.Add(lblStatus, 0, 3);
        mainTable.Controls.Add(buttonPanel, 1, 4);

        // 
        // DatabaseSettingsDialog
        // 
        this.AcceptButton = this.btnOK;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(600, 350);
        this.MinimumSize = new System.Drawing.Size(550, 300);
        this.Controls.Add(this.mainTable);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        this.MaximizeBox = true;
        this.MinimizeBox = true;
        this.Name = "DatabaseSettingsDialog";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Database Settings";
        this.ResumeLayout(false);
    }

    private void LoadDatabases()
    {
        foreach (var db in _databases)
        {
            cmbDatabase.Items.Add(db.Name);
        }
        
        if (cmbDatabase.Items.Count > 0)
        {
            cmbDatabase.SelectedIndex = 0;
            LoadSelectedDatabase();
        }
    }

    private void cmbDatabase_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadSelectedDatabase();
    }

    private void cmbDatabase_TextChanged(object? sender, EventArgs e)
    {
        var oldName = _currentDbName;
        var newName = cmbDatabase.Text;
        
        if (!string.IsNullOrWhiteSpace(newName) && oldName != null && oldName != newName)
        {
            var db = _databases.FirstOrDefault(d => d.Name == oldName);
            if (db != null && !_databases.Any(d => d.Name == newName))
            {
                db.Name = newName;
                _currentDbName = newName;
                
                var index = cmbDatabase.Items.IndexOf(oldName);
                if (index >= 0)
                {
                    cmbDatabase.Items[index] = newName;
                }
            }
        }
    }

    private string? _currentDbName;

    private void LoadSelectedDatabase()
    {
        var dbName = cmbDatabase.Text;
        _currentDbName = dbName;
        
        var db = _databases.FirstOrDefault(d => d.Name == dbName);
        if (db != null)
        {
            cmbProvider.SelectedItem = db.Provider;
            txtConnectionString.Text = db.ConnectionString;
            btnRemove.Enabled = _databases.Count > 1;
        }
    }

    private void btnAdd_Click(object? sender, EventArgs e)
    {
        var newName = "New Database";
        var counter = 1;
        while (_databases.Any(d => d.Name == newName))
        {
            newName = $"New Database {counter++}";
        }
        
        var newDb = new DatabaseSettings { Name = newName };
        _databases.Add(newDb);
        cmbDatabase.Items.Add(newName);
        cmbDatabase.Text = newName;
    }

    private void btnRemove_Click(object? sender, EventArgs e)
    {
        var dbName = cmbDatabase.Text;
        if (!string.IsNullOrEmpty(dbName) && _databases.Count > 1)
        {
            var db = _databases.FirstOrDefault(d => d.Name == dbName);
            if (db != null)
            {
                _databases.Remove(db);
                cmbDatabase.Items.Remove(dbName);
                cmbDatabase.SelectedIndex = 0;
            }
        }
    }

    private async void btnTest_Click(object? sender, EventArgs e)
    {
        var testSettings = new DatabaseSettings
        {
            Provider = cmbProvider.SelectedItem?.ToString() ?? "SqlServer",
            ConnectionString = txtConnectionString.Text
        };

        lblStatus.Text = "Testing...";
        lblStatus.ForeColor = SystemColors.ControlText;

        var success = await _databaseService.TestConnectionAsync(testSettings);

        if (success)
        {
            lblStatus.Text = "Connection successful!";
            lblStatus.ForeColor = Color.Green;
        }
        else
        {
            lblStatus.Text = "Connection failed!";
            lblStatus.ForeColor = Color.Red;
        }
    }

    private void btnOK_Click(object? sender, EventArgs e)
    {
        if (cmbDatabase.SelectedItem is string dbName)
        {
            var db = _databases.FirstOrDefault(d => d.Name == dbName);
            if (db != null)
            {
                db.Provider = cmbProvider.SelectedItem?.ToString() ?? "SqlServer";
                db.ConnectionString = txtConnectionString.Text;
            }
        }
        DialogResult = DialogResult.OK;
    }
}
