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
                ColumnDefinition[] cd = new ColumnDefinition[4];
                for (int j = 0; j < cd.Length; j++)
                    cd[j] = new ColumnDefinition();
                cd[0].Width = GridLength.Auto;
                cd[1].Width = new GridLength(16);
                cd[2].Width = new GridLength(32, GridUnitType.Star);
                cd[3].Width = new GridLength(16);
                foreach (var def in cd)
                    grid.ColumnDefinitions.Add(def);

                int i = 0;
                foreach (var condition in arg.Conditions[attr])
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    var wrapPanel = new WrapPanel();

                    var condTypeCombo = new ComboBox();
                    condTypeCombo.ItemsSource = Enum.GetValues(typeof(Argument.ArgumentCondition.ConditionType));
                    int k = i;
                    condTypeCombo.SelectionChanged += (s, e) =>
                    {
                        condition.CondType = (Argument.ArgumentCondition.ConditionType)e.AddedItems[0];
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
                    equalButton.Content = condition.ComparType == Argument.ArgumentCondition.ComparisonType.EQ ? "=" : "≠";
                    equalButton.Click += (s, e) =>
                    {
                        if (condition.ComparType == Argument.ArgumentCondition.ComparisonType.EQ)
                        {
                            condition.ComparType = Argument.ArgumentCondition.ComparisonType.NEQ;
                            equalButton.Content = "≠";
                        }
                        else
                        {
                            condition.ComparType = Argument.ArgumentCondition.ComparisonType.EQ;
                            equalButton.Content = "=";
                        }
                    };
                    Grid.SetColumn(equalButton, 1);
                    Grid.SetRow(equalButton, i);
                    grid.Children.Add(equalButton);
                    //wrapPanel.Children.Add(equalButton);

                    var segCombo = new ComboBox();
                    segCombo.ItemsSource = CurrentProject.Segments.ToList();
                    if (condition.CondType == Argument.ArgumentCondition.ConditionType.SEG)
                        segCombo.SelectedItem = condition.Value;
                    segCombo.SelectionChanged += (s, e) =>
                    {
                        condition.Value = ((ComboBox)s).SelectedItem.ToString();
                    };
                    segCombo.Tag = Argument.ArgumentCondition.ConditionType.SEG;
                    segCombo.Visibility = System.Windows.Visibility.Collapsed;
                    Grid.SetRow(segCombo, i);
                    Grid.SetColumn(segCombo, 2);
                    grid.Children.Add(segCombo);
                    //wrapPanel.Children.Add(segCombo);

                    var morphPanel = new StackPanel();
                    var values = condition.Value.Split(';');
                    var gramTypeCombo = new ComboBox();
                    gramTypeCombo.ItemsSource = CurrentProject.Gramtab.Keys.ToList();
                    gramTypeCombo.SelectedItem =
                        condition.CondType == Argument.ArgumentCondition.ConditionType.MORPH ?
                        values[0] :
                        CurrentProject.Gramtab.First().Key;

                    var gramValueCombo = new ComboBox();
                    gramValueCombo.ItemsSource = CurrentProject.Gramtab.ContainsKey(values[0]) ?
                        CurrentProject.Gramtab[values[0]].ToList() :
                        CurrentProject.Gramtab.First().Value.ToList();
                    gramValueCombo.SelectedItem = values.Length > 1 ?
                        values[1] :
                        gramTypeCombo.Items[0];

                    gramTypeCombo.SelectionChanged += (s, e) =>
                    {
                        gramValueCombo.ItemsSource = CurrentProject.Gramtab[gramTypeCombo.SelectedItem.ToString()].ToList();

                    };
                    gramValueCombo.SelectionChanged += (s, e) =>
                    {
                        condition.Value = String.Format("{0};{1}", gramTypeCombo.SelectedItem, gramValueCombo.SelectedItem);
                    };
                    morphPanel.Children.Add(gramTypeCombo);
                    morphPanel.Children.Add(gramValueCombo);
                    morphPanel.Tag = Argument.ArgumentCondition.ConditionType.MORPH;
                    morphPanel.Visibility = System.Windows.Visibility.Collapsed;
                    Grid.SetRow(morphPanel, i);
                    Grid.SetColumn(morphPanel, 2);
                    grid.Children.Add(morphPanel);
                    //wrapPanel.Children.Add(morphPanel);

                    var semTextBox = new TextBox();
                    semTextBox.Text = condition.Value;
                    semTextBox.TextChanged += (s, e) =>
                    {
                        condition.Value = semTextBox.Text;
                    };
                    semTextBox.Tag = Argument.ArgumentCondition.ConditionType.SEM;
                    Grid.SetColumn(semTextBox, 2);
                    Grid.SetRow(semTextBox, i);
                    grid.Children.Add(semTextBox);
                    //wrapPanel.Children.Add(semTextBox);

                    var removeCondButton = new Button();
                    removeCondButton.Width = 16;
                    removeCondButton.Content = "-";
                    removeCondButton.Click += (s, e) =>
                    {
                        arg.Conditions[attr].Remove(condition);
                        OpenPropsPanelV2(arg);
                    };
                    Grid.SetRow(removeCondButton, i);
                    Grid.SetColumn(removeCondButton, 3);
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