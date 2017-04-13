﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ontology;

namespace FactScheme
{
    //public partial class FactScheme
    //{
    public class Argument : ISchemeComponent
    {
        OntologyClass _klass;
        bool _useInheritance;
        protected uint _order;
        List<Condition> _conditions;

        public OntologyClass Klass
        {
            get { return _klass; }
        }
        public bool Inheritance
        {
            get { return _useInheritance; }
            set { _useInheritance = value; }
        }

        public uint Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public List<Condition> Conditions
        {
            get { return _conditions; }
        }

        public Argument(OntologyClass klass, string name = null, bool inherit = true)
        {
            _klass = klass;
            _useInheritance = inherit;
            //_tag = tag;
            _conditions = new List<Condition>();
        }

        public Condition AddContition(string attrName = "")
        {
            Condition cond = new Condition(this, attrName);
            _conditions.Add(cond);
            return cond;
        }

        public List<ISchemeComponent> Up()
        {
            return new List<ISchemeComponent>(); // return empty list for arguments cant have inputs
        }
        public List<Connection> Connections(ISchemeComponent other)
        {
            return new List<Connection>();
        }
    }
    //}
}
