using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//Klan vocabulary structure:
//[id]: <[ClassName]> [ClassType={stat, stat_head, semc, attr, sinonyms}]([ParentId*])

//IMPORTANT: Klan vocabs are in Windows-1251 encoding

//example:
//9: <Наука> stat()
//2: <Астрономия> stat( 9 )
//7: <Пространство> stat( 2 )
//11: <Физика> semc( 9 )
//6: <Небесное тело> stat( 2 )
//12: <Технология> stat_head()
//14: <персона> stat()
//13: <Учёный> stat( 9 14 )
//15: <звезда> stat( 6 )
//16: <планета> stat( 6 )

namespace Vocabularies
{

    public class KlanParser
    {
        public static Vocabulary Parse(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            var voc = Parse(fs);
            fs.Close();
            return voc;
        }
        public static Vocabulary Parse(Stream istream)
        {
            StreamReader reader = new StreamReader(istream, Encoding.GetEncoding(1251));
            
            Vocabulary voc = new Vocabulary();
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                var splits = str.Split(':', '<', '>', '(', ')');
                var classId = int.Parse(splits[0]);
                var className = splits[2];
                var classType = (TerminType) Enum.Parse(typeof(TerminType), splits[3].Trim(' '));
                var parentIds = from x in splits[4].Split(' ')
                                where !String.IsNullOrEmpty(x)
                                select int.Parse(x);
                voc.AddTermin(classId, className, classType, parentIds);
            }
            istream.Close();
            return voc;
        }
    }
}
