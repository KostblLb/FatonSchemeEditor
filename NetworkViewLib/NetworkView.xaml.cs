using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace network
{
    public class NodeAddedEventArgs : EventArgs
    {
        public Node Node { get; }
        public NodeAddedEventArgs(Node node)
        {
            Node = node;
        }
    }
    /// <summary>
    /// Canvas-like control for placing Nodes, drawing connections between them
    /// and gathering nodes network info
    /// </summary>
    public partial class NetworkView : Canvas
    {
        public IEnumerable<Node> Nodes
        {
            get { return this.Children.OfType<Node>(); }
        }

        private Node _selectedNode;
        public Node SelectedNode { get { return _selectedNode; } }

        /// <summary>
        /// This event is raised when a drag-n-drop operation is started 
        /// within a connector
        /// </summary>
        public event RoutedEventHandler ConnectorDrag
        {
            add { AddHandler(Connector.ConnectorDragEvent, value); }
            remove { RemoveHandler(Connector.ConnectorDragEvent, value); }
        }

        public event Connector.ConnectionAddedEventHandler ConnectionAdded
        {
            add { AddHandler(Connector.ConnectionAddedEvent, value); }
            remove { RemoveHandler(Connector.ConnectionAddedEvent, value); }
        }

        /// <summary>
        /// This event is raised continuously when a node panel is being moved
        /// </summary>
        public event RoutedEventHandler NodeMoved
        {
            add { AddHandler(Node.NodeMovedEvent, value); }
            remove { RemoveHandler(Node.NodeMovedEvent, value); }
        }

        /// <summary>
        /// This event is raised when a node is clicked
        /// </summary>
        public event RoutedEventHandler NodeSelected
        {
            add { AddHandler(Node.NodeSelectedEvent, value); }
            remove { RemoveHandler(Node.NodeSelectedEvent, value); }
        }


        public delegate void NodeAddedEventHandler(object sender, NodeAddedEventArgs e);

        public event NodeAddedEventHandler NodeAdded;

        #region UI drawing
        private Path connectorToMouseCurve;
        private Connector draggedConnector;
        private Dictionary<Tuple<Connector, Connector>, Path> drawnConnections; 

        private Path DrawBezier(Point start, Point end)
        {
            if (start == null)
                start = new Point();
            if (end == null)
                end = new Point();
            Path p = new Path();
            PathGeometry g = new PathGeometry();
            PathFigure f = new PathFigure();
            f.StartPoint = start;
            f.Segments.Add(new BezierSegment(
                new Point((end.X + start.X) * 0.5, start.Y),
                new Point((end.X + start.X) * 0.5, end.Y),
                end, 
                true));
            g.Figures.Add(f);
            p.Data = g;

            return p;
        }

        private void DrawConnections(Node node)
        {
            foreach (Connector conn in node.Connectors)
            {
                if (conn.Mode == Connector.ConnectorMode.Output)
                    foreach (Connector dest in conn.Connections)
                    {
                        AddConnectionPath(conn, dest);
                    }
                else
                {
                    if (conn.Connections.Count > 0)
                    {
                        Connector src = conn.Connections.First();
                        AddConnectionPath(src, conn);
                    }
                }
            }
        }

        public void DrawConnections()
        {
            foreach(Node node in Nodes)
            {
                DrawConnections(node);
            }
        }

        private void AddConnectionPath(Connector src, Connector dest)
        {
            Point start = src.TranslatePoint(new Point(src.ActualWidth / 2, src.ActualHeight / 2), this);
            Point end = dest.TranslatePoint(new Point(dest.ActualWidth / 2, dest.ActualHeight / 2), this);
            Path p;
            if(drawnConnections.ContainsKey(new Tuple<Connector, Connector>(src, dest)))
            {
                p = drawnConnections[(new Tuple<Connector, Connector>(src, dest))];
                p.Data = DrawBezier(start, end).Data;
            }
            else
            {
                p = DrawBezier(start, end);
                drawnConnections.Add(new Tuple<Connector, Connector>(src, dest), p);
                this.Children.Add(p);
            }
        }

        private void RemoveConnectionPath(Connector src, Connector dest)
        {
            if (!drawnConnections.ContainsKey(new Tuple<Connector, Connector>(src, dest)))
                return;
            else
            {
                Tuple<Connector, Connector> t = new Tuple<Connector, Connector>(src, dest);
                Path p = drawnConnections[t];
                this.Children.Remove(p);
                drawnConnections.Remove(t);
            }
        }

        private void somequery(object sender, QueryContinueDragEventArgs e)
        {
            if (!e.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton))
                connectorToMouseCurve.Visibility = Visibility.Hidden;
        }
        private void ConnectorDragOver(object sender, DragEventArgs e)
        {
            if (draggedConnector == null)
                return;
            Point start = draggedConnector.TranslatePoint(new Point(draggedConnector.ActualWidth/2, draggedConnector.ActualHeight/2), this);
            Point end = e.GetPosition(this);
            connectorToMouseCurve.Data = DrawBezier(start, end).Data;
        }

        private bool ValidateConnection(Connector src, Connector dest)
        {
            if (src.Mode == dest.Mode)
                return false;
            if (src.ParentNode == dest.ParentNode)
                return false;
            return true;
        }
        private void ConnectorDrop(object sender, DragEventArgs e)
        {
            if (!(e.Source is Connector) || draggedConnector == null)
                return;
            if(e.Source is Connector)
            {
                Connector src, dest;
                Path p = new Path();
                p.Data = connectorToMouseCurve.Data;
                if(draggedConnector.Mode == Connector.ConnectorMode.Output)
                {
                    src = draggedConnector;
                    dest = e.Source as Connector;
                }
                else
                {
                    dest = draggedConnector;
                    src = e.Source as Connector;
                }
                if (this.ValidateConnection(src, dest))
                {
                    this.AddConnectionPath(src, dest);
                }
                draggedConnector = null;
            }
        }
        private void NetworkViewConnectorDragStarted(object sender, RoutedEventArgs e)
        {
            draggedConnector = e.Source as Connector;
            connectorToMouseCurve.Visibility = Visibility.Visible;
        }

        private void NetworkViewConnectionAdded(object sender, ConnectionAddedEventArgs e)
        {
            
        }

        private void NetworkViewNodeMoved(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Node))
                return;
            Node node = e.Source as Node;
            DrawConnections(node);
        }

        private void NetworkViewLoaded(object sender, EventArgs e)
        {
            connectorToMouseCurve.Data = Geometry.Empty;
            connectorToMouseCurve.SetValue(Canvas.ZIndexProperty, 999);
            this.Children.Add(connectorToMouseCurve);
            DrawConnections();
        }

        private void NVNodeSelected(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Node))
                return;
            if (this._selectedNode != null)
                this._selectedNode.Deselect();
            this._selectedNode = e.Source as Node;
            this._selectedNode.Select();
        }


        #endregion

        #region Zoom&Pan
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(NetworkView), new PropertyMetadata(1.0));

        public static readonly DependencyProperty MinScaleProperty = DependencyProperty.Register(
            "MinScale", typeof(double), typeof(NetworkView), new PropertyMetadata(0.5));

        public static readonly DependencyProperty MaxScaleProperty = DependencyProperty.Register(
            "MaxScale", typeof(double), typeof(NetworkView), new PropertyMetadata(2.0));

        public static readonly DependencyProperty ScaleStepProperty = DependencyProperty.Register(
            "ScaleStep", typeof(double), typeof(NetworkView), new PropertyMetadata(0.1));

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        public double MaxScale
        {
            get { return (double)GetValue(MaxScaleProperty); }
            set { SetValue(MaxScaleProperty, value); }
        }
        public double MinScale
        {
            get { return (double)GetValue(MinScaleProperty); }
            set { SetValue(MinScaleProperty, value); }
        }
        public double ScaleStep
        {
            get { return (double)GetValue(ScaleStepProperty); }
            set { SetValue(ScaleStepProperty, value); }
        }

        private void Zoom(int delta)
        {
            if (delta > 0)
            {
                if (Scale < MaxScale)
                    Scale += ScaleStep;
            }
            else
            {
                if (Scale > MinScale)
                    Scale -= ScaleStep;
            }
        }

        private void NVMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom(e.Delta);
        }

        private Point mouseInitPos;
        private bool isPanning;
        private void NVMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.MiddleButton == MouseButtonState.Pressed)
            {
                mouseInitPos = e.GetPosition(this);
                isPanning = true;
            }
        }

        private void NVMouseMove(object sender, MouseEventArgs e)
        {
            //pan network view canvas
            if(e.MiddleButton == MouseButtonState.Pressed && isPanning)
            {
                Thickness newMargin = this.Margin;
                double dx = e.GetPosition(this).X - mouseInitPos.X;
                double dy = e.GetPosition(this).Y - mouseInitPos.Y;
                newMargin.Left += dx * Scale;
                newMargin.Top += dy * Scale;
                this.Margin = newMargin;
            }
        }
        #endregion Zoom&Pan

        public Node AddNode(NodeInfo info, bool select)
        {
            Node node = new Node(info);
            this.Children.Add(node);
            if (select)
                node.Select();
            NodeAdded?.Invoke(this, new NodeAddedEventArgs(node));
            return node;
        }

        public void RemoveNode(Node node)
        {
            if (node == null)
                return;
            if (this._selectedNode == node)
                this._selectedNode = null;
            this.Children.Remove(node);
            
            foreach(Connector conn in node.Connectors)
            {
                //prevent 'collection changed' error
                HashSet<Connector> safeConnections = new HashSet<Connector>(conn.Connections);
                foreach (Connector other in safeConnections)
                {
                    if (conn.Mode == Connector.ConnectorMode.Input)
                        RemoveConnectionPath(other, conn);
                    else
                        RemoveConnectionPath(conn, other);
                    conn.Disconnect(other);
                }
            }
        }

        public NetworkView()
        {
            InitializeComponent();
            this.ConnectorDrag += NetworkViewConnectorDragStarted;
            this.ConnectionAdded += NetworkViewConnectionAdded;
            this.NodeMoved += NetworkViewNodeMoved;
            this.NodeSelected += NVNodeSelected;

            connectorToMouseCurve = new Path();
            connectorToMouseCurve.Name = "ConnectorToMouse";
            connectorToMouseCurve.IsHitTestVisible = false;

            drawnConnections = new Dictionary<Tuple<Connector, Connector>, Path>();
            
            this.DataContext = this;
        }

    }
}
