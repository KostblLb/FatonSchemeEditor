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

namespace HelloForms
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            InitializeComponent();

            //Ontology Tree localization
            //check MSDN's LocalizableAttribute for proper localization!
            this.addArgumentMenuItem.Text = Locale.ONTOLOGY_TREE_ADD_ARG;
            this.addResultMenuItem.Text = Locale.ONTOLOGY_TREE_ADD_RESULT;

            ontologyTreeView.NodeMouseClick += (sender, args) => ontologyTreeView.SelectedNode = args.Node;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ontologyTreeView.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);

            tabPage1.AllowDrop = true;
            tabPage1.DragEnter += new DragEventHandler(tabPage_DragEnter);
            tabPage1.DragOver += new DragEventHandler(tabPage_DragOver);
            tabPage1.DragDrop += new DragEventHandler(tabPage_DragDrop);
            tabPage1.Paint += new PaintEventHandler(tabPage_Paint);

            DataGridViewComboBoxColumn conditionTypeColumn = dataGridView1.Columns[EditorConstants.CONDITION_DATAGRID_TYPE_COL] as DataGridViewComboBoxColumn;
            conditionTypeColumn.DataSource = Enum.GetValues(typeof(FactScheme.ConditionType));

            DataGridViewComboBoxColumn comparTypeColumn = dataGridView1.Columns[EditorConstants.CONDITION_DATAGRID_COMPAR_COL] as DataGridViewComboBoxColumn;
            comparTypeColumn.DataSource = Enum.GetValues(typeof(FactScheme.ComparisonType));

            if(!String.IsNullOrEmpty(Properties.Settings.Default["OntologyPath"] as String))
            {
                loadOntologyTree(Properties.Settings.Default["OntologyPath"] as String);
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

        private void tabPage_DragEnter(object sender, DragEventArgs e)
        {
            //0e.Effect = DragDropEffects.Link;
        }

        private void tabPage_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Console.WriteLine(e.Data.GetData(typeof(TreeNode)).ToString());
        }

        private void tabPage_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Console.WriteLine("DRAGGED TO TABS");
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = tabPage1.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            //TreeNode targetNode = treeView1.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            Console.WriteLine(draggedNode.ToString());
            Console.WriteLine(draggedNode.Tag.ToString());

            FactScheme scheme;
            Layout layout;
            if (tabPage1.Tag == null)
            {
                scheme = new FactScheme();
                layout = new Layout(tabPage1, dataGridView1, scheme);
                scheme.Layout = layout;
                tabPage1.Tag = scheme;
            }
            else
                scheme = (FactScheme)tabPage1.Tag;

            scheme.AddArgument((OntologyNode)draggedNode.Tag, targetPoint);
            tabPage1.Invalidate();

        }

        private void tabPage_Paint(object sender, PaintEventArgs e)
        {
            foreach(Control dragPanel in (sender as Control).Controls)
            {
                Control[] outputPanels = dragPanel.Controls.Find("outputPanel", false);
                if(outputPanels.Length > 0)
                {
                   foreach(Control control in outputPanels[0].Controls)
                    {
                      //  (control as Connector).DrawConnections(sender, e);
                    }
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

            //ontologyNodes = OntologyNode
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

            List<OntologyNode> ontology = OntologyBuilder.fromXmlTest(fstream);
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

        private void онтологиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("open file");
            openFileDialog1.FileName = "ontology.xml";
            openFileDialog1.ShowDialog();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void добавитьУсловиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }

        bool mousedown = false;
        Point motionstart = Point.Empty;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location);
            if (mousedown)
            {
                Point loc = ((Panel)sender).Location;
                Point mousedelta = new Point(-motionstart.X +  e.X, -motionstart.Y + e.Y);
                loc.X += mousedelta.X;
                loc.Y += mousedelta.Y;
                ((Panel)sender).Location = loc;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!mousedown)
            {
                mousedown = true;
                motionstart = e.Location;
            }
        }

        private void tabPage1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mousedown = false;
        }

        private void listView2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect_2(object sender, TreeViewEventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "scheme.xml";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.IO.Stream fstream = saveFileDialog1.OpenFile();
            System.Xml.Linq.XDocument doc = ((FactScheme)schemesTabControl.SelectedTab.Tag).ToXml();
            doc.Save(fstream);
            fstream.Close();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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
            List<OntologyNode.Attribute> attrs = ((OntologyClass)arg.Origin).OwnAttributes;
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
                List<Tuple<OntologyNode.Attribute, OntologyClass>> inheritedAttrs = ((OntologyClass)arg.Origin).InheritedAttributes;
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

        private void добавитьРезультатToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point location = ((ToolStripMenuItem)sender).Owner.Location;
            Layout layout = ((FactScheme)tabPage1.Tag).Layout;
            layout.AddResult(location);
        }

        private void добавитьФункторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point location = ((ToolStripMenuItem)sender).Owner.Location;
            Layout layout = ((FactScheme)tabPage1.Tag).Layout;
            layout.AddFunctor(location);
        }

        private void createSchemeTab(object sender, EventArgs e)
        {
            //create new fact scheme
            FactScheme scheme = new FactScheme();
            //scheme.Layout = 

            //create new tabpage
            TabPage tabPage = new TabPage(EditorConstants.DEFAULT_SCHEME_NAME);
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

            //add new page to the tab control
            tabPage.Controls.Add(elementHost);
            schemesTabControl.Controls.Add(tabPage);
            schemesTabControl.SelectedTab = tabPage;

            //attach new scheme to the new page
            tabPage.Tag = scheme;
        }

        private network.NetworkView getCurrentNetworkView()
        {
            TabPage tabPage = schemesTabControl.SelectedTab;
            if (tabPage.Tag == null)
                return null;
            network.NetworkView nv = tabPage.Controls.OfType<ElementHost>().First().Child as network.NetworkView;
            return nv;
        }

        private FactScheme getCurrentScheme()
        {
            return (FactScheme)schemesTabControl.SelectedTab.Tag;
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

            FactScheme scheme = (FactScheme)schemesTabControl.SelectedTab.Tag;

            FactScheme.Argument arg = scheme.AddArgument(ontologyClass, new Point());

            network.Node node = nv.AddNode(Medium.Convert(arg), true); 
        }

        private void NV_NodeAdded(object sender, network.NodeAddedEventArgs e)
        {
            return;
        }

        void test(string str ) { Console.WriteLine("string"); }
        void test(int i) { Console.WriteLine("int"); }

        private void NV_ConnectionAdded(object sender, network.ConnectionAddedEventArgs e)
        {
            var nv = sender as network.NetworkView;
            var scheme = getCurrentScheme();
            //retreive attributes of the real fact scheme objects
            var src = e.SourceConnector.Source;
            var dest = e.DestConnector.Source;
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
            FactScheme scheme = (FactScheme)schemesTabControl.SelectedTab.Tag;
            FactScheme.Argument arg = scheme.AddArgument(ontologyClass, new Point());

            network.Node node = nv.AddNode(Medium.Convert(arg), true);
        }

        private void addResultMenuItem_Click(object sender, EventArgs e)
        {
            network.NetworkView nv = getCurrentNetworkView();
            if (nv == null)
                return;

            OntologyClass ontologyClass = menuItemToClass(sender as ToolStripMenuItem);
            FactScheme scheme = (FactScheme)schemesTabControl.SelectedTab.Tag;
            FactScheme.Result result  = scheme.AddResult(ontologyClass);
            network.Node node = nv.AddNode(Medium.Convert(result), true);
        }
    }
}
