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
                (other.Mode != ConnectorMode.Input || other.Connections.Count == 0) ) //inout connector has only one connection
            {
                this._connections.Add(other);
                other.Connections.Add(this);
                
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
                Point a = this.Parent.Parent.Parent.PointToClient(this.PointToScreen(this.Location));
                Point b = (sender as Control).PointToClient(conn.PointToScreen(conn.Location));
                
                g.DrawLine(new Pen(Color.Red, 2), a, b);
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
