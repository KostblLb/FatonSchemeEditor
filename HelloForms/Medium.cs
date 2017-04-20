﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using network;
using System.Windows;
using System.Windows.Controls;
using FactScheme;
using Ontology;

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

            if (dst.ParentNode.Tag is FactScheme.Result)
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

            if (dst.ParentNode.Tag is FactScheme.Functor)
            {
                var functor = dst.ParentNode.Tag as FactScheme.Functor;
                functor.SetInput(src.Tag, 
                    src.ParentNode.Tag, 
                    dst.Tag as FactScheme.Functor.FunctorInput);
            }


        }

        ///convert factscheme argument into nv node
        ///
        public static NodeInfo Convert(FactScheme.Argument argument)  
        {
            NodeInfo info = new NodeInfo();

            info.Tag = argument;

            info.Attributes = new List<NodeInfo.AttributeInfo>();

            info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Klass.Name);
            argument.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Order")
                    info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Klass.Name);
            };

            //info.Header.InfoPanel = ArgumentInfoPanel(argument);

            var attrs = new List<OntologyNode.Attribute>(argument.Klass.OwnAttributes);

            var inheritedAttrs = argument.Klass.InheritedAttributes.Select(i => i.Item1);
            foreach(var inheritedAttr in inheritedAttrs)
                attrs.Add(inheritedAttr as OntologyNode.Attribute); 

            foreach(var attr in attrs)
            {
                NodeInfo.AttributeInfo attrInfo = new NodeInfo.AttributeInfo();
                attrInfo.Data = attr;
                attrInfo.IsInput = false;
                attrInfo.IsOutput = true;
                var attrName = new Label();
                attrName.Content = attr.Name;
                attrInfo.AttributePanel = attrName;
                info.Attributes.Add(attrInfo);
            }

            return info;
        }

        public static NodeInfo Convert(FactScheme.Result result)
        {
            NodeInfo info = new NodeInfo();

            info.Tag = result;

            info.Attributes = new List<NodeInfo.AttributeInfo>();

            info.Header.NameChangeable = true;
            info.NodeNameProperty = result.Name;
            info.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NodeNameProperty")
                    result.Name = (sender as NodeInfo).NodeNameProperty;
            };

            info.Header.InfoPanel = ResultInfoPanel(result);

            if (result.Reference is OntologyClass)
            {
                var attrs = new List<OntologyNode.Attribute>((result.Reference as OntologyClass).OwnAttributes);
                //if (argument.Origin.type == OntologyNode.Type.OntologyClass)
                //{
                    var inheritedAttrs = (result.Reference as OntologyClass).InheritedAttributes.Select(i => i.Item1);
                    foreach (var inheritedAttr in inheritedAttrs)
                        attrs.Add(inheritedAttr as OntologyNode.Attribute);
                //}

                foreach (var attr in attrs)
                {
                    NodeInfo.AttributeInfo attrInfo = new NodeInfo.AttributeInfo();
                    attrInfo.Data = attr;
                    attrInfo.IsInput = true;
                    attrInfo.IsOutput = true;
                    var attrName = new Label();
                    attrName.Content = attr.Name;
                    attrInfo.AttributePanel = attrName;
                    info.Attributes.Add(attrInfo);
                }
            }

            return info;
        }

        public static NodeInfo Convert(FactScheme.Functor functor)
        {
            var info = new NodeInfo();
            info.Tag = functor;

            info.NodeNameProperty = functor.ID;

            info.Attributes = new List<NodeInfo.AttributeInfo>();
            NodeInfo.AttributeInfo output = new NodeInfo.AttributeInfo();
            output.IsInput = false;
            output.IsOutput = true;
            var outputLabel = new Label();
            outputLabel.Content = "Output";
            output.AttributePanel = outputLabel;
            info.Attributes.Add(output);

            if (functor.NumArgs > 0) 
                for (int i = 0; i < functor.NumArgs; i++)
                {
                    NodeInfo.AttributeInfo attrInfo = new NodeInfo.AttributeInfo();
                    attrInfo.IsInput = true;
                    attrInfo.IsOutput = false;
                    attrInfo.Data = functor.Inputs[i];
                    //var attrName = new Label();
                    //attrName.Content = attr.Name;
                    //attrInfo.AttributePanel = attrName;
                    info.Attributes.Add(attrInfo);
                }
            //else
            //    Button
            return info; 
        }

        private static Style typeNameStyle;
        private static Style infoTextStyle;

        private static FrameworkElement ArgumentInfoPanel(FactScheme.Argument argument)
        {
            WrapPanel wrapPanel = new WrapPanel();
            Label typeName = new Label();
            typeName.Content = argument.Klass.Name;
            typeName.Style = typeNameStyle;
            //Label infoText = new Label();
            //infoText.DataContext = argument;
            //infoText.SetBinding(Label.ContentProperty, "Order");
            //infoText.Style = infoTextStyle;
            //infoText.
            //wrapPanel.Children.Add(infoText);
            wrapPanel.Children.Add(typeName);

            return wrapPanel;
        }

        private static FrameworkElement ResultInfoPanel(FactScheme.Result result)
        {
            StackPanel stackPanel = new StackPanel();

            ComboBox cb = new ComboBox();
            cb.ItemsSource = Enum.GetValues(typeof(FactScheme.ResultType));
            stackPanel.Children.Add(cb);

            WrapPanel wrapPanel = new WrapPanel();
            Connector editedArgConnector = new Connector(Connector.ConnectorMode.Input, null);
            editedArgConnector.Name = "_EDITED_ARGUMENT";
            Label editedArgName = new Label();
            editedArgName.Content = "";
            wrapPanel.Children.Add(editedArgConnector);
            wrapPanel.Children.Add(editedArgName);
            stackPanel.Children.Add(wrapPanel);

            cb.SelectionChanged += (s, e) =>
            {
                if (e.AddedItems.Contains(FactScheme.ResultType.Create))
                    wrapPanel.Visibility = Visibility.Collapsed;
                else
                    wrapPanel.Visibility = Visibility.Visible;
            };
            cb.SelectedValue = FactScheme.ResultType.Create;

            return stackPanel;
        }


        public static void NV_NodeAdded(NetworkView nv, Node node)
        {
            foreach(Connector connector in node.Connectors)
            {
                connector.ConnectionAdded += NV_ConnectionAdded;
            }
        }

        public static void NV_NodeRemoving(NetworkView nv, Node node)
        {

        }

        //public static void NV_ConnectionAdded(NetworkView nv, )
        public static void NV_ConnectionAdded(object sender, ConnectionAddedEventArgs e)
        {

        }


        //connect nodes by hand
        public static void LoadViewFromScheme(NetworkView nv, Scheme scheme)
        {
            var nodes = nv.Nodes;
            foreach(Result res in scheme.Results)
            {
                var dstNode = nodes.First(x => x.Tag == res);
                foreach(Result.Rule rule in res.Rules)
                {
                    var srcNode = nodes.First(x => x.Tag == rule.Reference);
                    var srcConn = srcNode.Connectors.First(x => 
                        x.Tag == rule.InputAttribute &&
                        x.Mode == Connector.ConnectorMode.Output);
                    var dstConn = dstNode.Connectors.First(x => x.Tag == rule.Attribute);
                    nv.AddConnection(srcConn, dstConn, false);
                }
                //add functors and conditions
            }
        }

        
    }
}
