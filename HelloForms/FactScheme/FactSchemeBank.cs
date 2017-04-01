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
            doc.Add(new XElement("FATON_SCHEMES_BANK"));

            foreach(FactScheme scheme in Schemes)
            {
                doc.Root.Add(scheme.ToXml().Root);
            }

            return doc;
        }
    }
}
