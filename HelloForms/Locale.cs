using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public static class Locale
    {
        #region ru
        public const string STATUS_PROJECT_LOADED = "Проект загружен.";
        public const string STATUS_ONTOLOGY_LOADED = "Онтология загружена.";
        public const string STATUS_DICTIONARY_LOADED = "Словарь загружен.";
        public const string STATUS_SEGMENTS_LOADED = "Дескриптор сегментов загружен.";
        public const string STATUS_GRAMTAB_LOADED = "Грамтаб загружен.";
        public const string STATUS_SAVED = "Сохранено";

        public const string ONTOLOGY_TREE_ADD_ARG = "Добавить как аргумент";
        public const string ONTOLOGY_TREE_ADD_RESULT = "Добавить как результат";

        public const string SCHEME_EXPORT_FILTER = "Faton Scheme file|*.xml";
        public const string DICTIONARY_FORMAT_FILTER = "Словарь KLAN|*.vc.th||*.*";

        public const string ERR_ONTOLOGY_NOT_LOADED = "Онтология не загружена";
        public const string ERR_DICTIONARY_NOPARENT = "Родитель {0} не определён для {1}";

        public const string SCHEME_TAB_NAME = "Схема ({0})";

        public const string SCHEME_CONDITION_TYPE_SELECT = "Тип условия";
        public const string SCHEME_CONDITION_ACTANT_NAME_DEFAULT = "ИмяАктанта";
        #endregion ru
    }
}
