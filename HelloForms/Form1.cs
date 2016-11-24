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

namespace HelloForms
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            treeView.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);

            tabPage1.AllowDrop = true;
            tabPage1.DragEnter += new DragEventHandler(tabPage_DragEnter);
            tabPage1.DragOver += new DragEventHandler(tabPage_DragOver);
            tabPage1.DragDrop += new DragEventHandler(tabPage_DragDrop);
            tabPage1.Paint += new PaintEventHandler(tabPage_Paint);
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
                List<OntologyNode.Attribute> attrs = ((Class)node).Attributes;
                
                List<Tuple<OntologyNode.Attribute, Class>> inheritedAttrs = ((Class)node).InheritedAttributes;
                /*
                Class parent = ((Class)node).parent;
                List<string> inheritance = new List<string>();
                while (parent != null)
                {
                    attrs.AddRange(parent.attrs);
                    for (int i = 0; i < parent.attrs.Count; i++)
                        inheritance.Add(parent.Name);
                    parent = parent.parent; // :(
                }*/
                foreach(OntologyNode.Attribute attr in attrs)
                {
                    string[] values = { attr.Name, attr.Type, "" };
                    ListViewItem item = new ListViewItem(values);
                    listView1.Items.Add(item);
                }
                foreach(Tuple<OntologyNode.Attribute, Class> inheritedAtt in inheritedAttrs)
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
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        private void tabPage_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
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

            /*
            // Confirm that the node at the drop location is not 
            // the dragged node or a descendant of the dragged node.
            if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
            {
                // If it is a move operation, remove the node from its current 
                // location and add it to the node at the drop location.
                if (e.Effect == DragDropEffects.Move)
                {
                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);
                }

                // If it is a copy operation, clone the dragged node 
                // and add it to the node at the drop location.
                else if (e.Effect == DragDropEffects.Copy)
                {
                    targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
                }

                // Expand the node at the location 
                // to show the dropped node.
                targetNode.Expand();
                
            }
            */
        }

        private void tabPage_Paint(object sender, PaintEventArgs e)
        {
            //TODO REMOVE
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.IO.Stream fstream = openFileDialog1.OpenFile();
            List<OntologyNode> ontology = OntologyBuilder.fromXmlTest(fstream);
            fstream.Close();

            ontology.Reverse();
            Stack<OntologyNode> s = new Stack<OntologyNode>(ontology); //BFS add ontology nodes to treeview
            ontology.Reverse();
            TreeNodeCollection baseNodeCollection = treeView.Nodes;
            while (s.Any())
            {
                OntologyNode ontNode = s.Pop();
                while(ontNode == null && s.Any()) // null may be last element in stack!
                {
                    ontNode = s.Pop();
                    if (baseNodeCollection[0].Parent == null || baseNodeCollection[0].Parent.Parent == null)
                        baseNodeCollection = treeView.Nodes;
                    else
                        baseNodeCollection = baseNodeCollection[0].Parent.Parent.Nodes; //get all ontNode parent's neighbors
                }
                if (ontNode == null)
                    break;
                TreeNode treeNode = new TreeNode(ontNode.Name);
                treeNode.Tag = ontNode;
                baseNodeCollection.Add(treeNode);
                if(ontNode.type == OntologyNode.Type.Class && ((Class)ontNode).Children.Count > 0)
                {
                    baseNodeCollection = treeNode.Nodes;
                    s.Push(null); //trick to control baseNodeCollection
                    foreach (Class child in ((Class)ontNode).Children)
                    {
                        s.Push(child);
                    }
                }
            }
            //ontologyNodes = OntologyNode
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
            Console.WriteLine("moving inside tab " + e.Location);
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
            System.Xml.Linq.XDocument doc = ((FactScheme)tabControl1.SelectedTab.Tag).ToXml();
            doc.Save(fstream);
            fstream.Close();
        }
    }
}
