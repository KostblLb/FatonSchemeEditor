using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faton;
using Shared;

namespace Ontology
{
    public class OntologyNode
    {
        /// <summary>
        /// The Tree-like list of Ontology nodes
        /// </summary>
        public static List<OntologyNode> Ontology; 

        public class Attribute
        {
            //CLASS is attr type for linking ontology classes typically to relation objects
            //TERMIN is a Klan vocabulary theme (aka Klan class)
            public enum AttributeType { STRING, INT, CLASS, DOMAIN, TERMIN};
            VocTheme _theme; //if type is voctheme

            readonly OntologyNode _parent;
            AttributeType _attrType;
            string _name;
            public Attribute(AttributeType myType, string myName)
            {
                _parent = null;
                _attrType = myType;
                _name = myName;
            }
            public Attribute(OntologyNode myParent, AttributeType myType, string myName) : this(myType, myName)
            {
                _parent = myParent;
            }
            public Attribute(VocTheme myTheme, string myName = null) : this(AttributeType.TERMIN, myName)
            {
                if (myName == null)
                    _name = myTheme.name;
                _theme = myTheme;
            }

            public AttributeType AttrType
            {
                get { return _attrType; }
                set { _attrType = value; }
            }
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public VocTheme Theme
            {
                get { return _theme; }
                set { _theme = value; }
            }
        }
        public List<Attribute> attrs;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        //public Type type;
        
        public void AddAttribute(Attribute att)
        {
            attrs.Add(att);
        }
        /// <summary>
        /// Returns ONLY own attributes
        /// </summary>
        public List<Attribute> OwnAttributes
        {
            get { return attrs; }
        }
        public OntologyNode(string myName)
        {
            _name = myName;
            attrs = new List<Attribute>();
        }
    }
    public class OntologyClass : OntologyNode
    {
        public List<OntologyClass> _parents;
        private List<OntologyClass> _children;
        public List<OntologyClass> Children
        {
            get { return _children; }
        }
        public List<OntologyClass> Parents
        {
            get { return _parents; }
        }
        
        /// <summary>
        /// Returns ONLY attributes inherited from parents
        /// </summary>
        public List<Tuple<Attribute, OntologyClass>> InheritedAttributes
        {
            get
            {
                List<Tuple<Attribute, OntologyClass>> inherited = new List<Tuple<Attribute, OntologyClass>>();
                if (!_parents.Any())
                    return inherited;
                Queue<OntologyClass> q = new Queue<OntologyClass>(_parents); //BFS for parents
                OntologyClass parent;
                while(q.Any())
                {
                    parent = q.Dequeue();
                    foreach (Attribute attr in parent.OwnAttributes)
                        inherited.Add(new Tuple<Attribute, OntologyClass>(attr, parent));
                    foreach (OntologyClass newParent in parent._parents)
                        q.Enqueue(newParent);
                }
                return inherited;
            }
        }

        /// <summary>
        /// Returns List of Own Attributes and Inherited Attributes, in this order.
        /// </summary>
        public List<Attribute> AllAttributes
        {
            get
            {
                List<Attribute> allAttrs = new List<Attribute>(this.OwnAttributes);
                List<Tuple<Attribute, OntologyClass >> inheritedAttrs = new List<Tuple<Attribute, OntologyClass>>(this.InheritedAttributes);
                foreach(Tuple<Attribute, OntologyClass> tuple in inheritedAttrs)
                {
                    allAttrs.Add(tuple.Item1);
                }
                return allAttrs;
            }
        }
        public OntologyClass(string myName) : base(myName)
        {
            _children = new List<OntologyClass>();
            _parents = new List<OntologyClass>();
        }

        public void AddChild(OntologyClass c)
        {
            _children.Add(c);
        }
        public void AddParent(OntologyClass p)
        {
            _parents.Add(p);
        }

        public OntologyClass Search(string name)
        {
            OntologyClass klass = null;
            HashSet<OntologyClass> viewed = new HashSet<OntologyClass>();
            Stack<OntologyClass> s = new Stack<OntologyClass>();
            s.Push(this);
            while (s.Any())
            {
                OntologyClass currentKlass = s.Pop();
                if (currentKlass.Name == name)
                {
                    klass = currentKlass;
                    break;
                }
                if (currentKlass.Children.Count > 0)
                {
                    var currentKlassViewed = true;
                    foreach (OntologyClass child in currentKlass.Children)
                        if (!viewed.Contains(child))
                        {
                            s.Push(child);
                            currentKlassViewed = false;
                        }
                    if (currentKlassViewed)
                        viewed.Add(currentKlass);
                }
            }
            return klass;
        }
    }
    
}



