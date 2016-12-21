using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloForms
{
    public partial class SettingsPathsDialog : Form
    {
        private String _ontologyPath;
        private String _gramtabPath;
        private String _somePath;

        public SettingsPathsDialog()
        {
            InitializeComponent();
        }

        private void SettingsPathsDialog_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default["OntologyPath"] != null)
                _ontologyPath = (String)Properties.Settings.Default["OntologyPath"];
            else
                _ontologyPath = null;

            if (Properties.Settings.Default["GramtabPath"] != null)
                _gramtabPath = (String)Properties.Settings.Default["GramtabPath"];
            else
                _gramtabPath = null;

            if (Properties.Settings.Default["SomePath"] != null)
                _somePath = (String)Properties.Settings.Default["SomePath"];
            else
                _somePath = null;

            textBoxOntologyPath.Text = _ontologyPath;
            textBoxGramtabPath.Text = _gramtabPath;
            textBoxSomethingPath.Text = _somePath;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["OntologyPath"] = _ontologyPath;
            Properties.Settings.Default["GramtabPath"] = _gramtabPath;
            Properties.Settings.Default["SomePath"] = _somePath;
            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBoxOntologyPath_TextChanged(object sender, EventArgs e)
        {
            _ontologyPath = ((TextBox)sender).Text;
        }

        private void textBoxGramtabPath_TextChanged(object sender, EventArgs e)
        {
            _gramtabPath = ((TextBox)sender).Text;
        }

        private void textBoxSomethingPath_TextChanged(object sender, EventArgs e)
        {
            _somePath = ((TextBox)sender).Text;
        }

        private void browseOntologyButton_Click(object sender, EventArgs e)
        {
            openOntologyDialog.ShowDialog();
        }

        private void openOntologyDialog_FileOk(object sender, CancelEventArgs e)
        {
            textBoxOntologyPath.Text = openOntologyDialog.FileName;
        }

        private void browseGramtabButton_Click(object sender, EventArgs e)
        {
            openGramtabDialog.ShowDialog();
        }

        private void openGramtabDialog_FileOk(object sender, CancelEventArgs e)
        {
            textBoxGramtabPath.Text = openGramtabDialog.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void openSomethingDialog_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
