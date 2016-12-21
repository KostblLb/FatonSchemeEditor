using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HelloForms
{
    public class OntologyNode
    {
        public enum Type { Class, Relation, Domain };
        public class Attribute
        {
            string type;
            string name;
            public Attribute(string myType, string myValue)
            {
                type = myType;
                name = myValue;
            }
            public string Type
            {
                get { return type; }
                set { type = value; }
            }
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }
        public List<Attribute> attrs;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Type type;
        public string TypeString
        {
            get
            {
                if (type == Type.Class)
                    return FatonConstants.NODE_CLASS;
                else if (type == Type.Domain)
                    return "Домен"; // FIXME
                else if (type == Type.Relation)
                    return FatonConstants.NODE_RELATION;
                else
                    return "";
            }
        }
        public void AddAttribute(Attribute att)
        {
            attrs.Add(att);
        }
        public List<Attribute> Attributes
        {
            get { return attrs; }
        }
        public OntologyNode(string myName, Type myType)
        {
            _name = myName;
            type = myType;
            attrs = new List<Attribute>();
        }
    }
    public class Class : OntologyNode
    {
        public List<Class> _parents;
        private List<Class> _children;
        public List<Class> Children
        {
            get { return _children; }
        }
        public List<Class> Parents
        {
            get { return _parents; }
        }
        
        /// <summary>
        /// returns ONLY attributes inherited from parents
        /// </summary>
        public List<Tuple<Attribute, Class>> InheritedAttributes
        {
            get
            {
                List<Tuple<Attribute, Class>> inherited = new List<Tuple<Attribute, Class>>();
                if (!_parents.Any())
                    return inherited;
                Queue<Class> q = new Queue<Class>(_parents); //BFS for parents
                Class parent;
                while(q.Any())
                {
                    parent = q.Dequeue();
                    foreach (Attribute attr in parent.Attributes)
                        inherited.Add(new Tuple<Attribute, Class>(attr, parent));
                    foreach (Class newParent in parent._parents)
                        q.Enqueue(newParent);
                }
                return inherited;
            }
        }
        public Class(string myName) : base(myName, Type.Class)
        {
            _children = new List<Class>();
            _parents = new List<Class>();
        }

        public void AddChild(Class c)
        {
            _children.Add(c);
        }
        public void AddParent(Class p)
        {
            _parents.Add(p);
        }
    }
    public class Relation : OntologyNode
    {
        Class arg1;
        Class arg2;
        public Relation(string myName, Class myArg1 , Class myArg2) : base(myName, Type.Relation)
        {
            arg1 = myArg1;
            arg2 = myArg2;
        }
    }

    
}



