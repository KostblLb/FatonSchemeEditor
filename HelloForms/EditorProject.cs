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

        public FactSchemeBank Bank { get { return _bank; } }
        public List<OntologyNode> Ontology { get { return _ontology; } }
        public List<VocTheme> Themes { get { return _themes; } }
        public XElement Markup { get; set; }

        public EditorProject()
        {
            _ontology = new List<OntologyNode>();
            _themes = new List<VocTheme>();
            _bank = new FactSchemeBank();
        }

        public EditorProject(Stream fstream) : this() {
            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);

            XElement root = doc.Element(EditorConstants.XML_EDITOR_ROOT_NAME);
            XElement xontology = root.Element(EditorConstants.XML_PROJECT_ONTOLOGY);
            XElement xthemes = root.Element(EditorConstants.XML_PROJECT_DICTIONARY);
            XElement xbank = root.Element(FatonConstants.XML_BANK_NAME);

            if (xontology != null)
                _ontology = OntologyBuilder.fromXml(xontology);
            if (xthemes != null)
            {
                _themes = new List<VocTheme>();
                foreach(XElement xtheme in xthemes.Elements())
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
                }
            }

            if (xbank != null)
            {
                _bank = FactSchemeBank.FromXml(xbank, Ontology);
            }
        }
        public void Save(Stream fstream) {
            XDocument doc = new XDocument();
            doc.Add(new XElement(EditorConstants.XML_EDITOR_ROOT_NAME));

            XElement xthemes = new XElement(EditorConstants.XML_PROJECT_DICTIONARY);
            foreach (var theme in _themes)
            {
                XElement xtheme = new XElement(EditorConstants.XML_PROJECT_DICTIONARYTHEMENAME,
                    new XAttribute(EditorConstants.XML_PROJECT_DICTIONARYTHEMENAME, theme.name));
                foreach (var parent in theme.parents)
                    xtheme.Add(new XElement(EditorConstants.XML_PROJECT_DICTIONARYBASE, parent.name));
                xthemes.Add(xtheme);
            }
            doc.Root.Add(_xontology);
            doc.Root.Add(xthemes);
            doc.Root.Add(Bank.ToXml());
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
    }
}
