using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocabularies
{
    public enum TerminType { stat, stat_head, semc, attr, sinonyms }
    public class Termin
    {
        public string Name { get; set; }
        public TerminType Type { get; set; }
        public List<Termin> Parents { get; set; }
        public List<Termin> Children { get; set; }

        public Termin(string name, TerminType type = TerminType.semc)
        {
            Name = name;
            Type = type;
            Parents = new List<Termin>();
            Children = new List<Termin>();
        } 
    }
    public class Vocabulary
    {
        Dictionary<string, int> strToId;
        Dictionary<int, Termin> Termins { get; }
        public Termin Root { get; }
        public Vocabulary()
        {
            strToId = new Dictionary<string, int>();
            Termins = new Dictionary<int, Termin>();
            Root = new Termin(Faton.FatonConstants.VOCABULARY_ROOT_NAME);
            Termins.Add(-1, Root); // add root termin
            strToId[Faton.FatonConstants.VOCABULARY_ROOT_NAME] = -1;
        }
        public Termin AddTermin(int id, string name, TerminType type, IEnumerable<int> parentIds)
        {
            strToId[name] = id;
            var term = new Termin(name, type);
            if (parentIds == null || !parentIds.Any())
            {
                Root.Children.Add(term);
                term.Parents.Add(Root);
            }
            foreach (var parentId in parentIds)
            {
                var parent = Termins[parentId];
                term.Parents.Add(Termins[parentId]); //guaranteed to find parent if using klan
                parent.Children.Add(term);
            }
            Termins.Add(id, term);
            return term;
        }
        public Termin this[string key]
        {
            get { return Termins[strToId[key]]; }
        }
        public IEnumerator<Termin> GetEnumerator()
        {
            return Termins.Values.GetEnumerator();
        }
    }
}
