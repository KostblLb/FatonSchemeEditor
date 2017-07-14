using System.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FactScheme;

namespace HelloForms
{
    public partial class MainWindow : Form
    {
        private void OpenPropsPanel(Argument arg)
        {
            propsPanel.Tag = arg;
            var mainPanel = new FlowLayoutPanel();
            mainPanel.AutoSize = true;
            mainPanel.FlowDirection = FlowDirection.TopDown;

            foreach (var attr in arg.Attributes)
            {
                var attrStackPanel = new FlowLayoutPanel();
                attrStackPanel.FlowDirection = FlowDirection.TopDown;
                attrStackPanel.BorderStyle = BorderStyle.Fixed3D;
                attrStackPanel.AutoSize = true;

                var attrName = new Label();
                attrName.Text = attr.Name;
                attrStackPanel.Controls.Add(attrName);
                foreach (var condition in arg.Conditions[attr])
                {
                    var wrapPanel = new FlowLayoutPanel();
                    wrapPanel.AutoSize = true;
                    wrapPanel.BorderStyle = BorderStyle.Fixed3D;

                    var condTypeCombo = new ComboBox();
                    condTypeCombo.DataSource = Enum.GetValues(typeof(Argument.ArgumentCondition.ConditionType));
                    condTypeCombo.BindingContext = new BindingContext(); //hack!
                    condTypeCombo.SelectedValueChanged += (s, e) =>
                    {
                        condition.CondType = (Argument.ArgumentCondition.ConditionType)condTypeCombo.SelectedItem;
                        foreach (Control control in wrapPanel.Controls)
                        {
                            if (control.Tag != null && !control.Tag.Equals(condTypeCombo.SelectedItem))
                                control.Visible = false;
                            else
                                control.Visible = true;
                        }
                    };
                    wrapPanel.Controls.Add(condTypeCombo);

                    var equalButton = new Button();
                    equalButton.Width = 16;
                    equalButton.Text = condition.ComparType == Argument.ArgumentCondition.ComparisonType.EQ ? "=" : "≠";
                    equalButton.Click += (s, e) =>
                    {
                        if (condition.ComparType == Argument.ArgumentCondition.ComparisonType.EQ)
                        {
                            condition.ComparType = Argument.ArgumentCondition.ComparisonType.NEQ;
                            equalButton.Text = "≠";
                        }
                        else
                        {
                            condition.ComparType = Argument.ArgumentCondition.ComparisonType.EQ;
                            equalButton.Text = "=";
                        }
                    };
                    wrapPanel.Controls.Add(equalButton);

                    var segCombo = new ComboBox();
                    segCombo.DataSource = CurrentProject.Segments.ToList();
                    segCombo.BindingContext = new BindingContext();
                    if (condition.CondType == Argument.ArgumentCondition.ConditionType.SEG)
                        segCombo.SelectedItem = condition.Value;
                    segCombo.SelectionChangeCommitted += (s, e) =>
                    {
                        condition.Value = ((ComboBox)s).SelectedItem.ToString();
                    };
                    segCombo.Tag = Argument.ArgumentCondition.ConditionType.SEG;
                    segCombo.Visible = false;
                    wrapPanel.Controls.Add(segCombo);

                    var morphPanel = new FlowLayoutPanel();
                    var values = condition.Value.Split(';');
                    morphPanel.AutoSize = true;
                    morphPanel.FlowDirection = FlowDirection.TopDown;
                    var gramTypeCombo = new ComboBox();
                    gramTypeCombo.DataSource = CurrentProject.Gramtab.Keys.ToList();
                    gramTypeCombo.BindingContext = new BindingContext();
                    gramTypeCombo.SelectedItem =
                        condition.CondType == Argument.ArgumentCondition.ConditionType.MORPH ?
                        values[0] :
                        CurrentProject.Gramtab.First().Key;

                    var gramValueCombo = new ComboBox();
                    gramValueCombo.DataSource = CurrentProject.Gramtab.ContainsKey(values[0]) ?
                        CurrentProject.Gramtab[values[0]].ToList() :
                        CurrentProject.Gramtab.First().Value.ToList();
                    gramValueCombo.BindingContext = new BindingContext();
                    gramValueCombo.SelectedItem = values.Length > 1 ? 
                        values[1] : 
                        gramTypeCombo.Items[0];

                    gramTypeCombo.SelectedValueChanged += (s, e) =>
                    {
                        gramValueCombo.DataSource = CurrentProject.Gramtab[gramTypeCombo.SelectedItem.ToString()].ToList();
                        gramValueCombo.BindingContext = new BindingContext();
                    };
                    gramValueCombo.SelectedValueChanged += (s, e) =>
                    {
                        condition.Value = String.Format("{0};{1}", gramTypeCombo.SelectedItem, gramValueCombo.SelectedItem);
                    };
                    morphPanel.Controls.Add(gramTypeCombo);
                    morphPanel.Controls.Add(gramValueCombo);
                    morphPanel.Tag = Argument.ArgumentCondition.ConditionType.MORPH;
                    morphPanel.Visible = false;
                    wrapPanel.Controls.Add(morphPanel);

                    var semCombo = new ComboBox();
                    semCombo.DataSource = new List<string>();
                    semCombo.Tag = Argument.ArgumentCondition.ConditionType.SEM;
                    //semCombo.Visible = false;
                    wrapPanel.Controls.Add(semCombo);

                    var removeCondButton = new Button();
                    removeCondButton.Width = 16;
                    removeCondButton.Text = "-";
                    removeCondButton.Click += (s, e) =>
                    {
                        arg.Conditions[attr].Remove(condition);
                        OpenPropsPanel(arg);
                    };
                    wrapPanel.Controls.Add(removeCondButton);

                    condTypeCombo.SelectedItem = condition.CondType;
                    attrStackPanel.Controls.Add(wrapPanel);
                }
                var addConditionButton = new Button();
                addConditionButton.Click += (s, e) =>
                {
                    arg.Conditions[attr].Add(new Argument.ArgumentCondition());
                    //replace w/ somthing faster
                    OpenPropsPanel(arg);
                };
                addConditionButton.Text = "+";
                attrStackPanel.Controls.Add(addConditionButton);
                mainPanel.Controls.Add(attrStackPanel);
            }
            propsPanel.Controls.Clear();
            propsPanel.Controls.Add(mainPanel);
        }
    }
}