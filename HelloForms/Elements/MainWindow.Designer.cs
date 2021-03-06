﻿namespace HelloForms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripNewScheme = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFatonCfgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importOntologyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDictionaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importGramtabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сегментовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ontologyTreeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addArgumentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addResultMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dictionaryTreeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.importOntologyFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.importDictionaryFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveProjectFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabsAndBankContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bankGroupBox = new System.Windows.Forms.GroupBox();
            this.bankListDataGrid = new System.Windows.Forms.DataGridView();
            this.segmentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.schemesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bankListFilter = new System.Windows.Forms.TextBox();
            this.propsGroupBox = new System.Windows.Forms.GroupBox();
            this.propsPanel = new System.Windows.Forms.Integration.ElementHost();
            this.schemesTabControl = new System.Windows.Forms.TabControl();
            this.schemeTabViewPage = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.addFnCatToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemeTabXMLPage = new System.Windows.Forms.TabPage();
            this.schemeXMLTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ontologyAndDictionaryTabsBox = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ontologyTreeView = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dictionaryTreeView = new System.Windows.Forms.TreeView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.importGramtabFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.importSegmentsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFatonCfgFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.schemeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSchemeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemeSegmentCombo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.editorProjectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.factSchemeBankBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            this.ontologyTreeMenuStrip.SuspendLayout();
            this.dictionaryTreeMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabsAndBankContainer)).BeginInit();
            this.tabsAndBankContainer.Panel1.SuspendLayout();
            this.tabsAndBankContainer.Panel2.SuspendLayout();
            this.tabsAndBankContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.bankGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankListDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schemesBindingSource)).BeginInit();
            this.propsGroupBox.SuspendLayout();
            this.schemesTabControl.SuspendLayout();
            this.schemeTabViewPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.schemeTabXMLPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.ontologyAndDictionaryTabsBox.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.schemeContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editorProjectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.factSchemeBankBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.mainToolStripNewScheme,
            this.openProjectToolStripMenu,
            this.saveProjectToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportFatonCfgToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(285, 26);
            this.newProjectToolStripMenuItem.Text = "Новый проект";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // mainToolStripNewScheme
            // 
            this.mainToolStripNewScheme.Name = "mainToolStripNewScheme";
            this.mainToolStripNewScheme.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mainToolStripNewScheme.Size = new System.Drawing.Size(285, 26);
            this.mainToolStripNewScheme.Text = "Новая схема";
            this.mainToolStripNewScheme.Click += new System.EventHandler(this.handleCreateSchemeToolstrip);
            // 
            // openProjectToolStripMenu
            // 
            this.openProjectToolStripMenu.Name = "openProjectToolStripMenu";
            this.openProjectToolStripMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenu.Size = new System.Drawing.Size(285, 26);
            this.openProjectToolStripMenu.Text = "Открыть проект...";
            this.openProjectToolStripMenu.Click += new System.EventHandler(this.openProjectToolStripMenu_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(285, 26);
            this.saveProjectToolStripMenuItem.Text = "Сохранить проект...";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(285, 26);
            this.saveAsToolStripMenuItem.Text = "Экспорт банка";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportFatonCfgToolStripMenuItem
            // 
            this.exportFatonCfgToolStripMenuItem.Enabled = false;
            this.exportFatonCfgToolStripMenuItem.Name = "exportFatonCfgToolStripMenuItem";
            this.exportFatonCfgToolStripMenuItem.Size = new System.Drawing.Size(285, 26);
            this.exportFatonCfgToolStripMenuItem.Text = "Экспорт конфигурации Faton";
            this.exportFatonCfgToolStripMenuItem.Click += new System.EventHandler(this.exportFatonCfgToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.importToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(910, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importOntologyToolStripMenuItem,
            this.importDictionaryToolStripMenuItem,
            this.importGramtabToolStripMenuItem,
            this.сегментовToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.importToolStripMenuItem.Text = "Импорт";
            // 
            // importOntologyToolStripMenuItem
            // 
            this.importOntologyToolStripMenuItem.Name = "importOntologyToolStripMenuItem";
            this.importOntologyToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.importOntologyToolStripMenuItem.Text = "Онтологии...";
            this.importOntologyToolStripMenuItem.Click += new System.EventHandler(this.importOntologyToolStripMenuItem_Click);
            // 
            // importDictionaryToolStripMenuItem
            // 
            this.importDictionaryToolStripMenuItem.Name = "importDictionaryToolStripMenuItem";
            this.importDictionaryToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.importDictionaryToolStripMenuItem.Text = "Словаря...";
            this.importDictionaryToolStripMenuItem.Click += new System.EventHandler(this.importDictionaryToolStripMenuItem_Click);
            // 
            // importGramtabToolStripMenuItem
            // 
            this.importGramtabToolStripMenuItem.Name = "importGramtabToolStripMenuItem";
            this.importGramtabToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.importGramtabToolStripMenuItem.Text = "Грамтаба...";
            this.importGramtabToolStripMenuItem.Click += new System.EventHandler(this.importGramtabToolStripMenuItem_Click);
            // 
            // сегментовToolStripMenuItem
            // 
            this.сегментовToolStripMenuItem.Name = "сегментовToolStripMenuItem";
            this.сегментовToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.сегментовToolStripMenuItem.Text = "Сегментов...";
            this.сегментовToolStripMenuItem.Click += new System.EventHandler(this.importSegmentsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.editToolStripMenuItem.Text = "Правка (Under construction)";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.undoToolStripMenuItem.Text = "Отменить";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.redoToolStripMenuItem.Text = "Повторить";
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
            this.ontologyTreeMenuStrip.Size = new System.Drawing.Size(112, 52);
            // 
            // addArgumentMenuItem
            // 
            this.addArgumentMenuItem.Name = "addArgumentMenuItem";
            this.addArgumentMenuItem.Size = new System.Drawing.Size(111, 24);
            this.addArgumentMenuItem.Text = "dgsg";
            this.addArgumentMenuItem.Click += new System.EventHandler(this.addArgumentMenuItem_Click);
            // 
            // addResultMenuItem
            // 
            this.addResultMenuItem.Name = "addResultMenuItem";
            this.addResultMenuItem.Size = new System.Drawing.Size(111, 24);
            this.addResultMenuItem.Text = "asdf";
            this.addResultMenuItem.Click += new System.EventHandler(this.addResultMenuItem_Click);
            // 
            // dictionaryTreeMenuStrip
            // 
            this.dictionaryTreeMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.dictionaryTreeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.dictionaryTreeMenuStrip.Name = "ontologyTreeMenuStrip";
            this.dictionaryTreeMenuStrip.Size = new System.Drawing.Size(240, 28);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(239, 24);
            this.toolStripMenuItem1.Text = "Добавить как аргумент";
            this.toolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripMenuItem1.Click += new System.EventHandler(this.addDictionaryArgumentMenuItem_Click);
            // 
            // openProjectDialog
            // 
            this.openProjectDialog.FileName = "project.xml";
            this.openProjectDialog.Filter = "Faton Editor Project|*.xml";
            this.openProjectDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openProjectDialog_FileOk);
            // 
            // importOntologyFileDialog
            // 
            this.importOntologyFileDialog.FileName = "ontology.xml";
            this.importOntologyFileDialog.Filter = "Ontology|*.xml";
            this.importOntologyFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.importOntologyFileDialog_FileOk);
            // 
            // importDictionaryFileDialog
            // 
            this.importDictionaryFileDialog.FileName = "dictionary";
            this.importDictionaryFileDialog.Filter = "Словарь KLAN|*.vc.th";
            this.importDictionaryFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.importDictionaryFileDialog_FileOk);
            // 
            // saveProjectFileDialog
            // 
            this.saveProjectFileDialog.FileName = "project.xml";
            this.saveProjectFileDialog.Filter = "Faton Editor Project|*.xml";
            this.saveProjectFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveProjectFileDialog_FileOk);
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
            this.tabsAndBankContainer.Panel1.Controls.Add(this.splitContainer1);
            this.tabsAndBankContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // tabsAndBankContainer.Panel2
            // 
            this.tabsAndBankContainer.Panel2.Controls.Add(this.schemesTabControl);
            this.tabsAndBankContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabsAndBankContainer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabsAndBankContainer.Size = new System.Drawing.Size(641, 688);
            this.tabsAndBankContainer.SplitterDistance = global::HelloForms.Properties.Settings.Default.TabsAndBankSplitter;
            this.tabsAndBankContainer.TabIndex = 1;
            this.tabsAndBankContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.window_SplitterMoved);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bankGroupBox);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propsGroupBox);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(283, 688);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 0;
            // 
            // bankGroupBox
            // 
            this.bankGroupBox.Controls.Add(this.bankListDataGrid);
            this.bankGroupBox.Controls.Add(this.bankListFilter);
            this.bankGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankGroupBox.Location = new System.Drawing.Point(0, 0);
            this.bankGroupBox.Name = "bankGroupBox";
            this.bankGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bankGroupBox.Size = new System.Drawing.Size(283, 175);
            this.bankGroupBox.TabIndex = 0;
            this.bankGroupBox.TabStop = false;
            this.bankGroupBox.Text = "Банк";
            // 
            // bankListDataGrid
            // 
            this.bankListDataGrid.AllowUserToAddRows = false;
            this.bankListDataGrid.AllowUserToDeleteRows = false;
            this.bankListDataGrid.AllowUserToResizeRows = false;
            this.bankListDataGrid.AutoGenerateColumns = false;
            this.bankListDataGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.bankListDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bankListDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.schemeSegmentCombo});
            this.bankListDataGrid.DataSource = this.schemesBindingSource;
            this.bankListDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bankListDataGrid.Location = new System.Drawing.Point(3, 40);
            this.bankListDataGrid.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.bankListDataGrid.Name = "bankListDataGrid";
            this.bankListDataGrid.RowTemplate.Height = 24;
            this.bankListDataGrid.Size = new System.Drawing.Size(277, 132);
            this.bankListDataGrid.TabIndex = 1;
            this.bankListDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.bankListDataGrid_CellClick);
            this.bankListDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.bankListDataGrid_CellMouseDown);
            this.bankListDataGrid.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.bankListDataGrid_UserDeletedRow);
            this.bankListDataGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.bankListDataGrid_UserDeletingRow);
            // 
            // segmentsBindingSource
            // 
            this.segmentsBindingSource.DataMember = "Segments";
            this.segmentsBindingSource.DataSource = this.editorProjectBindingSource;
            // 
            // schemesBindingSource
            // 
            this.schemesBindingSource.DataMember = "Schemes";
            this.schemesBindingSource.DataSource = this.factSchemeBankBindingSource;
            // 
            // bankListFilter
            // 
            this.bankListFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.bankListFilter.Enabled = false;
            this.bankListFilter.Location = new System.Drawing.Point(3, 18);
            this.bankListFilter.Name = "bankListFilter";
            this.bankListFilter.Size = new System.Drawing.Size(277, 22);
            this.bankListFilter.TabIndex = 0;
            this.bankListFilter.TextChanged += new System.EventHandler(this.bankListFilter_TextChanged);
            // 
            // propsGroupBox
            // 
            this.propsGroupBox.Controls.Add(this.propsPanel);
            this.propsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.propsGroupBox.Name = "propsGroupBox";
            this.propsGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.propsGroupBox.Size = new System.Drawing.Size(283, 509);
            this.propsGroupBox.TabIndex = 0;
            this.propsGroupBox.TabStop = false;
            this.propsGroupBox.Text = "Свойства";
            // 
            // propsPanel
            // 
            this.propsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsPanel.Location = new System.Drawing.Point(3, 18);
            this.propsPanel.Name = "propsPanel";
            this.propsPanel.Size = new System.Drawing.Size(277, 488);
            this.propsPanel.TabIndex = 0;
            this.propsPanel.Text = "elementHost1";
            this.propsPanel.Child = null;
            // 
            // schemesTabControl
            // 
            this.schemesTabControl.Controls.Add(this.schemeTabViewPage);
            this.schemesTabControl.Controls.Add(this.schemeTabXMLPage);
            this.schemesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemesTabControl.Location = new System.Drawing.Point(0, 0);
            this.schemesTabControl.Margin = new System.Windows.Forms.Padding(4);
            this.schemesTabControl.Name = "schemesTabControl";
            this.schemesTabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.schemesTabControl.SelectedIndex = 0;
            this.schemesTabControl.Size = new System.Drawing.Size(354, 688);
            this.schemesTabControl.TabIndex = 0;
            this.schemesTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.schemesTabControl_Selected);
            // 
            // schemeTabViewPage
            // 
            this.schemeTabViewPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.schemeTabViewPage.Controls.Add(this.toolStrip1);
            this.schemeTabViewPage.Location = new System.Drawing.Point(4, 25);
            this.schemeTabViewPage.Name = "schemeTabViewPage";
            this.schemeTabViewPage.Size = new System.Drawing.Size(346, 659);
            this.schemeTabViewPage.TabIndex = 0;
            this.schemeTabViewPage.Text = "Сеть";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(346, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(71, 24);
            this.toolStripButton1.Text = "Условие";
            this.toolStripButton1.Click += new System.EventHandler(this.addSchemeConditionButton_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFnCatToolStripItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(81, 24);
            this.toolStripDropDownButton1.Text = "Функтор";
            // 
            // addFnCatToolStripItem
            // 
            this.addFnCatToolStripItem.Name = "addFnCatToolStripItem";
            this.addFnCatToolStripItem.Size = new System.Drawing.Size(96, 26);
            this.addFnCatToolStripItem.Text = "&&";
            this.addFnCatToolStripItem.Click += new System.EventHandler(this.addFnCatToolStripItem_Click);
            // 
            // schemeTabXMLPage
            // 
            this.schemeTabXMLPage.Controls.Add(this.schemeXMLTextBox);
            this.schemeTabXMLPage.Location = new System.Drawing.Point(4, 25);
            this.schemeTabXMLPage.Name = "schemeTabXMLPage";
            this.schemeTabXMLPage.Size = new System.Drawing.Size(346, 659);
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
            this.schemeXMLTextBox.ReadOnly = true;
            this.schemeXMLTextBox.Size = new System.Drawing.Size(346, 659);
            this.schemeXMLTextBox.TabIndex = 0;
            this.schemeXMLTextBox.Text = "";
            this.schemeXMLTextBox.WordWrap = false;
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
            this.splitContainer2.Panel1.Controls.Add(this.ontologyAndDictionaryTabsBox);
            this.splitContainer2.Panel1MinSize = 200;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Size = new System.Drawing.Size(264, 688);
            this.splitContainer2.SplitterDistance = global::HelloForms.Properties.Settings.Default.OntologyAndAttributesSplitter;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.window_SplitterMoved);
            // 
            // ontologyAndDictionaryTabsBox
            // 
            this.ontologyAndDictionaryTabsBox.Controls.Add(this.tabPage1);
            this.ontologyAndDictionaryTabsBox.Controls.Add(this.tabPage2);
            this.ontologyAndDictionaryTabsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ontologyAndDictionaryTabsBox.Location = new System.Drawing.Point(0, 0);
            this.ontologyAndDictionaryTabsBox.Name = "ontologyAndDictionaryTabsBox";
            this.ontologyAndDictionaryTabsBox.SelectedIndex = 0;
            this.ontologyAndDictionaryTabsBox.Size = new System.Drawing.Size(264, 337);
            this.ontologyAndDictionaryTabsBox.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ontologyTreeView);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(256, 308);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Онтология";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ontologyTreeView
            // 
            this.ontologyTreeView.AllowDrop = true;
            this.ontologyTreeView.ContextMenuStrip = this.ontologyTreeMenuStrip;
            this.ontologyTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ontologyTreeView.Location = new System.Drawing.Point(3, 3);
            this.ontologyTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.ontologyTreeView.Name = "ontologyTreeView";
            this.ontologyTreeView.Size = new System.Drawing.Size(250, 302);
            this.ontologyTreeView.TabIndex = 1;
            this.ontologyTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dictionaryTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(256, 308);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Словарь";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dictionaryTreeView
            // 
            this.dictionaryTreeView.ContextMenuStrip = this.dictionaryTreeMenuStrip;
            this.dictionaryTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dictionaryTreeView.Location = new System.Drawing.Point(3, 3);
            this.dictionaryTreeView.Name = "dictionaryTreeView";
            this.dictionaryTreeView.Size = new System.Drawing.Size(250, 302);
            this.dictionaryTreeView.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.groupBox4.Size = new System.Drawing.Size(264, 346);
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
            this.listView1.Size = new System.Drawing.Size(258, 323);
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
            // importGramtabFileDialog
            // 
            this.importGramtabFileDialog.FileName = "gramtab.ini";
            this.importGramtabFileDialog.Filter = "Gramtab|*.ini|Any|*.*";
            this.importGramtabFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.importGramtabFileDialog_FileOk);
            // 
            // importSegmentsFileDialog
            // 
            this.importSegmentsFileDialog.FileName = "segments.xml";
            this.importSegmentsFileDialog.Filter = "Segments|*.xml|Any|*.*";
            this.importSegmentsFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.importSegmentsFileDialog_FileOk);
            // 
            // saveFatonCfgFileDialog
            // 
            this.saveFatonCfgFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFatonCfgFileDialog_FileOk);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(3, 697);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(910, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // schemeContextMenuStrip
            // 
            this.schemeContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.schemeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSchemeMenuItem});
            this.schemeContextMenuStrip.Name = "bankContextMenuStrip";
            this.schemeContextMenuStrip.Size = new System.Drawing.Size(179, 28);
            // 
            // removeSchemeMenuItem
            // 
            this.removeSchemeMenuItem.Name = "removeSchemeMenuItem";
            this.removeSchemeMenuItem.Size = new System.Drawing.Size(178, 24);
            this.removeSchemeMenuItem.Text = "Удалить схему";
            this.removeSchemeMenuItem.Click += new System.EventHandler(this.removeSchemeMenuItem_Click);
            // 
            // schemeSegmentCombo
            // 
            this.schemeSegmentCombo.DataPropertyName = "Segment";
            this.schemeSegmentCombo.DataSource = this.segmentsBindingSource;
            this.schemeSegmentCombo.HeaderText = "Сегмент";
            this.schemeSegmentCombo.Name = "schemeSegmentCombo";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.ContextMenuStrip = this.schemeContextMenuStrip;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Имя";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // editorProjectBindingSource
            // 
            this.editorProjectBindingSource.DataSource = typeof(HelloForms.EditorProject);
            // 
            // factSchemeBankBindingSource
            // 
            this.factSchemeBankBindingSource.DataSource = typeof(HelloForms.FactSchemeBank);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 722);
            this.Controls.Add(this.statusStrip1);
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
            this.ontologyTreeMenuStrip.ResumeLayout(false);
            this.dictionaryTreeMenuStrip.ResumeLayout(false);
            this.tabsAndBankContainer.Panel1.ResumeLayout(false);
            this.tabsAndBankContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabsAndBankContainer)).EndInit();
            this.tabsAndBankContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.bankGroupBox.ResumeLayout(false);
            this.bankGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankListDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schemesBindingSource)).EndInit();
            this.propsGroupBox.ResumeLayout(false);
            this.schemesTabControl.ResumeLayout(false);
            this.schemeTabViewPage.ResumeLayout(false);
            this.schemeTabViewPage.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.schemeTabXMLPage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ontologyAndDictionaryTabsBox.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.schemeContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editorProjectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.factSchemeBankBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripNewScheme;
        private System.Windows.Forms.ContextMenuStrip ontologyTreeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addArgumentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addResultMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip dictionaryTreeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenu;
        private System.Windows.Forms.OpenFileDialog openProjectDialog;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importOntologyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDictionaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importGramtabToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog importOntologyFileDialog;
        private System.Windows.Forms.OpenFileDialog importDictionaryFileDialog;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveProjectFileDialog;
        private System.Windows.Forms.ToolStripMenuItem сегментовToolStripMenuItem;
        private System.Windows.Forms.SplitContainer tabsAndBankContainer;
        private System.Windows.Forms.TabControl schemesTabControl;
        private System.Windows.Forms.TabPage schemeTabViewPage;
        private System.Windows.Forms.TabPage schemeTabXMLPage;
        private System.Windows.Forms.RichTextBox schemeXMLTextBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.OpenFileDialog importGramtabFileDialog;
        private System.Windows.Forms.OpenFileDialog importSegmentsFileDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox bankGroupBox;
        private System.Windows.Forms.GroupBox propsGroupBox;
        private System.Windows.Forms.TextBox bankListFilter;
        private System.Windows.Forms.Integration.ElementHost propsPanel;
        private System.Windows.Forms.ToolStripMenuItem exportFatonCfgToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFatonCfgFileDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.DataGridView bankListDataGrid;
        private System.Windows.Forms.BindingSource schemesBindingSource;
        private System.Windows.Forms.BindingSource factSchemeBankBindingSource;
        private System.Windows.Forms.BindingSource editorProjectBindingSource;
        private System.Windows.Forms.BindingSource segmentsBindingSource;
        private System.Windows.Forms.TabControl ontologyAndDictionaryTabsBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView ontologyTreeView;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView dictionaryTreeView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem addFnCatToolStripItem;
        private System.Windows.Forms.ContextMenuStrip schemeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeSchemeMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn schemeSegmentCombo;
    }
}

