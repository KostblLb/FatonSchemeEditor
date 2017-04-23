using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Linq;
using Ontology;

namespace FactScheme
{
    public class Scheme
    {
        string _name;
        List<Argument> _arguments;
        List<Condition> _conditions;
        List<Result> _results;
        List<Functor> _functors;
        uint _numArgs;

        XDocument _xml;
        bool _saved;

        public string Name { get { return _name; } }
        public string XMLName { get { return _name.Replace(' ', '_'); } }

        public readonly HashSet<ISchemeComponent> Components;
        public List<Argument> Arguments
        {
            get { return Components.OfType<Argument>().ToList(); }
        }
        public List<Condition> Conditions
        {
            get { return Components.OfType<Condition>().ToList(); }
        }
        public List<Result> Results
        {
            get { return Components.OfType<Result>().ToList(); }
        }
        public List<Functor> Functors
        {
            get { return Components.OfType<Functor>().ToList(); }
        }
        //public List<Argument> Arguments
        //{
        //    get { return _arguments; }
        //}
        //public List<Condition> Conditions
        //{
        //    get { return _conditions; }
        //}
        //public List<Result> Results
        //{
        //    get { return _results; }
        //}
        //public List<Functor> Functors
        //{
        //    get { return _functors; }
        //}

        public Scheme()
        {
            _arguments = new List<Argument>();
            _conditions = new List<Condition>();
            _results = new List<Result>();
            _functors = new List<Functor>();
            Components = new HashSet<ISchemeComponent>();
            _numArgs = 0;
            _saved = false;
        }

        public Scheme(string name) : this()
        {
            _name = name;
        }

        public Argument AddArgument(OntologyClass klass, bool useInheritance = true)
        {
            _saved = false;
            Argument arg = new Argument(klass, klass.Name);
            arg.Order = ++_numArgs;
            _arguments.Add(arg);
            Components.Add(arg);

            return arg;

        }

        public Result AddResult(OntologyClass ontologyClass, string name = null)
        {
            _saved = false;
            if (name == null)
                name = Faton.FatonConstants.RESULT_NAME_NEW;
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
            Components.Add(res);

            return res;
        }

        public Functor AddFunctor()
        {
            _saved = false;
            Functor func = new Functor();

            Components.Add(func);
            return func;
        }

        public Relation AddRelation()
        {
            Relation rel = new Relation();
            return rel;
        }
        
        public void RemoveComponent(ISchemeComponent component)
        {
            if (!Components.Contains(component))
                return;
            if(component is Argument)
            { //fix arguments order
                var argument = component as Argument;
                var argsGreater = from x in Arguments
                                  where x.Order > argument.Order
                                  select x;
                foreach (var arg in argsGreater)
                    arg.Order -= 1;
                _numArgs -= 1;
            }
            Components.Remove(component);
        }
        

        public XDocument ToXml()
        {
            //if (_saved)
            //    return _xml;

            XDocument doc = new XDocument();
            doc.Add(new XElement(XMLName));
            foreach(Argument arg in Arguments)
            {
                XElement xarg =
                    new XElement("Argument",
                        new XAttribute("Order", arg.Order),
                        new XAttribute("ClassName", arg.Klass.Name),
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

            foreach (Result res in Results)
            {
                List<XAttribute> xattrs_ = new List<XAttribute>();
                xattrs_.Add(new XAttribute("Name", res.Name));
                xattrs_.Add(new XAttribute("ClassName", (res.Reference as OntologyClass).Name));
                xattrs_.Add(new XAttribute("Type", res.Type));
                if (res.Type == ResultType.Edit)
                {
                    if (res.EditArgument == null)
                        throw new Exception("Result type is EDIT, but no argument set");
                    xattrs_.Add(new XAttribute("ArgEdit", res.EditArgument.Order));
                }

                XElement xres = new XElement("Result", xattrs_);

                foreach (Result.Rule rule in res.Rules)
                {
                    XElement xrul;
                    List<XAttribute> xattrs = new List<XAttribute>();
                    xattrs.Add(new XAttribute("Attribute", rule.Attribute.Name));
                    xattrs.Add(new XAttribute("Type", rule.Type));

                    if (rule.Type == Result.RuleType.FUNC)
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

            _xml = doc;
            _saved = true;
            return doc;
        }
    }

    
}
