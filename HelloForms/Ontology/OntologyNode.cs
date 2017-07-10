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
            public enum AttributeType { STRING, INT, OBJECT, DOMAIN};
            
            AttributeType _attrType;
            string _name;
            //optional value (like classname)
            public object Opt { get; set; }

            public Attribute(AttributeType myType, string myName)
            {
                _attrType = myType;
                _name = myName;
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

        private OntologyClass find(string name, bool up)
        {
            OntologyClass klass = null;
            HashSet<OntologyClass> viewed = new HashSet<OntologyClass>();
            Stack<OntologyClass> s = new Stack<OntologyClass>();
            s.Push(this);
            while (s.Any())
            {
                OntologyClass currentKlass = s.Pop();
                if (currentKlass.Name.Equals(name))
                {
                    klass = currentKlass;
                    break;
                }
                var collection = up ? currentKlass.Parents : currentKlass.Children;
                if (collection.Count > 0)
                {
                    var currentKlassViewed = true;
                    foreach (OntologyClass el in collection)
                        if (!viewed.Contains(el))
                        {
                            s.Push(el);
                            currentKlassViewed = false;
                        }
                    if (currentKlassViewed)
                        viewed.Add(currentKlass);
                }
            }
            return klass;
        }

        public OntologyClass FindParent(string name)
        {
            return find(name, true);
        }

        public OntologyClass FindChild(string name)
        {
            return find(name, false);
        }
    }
    
}



