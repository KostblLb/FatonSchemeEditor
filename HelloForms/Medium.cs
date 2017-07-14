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
using VocabularyExtractor;
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

        #region convertations
        ///convert factscheme argument into nv node
        ///
        public static NodeInfo Convert(FactScheme.Argument argument)
        {
            NodeInfo info = new NodeInfo();

            info.Tag = argument;

            info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Name);
            argument.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Order")
                    info.NodeNameProperty = string.Format("arg{0} {1}", argument.Order, argument.Name);
            };

            foreach (var attr in argument.Attributes)
            {
                NodeInfo.SectionInfo attrInfo = new NodeInfo.SectionInfo();
                attrInfo.Data = attr;
                attrInfo.IsInput = false;
                attrInfo.IsOutput = true;
                var attrName = new Label();
                if (argument.ArgType == Argument.ArgumentType.IOBJECT)
                    attrName.Content = attr.Name;
                else
                    attrName.Content = "Значение";
                attrName.ToolTip = attr.AttrType;

                //WrapPanel panel = new WrapPanel();
                //Button attrSetup = new Button();
                //attrSetup.Height = 16;
                //attrSetup.Width = 16;
                //attrSetup.Content = "::";
                //attrSetup.ToolTip = "Изменить ограничения атрибута";
                //attrSetup.HorizontalAlignment = HorizontalAlignment.Right;
                //attrSetup.Click += (s, e) =>
                //{
                //    Medium.onAttributeSetup(attr, argument);
                //};

                //panel.Children.Add(attrName);
                //panel.Children.Add(attrSetup);
                attrInfo.UIPanel = attrName;
                info.Sections.Add(attrInfo);
            }

            info.FillColor = System.Windows.Media.Colors.LightSkyBlue;

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
                    attrInfo.InputValidation = (s, e) =>
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
                            var parent = ((Argument)src).Klass.FindParent((string)((OntologyClass.Attribute)dst).Opt);
                            e.Valid = parent != null;
                            return;
                        }
                        if (src is OntologyNode.Attribute && dst is OntologyNode.Attribute)
                            e.Valid = ((OntologyNode.Attribute)src).AttrType == ((OntologyNode.Attribute)dst).AttrType;
                    };
                    var attrName = new Label();
                    attrName.Content = attr.Name;
                    attrName.ToolTip = attr.AttrType;
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

        public static NodeInfo Convert(FactScheme.Condition condition, Dictionary<string, List<string>> gramtab, List<string> segments)
        {
            var info = new NodeInfo();
            info.Tag = condition;

            info.NodeNameProperty = "Условие схемы";

            NodeInfo.SectionInfo[] args = { new NodeInfo.SectionInfo(), new NodeInfo.SectionInfo() };
            var arg1 = args[0];
            var arg2 = args[1];
            for (int i = 0; i<2; i++)
            {
                int j = i; //damn closures
                var arg = args[i];
                var lbl = new Label();
                lbl.Content = String.Format("Arg {0}", i+1);
                arg.UIPanel = lbl;
                arg.IsInput = true;
                arg.InputAdded += (object s, ConnectionEventArgs e) => {
                    arg.Data = e.SourceConnector.Tag;
                    condition.Args[j] = (FactScheme.Argument)e.SourceConnector.Tag;
                };
                if (condition.Args[i] != null)
                    arg.Data = condition.Args[i];
                info.Sections.Add(arg);
            }

            //toggle not\equal button
            var equalSection = new NodeInfo.SectionInfo();
            var equalButton = new Button();
            equalButton.Content = "=";
            equalButton.Click += (s, e) =>
            {
                if (condition.ComparType == FactScheme.Condition.ComparisonType.EQ)
                {
                    condition.ComparType = FactScheme.Condition.ComparisonType.NEQ;
                    equalButton.Content = "≠";
                }
                else
                {
                    condition.ComparType = FactScheme.Condition.ComparisonType.EQ;
                    equalButton.Content = "=";
                }
            };
            equalSection.UIPanel = equalButton;
            if (condition.ComparType == FactScheme.Condition.ComparisonType.NEQ)
            {
                condition.ComparType = FactScheme.Condition.ComparisonType.EQ;
                equalButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            info.Sections.Add(equalSection);

            
            var contextSelectionPanels = new StackPanel();
            var contextSelectionSection = new NodeInfo.SectionInfo();

            //type selection
            var typeInfoSection = new NodeInfo.SectionInfo();
            ComboBox typeCb = new ComboBox();
            typeCb.ItemsSource = Enum.GetValues(typeof(FactScheme.Condition.ConditionType));
            typeCb.Text = Locale.SCHEME_CONDITION_TYPE_SELECT;
            typeCb.SelectionChanged += (s, e) =>
            {
                var selection = typeCb.SelectedItem;
                foreach (FrameworkElement control in contextSelectionPanels.Children)
                    if (control.Tag.ToString().Equals(selection.ToString()))
                        control.Visibility = Visibility.Visible;
                    else
                        control.Visibility = Visibility.Collapsed;
                condition.Type = (FactScheme.Condition.ConditionType)selection;
            };
            typeCb.Margin = new Thickness(0, 3, 0, 3);
            typeInfoSection.UIPanel = typeCb;
            info.Sections.Add(typeInfoSection);

            //position selection
            var posCombo = new ComboBox();
            posCombo.ItemsSource = Enum.GetValues(typeof(FactScheme.Condition.ConditionPosition));
            posCombo.SelectionChanged += (s, e) => {
                condition.Position = (FactScheme.Condition.ConditionPosition)posCombo.SelectedItem;
            };
            posCombo.Tag = FactScheme.Condition.ConditionType.POS;
            posCombo.Visibility = Visibility.Collapsed;
            posCombo.SelectedValue = condition.Position;
            contextSelectionPanels.Children.Add(posCombo);

            //contact selection
            var contactCombo = new ComboBox();
            contactCombo.ItemsSource = Enum.GetValues(typeof(FactScheme.Condition.ConditionContact));
            contactCombo.SelectionChanged += (s, e) =>
            {
                condition.Contact = (FactScheme.Condition.ConditionContact)contactCombo.SelectedItem;
            };
            contactCombo.Visibility = Visibility.Collapsed;
            contactCombo.Tag = FactScheme.Condition.ConditionType.CONTACT;
            contactCombo.SelectedValue = condition.Contact;
            contextSelectionPanels.Children.Add(contactCombo);

            //shared segment selection
            var segSelectionPanel = new ComboBox();
            segSelectionPanel.Visibility = Visibility.Collapsed;
            segSelectionPanel.Tag = FactScheme.Condition.ConditionType.SEG;
            segSelectionPanel.ItemsSource = segments;
            segSelectionPanel.SelectionChanged += (s, e) =>
            {
                condition.Segment = segSelectionPanel.SelectedItem.ToString();
            };
            segSelectionPanel.SelectedValue = condition.Segment;
            contextSelectionPanels.Children.Add(segSelectionPanel);

            //semantic (attr-to-attr comparison) selection
            var semSelectionPanel = new StackPanel();
            semSelectionPanel.Visibility = Visibility.Collapsed;
            semSelectionPanel.Tag = FactScheme.Condition.ConditionType.SEM;

            ComboBox[] semSelectionArgs = new ComboBox[2];
            for(int i = 0; i<2; i++)
            {
                int j = i; //damn closures
                semSelectionArgs[j] = new ComboBox();
                semSelectionArgs[j].DisplayMemberPath = "Name";
                args[i].PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "Data")
                        semSelectionArgs[j].ItemsSource = ((FactScheme.Argument)args[j].Data).Attributes;
                };
                semSelectionArgs[i].SelectionChanged += (s, e) =>
                {
                    condition.SemAttrs[j] = (OntologyNode.Attribute)semSelectionArgs[j].SelectedItem;
                };
                semSelectionArgs[i].SelectedValue = condition.SemAttrs[i];
                semSelectionPanel.Children.Add(semSelectionArgs[i]);
            }
            contextSelectionPanels.Children.Add(semSelectionPanel);


            //syntactic (actants) selection
            var syntPanel = new StackPanel();
            syntPanel.Visibility = Visibility.Collapsed;
            syntPanel.Tag = FactScheme.Condition.ConditionType.SYNT;
            for (int i = 0; i<2; i++)
            {
                int j = i; //damn closures again
                
                string text = condition.ActantNames[j] ?? String.Format("Arg{0}{1}", i + 1, Locale.SCHEME_CONDITION_ACTANT_NAME_DEFAULT);
                var textBox = new TextBox();
                textBox.Text = text;
                textBox.TextChanged += (s, e) =>
                {
                    condition.ActantNames[j] = textBox.Text;
                };
                syntPanel.Children.Add(textBox);
            }
            contextSelectionPanels.Children.Add(syntPanel);


            //morph(gramtab) coherence selection
            var gramtabCombo = new ComboBox();
            gramtabCombo.Tag = FactScheme.Condition.ConditionType.MORPH;
            gramtabCombo.Visibility = Visibility.Collapsed;
            gramtabCombo.ItemsSource = gramtab.Keys;
            gramtabCombo.SelectionChanged += (s, e) =>
            {
                condition.MorphAttr = gramtabCombo.SelectedItem.ToString();
            };
            gramtabCombo.SelectedValue = condition.MorphAttr;
            contextSelectionPanels.Children.Add(gramtabCombo);

            contextSelectionSection.UIPanel = contextSelectionPanels;
            info.Sections.Add(contextSelectionSection);

            typeCb.SelectedValue = condition.Type;

            info.FillColor = System.Windows.Media.Colors.Gold;

            return info;
        }
        #endregion convertations

        #region various panels
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
        #endregion various panels

        //connect nodes after opening existing project
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
                        ( (rule.Attribute.AttrType == OntologyNode.Attribute.AttributeType.OBJECT &&
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
            
            foreach (var condition in scheme.Conditions)
            {
                var dstNode = nodes.First(x => x.Tag == condition);
                foreach (var arg in condition.Args)
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
