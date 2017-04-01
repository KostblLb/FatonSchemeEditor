using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HelloForms
{ 
    public class FactSchemeBank
    {
        public string Name;
        public List<FactScheme> Schemes;

        public FactSchemeBank(string name = "new_bank")
        {
            Schemes = new List<FactScheme>();
            Name = name;
        }

        public XDocument ToXml()
        {
            XDocument doc = new XDocument();
            XElement xbank = new XElement(FatonConstants.XML_BANK_NAME);
            foreach (FactScheme scheme in Schemes)
            {
                xbank.Add(scheme.ToXml().Root);
            }
            doc.Add(xbank);

            return doc;
        }
    }
}
