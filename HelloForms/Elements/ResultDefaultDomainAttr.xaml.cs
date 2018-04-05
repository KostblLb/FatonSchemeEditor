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

namespace HelloForms.Elements
{
    /// <summary>
    /// Логика взаимодействия для ResultDefaultDomainAttr.xaml
    /// </summary>
    public partial class ResultDefaultDomainAttr : UserControl
    {
        public ResultDefaultDomainAttr()
        {
            InitializeComponent();
        }
        public string Header
        {
            get { return expander.Header.ToString(); }
            set { expander.Header = value; }
        }
        public ComboBox GetComboBox() { return comboBox; }
    }
}
