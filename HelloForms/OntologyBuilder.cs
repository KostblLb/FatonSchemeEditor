using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HelloForms
{
    public class OntologyBuilder
    {
        public static List<OntologyNode> fromXml(System.IO.Stream istream)
        {
            List<Class> tempClasses = new List<Class>();
            List<OntologyNode> result = new List<OntologyNode>();
            Class parentClass = null;
            Class currentClass = null;
            Relation currentRelation = null;
            OntologyNode currentDomain = null;

            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            XmlReader xmlReader = XmlReader.Create(istream, settings);
            XmlNode node = doc.ReadNode(xmlReader);
            if (node.NodeType == XmlNodeType.XmlDeclaration)
                node = doc.ReadNode(xmlReader);
            Console.WriteLine("xml entry point: ", node.Value, node.Name);
            while (node != null)
            {
                Console.WriteLine(node.Name);
                switch (node.Name)
                {
                    case "ontology": //get into ontology
                        node = node.FirstChild;
                        Console.WriteLine("First child of ontology: " + node.Name);
                        break;
                    case "class":
                        string myName = node.Attributes["name"].Value;
                        Class myClass = new Class(myName); //, parentClass);
                        parentClass = myClass;
                        currentClass = myClass;
                        tempClasses.Add(myClass);
                        Console.WriteLine("Adding class " + myName);
                        if (node.FirstChild != null)
                            node = node.FirstChild;
                        else if (node.NextSibling != null)
                        {
                            //parentClass = myClass.parent;
                            node = node.NextSibling;
                        }
                        else
                        {
                            //parentClass = myClass.parent != null ? myClass.parent.parent : null;
                            node = node.ParentNode.NextSibling;
                        }
                        break;
                    case "attr":
                        string myType = node.Attributes["type"].Value;
                        OntologyNode.Attribute myAttr = new OntologyNode.Attribute(myType, node.InnerText);
                        if (node.ParentNode.Name == "class")
                            currentClass.AddAttribute(myAttr);
                        else if (node.ParentNode.Name == "relation")
                            currentRelation.AddAttribute(myAttr);
                        else
                            throw new Exception("Attribute " + node.Value + " has no parent!");

                        if (node.NextSibling != null)
                            node = node.NextSibling;
                        else while (node.NextSibling == null)
                            {
                                if (node.ParentNode == null)
                                    break; //root node?
                                node = node.ParentNode;
                                if (node.NextSibling != null)
                                    node = node.NextSibling;
                            }
                        /*if(node.ParentNode.NextSibling != null)
                        {
                            node = node.ParentNode.NextSibling;
                            if (node.Name == "class")
                                parentClass = parentClass.parent;
                        }
                        else
                            node = doc.ReadNode(xmlReader);
                            */
                        break;
                    case "base":
                        parentClass = tempClasses.Find(x => x.Name == node.InnerText);
                        if (parentClass != null)
                        {
                            currentClass.AddParent(parentClass);
                            parentClass.AddChild(currentClass);
                        }
                        if (node.NextSibling != null)
                            node = node.NextSibling;
                        else while (node.NextSibling == null)
                            {
                                if (node.ParentNode == null)
                                {
                                    break; //root node?
                                }
                                node = node.ParentNode;
                                if (node.Name == "class")
                                    //parentClass = parentClass.parent;
                                    if (node.NextSibling != null)
                                    {
                                        node = node.NextSibling;
                                    }
                            }
                        break;
                    case "relation":
                        string myRelationName = node.Attributes["name"].Value;
                        string myArg1 = node.Attributes["arg1"].Value;
                        string myArg2 = node.Attributes["arg2"].Value;
                        Class arg1Class = (Class)result.Find(x => x.Name == myArg1); //works if there are no classes with same name!
                        Class arg2Class = (Class)result.Find(x => x.Name == myArg2);
                        Relation myRelation = new Relation(myRelationName, arg1Class, arg2Class);
                        result.Add(myRelation);
                        currentRelation = myRelation;

                        Console.WriteLine("Adding relation " + myRelationName);
                        if (node.FirstChild != null)
                            node = node.FirstChild;
                        else if (node.NextSibling != null)
                            node = node.NextSibling;
                        else if (node.ParentNode.NextSibling != null)
                            node = node.ParentNode.NextSibling;
                        else
                            node = doc.ReadNode(xmlReader);
                        break;
                    case "domain":
                        string myDomainName = node.Attributes["name"].Value;
                        OntologyNode myDomain = new OntologyNode(myDomainName, OntologyNode.Type.Domain);
                        result.Add(myDomain);
                        currentDomain = myDomain;
                        if (node.FirstChild != null)
                            node = node.FirstChild;
                        else if (node.NextSibling != null)
                            node = node.NextSibling;
                        else if (node.ParentNode.NextSibling != null)
                            node = node.ParentNode.NextSibling;
                        else
                            node = doc.ReadNode(xmlReader);
                        break;
                    case "value":
                        OntologyNode.Attribute myDomainValue = new OntologyNode.Attribute("", node.InnerText);
                        if (node.NextSibling != null)
                            node = node.NextSibling;
                        else if (node.ParentNode.NextSibling != null)
                            node = node.ParentNode.NextSibling;
                        else
                            node = doc.ReadNode(xmlReader);
                        break;
                    default:
                        node = node.NextSibling;
                        break;
                }
            }
            foreach(Class tempClass in tempClasses)
            {
                if (!tempClass.Parents.Any())
                    result.Add(tempClass);
            }
            return result;
        }
        public static List<OntologyNode> fromXmlTest(Stream istream)
        {
            List<OntologyNode> result = new List<OntologyNode>();
            StreamReader sr = new StreamReader(istream);
            string xmlString = sr.ReadToEnd();

            Class currentClass = null;
            Relation currentRelation = null;

            XDocument doc = XDocument.Parse(xmlString);
            var ontology = doc.Element("ontology"); // aka Root
            var classes = from x in ontology.Elements()
                          where x.Name.LocalName == "class"
                          select x;
            foreach(XElement c in classes)
            {
                currentClass = new Class(c.Attribute("name").Value);
                var classAttrs = from x in c.Elements()
                            where x.Name.LocalName == "attr"
                            select x;
                var classParents = from x in c.Elements()
                              where x.Name.LocalName == "base"
                              select x;
                foreach(XElement attrElement in classAttrs)
                {
                    string attrName = ((XText)attrElement.FirstNode).Value; //get inner text of <attr>
                    string attrType = attrElement.Attribute("type").Value;
                    currentClass.Attributes.Add(new OntologyNode.Attribute(attrType, attrName));
                }
                if (classParents.Any())
                {
                    foreach (XElement parentElement in classParents)
                    {
                        Class parentClass = (Class) result.Find(x=> x.Name == ((XText)parentElement.FirstNode).Value);
                        if (parentClass == null)
                            throw new Exception(String.Format("base class {0} not defined for {1}", parentElement.Attribute("name").Value, currentClass.Name));
                        parentClass.AddChild(currentClass);
                        currentClass.AddParent(parentClass);
                    }
                }
                else
                    result.Add(currentClass);
            }
            return result;
        }
    }
}
