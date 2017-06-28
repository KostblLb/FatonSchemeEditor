using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Linq;
using Ontology;
using Shared;

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

        XElement _xml;
        bool _saved;

        public string Name { get { return _name; } set { _name = value; } }
        public string XMLName { get { return _name.Replace(' ', '_'); } }

        //TODO remove separate component accessors, leave only Components
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

        public Argument AddArgument(VocTheme theme)
        {
            _saved = false;
            Argument arg = new Argument(theme);
            arg.Order = ++_numArgs;
            _arguments.Add(arg);
            Components.Add(arg);

            return arg;
        }

        public Argument AddArgument(OntologyClass klass)
        {
            _saved = false;
            Argument arg = new Argument(klass, klass.Name);
            arg.Order = ++_numArgs;
            _arguments.Add(arg);
            Components.Add(arg);

            return arg;

        }

        public Condition AddCondition()
        {
            _saved = false;
            Condition cond = new Condition();
            _conditions.Add(cond);
            Components.Add(cond);
            return cond;
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
        

        public XElement ToXml()
        {
            //if (_saved)
            //    return _xml;

            XElement root = new XElement(XMLName);
            foreach(Argument arg in Arguments)
            {
                XElement xarg =
                    new XElement("Argument",
                        new XAttribute("Order", arg.Order),
                        new XAttribute("Type", arg.ArgType),
                        new XAttribute("ClassName", arg.Name),
                        new XAttribute("AllowInheritance", arg.Inheritance));
                foreach(var pair in arg.Сonditions)
                {
                    foreach (var cond in pair.Value)
                    {
                        XElement xcond =
                            new XElement("Condition",
                                new XAttribute("Attribute", pair.Key.Name),
                                new XAttribute("Type", cond.CondType),
                                new XAttribute("ComparType", cond.ComparType),
                                new XAttribute("Values", cond.Value));
                        xarg.Add(xcond);
                    }
                }
                root.Add(xarg);
            }

            foreach (Result res in Results)
            {
                List<XAttribute> xattrs_ = new List<XAttribute>();
                xattrs_.Add(new XAttribute("Name", res.Name));
                xattrs_.Add(new XAttribute("ClassName", (res.Reference as OntologyClass).Name));
                xattrs_.Add(new XAttribute("Type", res.Type));
                if (res.Type == ResultType.Edit)
                {
                    if (res.EditObject == null)
                        throw new Exception("Result type is EDIT, but no argument set");
                    if (res.EditObject is Argument)
                        xattrs_.Add(new XAttribute("ArgEdit", ((Argument)res.EditObject).Order));
                    else if (res.EditObject is Result)
                        xattrs_.Add(new XAttribute("ResultEdit", ((Result)res.EditObject).Name));
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
                        if (rule.Reference is Argument)
                            xattrs.Add(new XAttribute("ArgFrom", (rule.Reference as Argument).Order));
                        else
                            xattrs.Add(new XAttribute("ResultFrom", (rule.Reference as Result).Name));
                        xattrs.Add(new XAttribute("AttrFrom", rule.InputAttribute.Name));
                        xrul = new XElement("Rule", xattrs);
                    }
                    xres.Add(xrul);
                }

                root.Add(xres);
            }

            foreach (var condition in Conditions)
            {
                var xcond = new XElement("Condition");
                var xattrs = new List<XAttribute>();
                for (int i = 0; i < condition.Args.Count(); i++)
                {
                    var argName = string.Format("Arg{0}", i+1);
                    var xarg = new XAttribute(argName, condition.Args[i].Order);
                    xattrs.Add(xarg);
                }
                xattrs.Add(new XAttribute("Type", condition.Type));
                xattrs.Add(new XAttribute("Operation", condition.ComparType));
                switch (condition.Type)
                    //probably should just replace w/ "Value1=X Value2=Y"
                {
                    case Condition.ConditionType.CONTACT:
                        xattrs.Add(new XAttribute("Contact", condition.Contact));
                        break;
                    case Condition.ConditionType.MORPH:
                        xattrs.Add(new XAttribute("GramtabAttr", condition.MorphAttr));
                        break;
                    case Condition.ConditionType.POS:
                        xattrs.Add(new XAttribute("Position", condition.Position));
                        break;
                    case Condition.ConditionType.SEG:
                        xattrs.Add(new XAttribute("Segment", condition.Segment));
                        break;
                    case Condition.ConditionType.SEM:
                        xattrs.Add(new XAttribute("AttrName1", condition.SemAttrs[0].Name));
                        xattrs.Add(new XAttribute("AttrName2", condition.SemAttrs[1].Name));
                        break;
                    case Condition.ConditionType.SYNT:
                        xattrs.Add(new XAttribute("ActantName1", condition.ActantNames[0]));
                        xattrs.Add(new XAttribute("ActantName2", condition.ActantNames[1]));
                        break;
                }

                xcond.Add(xattrs);
                root.Add(xcond);
            }
            _xml = root;
            _saved = true;
            return root;
        }
    }

    
}
