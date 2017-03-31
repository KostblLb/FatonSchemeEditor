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
            foreach (OntologyNode.Attribute attr in ((OntologyClass)arg.Origin).OwnAttributes)
            {
                result.Add(attr.Name);
                //if(arg.)
                //! CHANGE COLOR IF CONDITION FOR ATTR EXISTS
            }
            if (inheritance)
                foreach (Tuple<OntologyNode.Attribute, OntologyClass> attr in ((OntologyClass)arg.Origin).InheritedAttributes)
                {
                    result.Add(attr.Item1.Name);
                }
            return result;
        }
        void toggleInheritClick(object sender, EventArgs e)
        {
            Control argPanel = ((Control)sender).Parent;
            FactScheme.Argument arg = argPanel.Parent.Tag as FactScheme.Argument;
            bool check = ((CheckBox)sender).Checked;

            foreach(Control attr in argPanel.Controls.Find("attrNameInherited", false))
            {
                attr.Visible = check;
            }

            Control outputs = argPanel.Parent.Controls.Find("outputPanel", false)[0];
            foreach (Control output in outputs.Controls.Find("outInherited", false))
            {
                output.Visible = check;
            }

            //argPanel.Controls.remove
            //TreeView tv = (TreeView) ((Control)sender).Parent.Controls.Find("AttributesTree", false)[0];
            // tv.Nodes.Clear();
            //foreach (string attr in listArgumentAttributes(arg, check))
            //    tv.Nodes.Add(attr);
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
                    Label ownerLabel = owner.SourceControl as Label;
                    FactScheme.Argument arg = ownerLabel.Parent.Parent.Tag as FactScheme.Argument;
                    arg.AddContition(ownerLabel.Text);
                    reloadArgConditions(arg);
                }
                else
                    MessageBox.Show("Не выбран атрибут"); //FIXME russian
            }
        }

        static bool mousedown = false;
        static Point motionstart = Point.Empty;
        static void panelMouseMove(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            //while (!(control is FlowLayoutPanel))
            while(!(control.Name.Equals("DraggablePanelBase")))
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
            control.ControlAdded += new ControlEventHandler(panelControlAdded);
        }

        static private void panelLocationChanged(object sender, EventArgs e)
        {
            if((sender as Control).Parent != null)
                (sender as Control).Parent.Invalidate();
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
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel.Name = "DraggablePanelBase";

            panel.MouseDown += new MouseEventHandler(panelMouseDown);
            panel.MouseMove += new MouseEventHandler(panelMouseMove);
            panel.MouseUp += new MouseEventHandler(panelMouseUp);
            panel.ControlAdded += new ControlEventHandler(panelControlAdded);
            panel.LocationChanged += new EventHandler(panelLocationChanged);

            return panel;
        }

        FlowLayoutPanel ArgumentPanel(FactScheme.Argument arg)
        {
            FlowLayoutPanel basePanel = DraggablePanel();
            FlowLayoutPanel argumentPanel = new FlowLayoutPanel();
            argumentPanel.ControlAdded += new ControlEventHandler(panelControlAdded);
            argumentPanel.Size = Size.Empty;
            argumentPanel.AutoSize = true;
            argumentPanel.FlowDirection = FlowDirection.TopDown;
            argumentPanel.BackColor = Color.AliceBlue;
            argumentPanel.BorderStyle = BorderStyle.FixedSingle;
            argumentPanel.Name = "argPanel";

            argumentPanel.Controls.Add(objNameLabel(arg.Name));
            Label label = new Label();
            label.Name = "ObjectType";
            label.Text = "arg" + arg.Order + " " + arg.TypeString;
            argumentPanel.Controls.Add(label);

            Splitter splitter1 = new Splitter();
            splitter1.BackColor = System.Drawing.Color.Black;
            splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(50, 1);
            argumentPanel.Controls.Add(splitter1);

            Panel output = new Panel();
            output.Size = Size.Empty;
            output.AutoSize = true;
            output.Name = "outputPanel";

            if (arg.Origin.type == OntologyNode.Type.Class)
            {
                CheckBox toggleInherit = new CheckBox();
                toggleInherit.Name = "UseInheritance";
                toggleInherit.AutoSize = true;
                toggleInherit.Text = "Наследовать атрибуты";
                toggleInherit.Checked = arg.Inheritance;
                toggleInherit.Click += new EventHandler(toggleInheritClick);
                argumentPanel.Controls.Add(toggleInherit);

                ContextMenuStrip cmenu = new ContextMenuStrip();
                ToolStripMenuItem itemAddCondition = new ToolStripMenuItem("Добавить условие");
                itemAddCondition.Click += new EventHandler(menuAddArgCondition);
                cmenu.Items.Add(itemAddCondition);

                int ownAttrsCount = arg.Origin.OwnAttributes.Count;
                List<OntologyNode.Attribute> allAttrs = (arg.Origin as OntologyClass).AllAttributes;
                int allAttrsCount = allAttrs.Count;
                for(int i = 0; i < allAttrsCount; i++)
                {
                    OntologyNode.Attribute attr = allAttrs[i];

                    //Connector cout = new Connector(attr, ConnectorMode.Output);
                    //output.Controls.Add(cout);

                    Label resultAttrName = new System.Windows.Forms.Label();
                    resultAttrName.AutoEllipsis = true;
                    resultAttrName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
                    if (i < ownAttrsCount)
                    {
                        resultAttrName.Name = "attrName";
                     //   cout.Name = "out";
                    }
                    else
                    {
                        resultAttrName.Name = "attrNameInherited";
                    //    cout.Name = "outInherited";
                    }
                    resultAttrName.Size = new System.Drawing.Size(150, 17);
                    //resultAttrName.TabIndex = 4;
                    resultAttrName.Text = attr.Name;
                    resultAttrName.ContextMenuStrip = cmenu;
                    resultAttrName.Tag = attr;
                    argumentPanel.Controls.Add(resultAttrName);
                    
                    
                }


                //attrsTree.ContextMenuStrip = cmenu;

                //attrsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(argTreeView_NodeMouseClick);
                //argumentPanel.Controls.Add(attrsTree);
                //attrsTree.Width = 200;
            }

            //label = new Label();

            basePanel.MouseClick += new MouseEventHandler(argumentPanelClick);
            basePanel.Tag = arg;

            basePanel.Controls.Add(argumentPanel);
            basePanel.Controls.Add(output);
            return basePanel;
        }
        FlowLayoutPanel ResultPanel(FactScheme.Result res)
        {
            OntologyClass ontologyClass = res.Reference as OntologyClass; // TODO ADD SUPPORT FOR EDIT MODE

            FlowLayoutPanel basePanel = DraggablePanel();
            FlowLayoutPanel resultPanel = new FlowLayoutPanel();
            resultPanel.ControlAdded += new ControlEventHandler(panelControlAdded);
            resultPanel.FlowDirection = FlowDirection.TopDown;
            resultPanel.AutoSize = true;
            resultPanel.Name = "resultPanel";
            //resultPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Label resultNameLabel = new System.Windows.Forms.Label();
            Label resultTypeLabel = new System.Windows.Forms.Label();
            Splitter splitter1 = new System.Windows.Forms.Splitter();

            resultNameLabel.AutoSize = true;
            resultNameLabel.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            resultNameLabel.Location = new System.Drawing.Point(8, 5);
            resultNameLabel.Name = "resultNameLabel";
            resultNameLabel.Size = new System.Drawing.Size(164, 23);
            //resultNameLabel.TabIndex = 0;
            resultNameLabel.Text = res.Name;

            resultTypeLabel.AutoSize = true;
            resultTypeLabel.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            resultTypeLabel.Location = new System.Drawing.Point(8, 28);
            resultTypeLabel.Name = "resultTypeLabel";
            resultTypeLabel.Size = new System.Drawing.Size(164, 19);
            //resultTypeLabel.TabIndex = 2;
            resultTypeLabel.Text = String.Format("{0}: {1}", res.Type, ontologyClass.Name);

            splitter1.BackColor = System.Drawing.Color.Black;
            splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            splitter1.Location = new System.Drawing.Point(8, 50);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(50, 3);
            //splitter1.TabIndex = 3;
            //splitter1.TabStop = false;

            resultPanel.Padding = new System.Windows.Forms.Padding(5);
            resultPanel.Size = new System.Drawing.Size(201, 90);
            resultPanel.AutoSize = true;
            resultPanel.BackColor = System.Drawing.Color.SeaShell;
            resultPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resultPanel.Controls.Add(resultNameLabel);
            resultPanel.Controls.Add(resultTypeLabel);
            resultPanel.Controls.Add(splitter1);

            Panel input = new Panel();
            input.Size = Size.Empty;
            input.AutoSize = true;
            input.Name = "inputPanel";

            Panel output = new Panel();
            output.Size = Size.Empty;
            output.AutoSize = true;
            output.Name = "outputPanel";
            
            foreach(OntologyNode.Attribute attr in ontologyClass.AllAttributes)
            {
                Label resultAttrName = new System.Windows.Forms.Label();
                resultAttrName.AutoEllipsis = true;
                resultAttrName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
                resultAttrName.Name = "resultAttrName";
                resultAttrName.Size = new System.Drawing.Size(150, 17);
                //resultAttrName.TabIndex = 4;
                resultAttrName.Text = attr.Name;
                resultPanel.Controls.Add(resultAttrName);

               // Connector cin = new Connector(attr, ConnectorMode.Input);
               // Connector cout = new Connector(attr, ConnectorMode.Output);
               // input.Controls.Add(cin);
               // output.Controls.Add(cout);
                //cout.Location = cin.Location = resultAttrName.Location;
            }

            //resultPanel.TabIndex = 0;
            basePanel.Controls.Add(input);
            basePanel.Controls.Add(resultPanel);
            basePanel.Controls.Add(output);
            basePanel.Tag = res;

            return basePanel;
        }
        FlowLayoutPanel FunctorPanel(FactScheme.Functor func)
        {
            FlowLayoutPanel basePanel = DraggablePanel();
            FlowLayoutPanel panel = new FlowLayoutPanel();

            panel.ControlAdded += new ControlEventHandler(panelControlAdded);
            panel.FlowDirection = FlowDirection.TopDown;
            panel.AutoSize = true;
            panel.Name = "functorPanel";
            //resultPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Label panelName = new System.Windows.Forms.Label();
            Splitter splitter1 = new System.Windows.Forms.Splitter();

            panelName.AutoSize = true;
            panelName.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            panelName.Location = new System.Drawing.Point(8, 5);
            panelName.Name = "resultNameLabel";
            panelName.Size = new System.Drawing.Size(164, 23);
            panelName.Text = func.ID;

            splitter1.BackColor = System.Drawing.Color.Black;
            splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            splitter1.Location = new System.Drawing.Point(8, 50);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(50, 1);


            panel.Padding = new System.Windows.Forms.Padding(5);
            panel.Size = new System.Drawing.Size(201, 90);
            panel.AutoSize = true;
            panel.BackColor = System.Drawing.Color.SeaShell;
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Controls.Add(panelName);
            //panel.Controls.Add(resultTypeLabel);
            panel.Controls.Add(splitter1);

            Panel inputPanel = new Panel();
            inputPanel.Size = Size.Empty;
            inputPanel.AutoSize = true;
            inputPanel.Name = "inputPanel";

            Panel outputPanel = new Panel();
            outputPanel.Size = Size.Empty;
            outputPanel.AutoSize = true;
            outputPanel.Name = "outputPanel";

            Label outputLabel = new Label();
            outputLabel.AutoEllipsis = true;
            outputLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            outputLabel.Name = "output";
            outputLabel.Size = new System.Drawing.Size(150, 17);
            outputLabel.Text = "Out";
            panel.Controls.Add(outputLabel);
           // outputPanel.Controls.Add(new Connector(func.Output, ConnectorMode.Output));

            for(int i = 0; i < func.Inputs.Count + 1; i++)
            {
                if(func.NumArgs == -1 || i < func.NumArgs)
                {
                    Label inputLabel = new Label();
                    inputLabel.AutoEllipsis = true;
                    inputLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
                    inputLabel.Name = "input";
                    inputLabel.Size = new System.Drawing.Size(150, 17);
                    inputLabel.Text = "In";
                    panel.Controls.Add(inputLabel);
                    object tag = null;
                    if (i < func.Inputs.Count)
                        tag = func.Inputs[i];

                   // inputPanel.Controls.Add(new Connector(tag, ConnectorMode.Input));
                }
            }

            basePanel.Controls.Add(inputPanel);
            basePanel.Controls.Add(panel);
            basePanel.Controls.Add(outputPanel);
            basePanel.Tag = func;

            return basePanel;
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
            FlowLayoutPanel panel = ArgumentPanel(arg);
            panel.Location = point;
            _panels.Add(panel);
            _parentControl.Controls.Add(panel);
            argumentPanelSelect(panel);

            FlowLayoutPanel argPanel = (FlowLayoutPanel)panel.Controls.Find("argPanel", false)[0];
            Panel output = (Panel)panel.Controls.Find("outputPanel", false)[0];
            Control[] attrNames = argPanel.Controls.Find("attrName", false); //set proper connector positions
            Control[] attrNamesInherited = argPanel.Controls.Find("attrNameInherited", false);
            Control[] z = new Control[attrNames.Length + attrNamesInherited.Length];
            attrNames.CopyTo(z, 0);
            attrNamesInherited.CopyTo(z, attrNames.Length);
            for (int i = 0; i < z.Length; i++)
            {
                Point newLoc = new Point(0, z[i].Location.Y);
                output.Controls[i].Location = newLoc;
            }
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

            FlowLayoutPanel resultPanel = (FlowLayoutPanel) panel.Controls.Find("resultPanel", false)[0];
            Panel input = (Panel)panel.Controls.Find("inputPanel", false)[0];
            Panel output = (Panel)panel.Controls.Find("outputPanel", false)[0];
            Control[] attrNames = resultPanel.Controls.Find("resultAttrName", false); //set proper connector positions
            for (int i = 0; i < attrNames.Length; i++)
            {
                Point newLoc = new Point(0, attrNames[i].Location.Y);
                input.Controls[i].Location = output.Controls[i].Location = newLoc;
            }
        }

        public void AddFunctor(Point point, FactScheme.Functor func = null)
        {
            if(func == null)
            {
                func = _scheme.AddFunctor();
            }
            FlowLayoutPanel panel = FunctorPanel(func);
            Point location = _parentControl.PointToClient(point);
            panel.Location = location;
            _parentControl.Controls.Add(panel);

            FlowLayoutPanel functorPanel = (FlowLayoutPanel)panel.Controls.Find("functorPanel", false)[0];
            Panel input = (Panel)panel.Controls.Find("inputPanel", false)[0];
            Panel output = (Panel)panel.Controls.Find("outputPanel", false)[0];
            Control[] inputLabels = functorPanel.Controls.Find("input", false);
            Control[] outputLabels = functorPanel.Controls.Find("output", false);
            for (int i = 0; i < inputLabels.Length; i++)
            {
                Point newLoc = new Point(0, inputLabels[i].Location.Y);
                input.Controls[i].Location = input.Controls[i].Location = newLoc;
            }
            for (int i = 0; i < outputLabels.Length; i++)
            {
                Point newLoc = new Point(0, outputLabels[i].Location.Y);
                output.Controls[i].Location = output.Controls[i].Location = newLoc;
            }
        }
    }
}
