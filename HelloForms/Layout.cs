using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HelloForms
{
    public class Layout  //info used to draw scheme in window
    {
        //static Brush argumentBrush = Brushes.Aqua;
        //static Brush resultBrush = Brushes.Azure;

        void reloadArgConditions(FactScheme.Argument arg)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = arg.Conditions;
            _argumentConditions.DataSource = bs;

        }
        private void argumentPanelSelect(FlowLayoutPanel argPanel)
        {
            Console.WriteLine("selected argument panel");
            _argumentConditions.Tag = argPanel.Tag;
            reloadArgConditions(argPanel.Tag as FactScheme.Argument);
        }

        private void argumentPanelClick(object sender, MouseEventArgs e)
        {
            argumentPanelSelect((FlowLayoutPanel)sender);
        }

        List<string> listArgumentAttributes(FactScheme.Argument arg, bool inheritance)
        {
            List<string> result = new List<string>();
            foreach (OntologyNode.Attribute attr in ((Class)arg.Tag).Attributes)
            {
                result.Add(attr.Name);
                //if(arg.)
                //! CHANGE COLOR IF CONDITION FOR ATTR EXISTS
            }
            if (inheritance)
                foreach (Tuple<OntologyNode.Attribute, Class> attr in ((Class)arg.Tag).InheritedAttributes)
                {
                    result.Add(attr.Item1.Name);
                }
            return result;
        }
        void toggleInheritClick(object sender, EventArgs e)
        {
            FactScheme.Argument arg = ((Control)sender).Parent.Tag as FactScheme.Argument;
            bool check = ((CheckBox)sender).Checked;
            arg.Inheritance = check;
            TreeView tv = (TreeView) ((Control)sender).Parent.Controls.Find("AttributesTree", false)[0];
            tv.Nodes.Clear();

            foreach (string attr in listArgumentAttributes(arg, check))
                tv.Nodes.Add(attr);
        }

        private void argTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) e.Node.TreeView.SelectedNode = e.Node;
        }
        void menuAddArgCondition(object sender, EventArgs e)
        {
            ToolStripItem item = (sender as ToolStripItem);
            if (item != null)
            {
                ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    TreeView controlTreeView = (TreeView)owner.SourceControl;
                    TreeNode selectedNode = controlTreeView.SelectedNode;
                    FactScheme.Argument arg = controlTreeView.Parent.Tag as FactScheme.Argument;
                    arg.AddContition(selectedNode.Text);
                    reloadArgConditions(arg);
                }
                else
                    MessageBox.Show("Не выбран атрибут");
            }
        }

        static bool mousedown = false;
        static Point motionstart = Point.Empty;
        static void panelMouseMove(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            while (!(control is FlowLayoutPanel))
                control = control.Parent;
            if (mousedown)
            {
                Point loc = ((Panel)control).Location;
                Point mousedelta = new Point(-motionstart.X + e.X, -motionstart.Y + e.Y);
                loc.X += mousedelta.X;
                loc.Y += mousedelta.Y;
                ((Panel)control).Location = loc;
                Console.WriteLine(loc);
            }
        }
        static private void panelMouseDown(object sender, MouseEventArgs e)
        {
            if (!mousedown)
            {
                mousedown = true;
                motionstart = e.Location;
            }
        }
        static private void panelMouseUp(object sender, MouseEventArgs e)
        {
            mousedown = false;
        }

        static private void panelControlAdded(object sender, ControlEventArgs e)
        {
            Control control = e.Control;
            control.MouseDown += new MouseEventHandler(panelMouseDown);
            control.MouseMove += new MouseEventHandler(panelMouseMove);
            control.MouseUp += new MouseEventHandler(panelMouseUp);
        }

        static private Label objNameLabel(string name)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Name = "ObjectName";
            label.Text = name;
            label.Font = new Font(label.Font, FontStyle.Bold);
            return label;
        }

        static FlowLayoutPanel DraggablePanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.AutoSize = true;

            panel.MouseDown += new MouseEventHandler(panelMouseDown);
            panel.MouseMove += new MouseEventHandler(panelMouseMove);
            panel.MouseUp += new MouseEventHandler(panelMouseUp);
            panel.ControlAdded += new ControlEventHandler(panelControlAdded);

            return panel;
        }

        FlowLayoutPanel ArgumentPanel(FactScheme.Argument arg)
        {
            FlowLayoutPanel argumentPanel = DraggablePanel();
            argumentPanel.Size = Size.Empty;
            argumentPanel.FlowDirection = FlowDirection.TopDown;
            argumentPanel.AutoSize = true;
            argumentPanel.BackColor = Color.AliceBlue;
            argumentPanel.BorderStyle = BorderStyle.FixedSingle;
            argumentPanel.Controls.Add(objNameLabel(arg.Name));

            Label label = new Label();
            label.Name = "ObjectType";
            label.Text = "arg" + arg.Order + " " + arg.TypeString;
            argumentPanel.Controls.Add(label);

            if (arg.Tag.type == OntologyNode.Type.Class)
            {
                CheckBox toggleInherit = new CheckBox();
                toggleInherit.Name = "UseInheritance";
                toggleInherit.AutoSize = true;
                toggleInherit.Text = "Наследовать атрибуты";
                toggleInherit.Checked = arg.Inheritance;
                toggleInherit.Click += new EventHandler(toggleInheritClick);
                argumentPanel.Controls.Add(toggleInherit);

                TreeView attrsTree = new TreeView();
                attrsTree.Name = "AttributesTree";
                Padding margin = new Padding(0,0,0,0);
                attrsTree.Margin = margin;
                attrsTree.BackColor = argumentPanel.BackColor;
                attrsTree.FullRowSelect = true;
                attrsTree.BorderStyle = BorderStyle.None;
                attrsTree.ShowLines = false;
                attrsTree.Indent = 0;
                
                //TreeNode root = attrsTree.Nodes.Add("Атрибуты");
                List<string>attrs = listArgumentAttributes(arg, arg.Inheritance);
                foreach (string attr in attrs)
                    //root.Nodes.Add(attr);
                    attrsTree.Nodes.Add(attr);
                ContextMenuStrip cmenu = new ContextMenuStrip();
                ToolStripMenuItem itemAddCondition = new ToolStripMenuItem("Добавить условие");
                itemAddCondition.Click += new EventHandler(menuAddArgCondition);
                cmenu.Items.Add(itemAddCondition);
                attrsTree.ContextMenuStrip = cmenu;

                attrsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(argTreeView_NodeMouseClick);
                argumentPanel.Controls.Add(attrsTree);
                attrsTree.Width = 200;
            }

            //label = new Label();

            argumentPanel.MouseClick += new MouseEventHandler(argumentPanelClick);
            argumentPanel.Tag = arg;
            return argumentPanel;
        }
        FlowLayoutPanel ResultPanel(FactScheme.Result res)
        {
            FlowLayoutPanel resultPanel = DraggablePanel();
            resultPanel.FlowDirection = FlowDirection.TopDown;
            Label label = new Label();
            resultPanel.BackColor = Color.PeachPuff;
            label = new Label();
            label.AutoSize = true;
            label.Name = "ObjectName";
            //label.Text = "Some Result";
            label.Text = res.Name;
            resultPanel.Controls.Add(label);
            return resultPanel;
        }

        Control _parentControl;
        DataGridView _argumentConditions;
        List<FlowLayoutPanel> _panels;
        public List<FlowLayoutPanel> Panels
        {
            get { return _panels; }
            set { _panels = value; }
        }

        FactScheme _scheme;
        public FactScheme Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
        }

        public Layout(Control myParentControl, DataGridView myArgumentConditionsControl ,FactScheme myScheme)
        {
            _panels = new List<FlowLayoutPanel>();
            _parentControl = myParentControl;
            _argumentConditions = myArgumentConditionsControl;
            _scheme = myScheme;
        }

        public void AddArgument(Point point, FactScheme.Argument arg)
        {
            FlowLayoutPanel argPanel = ArgumentPanel(arg);
            argPanel.Location = point;
            _panels.Add(argPanel);
            _parentControl.Controls.Add(argPanel);
            argumentPanelSelect(argPanel);
            argPanel.BringToFront();
        }

        public void AddResult(Point point, FactScheme.Result res = null)
        {
            if (res == null)
            {
                res = _scheme.AddResult();
            }
            FlowLayoutPanel panel = ResultPanel(res);
            Point location = _parentControl.PointToClient(point);
            panel.Location = location;
            _parentControl.Controls.Add(panel);
        }
    }
}
