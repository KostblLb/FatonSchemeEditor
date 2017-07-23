using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faton
{
    public static class FatonConstants
    {
        public const string NODE_CLASS = "CLASS";
        public const string NODE_RELATION = "RELATION";

        public const string RESULT_NAME_NEW = "NewResult";

        public const string XML_BANK_TAG = "Bank";
        public const string XML_SCHEME_TAG = "Scheme";
        public const string XML_SCHEME_NAME = "Name";

        public const string XML_ARGUMENT_TAG = "Argument";
        public const string XML_ARGUMENT_CONDITION_TAG = "Condition";
        public const string XML_ARGUMENT_GROUPTYPE = "GroupType";
        public const string XML_ARGUMENT_SEGMENT = "Segment";
        public const string XML_ARGUMENT_CLASSNAME = "ClassName";
        public const string XML_ARGUMENT_OBJECTTYPE = "ObjectType";
        public const string XML_ARGUMENT_COMPARETYPE = "TypeCompare";
        public const string XML_ARGUMENT_ORDER = "Order"; //not ppresented in faton
        public const string XML_ARGUMENT_CONDITION_ATTRNAME = "AttrName";
        public const string XML_ARGUMENT_CONDITION_OPERATION = "Operation";
        public const string XML_ARGUMENT_CONDITION_TYPE = "Type";
        public const string XML_ARGUMENT_CONDITION_DATA = "Data";
        public const string XML_ARGUMENT_CONDITION_ID = "ID";
        public const string XML_ARGUMENT_CONDITION_ARG1 = "Arg1";
        public const string XML_ARGUMENT_CONDITION_ARG2 = "Arg2";

        public const string XML_RESULT_TAG = "Result";
        public const string XML_RESULT_TYPE = "Type";
        public const string XML_RESULT_OBJECTTYPE = "ObjectType";
        public const string XML_RESULT_CLASSNAME = "ClassName";
        public const string XML_RESULT_RULE_TAG = "Rule";
        public const string XML_RESULT_RULE_TYPE = "Type";
        public const string XML_RESULT_RULE_ATTR = "AttrName";
        public const string XML_RESULT_RULE_ATTRFROM = "FromAttrName";
        public const string XML_RESULT_RULE_RESOURCE = "Resurce";
        public const string XML_RESULT_RULE_RESOURCETYPE = "ResourceType";

        public const string XML_CONDITIONCOMPLEX_TAG = "ConditionComplex";
        public const string XML_CONDITIONCOMPLEX_CONDITION_TAG = "Condition";
        

        //constants for generating ini config files for faton
        public const string CFG_SEG_PATH = "segments";
        public const string CFG_SEGVOC_PATH = "seg_voc";
        public const string CFG_TEXTFILE_PATH = "text";
        public const string CFG_SCHEMES_PATH = "schemes";
        public const string CFG_KLANVOC_PATH = "klan_voc";
        public const string CFG_TERMINVOC_PATH = "termin_voc";
        public const string CFG_ONT_PATH = "ontology";
        public const string CFG_ALEXVOC_PATH = "alex_voc";
        public const string CFG_LOGFILE_PATH = "log_file";
    }
}
