using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml.Linq;
using System.IO;
using Faton;
using Ontology;
using FactScheme;

namespace HelloForms
{
    public partial class MainWindow : Form
    {

        Dictionary<Scheme, ElementHost> NVHosts;

        Scheme CurrentScheme {
            get
            {
                return schemeTabViewPage.Tag as Scheme;
            }
            set
            {
                schemeTabViewPage.Controls.RemoveByKey(EditorConstants.TABPAGE_WPF_HOST_NAME);
                schemeTabViewPage.Controls.Add(NVHosts[value]);
                schemeTabViewPage.Tag = value;
            }
        }

        FactSchemeBank Bank;


        public MainWindow()
        {
            InitializeComponent();

            //Ontology Tree localization
            //check MSDN's LocalizableAttribute for proper localization!
            this.addArgumentMenuItem.Text = Locale.ONTOLOGY_TREE_ADD_ARG;
            this.addResultMenuItem.Text = Locale.ONTOLOGY_TREE_ADD_RESULT;

            ontologyTreeView.NodeMouseClick += (sender, args) => ontologyTreeView.SelectedNode = args.Node;

            NVHosts = new Dictionary<Scheme, ElementHost>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ontologyTreeView.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);

            DataGridViewComboBoxColumn conditionTypeColumn = dataGridView1.Columns[EditorConstants.CONDITION_DATAGRID_TYPE_COL] as DataGridViewComboBoxColumn;
            conditionTypeColumn.DataSource = Enum.GetValues(typeof(ConditionType));

            DataGridViewComboBoxColumn comparTypeColumn = dataGridView1.Columns[EditorConstants.CONDITION_DATAGRID_COMPAR_COL] as DataGridViewComboBoxColumn;
            comparTypeColumn.DataSource = Enum.GetValues(typeof(ComparisonType));

            if(!String.IsNullOrEmpty(Properties.Settings.Default["OntologyPath"] as String))
            {
                loadOntologyTree(Properties.Settings.Default["OntologyPath"] as String);
                createScheme();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Clear();
            OntologyNode node = (OntologyNode)e.Node.Tag;
            if (node.type == OntologyNode.Type.Class){
                listView1.Columns.Add("Атрибут"); //bad hardcode
                listView1.Columns.Add("Тип");
                listView1.Columns.Add("Унаследован");
                List<OntologyNode.Attribute> attrs = ((OntologyClass)node).OwnAttributes;
                
                List<Tuple<OntologyNode.Attribute, OntologyClass>> inheritedAttrs = ((OntologyClass)node).InheritedAttributes;

                foreach(OntologyNode.Attribute attr in attrs)
                {
                    string[] values = { attr.Name, attr.Type, "" };
                    ListViewItem item = new ListViewItem(values);
                    listView1.Items.Add(item);
                }
                foreach(Tuple<OntologyNode.Attribute, OntologyClass> inheritedAtt in inheritedAttrs)
                {
                    string[] values = { inheritedAtt.Item1.Name, inheritedAtt.Item1.Type, inheritedAtt.Item2.Name};
                    ListViewItem item = new ListViewItem(values);
                    listView1.Items.Add(item);
                }
            }
            else if (node.type == OntologyNode.Type.Relation)
            {
                listView1.Columns.Add("Атрибут");
                //foreach(OntologyNode.Attribute attr in (Relation))
            }
            
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Copy the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                //    DoDragDrop(new MyDataObject((e.Item as TreeNode).Tag), DragDropEffects.Move);
                DoDragDrop(new DataContainer((e.Item as TreeNode).Tag), DragDropEffects.Copy);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.IO.FileStream fstream = null;
            try
            {
                fstream = System.IO.File.Open((sender as OpenFileDialog).FileName, System.IO.FileMode.Open);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();

            XDocument doc = XDocument.Parse(xmlString);

            XElement xbank = doc.Root.Element(FatonConstants.XML_BANK_NAME);
            if (xbank == null)
                return;
            Bank = FactSchemeBank.FromXml(xbank, OntologyNode.Ontology); //assuming ontology is loaded
            

            if (doc.Element(EditorConstants.XML_EDITOR_ROOT_NAME) != null)
            {

            }

            fstream.Close();
        }

        private void loadOntologyTree(String filename)
        {
            System.IO.FileStream fstream = null;
            try
            {
                fstream = System.IO.File.Open(filename, System.IO.FileMode.Open);
            } catch(System.IO.FileNotFoundException e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            catch(System.IO.DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            List<OntologyNode> ontology = OntologyBuilder.fromXml(fstream);
            fstream.Close();

            ontology.Reverse();
            Stack<OntologyNode> s = new Stack<OntologyNode>(ontology); //BFS add ontology nodes to treeview
            ontology.Reverse();
            TreeNodeCollection baseNodeCollection = ontologyTreeView.Nodes;
            while (s.Any())
            {
                OntologyNode ontNode = s.Pop();
                while (ontNode == null && s.Any()) // null may be last element in stack!
                {
                    ontNode = s.Pop();
                    if (baseNodeCollection[0].Parent == null || baseNodeCollection[0].Parent.Parent == null)
                        baseNodeCollection = ontologyTreeView.Nodes;
                    else
                        baseNodeCollection = baseNodeCollection[0].Parent.Parent.Nodes; //get all ontNode parent's neighbors
                }
                if (ontNode == null)
                    break;
                TreeNode treeNode = new TreeNode(ontNode.Name);
                treeNode.Tag = ontNode;
                baseNodeCollection.Add(treeNode);
                if (ontNode.type == OntologyNode.Type.Class && ((OntologyClass)ontNode).Children.Count > 0)
                {
                    baseNodeCollection = treeNode.Nodes;
                    s.Push(null); //trick to control baseNodeCollection
                    foreach (OntologyClass child in ((OntologyClass)ontNode).Children)
                    {
                        s.Push(child);
                    }
                }
            }
            OntologyNode.Ontology = ontology;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OntologyNode.Ontology == null || OntologyNode.Ontology.Count == 0)
            {
                MessageBox.Show(Locale.ERR_ONTOLOGY_NOT_LOADED);
                return;
            }
            openFileDialog1.FileName = "scheme.xml";
            openFileDialog1.ShowDialog();
        }

        private void добавитьУсловиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }
        
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "scheme.xml";
            saveFileDialog1.Filter = Locale.FILE_FORMAT_FILTER;
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var dialog = sender as SaveFileDialog;
            System.IO.Stream fstream = saveFileDialog1.OpenFile();
            System.Xml.Linq.XDocument doc = new XDocument();
            XElement xbank = Bank.ToXml().Root;
            if (dialog.FilterIndex == EditorConstants.EDITOR_XML)
            {
                doc.Add(new XElement(EditorConstants.XML_EDITOR_ROOT_NAME));
                doc.Root.Add(xbank);
                XElement xmarkup = new XElement(EditorConstants.XML_EDITOR_MARKUP);
                foreach (Scheme scheme in Bank.Schemes)
                {
                    XElement xscheme = new XElement(scheme.XMLName);
                    network.NetworkView nv = NVHosts[scheme].Child as network.NetworkView;
                    foreach (network.Node node in nv.Nodes)
                    {
                        XElement xnode = new XElement("node", 
                            new XAttribute("name", node.TagName),
                            new XAttribute("left", node.Margin.Left),
                            new XAttribute("top", node.Margin.Top));
                        xscheme.Add(xnode);
                    }
                    xmarkup.Add(xscheme);
                }
                doc.Root.Add(xmarkup);
            }
            else
                doc.Add(xbank);
            doc.Save(fstream);
            fstream.Close();
        }

        private void pathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsPathsDialog dialog = new SettingsPathsDialog();
            DialogResult dr = dialog.ShowDialog(this);
            if(dr == DialogResult.OK)
            {
                loadOntologyTree(Properties.Settings.Default["OntologyPath"] as String);
            }
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].ErrorText = null;
        }
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].ErrorText = null;

            if (e.ColumnIndex != EditorConstants.CONDITION_DATAGRID_ATTR_COL)
            {
                e.Cancel = false;
                return;
            }

            FactScheme.Argument arg = ((DataGridView)sender).Tag as FactScheme.Argument;
            List<OntologyNode.Attribute> attrs = arg.Klass.OwnAttributes;
            String cellAttrName = e.FormattedValue as String;

            foreach (OntologyNode.Attribute attr in attrs)
            {
                if (attr.Name.Equals(cellAttrName))
                {
                    e.Cancel = false;
                    return;
                }
            }

            if (arg.Inheritance)
            {
                List<Tuple<OntologyNode.Attribute, OntologyClass>> inheritedAttrs = arg.Klass.InheritedAttributes;
                foreach(Tuple<OntologyNode.Attribute, OntologyClass> attr in inheritedAttrs)
                {
                    if (attr.Item2.Name.Equals(cellAttrName))
                    {
                        e.Cancel = false;
                        return;
                    }
                }
            }

            e.Cancel = true;
            dataGridView1.Rows[e.RowIndex].ErrorText = EditorConstants.CONDITION_DATAGRID_ERROR_ATT_NAME;

        }

        FactSchemeBank getCurrentBank()
        {
            return bankListView.Tag as FactSchemeBank;
        }

        private void updateBankListView()
        {
            if (Bank == null)
                return;
            bankListView.Items.Clear();
            foreach (Scheme fs in Bank.Schemes)
            {
                ListViewItem listItem = new ListViewItem(fs.Name);
                listItem.Tag = fs;
                bankListView.Items.Add(listItem);
            }
        }

        void initNVHost(Scheme scheme)
        {
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.AutoSize = true;
            elementHost.AllowDrop = true;
            elementHost.Name = EditorConstants.TABPAGE_WPF_HOST_NAME;

            //create a networkview
            network.NetworkView nv = new network.NetworkView();
            elementHost.ContextMenuStrip = layoutTabContextMenu;
            nv.ContextMenu = new System.Windows.Controls.ContextMenu();
            nv.Drop += Nv_Drop;
            nv.NodeAdded += NV_NodeAdded;
            nv.ConnectionAdded += NV_ConnectionAdded;
            elementHost.Child = nv;

            NVHosts.Add(scheme, elementHost);

            //add new page to the tab control
            //schemesTabControl.Controls.Add(tabPage);
            //schemesTabControl.SelectedTab = tabPage;
        }

        private void handleCreateSchemeToolstrip(object sender, EventArgs e)
        {
            createScheme();
        }
        private void createScheme()
        {
            //create new fact scheme
            Scheme scheme = new Scheme(EditorConstants.DEFAULT_SCHEME_NAME);
            if (Bank == null)
            {
                Bank = new FactSchemeBank(EditorConstants.DEFAULT_BANK_NAME);
                bankListView.Tag = Bank;
            }
            getCurrentBank().Schemes.Add(scheme);
            updateBankListView();

            initNVHost(scheme);
            CurrentScheme = scheme;
        }

        private network.NetworkView getCurrentNetworkView()
        {
            TabPage tabPage = schemesTabControl.SelectedTab;
            if (tabPage.Tag == null)
                return null;
            network.NetworkView nv = tabPage.Controls.OfType<ElementHost>().First().Child as network.NetworkView;
            return nv;
        }

        private void Nv_Drop(object sender, System.Windows.DragEventArgs e)
        {
            TabPage tabPage = schemesTabControl.SelectedTab;
            if (tabPage.Tag == null)
                return;
            network.NetworkView nv = tabPage.Controls.OfType<ElementHost>().First().Child as network.NetworkView;

            // Retrieve the client coordinates of the drop location.
            System.Windows.Point p = new System.Windows.Point(MousePosition.X, MousePosition.Y);
            System.Windows.Point targetPoint = nv.PointFromScreen(p);

            // Retrieve the node that was dragged.
            if (!e.Data.GetDataPresent(typeof(DataContainer)))
                return;
            DataContainer ontologyClassContainer = (DataContainer)e.Data.GetData(typeof(DataContainer));
            
            OntologyClass ontologyClass = ontologyClassContainer.Data as OntologyClass;

            //Layout layout;

            Scheme scheme = (Scheme)schemesTabControl.SelectedTab.Tag;

            FactScheme.Argument arg = scheme.AddArgument(ontologyClass);

            network.Node node = nv.AddNode(Medium.Convert(arg), true); 
        }

        private void NV_NodeAdded(object sender, network.NodeAddedEventArgs e)
        {
            return;
        }

        private void NV_ConnectionAdded(object sender, network.ConnectionAddedEventArgs e)
        {
            var nv = sender as network.NetworkView;
            //retreive attributes of the real fact scheme objects
            var src = e.SourceConnector;
            var dest = e.DestConnector;

            Medium.AddSchemeConnection(CurrentScheme, src, dest);
        }

        private OntologyClass menuItemToClass(ToolStripMenuItem item)
        {
            TreeNode selectedNode = ((item.GetCurrentParent() as ContextMenuStrip).SourceControl as TreeView).SelectedNode;
            OntologyClass ontologyClass = selectedNode.Tag as OntologyClass;
            return ontologyClass;
        }



        private void addArgumentMenuItem_Click(object sender, EventArgs e)
        {
            network.NetworkView nv = getCurrentNetworkView();
            if (nv == null)
                return;

            OntologyClass ontologyClass = menuItemToClass(sender as ToolStripMenuItem);
            Scheme scheme = (Scheme)schemesTabControl.SelectedTab.Tag;
            FactScheme.Argument arg = scheme.AddArgument(ontologyClass);

            network.Node node = nv.AddNode(Medium.Convert(arg), true);
        }

        private void addResultMenuItem_Click(object sender, EventArgs e)
        {
            network.NetworkView nv = getCurrentNetworkView();
            if (nv == null)
                return;

            OntologyClass ontologyClass = menuItemToClass(sender as ToolStripMenuItem);
            Scheme scheme = (Scheme)schemesTabControl.SelectedTab.Tag;
            FactScheme.Result result  = scheme.AddResult(ontologyClass);
            network.Node node = nv.AddNode(Medium.Convert(result), true);
        }

        private void bankListView_DoubleClick(object sender, EventArgs e)
        {
            if ((sender as ListView).SelectedItems.Count == 0)
                return;
            CurrentScheme = ((sender as ListView).SelectedItems[0].Tag as Scheme);
        }

        private void schemesTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == schemeTabXMLPage)
                schemeXMLTextBox.Text = CurrentScheme.ToXml().ToString();
            XMLHighlight.HighlightRTF(schemeXMLTextBox);
        }
    }
}
