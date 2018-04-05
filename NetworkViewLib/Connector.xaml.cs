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
    public class ConnectionEventArgs : RoutedEventArgs
    {
        public ConnectionEventArgs(RoutedEvent e, Connector src, Connector dst) : this(e)
        {
            Valid = true;
            SourceConnector = src;
            DestConnector = dst;
        }
        public ConnectionEventArgs(RoutedEvent e) : base(e)
        {
            Valid = true;
        }
        public ConnectionEventArgs(Connector src, Connector dst) : base()
        {
            Valid = true;
            SourceConnector = src;
            DestConnector = dst;
        }
        public Connector SourceConnector { get;}
        public Connector DestConnector { get; }
        public bool Valid { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для Connector.xaml
    /// </summary>
    public partial class Connector : UserControl
    {
        public enum ConnectorMode { Input, Output };

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ConnectorMode), typeof(Connector));
        public ConnectorMode Mode
        {
            get { return (ConnectorMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public delegate void ConnectionEventHandler(object sender, ConnectionEventArgs e);

        //todo rewrite connection events, using inputadded and outputadded. remove connectionadded
        public event ConnectionEventHandler ConnectionAdded;
        public event ConnectionEventHandler InputAdded
        {
            add { AddHandler(InputAddedEvent, value); }
            remove { RemoveHandler(InputAddedEvent, value); }
        }
        public event ConnectionEventHandler OutputAdded
        {
            add { AddHandler(OutputAddedEvent, value); }
            remove { RemoveHandler(OutputAddedEvent, value); }
        }

        /// <summary>
        /// event is raised before establishing a valid connection
        /// </summary>
        public event ConnectionEventHandler ConnectionBeforeAdd
        {
            add
            {
                AddHandler(ConnectionBeforeAddEvent, value);
            }
            remove
            {
                RemoveHandler(ConnectionBeforeAddEvent, value);
            }
        }

        public static readonly RoutedEvent ConnectionBeforeAddEvent =
            EventManager.RegisterRoutedEvent(
                "ConnectionBeforeAdd",
                RoutingStrategy.Bubble,
                typeof(ConnectionEventHandler),
                typeof(Connector));

        public static readonly RoutedEvent ConnectionAddedEvent = 
            EventManager.RegisterRoutedEvent(
                "ConnectionAdded",
                RoutingStrategy.Bubble,
                typeof(ConnectionEventHandler),
                typeof(Connector));
        public static readonly RoutedEvent ConnectionRemovedEvent =
            EventManager.RegisterRoutedEvent(
                "ConnectionRemoved",
                RoutingStrategy.Bubble,
                typeof(ConnectionEventHandler),
                typeof(Connector));

        public static readonly RoutedEvent InputAddedEvent =
            EventManager.RegisterRoutedEvent(
                "InputAdded",
                RoutingStrategy.Bubble,
                typeof(ConnectionEventHandler),
                typeof(Connector));

        public static readonly RoutedEvent OutputAddedEvent =
            EventManager.RegisterRoutedEvent(
                "OutputAdded",
                RoutingStrategy.Bubble,
                typeof(ConnectionEventHandler),
                typeof(Connector));

        public static readonly RoutedEvent ConnectorDragEvent =
            EventManager.RegisterRoutedEvent(
                "ConnectorDrag",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Connector));


        /// <summary>
        /// set of connectors from other nodes
        /// </summary>
        public HashSet<Connector> Connections { get; }
        
        /// <summary>
        /// node panel that hosts this connector
        /// </summary>
        public Node ParentNode { get; set; }

        public Connector()
        {
            InitializeComponent();
            this.Connections = new HashSet<Connector>();
            this.ParentNode = null;
        }

        public Connector(ConnectorMode mode, object tag) : this()
        {
            this.Mode = mode;
            this.Tag = tag;
        }

        /// <summary>
        /// two-way connection
        /// </summary>
        /// <param name="other"></param>
        public void Connect(Connector other, bool raiseEvent = true)
        {
            this.Connections.Add(other);
            other.Connections.Add(this);
            if (!raiseEvent)
                return;
            if (this.Mode == ConnectorMode.Output)
            {
                //RaiseConnectionAddedEvent(this, other);
                RaiseEvent(new ConnectionEventArgs(ConnectionAddedEvent, this, other));
                this.RaiseEvent(new ConnectionEventArgs(OutputAddedEvent, this, other));
                other.RaiseEvent(new ConnectionEventArgs(InputAddedEvent, this, other));
            }
            else
            {
                //RaiseConnectionAddedEvent(other, this);
                RaiseEvent(new ConnectionEventArgs(ConnectionAddedEvent, other, this));
                this.RaiseEvent(new ConnectionEventArgs(InputAddedEvent, other, this));
                other.RaiseEvent(new ConnectionEventArgs(OutputAddedEvent, other, this));
            }
        }

        /// <summary>
        /// two-way disconnection
        /// </summary>
        /// <param name="other"></param>
        public void Disconnect(Connector other)
        {
            if (!this.Connections.Contains(other)) return;
            this.Connections.Remove(other);
            other.Connections.Remove(this);
            if (this.Mode == ConnectorMode.Input)
                RaiseEvent(new ConnectionEventArgs(ConnectionRemovedEvent, other, this));
            else
                RaiseEvent(new ConnectionEventArgs(ConnectionRemovedEvent, this, other));
        }

        public bool ValidateConnection(object sender)
        {
            if (sender == null)
                return false;
            if (!(sender is Connector))
                return false;
            Connector other = sender as Connector;
            if (other.Mode == this.Mode)
            {
                Console.WriteLine("same mode");
                return false;
            }
            if (this.ParentNode == other.ParentNode)
            {
                Console.WriteLine("same parent node");
                return false;
            }
            if (this.Connections.Contains(other) || other.Connections.Contains(this))
                return false;
            Connector dst = other.Mode == ConnectorMode.Input ? other : this;
            if (dst.Connections.Count > 0) return false;
            return true;
        }

        private void Connector_DragEnter(object sender, DragEventArgs e)
        {
            Connector other = e.Data.GetData(typeof(Connector)) as Connector;
            if (!ValidateConnection(other))
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.Link;
        }
        private void Connector_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine("CONN: drop");
            Connector other = e.Data.GetData(typeof(Connector)) as Connector;
            if (!ValidateConnection(other))
                return;
            if (other.Mode == ConnectorMode.Output)
                RaiseEvent(new ConnectionEventArgs(ConnectionBeforeAddEvent, other, this));
            else
                other.RaiseEvent(new ConnectionEventArgs(ConnectionBeforeAddEvent, this, other));
        }

        private void Connector_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Console.WriteLine("dragging connection from " + e.Source);
                if (Connections.Count > 0 && Mode == ConnectorMode.Input)
                {
                    var src = Connections.First();
                    RaiseEvent(new ConnectionEventArgs(ConnectionRemovedEvent, src, this));
                    src.RaiseEvent(new RoutedEventArgs(ConnectorDragEvent));
                    DragDrop.DoDragDrop(src, src, DragDropEffects.Link);
                }
                else
                {
                    RaiseEvent(new RoutedEventArgs(ConnectorDragEvent));
                    DragDrop.DoDragDrop(this, this, DragDropEffects.Link);
                }
            }
        }

        private void Connector_Loaded(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
