using System.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using FactScheme;

namespace HelloForms
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private string[] DataSplit(string data)
        {
            var split = data.Split(';');
            if (split == null || split.Length == 0)
                return null;
            return split;
        }
        private void OpenPropsPanelV2(Argument arg)
        {
            var host = new System.Windows.Forms.Integration.ElementHost();
            host.Dock = System.Windows.Forms.DockStyle.Fill;

            var mainPanel = new StackPanel();
            mainPanel.CanVerticallyScroll = true;

            propsPanel.Tag = arg;
            
            foreach (var attr in arg.Attributes)
            {
                var attrStackPanel = new StackPanel();

                var attrName = new Label();
                attrName.Content = attr.Name;
                attrStackPanel.Children.Add(attrName);

                var grid = new Grid();
                ColumnDefinition[] cd = new ColumnDefinition[5];
                for (int j = 0; j < cd.Length; j++)
                    cd[j] = new ColumnDefinition();
                cd[0].Width = GridLength.Auto;
                cd[1].Width = new GridLength(16);
                cd[2].Width = new GridLength(32, GridUnitType.Star);
                cd[3].Width = new GridLength(32, GridUnitType.Star);
                cd[4].Width = new GridLength(16);
                foreach (var def in cd)
                    grid.ColumnDefinitions.Add(def);

                int i = 0;
                if (!arg.Conditions.ContainsKey(attr))
                    arg.Conditions.Add(attr, new List<Argument.ArgumentCondition>());
                foreach (var condition in arg.Conditions[attr])
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    var wrapPanel = new WrapPanel();

                    var condTypeCombo = new ComboBox();
                    condTypeCombo.ItemsSource = attr.Varattr ? new ArgumentConditionType[] { ArgumentConditionType.SEM } : Enum.GetValues(typeof(ArgumentConditionType));
                    int k = i;
                    condTypeCombo.SelectionChanged += (s, e) =>
                    {
                        condition.CondType = (ArgumentConditionType)e.AddedItems[0];
                        foreach (FrameworkElement control in grid.Children)
                        {
                            if (Grid.GetRow(control) != k)
                                continue;
                            if (control.Tag != null && !control.Tag.Equals(condTypeCombo.SelectedItem))
                                control.Visibility = System.Windows.Visibility.Collapsed;
                            else
                                control.Visibility = System.Windows.Visibility.Visible;
                        }
                    };
                    Grid.SetColumn(condTypeCombo, 0);
                    Grid.SetRow(condTypeCombo, i);
                    grid.Children.Add(condTypeCombo);
                    //wrapPanel.Children.Add(condTypeCombo);

                    var equalButton = new Button();
                    equalButton.Width = 16;
                    equalButton.Content = condition.Operation == ArgumentConditionOperation.EQ ? "=" : "≠";
                    equalButton.Click += (s, e) =>
                    {
                        if (condition.Operation == ArgumentConditionOperation.EQ)
                        {
                            condition.Operation = ArgumentConditionOperation.NEQ;
                            equalButton.Content = "≠";
                        }
                        else
                        {
                            condition.Operation = ArgumentConditionOperation.EQ;
                            equalButton.Content = "=";
                        }
                    };
                    Grid.SetColumn(equalButton, 1);
                    Grid.SetRow(equalButton, i);
                    grid.Children.Add(equalButton);
                    //wrapPanel.Children.Add(equalButton);

                    var segCombo = new ComboBox();
                    segCombo.ItemsSource = CurrentProject.Segments.ToList();
                    if (condition.CondType == ArgumentConditionType.SEG)
                        segCombo.SelectedItem = condition.Data;
                    segCombo.SelectionChanged += (s, e) =>
                    {
                        condition.Data = ((ComboBox)s).SelectedItem.ToString();
                    };
                    segCombo.Tag = ArgumentConditionType.SEG;
                    segCombo.Visibility = System.Windows.Visibility.Collapsed;
                    Grid.SetRow(segCombo, i);
                    Grid.SetColumnSpan(segCombo, 2);
                    Grid.SetColumn(segCombo, 2);
                    grid.Children.Add(segCombo);
                    //wrapPanel.Children.Add(segCombo);

                    var morphPanel = new StackPanel();
                    var values = condition.Data.Split(';');
                    var gramTypeCombo = new ComboBox();
                    gramTypeCombo.ItemsSource = CurrentProject.Gramtab.Keys.ToList();
                    var first = new KeyValuePair<string, List<string>>( "", new List<string>());
                    if (CurrentProject.Gramtab.Count > 0)
                        first = CurrentProject.Gramtab.First();
                    gramTypeCombo.SelectedItem =
                        condition.CondType == ArgumentConditionType.MORH ?
                        values[0] :
                        first.Key;

                    var gramValueCombo = new ComboBox();
                    gramValueCombo.ItemsSource = CurrentProject.Gramtab.ContainsKey(values[0]) ?
                        CurrentProject.Gramtab[values[0]].ToList() :
                        first.Value.ToList();
                    gramValueCombo.SelectedItem = values.Length > 1 ?
                        values[1] :
                        first.Key;

                    gramTypeCombo.SelectionChanged += (s, e) =>
                    {
                        gramValueCombo.ItemsSource = CurrentProject.Gramtab[gramTypeCombo.SelectedItem.ToString()].ToList();

                    };
                    gramValueCombo.SelectionChanged += (s, e) =>
                    {
                        condition.Data = String.Format("{0};{1}", gramTypeCombo.SelectedItem, gramValueCombo.SelectedItem);
                    };
                    morphPanel.Children.Add(gramTypeCombo);
                    morphPanel.Children.Add(gramValueCombo);
                    morphPanel.Tag = ArgumentConditionType.MORH;
                    morphPanel.Visibility = System.Windows.Visibility.Collapsed;
                    Grid.SetRow(morphPanel, i);
                    Grid.SetColumn(morphPanel, 2);
                    Grid.SetColumnSpan(morphPanel, 2);
                    grid.Children.Add(morphPanel);
                    //wrapPanel.Children.Add(morphPanel);

                    if (arg.ArgType != ArgumentType.TERMIN || attr.Name != "$Класс")
                    {
                        var semTextBox = new TextBox();
                        semTextBox.Text = condition.Data;
                        semTextBox.TextChanged += (s, e) =>
                        {
                            condition.Data = semTextBox.Text;
                        };
                        semTextBox.Tag = ArgumentConditionType.SEM;
                        Grid.SetColumn(semTextBox, 2);
                        Grid.SetColumnSpan(semTextBox, 2);
                        Grid.SetRow(semTextBox, i);
                        grid.Children.Add(semTextBox);
                    }
                    else
                    {
                        var semComboBox = new ComboBox();
                        var semTextBox = new TextBox();
                        var vocAttrs = new List<Vocabularies.Termin>();
                        var split = DataSplit(condition.Data);
                        foreach (var term in CurrentProject.Dictionary)
                        {
                            vocAttrs.Add(term);
                        }
                        semComboBox.ItemsSource = vocAttrs;
                        semComboBox.DisplayMemberPath = "Name";
                        if (split != null)
                        {
                            semComboBox.SelectedValue = vocAttrs.Find(x => x.Name == split[0]);
                            if (split.Length > 1)
                                semTextBox.Text = (split != null && split.Length > 1) ? split[1] : "";
                        }
                        else
                        {
                            semComboBox.SelectedValue = vocAttrs[0];
                            semTextBox.Text = "";
                        }
                        semComboBox.SelectionChanged += (s, e) =>
                        {
                            var semClassName = ((Vocabularies.Termin)semComboBox.SelectedItem).Name;
                            condition.Data = String.Format("{0};{1}", semClassName, semTextBox.Text);
                        };
                        semComboBox.Tag = ArgumentConditionType.SEM;
                        Grid.SetColumn(semComboBox, 2);
                        Grid.SetRow(semComboBox, i);
                        grid.Children.Add(semComboBox);

                        semTextBox.TextChanged += (s, e) =>
                        {
                            var semClassName = ((Vocabularies.Termin)semComboBox.SelectedItem).Name;
                            condition.Data = String.Format("{0};{1}", semClassName, semTextBox.Text);
                        };
                        semTextBox.Tag = ArgumentConditionType.SEM;
                        Grid.SetColumn(semTextBox, 3);
                        Grid.SetRow(semTextBox, i);
                        grid.Children.Add(semTextBox);

                    }

                    var removeCondButton = new Button();
                    removeCondButton.Width = 16;
                    removeCondButton.Content = "-";
                    removeCondButton.Click += (s, e) =>
                    {
                        arg.Conditions[attr].Remove(condition);
                        OpenPropsPanelV2(arg);
                    };
                    Grid.SetRow(removeCondButton, i);
                    Grid.SetColumn(removeCondButton, 4);
                    grid.Children.Add(removeCondButton);
                    //wrapPanel.Children.Add(removeCondButton);

                    condTypeCombo.SelectedItem = condition.CondType;
                    //attrStackPanel.Children.Add(wrapPanel);
                    i++;
                }
                attrStackPanel.Children.Add(grid);
                var addConditionButton = new Button();
                addConditionButton.Click += (s, e) =>
                {
                    arg.Conditions[attr].Add(new Argument.ArgumentCondition());
                    //replace w/ somthing faster
                    OpenPropsPanelV2(arg);
                };
                addConditionButton.Content = "+";
                attrStackPanel.Children.Add(addConditionButton);
                mainPanel.Children.Add(attrStackPanel);
            }
            propsPanel.Controls.Clear();
            propsPanel.Child = mainPanel;
        }
    }
}