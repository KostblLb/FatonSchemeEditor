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
    /// <summary>
    /// Логика взаимодействия для Node.xaml
    /// </summary>
    public partial class Node : Canvas
    {
        public HashSet<Connector> Connectors { get; }

        HashSet<Node> neighbors(bool incoming)
        {
            var neighbors = new HashSet<Node>();
            var connections = from x in Connectors
                              where x.Mode == (incoming ? Connector.ConnectorMode.Input : Connector.ConnectorMode.Output)
                              select x.Connections;
            foreach (var connection in connections)
            {
                var nodes = from x in connection
                            select x.ParentNode;
                foreach (Node node in nodes)
                    if (!neighbors.Contains(node))
                        neighbors.Add(node);
            }
            return neighbors;
        }
        public HashSet<Node> IncomingNeighbors
        {
            get { return neighbors(true); }
        }

        public HashSet<Node> OutgoingNeighbors
        {
            get { return neighbors(false); }
        }
        public static readonly RoutedEvent NodeMovedEvent =
            EventManager.RegisterRoutedEvent(
                "NodeMoved",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Node));

        public static readonly RoutedEvent NodeSelectedEvent =
            EventManager.RegisterRoutedEvent(
                "NodeSelectedEvent",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Node));

        public string TagName { get { return pName.Content.ToString(); } }
        public Node()
        {
            InitializeComponent();
            Connectors = new HashSet<Connector>();
            pNameEdit.DataContext = pName;
            this.DataContext = this;
        }

        public Node(NodeInfo info) : this()
        {
            this.Tag = info.Tag;
            this.pSelfConnector.Tag = info.Tag;

            if (info.Header.NameChangeable)
            {
                this.pName.MouseDoubleClick += NameLabelDClick;
            }
            Binding bind = new Binding("NodeNameProperty");
            bind.Source = info;
            bind.Mode = BindingMode.TwoWay;
            this.pName.SetBinding(Label.ContentProperty, bind);

            if (info.Header.InfoPanel != null)
                this.pInfoHeader.Children.Add(info.Header.InfoPanel);
            

            foreach (NodeInfo.AttributeInfo attrInfo in info.Attributes)
            {
                int numRows = pAttributesGrid.RowDefinitions.Count;
                pAttributesGrid.RowDefinitions.Add(new RowDefinition());
                if (attrInfo.IsInput)
                {
                    Connector conn = new Connector(Connector.ConnectorMode.Input, attrInfo.Data);
                    conn.SetValue(Grid.RowProperty, numRows);
                    conn.SetValue(Grid.ColumnProperty, 0);
                    pAttributesGrid.Children.Add(conn);
                }
                if (attrInfo.IsOutput)
                {
                    Connector conn = new Connector(Connector.ConnectorMode.Output, attrInfo.Data);
                    conn.SetValue(Grid.RowProperty, numRows);
                    conn.SetValue(Grid.ColumnProperty, 2);
                    pAttributesGrid.Children.Add(conn);
                }
                attrInfo.AttributePanel.SetValue(Grid.RowProperty, numRows);
                attrInfo.AttributePanel.SetValue(Grid.ColumnProperty, 1);
                pAttributesGrid.Children.Add(attrInfo.AttributePanel);
            }
            foreach (Connector c in pAttributesGrid.Children.OfType<Connector>().Union(
                pNameGrid.Children.OfType<Connector>()))
            {
                c.ParentNode = this;
                if (!Connectors.Contains(c)) //remove mb?
                    Connectors.Add(c);
            }

            if (info.Menu == null)
                this.ContextMenu = new ContextMenu();
            else
                this.ContextMenu = info.Menu;

        }

        #region UI
        private bool isDragging;
        private Point mouseDragStart;
        private Thickness panelDragStartMargin;

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            base.MeasureOverride(constraint);
            double width = base
                .InternalChildren
                .OfType<FrameworkElement>()
                .Max(i => i.DesiredSize.Width + (double)i.GetValue(Canvas.LeftProperty) + i.Margin.Right);

            double height = base
                .InternalChildren
                .OfType<FrameworkElement>()
                .Max(i => i.DesiredSize.Height + (double)i.GetValue(Canvas.TopProperty) + i.Margin.Bottom);

            return new Size(width, height);
        }

        private void NameLabelDClick(object sender, MouseButtonEventArgs e)
        {
            pName.Visibility = Visibility.Collapsed;
            pNameEdit.Visibility = Visibility.Visible;
            //Keyboard.Focus(pNameEdit);
        }

        private void NameLabelEditKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }

        private void NameLabelEditLostFocus(object sender, RoutedEventArgs e)
        {
            pName.Visibility = Visibility.Visible;
            pNameEdit.Visibility = Visibility.Collapsed;
        }

        private void NameEditButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CommonMouseDown(object sender, MouseButtonEventArgs e)
        {
            //begin panel dragging, ignoring connectors only
            Console.WriteLine("clicked " + e.Source);
            if (e.Source is Connector)
                return;
            mouseDragStart = e.GetPosition(this);
            panelDragStartMargin = this.Margin;
            isDragging = true;
            this.Select();
            RaiseEvent(new RoutedEventArgs(NodeSelectedEvent));
        }

        private void CommonMouseUp(object sender, MouseButtonEventArgs e)
        {
            //stop panel dragging, ignoring connectors only
            Console.WriteLine("released " + e.Source);
            if (e.Source is Connector)
                return;
            isDragging = false;
        }

        private void CommonMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;
            Point currentMousePos = e.GetPosition(this);
            double dx = currentMousePos.X - mouseDragStart.X;
            double dy = currentMousePos.Y - mouseDragStart.Y;
            Thickness newMargin = panelDragStartMargin;
            newMargin.Left += dx;
            newMargin.Top += dy;
            this.Margin = newMargin;

            panelDragStartMargin = newMargin;
            RaiseEvent(new RoutedEventArgs(NodeMovedEvent));
        }

        private void CanvasMouseLeave(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void NodeLoaded(object sender, EventArgs e)
        {
            foreach (Connector c in pAttributesGrid.Children.OfType<Connector>().Union(
                pNameGrid.Children.OfType<Connector>()))
            {
                c.ParentNode = this;
                if (!Connectors.Contains(c)) //remove mb?
                    Connectors.Add(c);
            }
        }

        #endregion UI

        private void Connector_ConnectorDrag(object sender, RoutedEventArgs e)
        {

        }

        public void Select()
        {
            this.pRect.Stroke = Brushes.Red;
            this.Focus();
        }
        public void Deselect()
        {
            this.pRect.Stroke = Brushes.Black;
        }


    }
}
