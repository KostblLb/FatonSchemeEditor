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
            public StackPanel InfoPanel;
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

        public struct AttributeInfo
        {
            public bool IsInput;
            public bool IsOutput;
            public object Data;
            public FrameworkElement AttributePanel;
        }
        public List<AttributeInfo> Attributes;

        public FrameworkElement Footer;

        public ContextMenu Menu;
    }
}
