using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public static class EditorConstants
    {
        public static int CONDITION_DATAGRID_TYPE_COL = 0;
        public static int CONDITION_DATAGRID_ATTR_COL = 1;
        public static int CONDITION_DATAGRID_COMPAR_COL = 2;
        public static int CONDITION_DATAGRID_VALUES_COL = 3;

        public const string TABPAGE_WPF_HOST_NAME = "TABPAGE_WPF_HOST";

        public const string CONDITION_DATAGRID_ERROR_ATT_NAME = "Такого атрибута не существует";

        public const string RESULT_NAME_NEW = "Новый результат";

        public const string DEFAULT_SCHEME_NAME = "Новая схема";
        public const string DEFAULT_BANK_NAME = "Новый банк";

        public static string XML_ROOT_NAME = "FATON_EDITOR_FILE";
        public static string XML_EDITOR_MARKUP = "FATON_EDITOR_MARKUP";

        public static int SIMPLE_XML = 1, EDITOR_XML = 2;
    }
}
