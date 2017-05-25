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
using KlanVocabularyExtractor;

namespace HelloForms
{
    public partial class MainWindow : Form
    {

        Dictionary<Scheme, ElementHost> NVHosts;
        System.Windows.Controls.ContextMenu NVViewContextMenu;

        Scheme CurrentScheme {
            get {return schemeTabViewPage.Tag as Scheme;}
            set
            {
                schemeTabViewPage.Controls.RemoveByKey(EditorConstants.TABPAGE_WPF_HOST_NAME);
                schemeTabViewPage.Controls.Add(NVHosts[value]);
                schemeTabViewPage.Tag = value;
            }
        }

        FactSchemeBank Bank;
        List<VocTheme> Themes;

        public MainWindow()
        {

            FloatingPointReset.Action();
            InitializeComponent();
            //Ontology Tree localization
            //check MSDN's LocalizableAttribute for proper localization!
            this.addArgumentMenuItem.Text = Locale.ONTOLOGY_TREE_ADD_ARG;
            this.addResultMenuItem.Text = Locale.ONTOLOGY_TREE_ADD_RESULT;

            ontologyTreeView.NodeMouseClick += (s, e) => ontologyTreeView.SelectedNode = e.Node;
            dictionaryTreeView.NodeMouseClick += (s, e) => dictionaryTreeView.SelectedNode = e.Node;

            Medium.onAttributeSetup += (attr, arg) => updateDataGrid(attr, arg);

            NVHosts = new Dictionary<Scheme, ElementHost>();
            NVViewContextMenu = new System.Windows.Controls.ContextMenu();
            NVViewContextMenu.Items.Add("N.View context menu");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ontologyTreeView.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
            dataGridView1.AutoGenerateColumns = false;

            var conditionTypeColumn = dataGridView1.Columns[EditorConstants.CONDITION_DATAGRID_TYPE_COL] as DataGridViewComboBoxColumn;
            conditionTypeColumn.ValueType = typeof(Argument.ArgumentCondition.ConditionType);
            conditionTypeColumn.DataSource = Enum.GetValues(typeof(Argument.ArgumentCondition.ConditionType));

            var comparTypeColumn = dataGridView1.Columns[EditorConstants.CONDITION_DATAGRID_COMPAR_COL] as DataGridViewComboBoxColumn;
            comparTypeColumn.ValueType = typeof(Argument.ArgumentCondition.ComparisonType);
            comparTypeColumn.DataSource = Enum.GetValues(typeof(Argument.ArgumentCondition.ComparisonType));


            loadDictionary(Properties.Settings.Default["DictionaryPath"] as String);
            loadOntologyTree(Properties.Settings.Default["OntologyPath"] as String);
            createScheme();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Clear();
            OntologyNode node = (OntologyNode)e.Node.Tag;
            if (node is OntologyClass){
                listView1.Columns.Add("Атрибут"); //bad hardcode
                listView1.Columns.Add("Тип");
                listView1.Columns.Add("Унаследован");
                List<OntologyNode.Attribute> attrs = ((OntologyClass)node).OwnAttributes;
                
                List<Tuple<OntologyNode.Attribute, OntologyClass>> inheritedAttrs = ((OntologyClass)node).InheritedAttributes;

                foreach(OntologyNode.Attribute attr in attrs)
                {
                    string theme = attr.AttrType == OntologyNode.Attribute.AttributeType.TERMIN ? "|" + attr.Theme.name : "";
                    string[] values = { attr.Name, attr.AttrType.ToString() + theme, "" };
                    ListViewItem item = new ListViewItem(values);
                    listView1.Items.Add(item);
                }
                foreach(Tuple<OntologyNode.Attribute, OntologyClass> inheritedAtt in inheritedAttrs)
                {
                    string theme = inheritedAtt.Item1.AttrType == OntologyNode.Attribute.AttributeType.TERMIN ? "|" + inheritedAtt.Item1.Theme.name : "";
                    string[] values = { inheritedAtt.Item1.Name, inheritedAtt.Item1.AttrType.ToString() + theme, inheritedAtt.Item2.Name};
                    ListViewItem item = new ListViewItem(values);
                    listView1.Items.Add(item);
                }
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

            XElement xmarkup = doc.Root.Element(EditorConstants.XML_EDITOR_MARKUP);
            if (xmarkup != null)
            {
                foreach(Scheme scheme in Bank.Schemes)
                {
                    XElement xscheme = xmarkup.Element(scheme.Name);
                    ElementHost host = initNVHost(scheme);
                    network.NetworkView nv = host.Child as network.NetworkView;
                    foreach (XElement xel in xscheme.Elements())
                    {
                        network.Node node = null;
                        System.Windows.Thickness margin = new System.Windows.Thickness();
                        if (xel.Attribute("type").Value == typeof(Argument).ToString())
                        {
                            Argument arg = scheme.Arguments.First(x => x.Order == int.Parse(xel.Attribute("id").Value));
                            node = nv.AddNode(Medium.Convert(arg));
                        }
                        else if (xel.Attribute("type").Value == typeof(Result).ToString())
                        {
                            Result res = scheme.Results.First(x => x.Name == xel.Attribute("id").Value);
                            node = nv.AddNode(Medium.Convert(res));
                        }
                        else if (xel.Attribute("type").Value == typeof(Functor).ToString())
                        {
                            Functor f = scheme.Functors.First(x => x.ID == xel.Attribute("id").Value);
                            node = nv.AddNode(Medium.Convert(f));
                        }
                        else
                            continue;
                        int left = int.Parse(xel.Attribute("left").Value);
                        int top = int.Parse(xel.Attribute("top").Value);
                        margin.Left = left;
                        margin.Top = top;
                        node.Margin = margin;
                    }
                    Medium.LoadViewFromScheme(nv, scheme);
                }
            }
            updateBankListView();
            fstream.Close();
        }

#region resource loading
        private void loadOntologyTree(String filename)
        {
            if (String.IsNullOrEmpty(filename))
                return;
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

            List<OntologyNode> ontology = OntologyBuilder.fromXml(fstream, Themes);
            fstream.Close();

            ontology.Reverse();
            Stack<OntologyNode> s = new Stack<OntologyNode>(ontology); //DFS add ontology nodes to treeview
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
                if (ontNode is OntologyClass && ((OntologyClass)ontNode).Children.Count > 0)
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

        private void loadDictionary(String path)
        {

            FloatingPointReset.Action();
            if (path == null || path.Length == 0)
                return;
            Extractor ex = new Extractor(ref path);
            List<VocTheme> themes = ex.Themes();
            FloatingPointReset.Action();


            //list always comes ordered so that parents are always defined before children (?)
            themes.Reverse();
            Stack<VocTheme> s = new Stack<VocTheme>(themes);
            TreeNodeCollection baseNodeCollection = dictionaryTreeView.Nodes;
            while (s.Any())
            {
                VocTheme theme = s.Pop();
                if (theme == null)
                {
                    if (baseNodeCollection[0].Parent.Parent == null)
                        baseNodeCollection = dictionaryTreeView.Nodes;
                    else
                        baseNodeCollection = baseNodeCollection[0].Parent.Parent.Nodes;
                    continue;
                }
                TreeNode treeNode = new TreeNode(theme.name);
                treeNode.Tag = theme;

                //make sure children are not added to the root
                if (theme.parents.Count == 0 || baseNodeCollection != dictionaryTreeView.Nodes)
                    baseNodeCollection.Add(treeNode);

                if (theme.children.Count > 0)
                {
                    s.Push(null);
                    foreach (VocTheme child in theme.children)
                        s.Push(child);
                    baseNodeCollection = treeNode.Nodes;
                }
            }

            themes.Reverse();
            Themes = themes;
        }

#endregion

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
            XElement xbank = null;
            try
            {
                xbank = Bank.ToXml().Root;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
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
                        string id;
                        if (node.Tag is Argument)
                            id = (node.Tag as Argument).Order.ToString();
                        else
                            id = (node.TagName);
                        XElement xnode = new XElement("node",
                            new XAttribute("type", node.Tag.GetType().ToString()),
                            new XAttribute("id", id),
                            new XAttribute("left", (int)node.Margin.Left),
                            new XAttribute("top", (int)node.Margin.Top));
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
                loadDictionary(Properties.Settings.Default["DictionaryPath"] as String);
            }
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

        //private System.Windows.Thickness nvCanvasOffset = new System.Windows.Thickness( -short.MaxValue / 2.0, -short.MaxValue / 2.0 , 0, 0);
        ElementHost initNVHost(Scheme scheme)
        {
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.AutoSize = true;
            elementHost.AllowDrop = true;
            elementHost.Name = EditorConstants.TABPAGE_WPF_HOST_NAME;

            //create a networkview
            network.NetworkView nv = new network.NetworkView();
            
            nv.Drop += Nv_Drop;
            nv.NodeAdded += NV_NodeAdded;
            nv.NodeRemoving += NV_NodeRemoving;
            nv.ConnectionAdded += NV_ConnectionAdded;
            nv.ConnectionRemoved += NV_ConnectionRemoved;
            nv.ContextMenu = NVViewContextMenu;
            nv.NodeSelected += (s, e) =>
            {
            };

            elementHost.Child = nv;
            //nv.Margin = nvCanvasOffset;

            NVHosts.Add(scheme, elementHost);

            return elementHost;
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
            dataGridView1.Enabled = false;
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

        private void NV_NodeRemoving(object sender, System.Windows.RoutedEventArgs e)
        {
            var node = e.Source as network.Node;
            
            ISchemeComponent comp = ((network.Node)e.Source).Tag as ISchemeComponent;
            var oneighbors = node.OutgoingNeighbors;
            foreach (var neighbor in oneighbors)
                (neighbor.Tag as ISchemeComponent).RemoveUpper(comp);
            CurrentScheme.RemoveComponent(comp);
            Console.WriteLine("NV_NodeRemoving!");
            dataGridView1.Enabled = false;
        }

        private void NV_ConnectionAdded(object sender, network.ConnectionEventArgs e)
        {
            var nv = sender as network.NetworkView;
            var src = e.SourceConnector;
            var dest = e.DestConnector;

            Medium.AddSchemeConnection(CurrentScheme, src, dest);
        }

        void NV_ConnectionRemoved(object sender, network.ConnectionEventArgs e)
        {
            Medium.RemoveSchemeConnection(e.SourceConnector, e.DestConnector);
        }

        private OntologyClass menuItemToClass(ToolStripMenuItem item)
        {
            TreeNode selectedNode = ((item.GetCurrentParent() as ContextMenuStrip).SourceControl as TreeView).SelectedNode;
            OntologyClass ontologyClass = selectedNode.Tag as OntologyClass;
            return ontologyClass;
        }

        private VocTheme menuItemToTheme(ToolStripMenuItem item)
        {
            TreeNode selectedNode = ((item.GetCurrentParent() as ContextMenuStrip).SourceControl as TreeView).SelectedNode;
            VocTheme theme = selectedNode.Tag as VocTheme;
            return theme;
        }


        private void addDictionaryArgumentMenuItem_Click(object sender, EventArgs e)
        {
            network.NetworkView nv = getCurrentNetworkView();
            if (nv == null)
                return;
            var theme = menuItemToTheme(sender as ToolStripMenuItem);
            FactScheme.Argument arg = CurrentScheme.AddArgument(theme);

            network.Node node = nv.AddNode(Medium.Convert(arg), true);
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
            dataGridView1.Enabled = false;
        }

        private void schemesTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == schemeTabXMLPage)
                try
                {
                    schemeXMLTextBox.Text = CurrentScheme.ToXml().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            XMLHighlight.HighlightRTF(schemeXMLTextBox);
        }

        private void window_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if ((sender as SplitContainer).DataBindings["SplitterDistance"] != null)
                (sender as SplitContainer).DataBindings["SplitterDistance"].WriteValue();
        }

        private void bankListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.CancelEdit)
                return;
            var item = (sender as ListView).SelectedItems[0];
            var scheme = item.Tag as Scheme;
            if (scheme != null && e.Label != "")
                scheme.Name = e.Label;
        }

        private void updateDataGrid(OntologyNode.Attribute attr, Argument arg)
        {
            dataGridView1.Tag = arg;
            dataGridView1.Enabled = true;
            argumentConditionsGroupBox.Text = String.Format(
                "Ограничения arg{0} {1} :: Атрибут {2}",
                arg.Order, arg.Name, attr.Name);
            //updateDataGrid();
            var binding = new BindingSource();
            if (!arg.Сonditions.ContainsKey(attr))
                arg.Сonditions.Add(attr, new List<Argument.ArgumentCondition>());
            binding.DataSource = arg.Сonditions[attr];
            dataGridView1.DataSource = binding;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }
    }
}
