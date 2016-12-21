namespace HelloForms
{
    partial class SettingsPathsDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.browseGramtabButton = new System.Windows.Forms.Button();
            this.textBoxGramtabPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSomethingPath = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxOntologyPath = new System.Windows.Forms.TextBox();
            this.browseOntologyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.openOntologyDialog = new System.Windows.Forms.OpenFileDialog();
            this.openGramtabDialog = new System.Windows.Forms.OpenFileDialog();
            this.openSomethingDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.browseOntologyButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.browseGramtabButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxOntologyPath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxGramtabPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSomethingPath, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(763, 107);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // browseGramtabButton
            // 
            this.browseGramtabButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browseGramtabButton.Location = new System.Drawing.Point(685, 38);
            this.browseGramtabButton.Name = "browseGramtabButton";
            this.browseGramtabButton.Size = new System.Drawing.Size(75, 29);
            this.browseGramtabButton.TabIndex = 2;
            this.browseGramtabButton.Text = "Обзор...";
            this.browseGramtabButton.UseVisualStyleBackColor = true;
            this.browseGramtabButton.Click += new System.EventHandler(this.browseGramtabButton_Click);
            // 
            // textBoxGramtabPath
            // 
            this.textBoxGramtabPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxGramtabPath.Location = new System.Drawing.Point(122, 41);
            this.textBoxGramtabPath.Name = "textBoxGramtabPath";
            this.textBoxGramtabPath.Size = new System.Drawing.Size(557, 22);
            this.textBoxGramtabPath.TabIndex = 5;
            this.textBoxGramtabPath.TextChanged += new System.EventHandler(this.textBoxGramtabPath_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Морф. таблица:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ещё что-то:";
            // 
            // textBoxSomethingPath
            // 
            this.textBoxSomethingPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxSomethingPath.Location = new System.Drawing.Point(122, 76);
            this.textBoxSomethingPath.Name = "textBoxSomethingPath";
            this.textBoxSomethingPath.Size = new System.Drawing.Size(557, 22);
            this.textBoxSomethingPath.TabIndex = 6;
            this.textBoxSomethingPath.TextChanged += new System.EventHandler(this.textBoxSomethingPath_TextChanged);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(685, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 29);
            this.button3.TabIndex = 3;
            this.button3.Text = "Обзор...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxOntologyPath
            // 
            this.textBoxOntologyPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxOntologyPath.Location = new System.Drawing.Point(122, 6);
            this.textBoxOntologyPath.Name = "textBoxOntologyPath";
            this.textBoxOntologyPath.Size = new System.Drawing.Size(557, 22);
            this.textBoxOntologyPath.TabIndex = 4;
            this.textBoxOntologyPath.TextChanged += new System.EventHandler(this.textBoxOntologyPath_TextChanged);
            // 
            // browseOntologyButton
            // 
            this.browseOntologyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browseOntologyButton.Location = new System.Drawing.Point(685, 3);
            this.browseOntologyButton.Name = "browseOntologyButton";
            this.browseOntologyButton.Size = new System.Drawing.Size(75, 29);
            this.browseOntologyButton.TabIndex = 1;
            this.browseOntologyButton.Text = "Обзор...";
            this.browseOntologyButton.UseVisualStyleBackColor = true;
            this.browseOntologyButton.Click += new System.EventHandler(this.browseOntologyButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Онтология:";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(697, 126);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 30);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(616, 126);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 30);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // openOntologyDialog
            // 
            this.openOntologyDialog.FileName = "ontology.xml";
            this.openOntologyDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openOntologyDialog_FileOk);
            // 
            // openGramtabDialog
            // 
            this.openGramtabDialog.FileName = "gramtab.xml";
            this.openGramtabDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openGramtabDialog_FileOk);
            // 
            // openSomethingDialog
            // 
            this.openSomethingDialog.FileName = "something";
            this.openSomethingDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openSomethingDialog_FileOk);
            // 
            // SettingsPathsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(792, 165);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsPathsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки путей";
            this.Load += new System.EventHandler(this.SettingsPathsDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseOntologyButton;
        private System.Windows.Forms.Button browseGramtabButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxOntologyPath;
        private System.Windows.Forms.TextBox textBoxGramtabPath;
        private System.Windows.Forms.TextBox textBoxSomethingPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.OpenFileDialog openOntologyDialog;
        private System.Windows.Forms.OpenFileDialog openGramtabDialog;
        private System.Windows.Forms.OpenFileDialog openSomethingDialog;
    }
}