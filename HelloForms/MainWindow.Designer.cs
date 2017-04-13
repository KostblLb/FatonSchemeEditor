namespace HelloForms
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripNewScheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutTabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьРезультатToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьФункторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.argAttrsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьУсловиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.conditionBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.ontologyTreeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addArgumentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addResultMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemesListBinding = new System.Windows.Forms.BindingSource(this.components);
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ontologyTreeView = new System.Windows.Forms.TreeView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabsAndBankContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bankListView = new System.Windows.Forms.ListView();
            this.nvAndConditionsContainer = new System.Windows.Forms.SplitContainer();
            this.schemesTabControl = new System.Windows.Forms.TabControl();
            this.schemeTabViewPage = new System.Windows.Forms.TabPage();
            this.schemeTabXMLPage = new System.Windows.Forms.TabPage();
            this.schemeXMLTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.conditionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            this.layoutTabContextMenu.SuspendLayout();
            this.argAttrsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.conditionBindingSource1)).BeginInit();
            this.ontologyTreeMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.schemesListBinding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabsAndBankContainer)).BeginInit();
            this.tabsAndBankContainer.Panel1.SuspendLayout();
            this.tabsAndBankContainer.Panel2.SuspendLayout();
            this.tabsAndBankContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nvAndConditionsContainer)).BeginInit();
            this.nvAndConditionsContainer.Panel1.SuspendLayout();
            this.nvAndConditionsContainer.Panel2.SuspendLayout();
            this.nvAndConditionsContainer.SuspendLayout();
            this.schemesTabControl.SuspendLayout();
            this.schemeTabXMLPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conditionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.mainToolStripNewScheme});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.openToolStripMenuItem.Text = "Открыть...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.saveAsToolStripMenuItem.Text = "Сохранить...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // mainToolStripNewScheme
            // 
            this.mainToolStripNewScheme.Name = "mainToolStripNewScheme";
            this.mainToolStripNewScheme.Size = new System.Drawing.Size(217, 26);
            this.mainToolStripNewScheme.Text = "Новая схема";
            this.mainToolStripNewScheme.Click += new System.EventHandler(this.handleCreateSchemeToolstrip);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.настройкиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(910, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pathsToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(96, 24);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // pathsToolStripMenuItem
            // 
            this.pathsToolStripMenuItem.Name = "pathsToolStripMenuItem";
            this.pathsToolStripMenuItem.Size = new System.Drawing.Size(117, 26);
            this.pathsToolStripMenuItem.Text = "Пути";
            this.pathsToolStripMenuItem.Click += new System.EventHandler(this.pathsToolStripMenuItem_Click);
            // 
            // layoutTabContextMenu
            // 
            this.layoutTabContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.layoutTabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьРезультатToolStripMenuItem,
            this.добавитьФункторToolStripMenuItem});
            this.layoutTabContextMenu.Name = "layoutTabContextMenu";
            this.layoutTabContextMenu.Size = new System.Drawing.Size(223, 56);
            // 
            // добавитьРезультатToolStripMenuItem
            // 
            this.добавитьРезультатToolStripMenuItem.Name = "добавитьРезультатToolStripMenuItem";
            this.добавитьРезультатToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.добавитьРезультатToolStripMenuItem.Text = "Добавить результат";
            // 
            // добавитьФункторToolStripMenuItem
            // 
            this.добавитьФункторToolStripMenuItem.Name = "добавитьФункторToolStripMenuItem";
            this.добавитьФункторToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.добавитьФункторToolStripMenuItem.Text = "Добавить функтор";
            // 
            // argAttrsContextMenu
            // 
            this.argAttrsContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.argAttrsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьУсловиеToolStripMenuItem});
            this.argAttrsContextMenu.Name = "contextMenuStrip1";
            this.argAttrsContextMenu.Size = new System.Drawing.Size(212, 30);
            // 
            // добавитьУсловиеToolStripMenuItem
            // 
            this.добавитьУсловиеToolStripMenuItem.Name = "добавитьУсловиеToolStripMenuItem";
            this.добавитьУсловиеToolStripMenuItem.Size = new System.Drawing.Size(211, 26);
            this.добавитьУсловиеToolStripMenuItem.Text = "Добавить условие";
            this.добавитьУсловиеToolStripMenuItem.Click += new System.EventHandler(this.добавитьУсловиеToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // ontologyTreeMenuStrip
            // 
            this.ontologyTreeMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ontologyTreeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addArgumentMenuItem,
            this.addResultMenuItem});
            this.ontologyTreeMenuStrip.Name = "ontologyTreeMenuStrip";
            this.ontologyTreeMenuStrip.Size = new System.Drawing.Size(118, 56);
            // 
            // addArgumentMenuItem
            // 
            this.addArgumentMenuItem.Name = "addArgumentMenuItem";
            this.addArgumentMenuItem.Size = new System.Drawing.Size(117, 26);
            this.addArgumentMenuItem.Text = "dgsg";
            this.addArgumentMenuItem.Click += new System.EventHandler(this.addArgumentMenuItem_Click);
            // 
            // addResultMenuItem
            // 
            this.addResultMenuItem.Name = "addResultMenuItem";
            this.addResultMenuItem.Size = new System.Drawing.Size(117, 26);
            this.addResultMenuItem.Text = "asdf";
            this.addResultMenuItem.Click += new System.EventHandler(this.addResultMenuItem_Click);
            // 
            // mainContainer
            // 
            this.mainContainer.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::HelloForms.Properties.Settings.Default, "MainSplitter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainContainer.Location = new System.Drawing.Point(3, 31);
            this.mainContainer.Margin = new System.Windows.Forms.Padding(4);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.splitContainer2);
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.tabsAndBankContainer);
            this.mainContainer.Size = new System.Drawing.Size(910, 688);
            this.mainContainer.SplitterDistance = global::HelloForms.Properties.Settings.Default.MainSplitter;
            this.mainContainer.SplitterWidth = 5;
            this.mainContainer.TabIndex = 1;
            this.mainContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.window_SplitterMoved);
            // 
            // splitContainer2
            // 
            this.splitContainer2.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::HelloForms.Properties.Settings.Default, "OntologyAndAttributesSplitter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer2.Panel1MinSize = 200;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Size = new System.Drawing.Size(200, 688);
            this.splitContainer2.SplitterDistance = global::HelloForms.Properties.Settings.Default.OntologyAndAttributesSplitter;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.window_SplitterMoved);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ontologyTreeView);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.groupBox3.Size = new System.Drawing.Size(200, 337);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Дерево онтологии";
            // 
            // ontologyTreeView
            // 
            this.ontologyTreeView.AllowDrop = true;
            this.ontologyTreeView.ContextMenuStrip = this.ontologyTreeMenuStrip;
            this.ontologyTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ontologyTreeView.Location = new System.Drawing.Point(3, 20);
            this.ontologyTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.ontologyTreeView.Name = "ontologyTreeView";
            this.ontologyTreeView.Size = new System.Drawing.Size(194, 314);
            this.ontologyTreeView.TabIndex = 1;
            this.ontologyTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.groupBox4.Size = new System.Drawing.Size(200, 346);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Атрибуты класса";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 20);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(194, 323);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Атрибут";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Тип";
            this.columnHeader2.Width = 52;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Унаследован";
            this.columnHeader3.Width = 99;
            // 
            // tabsAndBankContainer
            // 
            this.tabsAndBankContainer.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::HelloForms.Properties.Settings.Default, "TabsAndBankSplitter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tabsAndBankContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsAndBankContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.tabsAndBankContainer.Location = new System.Drawing.Point(0, 0);
            this.tabsAndBankContainer.Name = "tabsAndBankContainer";
            // 
            // tabsAndBankContainer.Panel1
            // 
            this.tabsAndBankContainer.Panel1.Controls.Add(this.groupBox2);
            this.tabsAndBankContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // tabsAndBankContainer.Panel2
            // 
            this.tabsAndBankContainer.Panel2.Controls.Add(this.nvAndConditionsContainer);
            this.tabsAndBankContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabsAndBankContainer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabsAndBankContainer.Size = new System.Drawing.Size(705, 688);
            this.tabsAndBankContainer.SplitterDistance = global::HelloForms.Properties.Settings.Default.TabsAndBankSplitter;
            this.tabsAndBankContainer.TabIndex = 1;
            this.tabsAndBankContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.window_SplitterMoved);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bankListView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox2.Size = new System.Drawing.Size(140, 688);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Банк схем";
            // 
            // bankListView
            // 
            this.bankListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankListView.FullRowSelect = true;
            this.bankListView.GridLines = true;
            this.bankListView.Location = new System.Drawing.Point(3, 20);
            this.bankListView.Name = "bankListView";
            this.bankListView.Size = new System.Drawing.Size(134, 665);
            this.bankListView.TabIndex = 0;
            this.bankListView.UseCompatibleStateImageBehavior = false;
            this.bankListView.View = System.Windows.Forms.View.List;
            this.bankListView.DoubleClick += new System.EventHandler(this.bankListView_DoubleClick);
            // 
            // nvAndConditionsContainer
            // 
            this.nvAndConditionsContainer.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::HelloForms.Properties.Settings.Default, "ArgGridViewSplitter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nvAndConditionsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nvAndConditionsContainer.Location = new System.Drawing.Point(0, 0);
            this.nvAndConditionsContainer.Margin = new System.Windows.Forms.Padding(4);
            this.nvAndConditionsContainer.Name = "nvAndConditionsContainer";
            this.nvAndConditionsContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // nvAndConditionsContainer.Panel1
            // 
            this.nvAndConditionsContainer.Panel1.Controls.Add(this.schemesTabControl);
            this.nvAndConditionsContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // nvAndConditionsContainer.Panel2
            // 
            this.nvAndConditionsContainer.Panel2.Controls.Add(this.groupBox1);
            this.nvAndConditionsContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nvAndConditionsContainer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nvAndConditionsContainer.Size = new System.Drawing.Size(561, 688);
            this.nvAndConditionsContainer.SplitterDistance = global::HelloForms.Properties.Settings.Default.ArgGridViewSplitter;
            this.nvAndConditionsContainer.SplitterWidth = 5;
            this.nvAndConditionsContainer.TabIndex = 1;
            this.nvAndConditionsContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.window_SplitterMoved);
            // 
            // schemesTabControl
            // 
            this.schemesTabControl.Controls.Add(this.schemeTabViewPage);
            this.schemesTabControl.Controls.Add(this.schemeTabXMLPage);
            this.schemesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemesTabControl.Location = new System.Drawing.Point(0, 0);
            this.schemesTabControl.Margin = new System.Windows.Forms.Padding(4);
            this.schemesTabControl.Name = "schemesTabControl";
            this.schemesTabControl.SelectedIndex = 0;
            this.schemesTabControl.Size = new System.Drawing.Size(561, 500);
            this.schemesTabControl.TabIndex = 0;
            this.schemesTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.schemesTabControl_Selected);
            // 
            // schemeTabViewPage
            // 
            this.schemeTabViewPage.Location = new System.Drawing.Point(4, 25);
            this.schemeTabViewPage.Name = "schemeTabViewPage";
            this.schemeTabViewPage.Size = new System.Drawing.Size(553, 471);
            this.schemeTabViewPage.TabIndex = 0;
            this.schemeTabViewPage.Text = "Сеть";
            this.schemeTabViewPage.UseVisualStyleBackColor = true;
            // 
            // schemeTabXMLPage
            // 
            this.schemeTabXMLPage.Controls.Add(this.schemeXMLTextBox);
            this.schemeTabXMLPage.Location = new System.Drawing.Point(4, 25);
            this.schemeTabXMLPage.Name = "schemeTabXMLPage";
            this.schemeTabXMLPage.Size = new System.Drawing.Size(553, 471);
            this.schemeTabXMLPage.TabIndex = 1;
            this.schemeTabXMLPage.Text = "XML";
            this.schemeTabXMLPage.UseVisualStyleBackColor = true;
            // 
            // schemeXMLTextBox
            // 
            this.schemeXMLTextBox.AcceptsTab = true;
            this.schemeXMLTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemeXMLTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.schemeXMLTextBox.Location = new System.Drawing.Point(0, 0);
            this.schemeXMLTextBox.Name = "schemeXMLTextBox";
            this.schemeXMLTextBox.Size = new System.Drawing.Size(553, 471);
            this.schemeXMLTextBox.TabIndex = 0;
            this.schemeXMLTextBox.Text = "";
            this.schemeXMLTextBox.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(561, 183);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ограничения аргумента";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridView1.DataSource = this.conditionBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(4, 19);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(553, 160);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValidated);
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ConditionType";
            this.dataGridViewTextBoxColumn1.HeaderText = "ConditionType";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Attribute";
            this.dataGridViewTextBoxColumn2.HeaderText = "Attribute";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ComparisonType";
            this.dataGridViewTextBoxColumn3.HeaderText = "ComparisonType";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn4.HeaderText = "Value";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // conditionBindingSource
            // 
            this.conditionBindingSource.AllowNew = true;
            this.conditionBindingSource.DataSource = typeof(FactScheme.Condition);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 722);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindow";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "FATON Scheme Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.layoutTabContextMenu.ResumeLayout(false);
            this.argAttrsContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.conditionBindingSource1)).EndInit();
            this.ontologyTreeMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.schemesListBinding)).EndInit();
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabsAndBankContainer.Panel1.ResumeLayout(false);
            this.tabsAndBankContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabsAndBankContainer)).EndInit();
            this.tabsAndBankContainer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.nvAndConditionsContainer.Panel1.ResumeLayout(false);
            this.nvAndConditionsContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nvAndConditionsContainer)).EndInit();
            this.nvAndConditionsContainer.ResumeLayout(false);
            this.schemesTabControl.ResumeLayout(false);
            this.schemeTabXMLPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.conditionBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.TabControl schemesTabControl;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer nvAndConditionsContainer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource conditionBindingSource;
        private System.Windows.Forms.ContextMenuStrip argAttrsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem добавитьУсловиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathsToolStripMenuItem;
        private System.Windows.Forms.BindingSource conditionBindingSource1;
        private System.Windows.Forms.DataGridViewComboBoxColumn conditionTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn comparisonTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.ContextMenuStrip layoutTabContextMenu;
        private System.Windows.Forms.ToolStripMenuItem добавитьРезультатToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьФункторToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripNewScheme;
        private System.Windows.Forms.ContextMenuStrip ontologyTreeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addArgumentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addResultMenuItem;
        private System.Windows.Forms.SplitContainer tabsAndBankContainer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView bankListView;
        private System.Windows.Forms.BindingSource schemesListBinding;
        private System.Windows.Forms.TabPage schemeTabViewPage;
        private System.Windows.Forms.TabPage schemeTabXMLPage;
        private System.Windows.Forms.RichTextBox schemeXMLTextBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView ontologyTreeView;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

