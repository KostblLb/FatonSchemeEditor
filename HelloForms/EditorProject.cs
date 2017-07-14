using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using Ontology;
using FactScheme;
using Shared;
using System.IO;
using Faton;
using VocabularyExtractor;

namespace HelloForms
{
    /// <summary>
    /// project file structure:
    /// * ontology (direct copy from input)
    /// * dictionary
    /// * schemes bank
    /// * editor markup
    /// </summary>
    class EditorProject
    {
        private XElement _xontology;
        private FactSchemeBank _bank;
        private List<OntologyNode> _ontology;
        private List<VocTheme> _themes;
        private Dictionary<string, List<string>> _gramtab;
        private List<string> _segments;

        public FactSchemeBank Bank { get { return _bank; } }
        public List<OntologyNode> Ontology { get { return _ontology; } }
        public List<VocTheme> Themes { get { return _themes; } }
        public Dictionary<string, List<string>> Gramtab { get { return _gramtab; } }
        public List<string> Segments { get { return _segments; } }
        public XElement Markup { get; set; }

        public EditorProject()
        {
            _xontology = new XElement(EditorConstants.XML_PROJECT_ONTOLOGY);
            _ontology = new List<OntologyNode>();
            _themes = new List<VocTheme>();
            _bank = new FactSchemeBank();
            _gramtab = new Dictionary<string, List<string>>();
            _segments = new List<string>();
        }

        public EditorProject(Stream fstream) : this()
        {
            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);

            XElement root = doc.Element(EditorConstants.XML_EDITOR_ROOT_NAME);
            XElement xontology = root.Element(EditorConstants.XML_PROJECT_ONTOLOGY);
            XElement xthemes = root.Element(EditorConstants.XML_PROJECT_DICTIONARY);
            XElement xbank = root.Element(FatonConstants.XML_BANK_NAME);
            Markup = root.Element(EditorConstants.XML_EDITOR_MARKUP);

            if (xontology != null)
            {
                _ontology = OntologyBuilder.fromXml(xontology);
                _xontology = xontology;
            }
            if (xthemes != null)
            {
                _themes = new List<VocTheme>();
                foreach (XElement xtheme in xthemes.Elements())
                {
                    string name = xtheme.Attribute(EditorConstants.XML_PROJECT_DICTIONARYTHEMENAME).Value;
                    VocTheme theme = new VocTheme(ref name);
                    if (xtheme.Elements().Count() > 0)
                        theme.root = false;
                    foreach (XElement xbase in xtheme.Elements())
                    {
                        string basename = xbase.Value;
                        VocTheme parent = Themes.Find(x => x.name == basename);
                        if (parent == null)
                            throw new Exception(string.Format(Locale.ERR_DICTIONARY_NOPARENT, basename, theme.name));
                        else
                        {
                            parent.children.Add(theme);
                            theme.parents.Add(parent);
                        }
                    }
                    _themes.Add(theme);
                }
            }

            if (xbank != null)
            {
                _bank = FactSchemeBank.FromXml(xbank, Ontology);
            }
        }
        public void Save(Stream fstream)
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement(EditorConstants.XML_EDITOR_ROOT_NAME));

            XElement xthemes = new XElement(EditorConstants.XML_PROJECT_DICTIONARY);
            foreach (var theme in _themes)
            {
                XElement xtheme = new XElement(EditorConstants.XML_PROJECT_DICTIONARYTHEME,
                    new XAttribute(EditorConstants.XML_PROJECT_DICTIONARYTHEMENAME, theme.name));
                foreach (var parent in theme.parents)
                    xtheme.Add(new XElement(EditorConstants.XML_PROJECT_DICTIONARYBASE, parent.name));
                xthemes.Add(xtheme);
            }
            doc.Root.Add(_xontology);
            doc.Root.Add(xthemes);
            doc.Root.Add(Bank.ToXml().Root);
            doc.Root.Add(Markup);
            doc.Save(fstream);
        }
        public void Export(string filename, FactSchemeBank bank) { }

        public List<OntologyNode> LoadOntology(Stream fstream)
        {
            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);
            _xontology = new XElement(doc.Root);
            _xontology.Name = EditorConstants.XML_PROJECT_ONTOLOGY;
            _ontology = OntologyBuilder.fromXml(_xontology);

            return _ontology;
        }

        public List<VocTheme> LoadDictionary(string path)
        {
            FloatingPointReset.Action();
            if (path == null || path.Length == 0)
                return null;
            Extractor ex = new Extractor();
            ex.ParseKlanVocabulary(ref path);
            _themes = ex.Themes();
            FloatingPointReset.Action();

            return _themes;
        }

        public Dictionary<string, List<string>> LoadGramtab(Stream fstream)
        {
            StreamReader sr = new StreamReader(fstream);
            String fstring = sr.ReadToEnd();
            StringReader reader = new StringReader(fstring);
            int idx = fstring.IndexOf("<attr>"); //hardcoded ini file section name
            string str = "";
            while (!str.Equals("<attr>"))
                str = reader.ReadLine();
            str = reader.ReadLine();
            do
            {
                List<string> gramtabParam = new List<string>();
                if (str.First().Equals('#'))
                    continue;
                string[] paramStr = str.Split('=');
                int eqidx = str.IndexOf("=");
                var paramName = paramStr[0];
                var paramList = paramStr[1].Split('{')[1].Split('}')[0]; // exclude '{' and '}'
                gramtabParam = new List<string>((paramList.Split(',').ToList()));
                _gramtab.Add(paramName, gramtabParam);
                str = reader.ReadLine();
            } while (!string.IsNullOrEmpty(str) && !str.First().Equals('<'));
                return _gramtab;
        }

        public List<string> LoadSegments(Stream fstream)
        {
            var resut = new List<string>();

            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);

            foreach (var xclass in doc.Root.Elements())
                resut.Add(xclass.Attribute("name").Value);

            _segments = resut;
            return _segments;
        }
    }
}
