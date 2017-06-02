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

        public struct HeaderInfo
        {
            public string Name;
            public bool NameChangeable;
            public object Data;
        }
        public HeaderInfo Header;

        public event PropertyChangedEventHandler PropertyChanged;
        public string NodeNameProperty
        {
            get { return Header.Name; }
            set
            {
                Header.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NodeNameProperty"));
            }
        }

        public struct SectionInfo
        {
            //connection
            public bool IsInput;
            public bool IsOutput;
            public Connector.ConnectionEventHandler InputValidation;
            //ui
            public object Data;
            public FrameworkElement UIPanel;
        }
        public List<SectionInfo> Sections;
        
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
