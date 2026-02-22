namespace DBClip.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPlay = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnDatabase = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.btnAddScript = new System.Windows.Forms.ToolStripButton();
            this.btnAddFolder = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUndo = new System.Windows.Forms.ToolStripButton();
            this.btnRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblRowCount = new System.Windows.Forms.ToolStripLabel();
            this.numRowCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.chkCacheResults = new System.Windows.Forms.ToolStripButton();
            this.btnResetLayout = new System.Windows.Forms.ToolStripButton();
            this.cmbDatabaseSelect = new System.Windows.Forms.ToolStripComboBox();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerMain.SplitterWidth = 6;
            this.panelTreeViewContainer = new System.Windows.Forms.Panel();
            this.panelSearchBar = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.treeViewScripts = new System.Windows.Forms.TreeView();
            this.splitContainerMiddle = new System.Windows.Forms.SplitContainer();
            this.splitContainerMiddle.SplitterWidth = 6;
            this.tableLayoutPanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.txtScriptEditor = new System.Windows.Forms.RichTextBox();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.splitterRight = new System.Windows.Forms.Splitter();
            this.dgvContext = new System.Windows.Forms.DataGridView();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStripTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMiddle)).BeginInit();
            this.splitContainerMiddle.Panel1.SuspendLayout();
            this.splitContainerMiddle.Panel2.SuspendLayout();
            this.splitContainerMiddle.SuspendLayout();
            this.panelTreeViewContainer.SuspendLayout();
            this.panelSearchBar.SuspendLayout();
            this.tableLayoutPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContext)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStripTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPlay,
            this.btnStop,
            this.btnSave,
            this.btnDatabase,
            this.toolStripSeparator1,
            this.btnAddScript,
            this.btnAddFolder,
            this.btnDelete,
            this.toolStripSeparator2,
            this.btnUndo,
            this.btnRedo,
            this.toolStripSeparator3,
            this.lblRowCount,
            this.numRowCount,
            this.chkCacheResults,
            this.btnResetLayout,
            this.toolStripSeparator4,
            this.cmbDatabaseSelect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1234, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmbDatabaseSelect
            // 
            this.cmbDatabaseSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseSelect.Name = "cmbDatabaseSelect";
            this.cmbDatabaseSelect.Size = new System.Drawing.Size(150, 28);
            this.cmbDatabaseSelect.SelectedIndexChanged += new System.EventHandler(this.cmbDatabaseSelect_SelectedIndexChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPlay.Image = CreatePlayIcon();
            this.btnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(36, 36);
            this.btnPlay.Text = "Run Script";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = CreateStopIcon();
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(36, 36);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSave
            //
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = CreateSaveIcon();
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(36, 36);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDatabase
            // 
            this.btnDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDatabase.Image = CreateDatabaseIcon();
            this.btnDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDatabase.Name = "btnDatabase";
            this.btnDatabase.Size = new System.Drawing.Size(36, 36);
            this.btnDatabase.Text = "Database Settings";
            this.btnDatabase.Click += new System.EventHandler(this.btnDatabase_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // btnAddScript
            // 
            this.btnAddScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddScript.Image = CreateScriptIcon();
            this.btnAddScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddScript.Name = "btnAddScript";
            this.btnAddScript.Size = new System.Drawing.Size(36, 36);
            this.btnAddScript.Text = "Add Script";
            this.btnAddScript.Click += new System.EventHandler(this.btnAddScript_Click);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddFolder.Image = CreateFolderIcon();
            this.btnAddFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(36, 36);
            this.btnAddFolder.Text = "Add Folder";
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = CreateDeleteIcon();
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(36, 36);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // btnUndo
            // 
            this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUndo.Image = CreateUndoIcon();
            this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(36, 36);
            this.btnUndo.Text = "Undo";
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRedo.Image = CreateRedoIcon();
            this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(36, 36);
            this.btnRedo.Text = "Redo";
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // lblRowCount
            // 
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(65, 20);
            this.lblRowCount.Text = "Row Count:";
            // 
            // numRowCount
            // 
            this.numRowCount.Name = "numRowCount";
            this.numRowCount.Size = new System.Drawing.Size(60, 28);
            this.numRowCount.Text = "1000";
            this.numRowCount.Leave += new System.EventHandler(this.numRowCount_Leave);
            // 
            // chkCacheResults
            // 
            this.chkCacheResults.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.chkCacheResults.Image = CreateCacheOffIcon();
            this.chkCacheResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkCacheResults.Name = "chkCacheResults";
            this.chkCacheResults.Size = new System.Drawing.Size(90, 36);
            this.chkCacheResults.Text = "Cache Off";
            this.chkCacheResults.Click += new System.EventHandler(this.chkCacheResults_Click);
            // 
            // btnResetLayout
            //
            this.btnResetLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnResetLayout.Image = CreateResetIcon();
            this.btnResetLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetLayout.Name = "btnResetLayout";
            this.btnResetLayout.Size = new System.Drawing.Size(36, 36);
            this.btnResetLayout.Text = "Reset Layout";
            this.btnResetLayout.Click += new System.EventHandler(this.btnResetLayout_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.IsSplitterFixed = false;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 39);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.panelTreeViewContainer);
            this.splitContainerMain.Panel1MinSize = 100;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerMiddle);
            this.splitContainerMain.Panel2MinSize = 300;
            this.splitContainerMain.Size = new System.Drawing.Size(1234, 629);
            this.splitContainerMain.SplitterDistance = 300;
            this.splitContainerMain.TabIndex = 1;
            // 
            // splitContainerMiddle
            // 
            this.splitContainerMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMiddle.IsSplitterFixed = false;
            this.splitContainerMiddle.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMiddle.Name = "splitContainerMiddle";
            this.splitContainerMiddle.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.splitContainerMiddle.SplitterDistance = 750;
            // 
            // splitContainerMiddle.Panel1
            // 
            this.splitContainerMiddle.Panel1.Controls.Add(this.tableLayoutPanelRight);
            this.splitContainerMiddle.Panel1MinSize = 100;
            // 
            // splitContainerMiddle.Panel2
            // 
            this.splitContainerMiddle.Panel2.Controls.Add(this.dgvContext);
            this.splitContainerMiddle.Panel2MinSize = 100;
            this.splitContainerMiddle.Size = new System.Drawing.Size(1034, 629);
            // 
            // panelTreeViewContainer
            // 
            this.panelTreeViewContainer.Controls.Add(this.treeViewScripts);
            this.panelTreeViewContainer.Controls.Add(this.panelSearchBar);
            this.panelTreeViewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTreeViewContainer.Location = new System.Drawing.Point(0, 0);
            this.panelTreeViewContainer.Name = "panelTreeViewContainer";
            this.panelTreeViewContainer.Size = new System.Drawing.Size(300, 628);
            this.panelTreeViewContainer.TabIndex = 1;
            // 
            // panelSearchBar
            // 
            this.panelSearchBar.Controls.Add(this.txtSearch);
            this.panelSearchBar.Controls.Add(this.btnClearSearch);
            this.panelSearchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearchBar.Location = new System.Drawing.Point(0, 0);
            this.panelSearchBar.Name = "panelSearchBar";
            this.panelSearchBar.Padding = new System.Windows.Forms.Padding(8);
            this.panelSearchBar.Size = new System.Drawing.Size(300, 144);
            this.panelSearchBar.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(8, 8);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Search scripts...";
            this.txtSearch.Size = new System.Drawing.Size(234, 200);
            this.txtSearch.TabIndex = 0;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClearSearch.Location = new System.Drawing.Point(242, 8);
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(250, 28);
            this.btnClearSearch.TabIndex = 1;
            this.btnClearSearch.Text = "X";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            //
            // panelTreeViewContainer
            // 
            this.panelTreeViewContainer.Controls.Add(this.treeViewScripts);
            this.panelTreeViewContainer.Controls.Add(this.panelSearchBar);
            this.panelTreeViewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTreeViewContainer.Location = new System.Drawing.Point(0, 0);
            this.panelTreeViewContainer.Name = "panelTreeViewContainer";
            this.panelTreeViewContainer.Size = new System.Drawing.Size(300, 628);
            this.panelTreeViewContainer.TabIndex = 1;
            // 
            // treeViewScripts
            // 
            this.treeViewScripts.AllowDrop = true;
            this.treeViewScripts.ContextMenuStrip = this.contextMenuStripTree;
            this.treeViewScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewScripts.FullRowSelect = true;
            this.treeViewScripts.HideSelection = false;
            this.treeViewScripts.LabelEdit = true;
            this.treeViewScripts.Location = new System.Drawing.Point(0, 0);
            this.treeViewScripts.Name = "treeViewScripts";
            this.treeViewScripts.Size = new System.Drawing.Size(300, 528);
            this.treeViewScripts.TabIndex = 2;
            this.treeViewScripts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewScripts_AfterSelect);
            this.treeViewScripts.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewScripts_AfterLabelEdit);
            this.treeViewScripts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewScripts_MouseDown);
            this.treeViewScripts.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewScripts_ItemDrag);
            this.treeViewScripts.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewScripts_DragDrop);
            this.treeViewScripts.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewScripts_DragEnter);
            this.treeViewScripts.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewScripts_DragOver);
            // 
            // tableLayoutPanelRight
            // 
            this.tableLayoutPanelRight.ColumnCount = 1;
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRight.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRight.Name = "tableLayoutPanelRight";
            this.tableLayoutPanelRight.RowCount = 3;
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRight.Size = new System.Drawing.Size(1034, 300);
            this.tableLayoutPanelRight.TabIndex = 0;
            // 
            // txtScriptEditor
            // 
            this.txtScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScriptEditor.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtScriptEditor.Location = new System.Drawing.Point(0, 0);
            this.txtScriptEditor.Multiline = true;
            this.txtScriptEditor.Name = "txtScriptEditor";
            this.txtScriptEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.txtScriptEditor.Size = new System.Drawing.Size(1034, 300);
            this.txtScriptEditor.TabIndex = 0;
            this.txtScriptEditor.WordWrap = false;
            this.txtScriptEditor.TextChanged += new System.EventHandler(this.txtScriptEditor_TextChanged);
            // 
            // splitterRight
            // 
            this.splitterRight.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitterRight.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitterRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterRight.Location = new System.Drawing.Point(0, 300);
            this.splitterRight.Name = "splitterRight";
            this.splitterRight.Size = new System.Drawing.Size(1034, 8);
            this.splitterRight.TabIndex = 1;
            this.splitterRight.TabStop = true;
            // 
            // dgvResults
            // 
            this.dgvResults.AllowDrop = true;
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(0, 0);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.RowTemplate.Height = 25;
            this.dgvResults.Size = new System.Drawing.Size(1034, 300);
            this.dgvResults.TabIndex = 0;
            this.dgvResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellClick);
            this.dgvResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvResults_KeyDown);
            // 
            // tabControlResults
            // 
            this.tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResults.Location = new System.Drawing.Point(0, 0);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(1034, 300);
            // 
            // Add controls to TableLayoutPanel
            // 
            this.tableLayoutPanelRight.Controls.Add(this.txtScriptEditor, 0, 0);
            this.tableLayoutPanelRight.Controls.Add(this.splitterRight, 0, 1);
            this.tableLayoutPanelRight.Controls.Add(this.tabControlResults, 0, 2);
            // 
            // dgvContext
            // 
            this.dgvContext.AllowDrop = true;
            this.dgvContext.AllowUserToAddRows = false;
            this.dgvContext.AllowUserToDeleteRows = false;
            this.dgvContext.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvContext.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContext.Columns.Add("Name", "Name");
            this.dgvContext.Columns.Add("Value", "Value");
            this.dgvContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvContext.Location = new System.Drawing.Point(0, 0);
            this.dgvContext.Name = "dgvContext";
            this.dgvContext.RowHeadersVisible = false;
            this.dgvContext.RowTemplate.Height = 25;
            this.dgvContext.Size = new System.Drawing.Size(1034, 300);
            this.dgvContext.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 668);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1234, 32);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1219, 20);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            // 
            // contextMenuStripTree
            // 
            this.contextMenuStripTree.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duplicateToolStripMenuItem,
            this.addScriptToolStripMenuItem,
            this.addFolderToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripTree.Name = "contextMenuStripTree";
            this.contextMenuStripTree.Size = new System.Drawing.Size(149, 142);
            // 
            // addScriptToolStripMenuItem
            // 
            this.addScriptToolStripMenuItem.Name = "addScriptToolStripMenuItem";
            this.addScriptToolStripMenuItem.Size = new System.Drawing.Size(148, 28);
            this.addScriptToolStripMenuItem.Text = "Add Script";
            this.addScriptToolStripMenuItem.Click += new System.EventHandler(this.addScriptToolStripMenuItem_Click);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(148, 28);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(148, 28);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(148, 28);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(148, 28);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 700);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBClip - Database Script Manager";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMiddle)).EndInit();
            this.splitContainerMiddle.Panel1.ResumeLayout(false);
            this.splitContainerMiddle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMiddle)).EndInit();
            this.panelTreeViewContainer.ResumeLayout(false);
            this.panelSearchBar.ResumeLayout(false);
            this.tableLayoutPanelRight.ResumeLayout(false);
            this.tableLayoutPanelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContext)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStripTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private static System.Drawing.Bitmap CreatePlayIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(16, 124, 16));
            var points = new System.Drawing.Point[] {
                new System.Drawing.Point(6, 4),
                new System.Drawing.Point(20, 12),
                new System.Drawing.Point(6, 20)
            };
            g.FillPolygon(brush, points);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateStopIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(209, 52, 56));
            g.FillRectangle(brush, 5, 5, 14, 14);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateSaveIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 120, 212));
            g.FillRectangle(brush, 4, 2, 16, 18);
            using var pen = new System.Drawing.Pen(System.Drawing.Color.White, 1);
            g.DrawLine(pen, 8, 8, 16, 8);
            g.DrawLine(pen, 8, 12, 16, 12);
            g.DrawLine(pen, 8, 16, 14, 16);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateDatabaseIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(86, 156, 214));
            g.FillEllipse(brush, 2, 2, 20, 10);
            g.FillRectangle(brush, 2, 7, 20, 10);
            g.FillEllipse(brush, 2, 12, 20, 10);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateScriptIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(86, 156, 214));
            g.FillPolygon(brush, new System.Drawing.Point[] {
                new System.Drawing.Point(4, 2),
                new System.Drawing.Point(16, 2),
                new System.Drawing.Point(20, 6),
                new System.Drawing.Point(20, 22),
                new System.Drawing.Point(8, 22),
                new System.Drawing.Point(4, 18)
            });
            using var pen = new System.Drawing.Pen(System.Drawing.Color.White, 1);
            g.DrawLine(pen, 8, 8, 16, 8);
            g.DrawLine(pen, 8, 12, 16, 12);
            g.DrawLine(pen, 8, 16, 14, 16);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateFolderIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 194, 134));
            g.FillPolygon(brush, new System.Drawing.Point[] {
                new System.Drawing.Point(2, 6),
                new System.Drawing.Point(8, 6),
                new System.Drawing.Point(10, 2),
                new System.Drawing.Point(20, 2),
                new System.Drawing.Point(22, 6),
                new System.Drawing.Point(22, 20),
                new System.Drawing.Point(2, 20)
            });
            return bmp;
        }

        private static System.Drawing.Bitmap CreateDeleteIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(209, 52, 56), 2);
            g.DrawLine(pen, 6, 6, 18, 18);
            g.DrawLine(pen, 18, 6, 6, 18);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateUndoIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 0, 0), 2);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            g.DrawLine(pen, 16, 12, 6, 12);
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0));
            g.FillPolygon(brush, new System.Drawing.Point[] {
                new System.Drawing.Point(6, 6),
                new System.Drawing.Point(14, 12),
                new System.Drawing.Point(6, 18)
            });
            return bmp;
        }

        private static System.Drawing.Bitmap CreateRedoIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 0, 0), 2);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            g.DrawLine(pen, 8, 12, 18, 12);
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0));
            g.FillPolygon(brush, new System.Drawing.Point[] {
                new System.Drawing.Point(18, 6),
                new System.Drawing.Point(10, 12),
                new System.Drawing.Point(18, 18)
            });
            return bmp;
        }

        private static System.Drawing.Bitmap CreateResetIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 0, 0), 2);
            g.DrawRectangle(pen, 4, 4, 16, 16);
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0));
            g.FillRectangle(brush, 8, 8, 8, 8);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateCacheOffIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(128, 128, 128), 2);
            g.DrawEllipse(pen, 3, 3, 18, 18);
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(200, 200, 200));
            g.FillRectangle(brush, 8, 2, 8, 20);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateCacheOnIcon()
        {
            var bmp = new System.Drawing.Bitmap(24, 24);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 120, 212), 2);
            g.DrawEllipse(pen, 3, 3, 18, 18);
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 120, 212));
            g.FillRectangle(brush, 8, 2, 8, 20);
            using var checkPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 120, 212), 2);
            g.DrawLine(checkPen, 7, 12, 10, 15);
            g.DrawLine(checkPen, 10, 15, 16, 9);
            return bmp;
        }

        private static System.Drawing.Bitmap CreateClearSearchIcon()
        {
            var bmp = new System.Drawing.Bitmap(16, 16);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(128, 128, 128), 2);
            g.DrawLine(pen, 3, 3, 12, 12);
            g.DrawLine(pen, 12, 3, 3, 12);
            return bmp;
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPlay;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnDatabase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddScript;
        private System.Windows.Forms.ToolStripButton btnAddFolder;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnUndo;
        private System.Windows.Forms.ToolStripButton btnRedo;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelTreeViewContainer;
        private System.Windows.Forms.Panel panelSearchBar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.TreeView treeViewScripts;
        private System.Windows.Forms.SplitContainer splitContainerMiddle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRight;
        private System.Windows.Forms.RichTextBox txtScriptEditor;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Splitter splitterRight;
        private System.Windows.Forms.DataGridView dgvContext;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTree;
        private System.Windows.Forms.ToolStripMenuItem addScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lblRowCount;
        private System.Windows.Forms.ToolStripTextBox numRowCount;
        private System.Windows.Forms.ToolStripButton chkCacheResults;
        private System.Windows.Forms.ToolStripButton btnResetLayout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox cmbDatabaseSelect;
        private System.Windows.Forms.TabControl tabControlResults;
    }
}
