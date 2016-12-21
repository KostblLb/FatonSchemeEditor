using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Linq;

namespace HelloForms
{
    public partial class FactScheme
    {
        public partial class Argument { } //TODO add dictionary entry argument type
        public partial class Condition { }
        public partial class Result { }

        List<Argument> _arguments;
        List<Condition> _conditions;
        List<Result> _results;
        uint _numArgs;
        Layout _layout;

        public List<Argument> Arguments
        {
            get { return _arguments; }
        }
        public List<Condition> Conditions
        {
            get { return _conditions; }
        }
        public List<Result> Results
        {
            get { return _results; }
        }
        public Layout Layout
        {
            get { return _layout; }
            set { _layout = value; }
        }

        public FactScheme()
        {
            _arguments = new List<Argument>();
            _conditions = new List<Condition>();
            _results = new List<Result>();
            _numArgs = 0;
        }

        public void AddArgument(OntologyNode node, Point point, bool useInheritance = true)
        {
            Argument arg = new Argument(node.Name, useInheritance, node);
            arg.Order = ++_numArgs;
            _arguments.Add(arg);
            _layout.AddArgument(point, arg);
        }

        public Result AddResult()
        {
            String name = EditorConstants.RESULT_NAME_NEW;
            int defaultNamesCount = 0;
            foreach(Result r in _results)
            {
                if (r.Name.Contains(name))
                    defaultNamesCount++;
            }
            if (defaultNamesCount > 0)
                name += string.Format("({0})", defaultNamesCount);
            Result res = new Result(name);
            _results.Add(res);
            return res;
        }

        public void AddArgument(string dictionaryEntry) { }

        public XDocument ToXml() //TODO MOVE TO LINQ?
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("root"));
            foreach(Argument arg in _arguments)
            {
                XElement xarg =
                    new XElement("Argument",
                        new XAttribute("Order", arg.Order),
                        new XAttribute("Type", arg.TypeString),
                        new XAttribute("ClassName", arg.Name),
                        new XAttribute("AllowInheritance", arg.Inheritance));
                foreach(Condition cond in arg.Conditions)
                {
                    XElement xcond =
                        new XElement("Condition",
                            new XAttribute("Type", cond.ConditionType),
                            new XAttribute("Attribute", cond.Attribute),
                            new XAttribute("ComparType", cond.ComparisonType),
                            new XAttribute("Values", cond.Value));
                    xarg.Add(xcond);
                }
                doc.Root.Add(xarg);
            }
            
            return doc;
        }
    }

    
}
