using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using network;
using System.Windows;
using System.Windows.Controls;
using FactScheme;
using Ontology;
using KlanVocabularyExtractor;

namespace HelloForms
{
    /// <summary>
    /// Provides two way interactions between fact scheme editor and Network Views.
    /// <para/>Medium knows specifications of fact schemes and how they should look like in a NV.
    /// <para/>Medium is subscribed to certain events raised by NV and its components
    /// </summary>
    public class Medium
    {
        public static void AddSchemeConnection(Scheme scheme, Connector src, Connector dst)
        {
            if (dst.Tag == null || src.Tag == null)
                throw new Exception("connector not attached to attribute");

            if (dst.Tag is FactScheme.Result) //very specific code for linking EDIT connector of a result
            {
                ((Result)dst.Tag).EditObject = src.ParentNode.Tag as ISchemeComponent;
            }
            else if (dst.ParentNode.Tag is FactScheme.Result)
            {
                var dstAttr = dst.Tag as OntologyNode.Attribute;
                var srcAttr = src.Tag as OntologyNode.Attribute;
                var result = dst.ParentNode.Tag as FactScheme.Result;
                if (src.ParentNode.Tag is FactScheme.Argument)
                    result.AddRule(FactScheme.Result.RuleType.DEF,
                        dstAttr,
                        src.ParentNode.Tag as ISchemeComponent,
                        srcAttr);
                else if (src.ParentNode.Tag is FactScheme.Functor)
                    result.AddRule(FactScheme.Result.RuleType.FUNC,
                        dstAttr,
                        src.ParentNode.Tag as ISchemeComponent,
                        srcAttr);
            }

            else if (dst.ParentNode.Tag is FactScheme.Functor)
            {
                //wont work
                //var functor = dst.ParentNode.Tag as FactScheme.Functor;
                //functor.SetInput(src.Tag, 
                //    src.ParentNode.Tag, 
                //    dst.Tag as FactScheme.Functor.FunctorInput);
            }
        }

        public static void RemoveSchemeConnection(Connector src, Connector dst)
        {
            ISchemeComponent dstComponent = dst.ParentNode.Tag as ISchemeComponent;
            if (dst.Tag is OntologyNode.Attribute)
                dstComponent.Free(dst.Tag as OntologyNode.Attribute);
            else
            { ///
            }
        }

        ///convert factscheme argument into nv node
        ///
        public static NodeInfo Convert(FactScheme.Argument argument)
        {
            NodeInfo info = new NodeInfo();

            info.Tag = argument;

            info.Sections = new List<NodeInfo.SectionInfo>();
            
            info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Name);
            argument.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Order")
                    info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Name);
            };

            if (argument.ArgType == Argument.ArgumentType.IOBJECT)
            {
                var attrs = new List<OntologyNode.Attribute>(argument.Klass.OwnAttributes);

                var inheritedAttrs = argument.Klass.InheritedAttributes.Select(i => i.Item1);
                foreach (var inheritedAttr in inheritedAttrs)
                    attrs.Add(inheritedAttr as OntologyNode.Attribute);

                foreach (var attr in attrs)
                {
                    NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                    attrInfo.Data = attr;
                    attrInfo.IsInput = false;
                    attrInfo.IsOutput = true;
                    var attrName = new Label();
                    attrName.Content = attr.Name;
                    attrInfo.UIPanel = attrName;
                    info.Sections.Add(attrInfo);
                }
            }

            else
            {
                NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                VocTheme theme = argument.Theme;
                var attr = new OntologyNode.Attribute(theme);
                attrInfo.Data = attr;
                attrInfo.IsOutput = true;
                var attrName = new Label();
                attrName.Content = "Значение";
                attrInfo.UIPanel = attrName;
                info.Sections.Add(attrInfo);
            }

            return info;
        }

        public static NodeInfo Convert(FactScheme.Result result)
        {
            NodeInfo info = new NodeInfo();

            info.Tag = result;

            info.Header.NameChangeable = true;
            info.NodeNameProperty = result.Name;
            info.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NodeNameProperty")
                    result.Name = (sender as NodeInfo).NodeNameProperty;
            };

            var resultInfo = new NodeInfo.SectionInfo();
            resultInfo.Data = result;
            resultInfo.UIPanel = ResultInfoPanel(result);
            resultInfo.IsInput = true;
            resultInfo.InputValidation = (s, e) =>
            {
                var input = e.SourceConnector.ParentNode.Tag;
                if (input is Argument)
                {
                    if (result.Reference !=((Argument)input).Klass)
                        e.Valid = false;
                }
                else if (input is Result)
                {
                    if (result.Reference != ((Result)input).Reference)
                        e.Valid = false;
                }
            };
            info.Sections.Add(resultInfo);

            if (result.Reference is OntologyClass)
            {
                var attrs = new List<OntologyNode.Attribute>((result.Reference as OntologyClass).OwnAttributes);
                var inheritedAttrs = (result.Reference as OntologyClass).InheritedAttributes.Select(i => i.Item1);
                foreach (var inheritedAttr in inheritedAttrs)
                    attrs.Add(inheritedAttr as OntologyNode.Attribute);

                foreach (var attr in attrs)
                {
                    var attrInfo = new NodeInfo.SectionInfo();
                    attrInfo.Data = attr;
                    attrInfo.IsInput = true;
                    attrInfo.IsOutput = true;
                    attrInfo.InputValidation = (s, e) => {
                        var src = e.SourceConnector.Tag as OntologyNode.Attribute;
                        var dst = e.DestConnector.Tag as OntologyNode.Attribute;
                        if (src == null || dst == null) {
                            e.Valid = false;
                            return;
                        }
                        if (src.AttrType != dst.AttrType)
                            e.Valid = false;
                        else if (src.AttrType == OntologyNode.Attribute.AttributeType.TERMIN)
                        {
                            if (src.Theme != dst.Theme)
                                e.Valid = false;
                        }
                    };
                    var attrName = new Label();
                    attrName.Content = attr.Name;
                    attrInfo.UIPanel = attrName;
                    info.Sections.Add(attrInfo);
                }
            }

            return info;
        }

        public static NodeInfo Convert(FactScheme.Functor functor)
        {
            var info = new NodeInfo();
            info.Tag = functor;

            info.NodeNameProperty = functor.ID;
            
            var output = new NodeInfo.SectionInfo();
            output.IsInput = false;
            output.IsOutput = true;
            var outputLabel = new Label();
            outputLabel.Content = "Output";
            output.UIPanel = outputLabel;
            info.Sections.Add(output);

            if (functor.NumArgs > 0)
                for (int i = 0; i < functor.NumArgs; i++)
                {
                    NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                    attrInfo.IsInput = true;
                    attrInfo.IsOutput = false;
                    attrInfo.Data = functor.Inputs[i];
                    info.Sections.Add(attrInfo);
                }
            //else
            //    Button
            return info;
        }

        private static Style typeNameStyle;
        private static Style infoTextStyle;

        private static StackPanel ResultInfoPanel(FactScheme.Result result)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(5);

            ComboBox cb = new ComboBox();
            cb.ItemsSource = Enum.GetValues(typeof(FactScheme.ResultType));
            stackPanel.Children.Add(cb);

            TextBlock editedArgName = new TextBlock();
            editedArgName.Text = result.Reference.Name;
            stackPanel.Children.Add(editedArgName);

            cb.SelectionChanged += (s, e) =>
            {
                if (e.AddedItems.Contains(FactScheme.ResultType.Create))
                {
                    result.Type = ResultType.Create;
                }
                else
                {
                    result.Type = ResultType.Edit;
                }
            };
            cb.SelectedValue = FactScheme.ResultType.Create;

            return stackPanel;
        }

        //connect nodes by hand
        public static void LoadViewFromScheme(NetworkView nv, Scheme scheme)
        {
            var nodes = nv.Nodes;
            foreach (Result res in scheme.Results)
            {
                var dstNode = nodes.First(x => x.Tag == res);
                foreach (Result.Rule rule in res.Rules)
                {
                    var srcNode = nodes.First(x => x.Tag == rule.Reference);
                    var srcConn = srcNode.Connectors.First(x =>
                        x.Tag == rule.InputAttribute &&
                        x.Mode == Connector.ConnectorMode.Output);
                    var dstConn = dstNode.Connectors.First(x => x.Tag == rule.Attribute);
                    nv.AddConnection(srcConn, dstConn, false);
                }
                if (res.EditObject != null)
                {
                    var srcNode = nodes.First(x => x.Tag == res.EditObject);
                    var srcConn = srcNode.Connectors.First(x =>
                        x.Tag == res.EditObject &&
                        x.Mode == Connector.ConnectorMode.Output);
                    var dstConn = dstNode.Connectors.First(x => 
                        x.Tag == res && 
                        x.Mode == Connector.ConnectorMode.Input);
                    nv.AddConnection(srcConn, dstConn, false);
                }
                //add functors and conditions
            }
        }


    }
}
