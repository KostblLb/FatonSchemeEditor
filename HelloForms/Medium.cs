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
using System.Windows.Data;

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
            //if (dst.Tag == null || src.Tag == null)
            //    throw new Exception("connector not attached to attribute");

            if (dst.Tag is FactScheme.Result && src.Tag is ISchemeComponent) //very specific code for linking EDIT connector of a result
            {
                ((Result)dst.Tag).EditObject = src.ParentNode.Tag as ISchemeComponent;
            }
            else if (dst.ParentNode.Tag is FactScheme.Result)
            {
                var dstAttr = dst.Tag as OntologyNode.Attribute;
                var srcAttr = src.Tag as OntologyNode.Attribute;
                var result = dst.ParentNode.Tag as FactScheme.Result;
                if (src.ParentNode.Tag is FactScheme.Argument)
                    result.AddRule(FactScheme.Result.RuleType.ATTR,
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
                var functor = dst.ParentNode.Tag as FactScheme.Functor;
                if (src.Tag is OntologyNode.Attribute)
                    (dst.Tag as FactScheme.Functor.FunctorInput).Set(src.Tag as OntologyNode.Attribute, src.ParentNode.Tag as ISchemeComponent);
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

        #region convertations
        ///convert factscheme argument into nv node
        ///
        public static NodeInfo Convert(FactScheme.Argument argument, Vocabularies.Vocabulary vocabulary = null)
        {
            NodeInfo info = new NodeInfo();

            info.Tag = argument;

            info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Name);
            argument.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Order")
                    info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Name);
                //if (e.PropertyName == "")
            };

            if (argument.ArgType == ArgumentType.IOBJECT)
            {
                foreach (var attr in argument.Attributes)
                {
                    NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                    attrInfo.Data = attr;
                    attrInfo.IsInput = false;
                    attrInfo.IsOutput = true;
                    var attrName = new Label();
                    attrName.Content = attr.Name;
                    attrName.ToolTip = (attr.AttrType == OntologyNode.Attribute.AttributeType.DOMAIN || attr.AttrType == OntologyNode.Attribute.AttributeType.OBJECT) ? attr.Opt : attr.AttrType;

                    attrInfo.UIPanel = attrName;
                    info.Sections.Add(attrInfo);
                }

                info.FillColor = System.Windows.Media.Colors.LightSkyBlue;
            }
            else
            {
                for (int i = 0; i < 2; i++) // $Class and $Value are 2 mandatory attributes
                {
                    var attr = argument.Attributes[i];
                    NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                    attrInfo.Data = attr;
                    attrInfo.IsInput = false;
                    attrInfo.IsOutput = true;
                    var attrName = new Label();
                    attrName.Content = attr.Name;
                    attrName.ToolTip = (attr.AttrType == OntologyNode.Attribute.AttributeType.DOMAIN || attr.AttrType == OntologyNode.Attribute.AttributeType.OBJECT) ? attr.Opt : attr.AttrType;

                    attrInfo.UIPanel = attrName;
                    info.Sections.Add(attrInfo);
                }
                var plusInfo = new NodeInfo.SectionInfo();
                var plusBtn = new Button();
                plusBtn.Content = "+";
                plusBtn.Click += (s, e) =>
                {
                    var newAttr = new OntologyNode.Attribute(OntologyNode.Attribute.AttributeType.STRING, vocabulary.First().Name, true);
                    argument.Attributes.Add(newAttr);
                    info.Sections.Add(TerminVarAttrInfo(argument, newAttr, vocabulary, info));
                };
                plusInfo.UIPanel = plusBtn;
                info.Sections.Add(plusInfo);
                for (int i = 2; i < argument.Attributes.Count; i++)
                    info.Sections.Add(TerminVarAttrInfo(argument, argument.Attributes[i], vocabulary, info));
                info.FillColor = System.Windows.Media.Colors.DeepSkyBlue;
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
                    if (result.Reference != ((Argument)input).Klass)
                        e.Valid = false;
                }
                else if (input is Result)
                {
                    if (result.Reference != ((Result)input).Reference)
                        e.Valid = false;
                }
                else e.Valid = false;
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
                    //attrInfo.IsOutput = true;
                    attrInfo.InputValidation += (s, e) =>
                    {
                        var src = e.SourceConnector.Tag;
                        var dst = e.DestConnector.Tag;
                        if (src == null || dst == null)
                        {
                            e.Valid = false;
                            return;
                        }
                        if (src is Argument && dst is OntologyNode.Attribute)
                        {
                            if (((Argument)src).Klass == null)
                            {
                                e.Valid = false;
                                return;
                            }
                            var parent = ((Argument)src).Klass.FindParent((string)((OntologyClass.Attribute)dst).Opt);
                            e.Valid = parent != null;
                            return;
                        }
                        if (src is OntologyNode.Attribute && dst is OntologyNode.Attribute)
                        {
                            if (((OntologyNode.Attribute)src).AttrType != ((OntologyNode.Attribute)dst).AttrType)
                            {
                                e.Valid = false;
                                return;
                            }
                            var attrType = ((OntologyNode.Attribute)src).AttrType;
                            if (attrType == OntologyNode.Attribute.AttributeType.DOMAIN || attrType == OntologyNode.Attribute.AttributeType.OBJECT)
                                e.Valid = ((OntologyNode.Attribute)src).Opt == ((OntologyNode.Attribute)dst).Opt;
                        }
                    };
                    FrameworkElement attrName;
                    if (attr.AttrType == OntologyNode.Attribute.AttributeType.OBJECT)
                    {
                        attrName = new Label();
                        ((Label)attrName).Content = attr.Name;
                    }
                    else if(attr.AttrType == OntologyNode.Attribute.AttributeType.DOMAIN)
                    {
                        attrName = new Elements.ResultDefaultDomainAttr();
                        var items = new List<ComboBoxItem>();
                        var selectedIndex = 0;
                        if (!Ontology.Ontology.Domains.ContainsKey((string)(attr.Opt)))
                        {
                            var item = new ComboBoxItem();
                            item.Content = "domain doesnt exist";
                            items.Add(item);
                        }

                        else for (int i = 0; i < Ontology.Ontology.Domains[(string)attr.Opt].Count; i++)
                            {
                                var domainValue = Ontology.Ontology.Domains[(string)attr.Opt][i];
                                var item = new ComboBoxItem();
                                item.Content = domainValue;
                                item.Selected += (s, e) => {
                                    if (result.Rules.ContainsKey(attr.Name))
                                        result.Rules[attr.Name].Default = domainValue;
                                    else
                                    {
                                        var rule = new Result.Rule(attr, domainValue);
                                        result.Rules[attr.Name] = rule;
                                    }
                                };
                                items.Add(item);
                                if (result.Rules.ContainsKey(attr.Name) && result.Rules[attr.Name].Default == domainValue) selectedIndex = i;
                            }
                        ((Elements.ResultDefaultDomainAttr)attrName).GetComboBox().ItemsSource = items;
                        ((Elements.ResultDefaultDomainAttr)attrName).GetComboBox().SelectedIndex = selectedIndex;
                        ((Elements.ResultDefaultDomainAttr)attrName).Header = attr.Name;
                        if (result.Rules.ContainsKey(attr.Name))
                        {
                            ((Elements.ResultDefaultDomainAttr)attrName).GetComboBox().SelectedItem = result.Rules[attr.Name].Default;
                        }
                    }
                    else
                    {
                        attrName = new Elements.UserControl1();
                        ((Elements.UserControl1)attrName).Header = attr.Name;
                        if (result.Rules.ContainsKey(attr.Name))
                        {
                            ((Elements.UserControl1)attrName).GetTextBox().Text = result.Rules[attr.Name].Default;
                        }
                        ((Elements.UserControl1)attrName).GetTextBox().TextChanged += (s, e) =>
                        {
                            if (result.Rules.ContainsKey(attr.Name))
                                result.Rules[attr.Name].Default = ((TextBox)s).Text;
                            else
                            {
                                var rule = new Result.Rule(attr, ((TextBox)s).Text);
                                result.Rules[attr.Name] = rule;
                            }
                        };
                    }
                    attrInfo.UIPanel = attrName;
                    info.Sections.Add(attrInfo);
                }
            }

            info.FillColor = System.Windows.Media.Colors.LightSeaGreen;

            return info;
        }

        public static NodeInfo Convert(FactScheme.Functor functor)
        {
            var info = new NodeInfo();
            info.Tag = functor;

            info.NodeNameProperty = functor.Name;

            for (int i = 0; i < functor.Inputs.Count; i++)
            {
                NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                attrInfo.IsInput = true;
                attrInfo.IsOutput = false;
                attrInfo.Data = functor.Inputs[i];
                var l = new Label();
                l.Content = functor.Inputs[i].name;
                attrInfo.UIPanel = l;
                info.Sections.Add(attrInfo);
            }

            var output = new NodeInfo.SectionInfo();
            output.Data = functor.Output;
            output.IsInput = false;
            output.IsOutput = true;
            var outputLabel = new Label();
            outputLabel.Content = "Output";
            output.UIPanel = outputLabel;
            info.Sections.Add(output);

            //else
            //    Button
            return info;
        }


        private static NodeInfo.SectionInfo ConditionArgSection(Argument arg, string text)
        {
            var argInfo = new NodeInfo.SectionInfo();
            var lbl = new Label();
            lbl.Content = text;
            argInfo.UIPanel = lbl;
            argInfo.IsInput = true;
            if (arg != null)
                argInfo.Data = arg;
            return argInfo;
        }
        public static NodeInfo Convert(FactScheme.Condition condition, Dictionary<string, List<string>> gramtab, List<string> segments)
        {
            var info = new NodeInfo();
            info.Tag = condition;

            info.NodeNameProperty = "Условие схемы";

            var argInfo1 = Medium.ConditionArgSection(condition.Arg1, "Arg1");
            argInfo1.InputAdded += (object s, ConnectionEventArgs e) =>
            {
                argInfo1.Data = e.SourceConnector.Tag;
                condition.Arg1 = (FactScheme.Argument)e.SourceConnector.Tag;
            };
            var argInfo2 = Medium.ConditionArgSection(condition.Arg2, "Arg2");
            argInfo2.InputAdded += (object s, ConnectionEventArgs e) =>
            {
                argInfo2.Data = e.SourceConnector.Tag;
                condition.Arg2 = (FactScheme.Argument)e.SourceConnector.Tag;
            };
            info.Sections.Add(argInfo1);
            info.Sections.Add(argInfo2);
            NodeInfo.SectionInfo[] args = { argInfo1, argInfo2 };
            foreach (var arg in args) {
                arg.InputValidation += (object s, ConnectionEventArgs e) => {
                    if (e.SourceConnector.Tag.GetType() != typeof(Argument))
                        e.Valid = false;
                };
            }

            //toggle not\equal button
            var equalSection = new NodeInfo.SectionInfo();
            var equalButton = new Button();
            equalButton.Content = "=";
            equalButton.Click += (s, e) =>
            {
                if (condition.Operation == ConditionOperation.EQ)
                {
                    condition.Operation = ConditionOperation.NEQ;
                    equalButton.Content = "≠";
                }
                else
                {
                    condition.Operation = ConditionOperation.EQ;
                    equalButton.Content = "=";
                }
            };
            equalSection.UIPanel = equalButton;
            if (condition.Operation == ConditionOperation.NEQ)
            {
                equalButton.Content = "≠";
            }
            info.Sections.Add(equalSection);


            var contextSelectionPanels = new StackPanel();
            var contextSelectionSection = new NodeInfo.SectionInfo();

            //type selection
            var typeInfoSection = new NodeInfo.SectionInfo();
            ComboBox typeCb = new ComboBox();
            typeCb.ItemsSource = Enum.GetValues(typeof(ConditionType));
            typeCb.Text = Locale.SCHEME_CONDITION_TYPE_SELECT;
            typeCb.SelectionChanged += (s, e) =>
            {
                var selection = typeCb.SelectedItem;
                foreach (FrameworkElement control in contextSelectionPanels.Children)
                    if (control.Tag.ToString().Equals(selection.ToString()))
                        control.Visibility = Visibility.Visible;
                    else
                        control.Visibility = Visibility.Collapsed;
                condition.Type = (ConditionType)selection;
            };
            typeCb.Margin = new Thickness(0, 3, 0, 3);
            typeInfoSection.UIPanel = typeCb;
            info.Sections.Add(typeInfoSection);

            //position selection
            var posCombo = new ComboBox();
            posCombo.ItemsSource = Enum.GetValues(typeof(ConditionPosition));
            posCombo.SelectionChanged += (s, e) =>
            {
                condition.Data = ((ConditionPosition)posCombo.SelectedItem).ToString();
            };
            posCombo.Tag = ConditionType.POS;
            posCombo.Visibility = Visibility.Collapsed;
            posCombo.SelectedValue = condition.Data;
            contextSelectionPanels.Children.Add(posCombo);

            //contact selection
            var contactCombo = new ComboBox();
            contactCombo.ItemsSource = Enum.GetValues(typeof(ConditionContact));
            contactCombo.SelectionChanged += (s, e) =>
            {
                condition.Data = ((ConditionContact)contactCombo.SelectedItem).ToString();
            };
            contactCombo.Visibility = Visibility.Collapsed;
            contactCombo.Tag = ConditionType.CONTACT;
            contactCombo.SelectedValue = condition.Data;
            contextSelectionPanels.Children.Add(contactCombo);

            //shared segment selection
            var segSelectionPanel = new ComboBox();
            segSelectionPanel.Visibility = Visibility.Collapsed;
            segSelectionPanel.Tag = ConditionType.SEG;
            segSelectionPanel.ItemsSource = segments;
            segSelectionPanel.SelectionChanged += (s, e) =>
            {
                condition.Data = (string)segSelectionPanel.SelectedItem;
            };
            segSelectionPanel.SelectedValue = condition.Data;
            contextSelectionPanels.Children.Add(segSelectionPanel);

            //semantic (attr-to-attr comparison) selection
            var semSelectionPanel = new StackPanel();
            semSelectionPanel.Visibility = Visibility.Collapsed;
            semSelectionPanel.Tag = ConditionType.SEM;

            ComboBox[] semSelectionArgs = new ComboBox[2];
            string[] vals = condition.Data?.Split(';');
            for (int i = 0; i < 2; i++)
            {
                int j = i; //damn closures
                semSelectionArgs[j] = new ComboBox();
                semSelectionArgs[j].DisplayMemberPath = "Name";
                if (args[i].Data != null)
                    semSelectionArgs[i].ItemsSource = ((FactScheme.Argument)args[i].Data).Attributes;
                args[i].PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "Data")
                        semSelectionArgs[j].ItemsSource = ((FactScheme.Argument)args[j].Data).Attributes;
                };
                semSelectionArgs[i].SelectionChanged += (s, e) =>
                {
                    condition.Data = String.Format("{0};{1}",
                        semSelectionArgs[0].SelectedItem?.ToString(),
                        semSelectionArgs[1].SelectedItem?.ToString());
                };
                semSelectionArgs[i].SelectedValue = vals?.Length > i ?
                    vals[i] : null;
                semSelectionPanel.Children.Add(semSelectionArgs[i]);
            }
            contextSelectionPanels.Children.Add(semSelectionPanel);


            //syntactic (actants) selection
            var syntPanel = new StackPanel();
            syntPanel.Visibility = Visibility.Collapsed;
            syntPanel.Tag = ConditionType.SYNT;

            var modelName = new TextBox();
            var actantName = new TextBox();
            string[] syntData = condition.Data?.Split(';');
            string modelNameText = syntData?.Length > 0 ?
                syntData[0] : "ModelName";
            modelName.Text = modelNameText;
            modelName.TextChanged += (s, e) =>
            {
                condition.Data = String.Format("{0};{1}",
                    modelName.Text, actantName.Text);
            };

            string actantNameText = syntData?.Length > 1 ?
                syntData[1] : "ActantName";
            actantName.Text = actantNameText;
            actantName.TextChanged += (s, e) =>
            {
                condition.Data = String.Format("{0};{1}",
                    modelName.Text, actantName.Text);
            };

            syntPanel.Children.Add(modelName);
            syntPanel.Children.Add(actantName);
            contextSelectionPanels.Children.Add(syntPanel);


            //morph(gramtab) coherence selection
            var gramtabCombo = new ComboBox();
            gramtabCombo.Tag = ConditionType.MORH;
            gramtabCombo.Visibility = Visibility.Collapsed;
            gramtabCombo.ItemsSource = gramtab.Keys;
            gramtabCombo.SelectionChanged += (s, e) =>
            {
                condition.Data = (string)gramtabCombo.SelectedItem;
            };
            gramtabCombo.SelectedValue = condition.Data;
            contextSelectionPanels.Children.Add(gramtabCombo);

            contextSelectionSection.UIPanel = contextSelectionPanels;
            info.Sections.Add(contextSelectionSection);

            typeCb.SelectedValue = condition.Type;

            info.FillColor = System.Windows.Media.Colors.Gold;

            return info;
        }
        #endregion convertations

        #region various panels

        static NodeInfo.SectionInfo TerminVarAttrInfo(Argument argument, OntologyNode.Attribute attr, Vocabularies.Vocabulary vocabulary, NodeInfo nodeInfo)
        {
            NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
            attrInfo.Data = attr;
            attrInfo.IsOutput = true;
            var attrName = new TerminAttribute();
            attrName.RemoveAttrButton.Click += (s, e) =>
            {
                Console.WriteLine("removed varattr");
                argument.Attributes.Remove(attr);
                nodeInfo.Sections.Remove(attrInfo);
                // re-render node
            };
            var vocList = vocabulary.ToList();
            attrName.AttrNameComboBox.ItemsSource = vocList;
            attrName.AttrNameComboBox.SelectedIndex = vocList.FindIndex(t => t.Name == attr.Name);
            attrName.AttrNameComboBox.SelectionChanged += (s, e) => {
                attr.Name = e.AddedItems[0].ToString();
            };
            attrName.ToolTip = attr.AttrType;

            attrInfo.UIPanel = attrName;
            return attrInfo;
        }

        private static StackPanel ResultInfoPanel(FactScheme.Result result)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(5);

            ComboBox cb = new ComboBox();
            cb.ItemsSource = Enum.GetValues(typeof(FactScheme.ResultType));
            cb.SelectedValue = result.Type;

            stackPanel.Children.Add(cb);

            TextBlock editedArgName = new TextBlock();
            editedArgName.Text = result.Reference.Name;
            stackPanel.Children.Add(editedArgName);

            cb.SelectionChanged += (s, e) =>
            {
                if (e.AddedItems.Contains(FactScheme.ResultType.CREATE))
                {
                    result.Type = ResultType.CREATE;
                }
                else
                {
                    result.Type = ResultType.EDIT;
                }
            };

            return stackPanel;
        }
        #endregion various panels

        //connect nodes after opening existing project
        public static void LoadViewFromScheme(NetworkView nv, Scheme scheme)
        {
            var nodes = nv.Nodes;
            foreach (Result res in scheme.Results)
            {
                var dstNode = nodes.First(x => x.Tag == res);
                foreach (var rulePair in res.Rules)
                {
                    var rule = rulePair.Value;
                    if (rule.Reference == null) continue;
                    var srcNode = nodes.First(x => x.Tag == rule.Reference);
                    var srcConn = srcNode.Connectors.First(x =>
                        ((rule.Attribute.AttrType == OntologyNode.Attribute.AttributeType.OBJECT &&
                            x.Tag == rule.Reference) ||
                        x.Tag == rule.InputAttribute) &&
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
            }

            foreach(Functor fun in scheme.Functors)
            {
                var dstNode = nodes.First(x => x.Tag == fun);
                foreach(var input in fun.Inputs)
                {
                    var srcNode = nodes.First(x => x.Tag == input.source);
                    var srcConn = srcNode.Connectors.First(x =>
                        (x.Tag == input.value) &&
                        x.Mode == Connector.ConnectorMode.Output);
                    var dstConn = dstNode.Connectors.First(x => x.Tag == input);
                    nv.AddConnection(srcConn, dstConn, false);
                }
            }

            foreach (var condition in scheme.Conditions)
            {
                var dstNode = nodes.First(x => x.Tag == condition);
                Argument[] args = { condition.Arg1, condition.Arg2 };
                foreach (var arg in args)
                {
                    var argNode = nodes.First(x => x.Tag == arg);
                    var srcConn = argNode.HeadConnector;
                    var dstConn = dstNode.Info.Sections.First(x => x.Data == arg).Input;
                    nv.AddConnection(srcConn, dstConn, false);
                }
            }
        }


    }
}
