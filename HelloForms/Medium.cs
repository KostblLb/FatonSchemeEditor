﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using network;
using System.Windows;
using System.Windows.Controls;

namespace HelloForms
{
    /// <summary>
    /// Provides two way interactions between fact scheme editor and Network Views.
    /// <para/>Medium knows specifications of fact schemes and how they should look like in a NV.
    /// <para/>Medium is subscribed to certain events raised by NV and its components
    /// </summary>
    public class Medium
    {
        ///convert factscheme argument into nv node
        ///
        public static NodeInfo Convert(FactScheme.Argument argument)  
        {
            NodeInfo info = new NodeInfo();
            info.Attributes = new List<NodeInfo.AttributeInfo>(); 

            info.Header.Name = argument.Name;
            info.Header.InfoPanel = ArgumentInfoPanel(argument);

            var attrs = new List<OntologyNode.Attribute>(argument.Origin.OwnAttributes);
            if(argument.Origin.type == OntologyNode.Type.Class)
            {
                var inheritedAttrs = (argument.Origin as OntologyClass).InheritedAttributes.Select(i => i.Item1);
                foreach(var inheritedAttr in inheritedAttrs)
                    attrs.Add(inheritedAttr as OntologyNode.Attribute); 
            }

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
            info.Attributes = new List<NodeInfo.AttributeInfo>();

            info.Header.Name = result.Name;
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

        private static Style typeNameStyle;
        private static Style infoTextStyle;

        private static FrameworkElement ArgumentInfoPanel(FactScheme.Argument argument)
        {
            WrapPanel wrapPanel = new WrapPanel();
            Label typeName = new Label();
            typeName.Content = argument.Name;
            typeName.Style = typeNameStyle;
            Label infoText = new Label();
            infoText.Content = "arg" + argument.Order;
            infoText.Style = infoTextStyle;
            wrapPanel.Children.Add(infoText);
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

            //Label typeName = new Label();
            //typeName.Content = argument.Name;
            //typeName.Style = typeNameStyle;
            //Label infoText = new Label();
            //infoText.Content = "arg" + argument.Order;
            //infoText.Style = infoTextStyle;
            //wrapPanel.Children.Add(infoText);
            //wrapPanel.Children.Add(typeName);

            return stackPanel;
        }

        public static void NV_NodeAdded(NetworkView nv, Node node)
        {
            foreach(Connector connector in node.Connectors)
            {
                connector.ConnectionAdded += NV_ConnectionAdded;
            }
        }
        //public static void NV_ConnectionAdded(NetworkView nv, )
        public static void NV_ConnectionAdded(object sender, ConnectionAddedEventArgs e)
        {

        }
    }
}
