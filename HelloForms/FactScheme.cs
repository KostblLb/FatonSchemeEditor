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

        public Argument AddArgument(OntologyNode node, Point point, bool useInheritance = true)
        {
            Argument arg = new Argument(node.Name, useInheritance, node);
            arg.Order = ++_numArgs;
            _arguments.Add(arg);

            //call medium somewhere here?
            //_layout.AddArgument(point, arg);
            return arg;

        }

        public Result AddResult(OntologyClass ontologyClass = null)
        {
            String name = EditorConstants.RESULT_NAME_NEW;
            int defaultNamesCount = 0;
            foreach (Result r in _results)
            {
                if (r.Name.Contains(name))
                    defaultNamesCount++;
            }
            if (defaultNamesCount > 0)
                name += string.Format("({0})", defaultNamesCount);
            Result res = new Result(name, ResultType.Create, ontologyClass);
            _results.Add(res);
            return res;
        }

        public Functor AddFunctor()
        {
            Functor func = new FunctorCat();
            return func;
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

            foreach (Result res in _results)
            {
                List<XAttribute> xattrs_ = new List<XAttribute>();
                xattrs_.Add(new XAttribute("Name", res.Name));
                xattrs_.Add(new XAttribute("Type", res.Type));
                if (res.Reference is OntologyClass)
                {
                    xattrs_.Add(new XAttribute("ReferenceType", "OntologyClass"));
                    xattrs_.Add(new XAttribute("Reference", (res.Reference as OntologyClass).Name));
                }
                else
                {
                    xattrs_.Add(new XAttribute("ReferenceType", "FIXMENOW"));
                    xattrs_.Add(new XAttribute("Reference", (res.Reference as FactScheme.Argument).Name));
                }
                XElement xres = new XElement("Result", xattrs_);

                foreach (FactScheme.Result.Rule rule in res.Rules)
                {
                    XElement xrul;
                    List<XAttribute> xattrs = new List<XAttribute>();
                    xattrs.Add(new XAttribute("Attribute", rule.Attribute.Name));
                    xattrs.Add(new XAttribute("Type", rule.Type));

                    if (rule.Type == Result.RuleType.Function)
                    {
                        Functor f = rule.Reference as Functor;
                        xattrs.Add(new XAttribute("FunctorID", f.ID));
                        xattrs.Add(new XAttribute("Default", f.DefaultValue));
                        xrul = new XElement("Rule", xattrs);
                        foreach(object input in f.Inputs)
                        {
                            xrul.Add(new XElement("input", 
                                        new XAttribute("FIXME", "fixme"))); 
                        }
                    }
                    else
                    {
                        xattrs.Add(new XAttribute("ArgFrom", (rule.Reference as Argument).Order));
                        xattrs.Add(new XAttribute("AttrFrom", rule.InputAttribute.Name));
                        xrul = new XElement("Rule", xattrs);
                    }
                    xres.Add(xrul);
                }

                doc.Root.Add(xres);
            }
            
            return doc;
        }
    }

    
}
