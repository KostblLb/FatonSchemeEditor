using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloForms
{
    public enum ConnectorMode { Input, Output};

    public partial class Connector : UserControl
    {
        ConnectorMode _mode;
        HashSet<Connector> _connections;

        public ConnectorMode Mode
        {
            get { return _mode; }
        }
        public HashSet<Connector> Connections
        {
            get { return _connections; }
        }

        private void mouseMoveHandler(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left){
                DragDropEffects dropEffect = this.DoDragDrop(this, DragDropEffects.Link);
            }
        }

        private void dragEnterHandler(object sender, DragEventArgs e)
        {
            Connector other = e.Data.GetData(typeof(Connector)) as Connector;
            if (other.Mode != this._mode)
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void dragDropHandler(object sender, DragEventArgs e)
        {
            Connector other = e.Data.GetData(typeof(Connector)) as Connector;
            if (this._mode != other.Mode  && 
                !this._connections.Contains(other) &&
                (other.Mode != ConnectorMode.Input || other.Connections.Count == 0) && //inout connector has only one connection
                (this._mode != ConnectorMode.Input || this._connections.Count == 0) )
            {
                Connector input, output;
                this._connections.Add(other);
                other.Connections.Add(this);
                if(this._mode == ConnectorMode.Input)
                {
                    input = this;
                    output = other;
                }
                else
                {
                    input = other;
                    output = this;
                }

                //input: result or func
                if (input.Parent.Parent.Tag is FactScheme.Result)
                {
                    FactScheme.Result result = input.Parent.Parent.Tag as FactScheme.Result;
                    object ruleReference = output.Parent.Parent.Tag;
                    if (ruleReference is FactScheme.Argument || 
                        ruleReference is FactScheme.Result)
                    {
                        result.AddRule(FactScheme.Result.RuleType.Define,
                                        input.Tag as OntologyNode.Attribute,
                                        ruleReference,
                                        output.Tag as OntologyNode.Attribute);
                    }
                    else if(ruleReference is Functor)
                    {
                        result.AddRule(FactScheme.Result.RuleType.Function,
                                        input.Tag as OntologyNode.Attribute,
                                        ruleReference,
                                        null);
                    }
                }
                else if(input.Parent.Parent.Tag is Functor){

                }
                else
                {
                    throw new Exception("only RESULTS and FUNCTORS have inputs");
                }
                
                this.Parent.Parent.Parent.Invalidate(); //FIXME will break someday
            }
        }

        public void DrawConnections(object sender, PaintEventArgs e)
        {
            if (this._mode == ConnectorMode.Input)
                return;

            Graphics g = e.Graphics;
            foreach(Connector conn in this._connections)
            {
                Point a = (sender as Control).PointToClient(this.PointToScreen(this.Location));
                Point d = (sender as Control).PointToClient(conn.PointToScreen(conn.Location));
                a.Y -= this.Location.Y;
                d.Y -= conn.Location.Y;
                Point b = new Point(a.X + (d.X-a.X)/2, a.Y);
                Point c = new Point(a.X + (d.X-a.X)/2, d.Y);
                //g.DrawLine(new Pen(Color.Black, 1), a, d);
                g.DrawBezier(new Pen(Color.Black, 1), a, b, c, d);
            }
        }

        public Connector(object tag, ConnectorMode mode = ConnectorMode.Input)
        {
            InitializeComponent();
            _mode = mode;
            _connections = new HashSet<Connector>();
            this.Tag = tag;

            this.MouseMove += new MouseEventHandler(mouseMoveHandler);
            this.DragEnter += new DragEventHandler(dragEnterHandler);
            this.DragDrop += new DragEventHandler(dragDropHandler);
        }

    }
}
