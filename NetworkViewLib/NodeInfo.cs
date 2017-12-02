using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace network
{
    public class NodeInfo : INotifyPropertyChanged
    {
        public object Tag;
        public event PropertyChangedEventHandler PropertyChanged;

        public class HeaderInfo
        {
            public string Name;
            public bool NameChangeable;
            public object Data;
        }
        public HeaderInfo Header { get; }

        public string NodeNameProperty
        {
            get { return Header.Name; }
            set
            {
                Header.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NodeNameProperty"));
            }
        }

        public class SectionInfo : INotifyPropertyChanged
        {
            //connectability
            public Connector Input;
            public Connector Output;
            public bool IsInput;
            public bool IsOutput;
            public Connector.ConnectionEventHandler InputValidation;
            public Connector.ConnectionEventHandler InputAdded;
            public Connector.ConnectionEventHandler OutputAdded;
            //add connection removed events

            //data
            object _data;
            //ui
            public FrameworkElement UIPanel;

            public event PropertyChangedEventHandler PropertyChanged;
            public object Data
            {
                get { return _data; }
                set
                {
                    _data = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data"));
                }
            }
        }
        public List<SectionInfo> Sections { get; }
        
        public ContextMenu Menu;

        public System.Windows.Media.Color FillColor;

        public NodeInfo()
        {
            Sections = new List<SectionInfo>();
            Header = new HeaderInfo();
            FillColor = System.Windows.Media.Colors.White;
        }
    }
}
