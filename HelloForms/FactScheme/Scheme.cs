using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.Serialization;
using Ontology;
using Vocabularies;
using Faton;

namespace FactScheme
{
    public class Scheme
    {
        string _name;
        uint _numArgs;
        uint _numConds;

        XElement _xml;
        bool _saved;

        [XmlAttribute]
        public string Name { get { return _name; } set { _name = value; } }

        [XmlAttribute]
        public string Segment { get; set; }

        [XmlIgnore]
        public string XMLName { get { return _name.Replace(' ', '_'); } }
        
        [XmlIgnore]
        public readonly HashSet<ISchemeComponent> Components;
        
        
        [XmlElement(ElementName = FatonConstants.XML_ARGUMENT_TAG)]
        public List<Argument> Arguments
        {
            get { return Components.OfType<Argument>().ToList(); }
        }
        //[XmlElement(ElementName = FatonConstants.XML_ARGUMENT_CONDITION_TAG)]
        [XmlIgnore]
        public List<Condition> Conditions
        {
            get { return Components.OfType<Condition>().ToList(); }
        }
        [XmlElement(ElementName = FatonConstants.XML_CONDITIONCOMPLEX_TAG)]
        public List<ConditionComplex> XMLConditionComplexes {
            get {
                var result = new List<ConditionComplex>();
                var comps = new Dictionary<Tuple<Argument, Argument>, ConditionComplex>();
                foreach (var cond in Conditions)
                {
                    var argsTup = new Tuple<Argument, Argument>(cond.Arg1, cond.Arg2);
                    if (!comps.ContainsKey(argsTup))
                        comps[argsTup] = new ConditionComplex(cond.Arg1, cond.Arg2);
                    comps[argsTup].Conditions.Add(cond);
                }
                foreach (var pair in comps)                
                    result.Add(pair.Value);
                return result;
            }
            set { return; }
        }
        [XmlElement(ElementName = FatonConstants.XML_RESULT_TAG)]
        public List<Result> Results
        {
            get { return Components.OfType<Result>().ToList(); }
        }
        [XmlIgnore] //ignore so far
        public List<Functor> Functors
        {
            get { return Components.OfType<Functor>().ToList(); }
        }

        public Scheme()
        {
            Segment = "";
            Components = new HashSet<ISchemeComponent>();
            _numArgs = 0;
            _numConds = 0;
            _saved = false;
        }

        public Scheme(string name) : this()
        {
            _name = name;
        }

        public Argument AddArgument(Termin theme)
        {
            _saved = false;
            Argument arg = new Argument(theme);
            arg.Order = ++_numArgs;
            Components.Add(arg);

            return arg;
        }

        public Argument AddArgument(OntologyClass klass)
        {
            _saved = false;
            Argument arg = new Argument(klass, klass.Name);
            arg.Order = ++_numArgs;
            Components.Add(arg);

            return arg;
        }

        public Condition AddCondition()
        {
            _saved = false;
            Condition cond = new Condition();
            cond.ID = ++_numConds;
            Components.Add(cond);
            return cond;
        }

        public Result AddResult(OntologyClass ontologyClass, string name = null)
        {
            _saved = false;
            if (name == null)
                name = Faton.FatonConstants.RESULT_NAME_NEW;
            int defaultNamesCount = 0;
            foreach (Result r in Results)
            {
                if (r.Name.Contains(name))
                    defaultNamesCount++;
            }
            if (defaultNamesCount > 0)
                name += string.Format("({0})", defaultNamesCount);
            Result res = new Result(name, ResultType.CREATE, ontologyClass);
            
            Components.Add(res);

            return res;
        }
 
        public Functor AddFunctor<T>() where T: Functor, new()
        {
            _saved = false;
            Functor func = new T();

            Components.Add(func);
            return func;
        }
        
        public void RemoveComponent(ISchemeComponent component)
        {
            if (!Components.Contains(component))
                return;

            //do something about duplicated Id code
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
            if (component is Condition)
            {
                var condition = component as Condition;
                var condsGreater = from x in Conditions
                                  where x.ID > condition.ID
                                  select x;
                foreach (var cond in condsGreater)
                    cond.ID -= 1;
                _numConds -= 1;
            }
            Components.Remove(component);
        }
        

        public XElement ToXml()
        {
            //if (_saved)
            //    return _xml;

            XElement root = new XElement(FatonConstants.XML_SCHEME_TAG, 
                new XAttribute(FatonConstants.XML_SCHEME_NAME, Name),
                new XAttribute(FatonConstants.XML_SCHEME_SEGMENT, Segment));
            foreach(Argument arg in Arguments)
            {
                XElement xarg =
                    new XElement(FatonConstants.XML_ARGUMENT_TAG,
                        new XAttribute(FatonConstants.XML_ARGUMENT_ORDER, arg.Order),
                        new XAttribute(FatonConstants.XML_ARGUMENT_OBJECTTYPE, arg.ArgType),
                        new XAttribute(FatonConstants.XML_ARGUMENT_CLASSNAME, arg.Name),
                        new XAttribute(FatonConstants.XML_ARGUMENT_COMPARETYPE, arg.CompareType));
                foreach(var pair in arg.Conditions)
                {
                    foreach (var cond in pair.Value)
                    {
                        XElement xcond =
                            new XElement(FatonConstants.XML_ARGUMENT_CONDITION_TAG,
                                new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_ATTRNAME, pair.Key.Name),
                                new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_TYPE, cond.CondType),
                                new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_OPERATION, cond.Operation),
                                new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_DATA, cond.Data));
                        xarg.Add(xcond);
                    }
                }
                root.Add(xarg);
            }

            foreach (Result res in Results)
            {
                List<XAttribute> xattrs_ = new List<XAttribute>();
                xattrs_.Add(new XAttribute(FatonConstants.XML_RESULT_NAME, res.Name));
                xattrs_.Add(new XAttribute(FatonConstants.XML_RESULT_CLASSNAME, (res.Reference as OntologyClass).Name));
                xattrs_.Add(new XAttribute(FatonConstants.XML_RESULT_TYPE, res.Type));
                if (res.Type == ResultType.EDIT)
                {
                    if (res.EditObject == null)
                        throw new Exception("Result type is EDIT, but no argument set");
                    if (res.EditObject is Argument)
                        xattrs_.Add(new XAttribute(FatonConstants.XML_RESULT_ARGEDIT, ((Argument)res.EditObject).Order));
                    else if (res.EditObject is Result)
                        xattrs_.Add(new XAttribute(FatonConstants.XML_RESULT_RESEDIT, ((Result)res.EditObject).Name));
                }

                XElement xres = new XElement(FatonConstants.XML_RESULT_TAG, xattrs_);

                foreach (Result.Rule rule in res.Rules)
                {
                    XElement xrul;
                    List<XAttribute> xattrs = new List<XAttribute>();
                    xattrs.Add(new XAttribute(FatonConstants.XML_RESULT_RULE_TYPE, rule.Type));
                    xattrs.Add(new XAttribute(FatonConstants.XML_RESULT_RULE_ATTR, rule.Attribute.Name));

                    if (rule.Type == Result.RuleType.FUNC)
                    {
                        Functor f = rule.Reference as Functor;
                        xattrs.Add(new XAttribute("FunctorName", f.Name));
                        xattrs.Add(new XAttribute("FunctorID", f.CID));
                        xattrs.Add(new XAttribute("Default", f.DefaultValue));
                        xrul = new XElement("Rule", xattrs);
                        foreach(Functor.FunctorInput input in f.Inputs)
                        {
                            string resourceName = "", resourceType = "";
                            if (input.source is FactScheme.Argument)
                            {
                                resourceName = ((FactScheme.Argument)input.source).Order.ToString();
                                resourceType = "ARG";
                            }
                            
                            xrul.Add(new XElement(FatonConstants.XML_RESULT_RULE_FUNCTOR_INPUT, 
                                        new XAttribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_RESOURCETYPE, resourceType),
                                        new XAttribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_RESOURCE, resourceName),
                                        new XAttribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_ATTRFROM, input.value.Name))); 
                        }
                    }
                    else
                    {
                        xattrs.Add(new XAttribute(FatonConstants.XML_RESULT_RULE_RESOURCETYPE, rule.ResourceType));
                        if (rule.Reference is Argument)
                            xattrs.Add(new XAttribute(FatonConstants.XML_RESULT_RULE_RESOURCE, (rule.Reference as Argument).Order));
                        else
                            xattrs.Add(new XAttribute(FatonConstants.XML_RESULT_RULE_RESOURCE, (rule.Reference as Result).Name));
                        if (rule.Attribute.AttrType != OntologyNode.Attribute.AttributeType.OBJECT)
                            xattrs.Add(new XAttribute(FatonConstants.XML_RESULT_RULE_ATTRFROM, rule.InputAttribute.Name));
                        xrul = new XElement(FatonConstants.XML_RESULT_RULE_TAG, xattrs);
                    }
                    xres.Add(xrul);
                }

                root.Add(xres);
            }

            foreach (var conditionComplex in XMLConditionComplexes) {
                var xcomplex = new XElement(FatonConstants.XML_CONDITIONCOMPLEX_TAG,
                    new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_ARG1, conditionComplex.Arg1),
                    new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_ARG2, conditionComplex.Arg2));
                foreach (var condition in conditionComplex.Conditions)
                {
                    var xcond = new XElement(FatonConstants.XML_ARGUMENT_CONDITION_TAG);
                    var xattrs = new List<XAttribute>();
                    if (condition.Arg1 == null || condition.Arg2 == null)
                        throw new ArgumentNullException("Scheme conditions must have both arguments set");
                    xattrs.Add(new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_ID, condition.ID));
                    xattrs.Add(new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_TYPE, condition.Type));
                    xattrs.Add(new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_OPERATION, condition.Operation));
                    xattrs.Add(new XAttribute(FatonConstants.XML_ARGUMENT_CONDITION_DATA, condition.Data?? ""));

                    xcond.Add(xattrs);
                    xcomplex.Add(xcond);
                }
                root.Add(xcomplex);
            }

            _xml = root;
            _saved = true;
            return root;
        }
    }

    
}
