using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
    public class EditorProject
    {
        private List<OntologyNode> _ontology;
        private List<VocTheme> _themes;
        private Dictionary<string, List<string>> _gramtab;
        private List<string> _segments;
        private Dictionary<string, string> _paths;
        private string _projectPath;

        private string getPath(string key)
        {
            if (_paths.ContainsKey(key))
                return _paths[key];
            else
                return null;
        }
        
        public FactSchemeBank Bank { get; set; }
        [XmlIgnore]
        public List<OntologyNode> Ontology { get { return _ontology; } }
        [XmlElement(ElementName = EditorConstants.XML_PROJECT_ONTOLOGY)]
        public string OntologyPath
        {
            get { return getPath(EditorConstants.XML_PROJECT_ONTOLOGY); }
            set { _paths[EditorConstants.XML_PROJECT_ONTOLOGY] = value; }
        }
        [XmlIgnore]
        public List<VocTheme> Dictionary { get { return _themes; } }
        [XmlElement(ElementName = EditorConstants.XML_PROJECT_DICTIONARY)]
        public string DictionaryPath
        {
            get { return getPath(EditorConstants.XML_PROJECT_DICTIONARY); }
            set { _paths[EditorConstants.XML_PROJECT_DICTIONARY] = value; }
        }
        [XmlIgnore]
        public Dictionary<string, List<string>> Gramtab { get { return _gramtab; } }
        [XmlElement(ElementName = EditorConstants.XML_PROJECT_GRAMTAB)]
        public string GramtabPath
        {
            get { return getPath(EditorConstants.XML_PROJECT_GRAMTAB); }
            set { _paths[EditorConstants.XML_PROJECT_GRAMTAB] = value; }
        }
        [XmlIgnore]
        public List<string> Segments { get { return _segments; } }
        [XmlElement(ElementName = EditorConstants.XML_PROJECT_SEGMENTS)]
        public string SegmentsPath
        {
            get { return getPath(EditorConstants.XML_PROJECT_SEGMENTS); }
            set { _paths[EditorConstants.XML_PROJECT_SEGMENTS] = value; }
        }
        public XElement Markup { get; set; }

        public EditorProject()
        {
            _paths = new Dictionary<string, string>();
            _ontology = new List<OntologyNode>();
            _themes = new List<VocTheme>();
            Bank = new FactSchemeBank();
            _gramtab = new Dictionary<string, List<string>>();
            _segments = new List<string>();
        }
        
        private Stream GetStream(XElement root, string xelement)
        {
            string path = root.Element(xelement).Value;
            Stream stream = new FileStream(path, FileMode.Open);
            return stream;
        }
        public EditorProject(Stream fstream, string path) : this()
        {
            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);

            XElement root = doc.Element(EditorConstants.XML_EDITOR_ROOT_NAME);
            //load ontology
            var ontPath = root.Element(EditorConstants.XML_PROJECT_ONTOLOGY).Value;
            _paths[EditorConstants.XML_PROJECT_ONTOLOGY] = ontPath;
            if (!System.IO.Path.IsPathRooted(ontPath))
                ontPath = path + ontPath;
            var ontStream = new FileStream(ontPath, FileMode.Open);
            LoadOntology(ontStream);
            ontStream.Close();

            //load dictionary
            var themesPath = root.Element(EditorConstants.XML_PROJECT_DICTIONARY).Value;
            var tmp = themesPath;
            if (!System.IO.Path.IsPathRooted(themesPath))
                themesPath = path + themesPath;
            _themes = LoadDictionary(themesPath);
            _paths[EditorConstants.XML_PROJECT_DICTIONARY] = tmp;


            //load segments
            var segPath = root.Element(EditorConstants.XML_PROJECT_SEGMENTS).Value;
            _paths[EditorConstants.XML_PROJECT_SEGMENTS] = segPath;
            if (!System.IO.Path.IsPathRooted(segPath))
                segPath = path + segPath;
            var segStream = new FileStream(segPath, FileMode.Open);
            _segments = LoadSegments(segStream);
            segStream.Close();

            //load gramtab
            var gramtabPath = root.Element(EditorConstants.XML_PROJECT_GRAMTAB).Value;
            _paths[EditorConstants.XML_PROJECT_GRAMTAB] = gramtabPath;
            if (!System.IO.Path.IsPathRooted(gramtabPath))
                gramtabPath = path + gramtabPath;
            var gramtabStream = new FileStream(gramtabPath, FileMode.Open);
            _gramtab = LoadGramtab(gramtabStream);
            gramtabStream.Close();
            
            XElement xbank = root.Element(FatonConstants.XML_BANK_TAG);
            Markup = root.Element(EditorConstants.XML_EDITOR_MARKUP);

            //saved for the future
            //if (xthemes != null)
            //{
            //    _themes = new List<VocTheme>();
            //    foreach (XElement xtheme in xthemes.Elements())
            //    {
            //        string name = xtheme.Attribute(EditorConstants.XML_PROJECT_DICTIONARYTHEMENAME).Value;
            //        VocTheme theme = new VocTheme(ref name);
            //        if (xtheme.Elements().Count() > 0)
            //            theme.root = false;
            //        foreach (XElement xbase in xtheme.Elements())
            //        {
            //            string basename = xbase.Value;
            //            VocTheme parent = Themes.Find(x => x.name == basename);
            //            if (parent == null)
            //                throw new Exception(string.Format(Locale.ERR_DICTIONARY_NOPARENT, basename, theme.name));
            //            else
            //            {
            //                parent.children.Add(theme);
            //                theme.parents.Add(parent);
            //            }
            //        }
            //        _themes.Add(theme);
            //    }
            //}

            if (xbank != null)
            {
                Bank = FactSchemeBank.FromXml(xbank, Ontology);
            }

            _projectPath = path;
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
            foreach (var path in _paths)
                doc.Root.Add(new XElement(path.Key, path.Value));
            doc.Root.Add(Bank.ToXml().Root);
            doc.Root.Add(Markup);
            doc.Save(fstream);
        }
        public void Export(string filename, FactSchemeBank bank) { }
        public void GenFatonCfg(string filename)
        {
            Dictionary<string, string> cfg = new Dictionary<string, string>();
            cfg[FatonConstants.CFG_ONT_PATH] = _paths[EditorConstants.XML_PROJECT_ONTOLOGY];
            cfg[FatonConstants.CFG_KLANVOC_PATH] = _paths[EditorConstants.XML_PROJECT_DICTIONARY];
            //cfg[FatonConstants.CFG_SCHEMES_PATH] = 
            cfg[FatonConstants.CFG_SEG_PATH] = _paths[EditorConstants.XML_PROJECT_SEGMENTS];
            //var fstream = new FileStream(filename, FileMode.Create);
            var sw = new StreamWriter(filename);
            foreach (var entry in cfg)
            {
                sw.WriteLine(String.Format("{0}={1}", entry.Key, entry.Value));
            }
            sw.Close();
        }

        public List<OntologyNode> LoadOntology(Stream fstream, string path = null)
        {
            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);
            _ontology = OntologyBuilder.fromXml(doc.Root);
            if (path != null)
                _paths[EditorConstants.XML_PROJECT_ONTOLOGY] = path;

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
            if (path != null)
                _paths[EditorConstants.XML_PROJECT_DICTIONARY] = path;
            return _themes;
        }

        public Dictionary<string, List<string>> LoadGramtab(Stream fstream, string path = null)
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
            if (path != null)
                _paths[EditorConstants.XML_PROJECT_GRAMTAB] = path;
            return _gramtab;
        }

        public List<string> LoadSegments(Stream fstream, string path = null)
        {
            var resut = new List<string>();

            StreamReader sr = new StreamReader(fstream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);

            foreach (var xclass in doc.Root.Elements())
                resut.Add(xclass.Attribute("name").Value);

            _segments = resut;
            if (path != null)
                _paths[EditorConstants.XML_PROJECT_SEGMENTS] = path;
            return _segments;
        }
    }
}
