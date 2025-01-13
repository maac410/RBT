using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BT
{
    public partial class SimpleForm : Window
    {
        private HashSet<string> selectedCategories = new HashSet<string>();
        private List<string> categories;
        private List<string> parameters;
        private HashSet<string> processedCategories;

        int newRowIndex = 0;
        int newRowIndex2 = 0;

        public SimpleForm(List<string> categories, List<string> parameters, HashSet<string> processedCategories)
        {
            InitializeComponent();
            this.categories = categories;
            this.parameters = parameters;
            this.processedCategories = processedCategories;

            DisplayInfo();
        }

        private void DisplayInfo()
        {
            // Dynamically add rows if necessary
            for (int i = grid.RowDefinitions.Count; i < categories.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            // Create ListBoxes for each category
            foreach (string category in categories)
            {
                // Create the ListBox for this category
                ListBox newListBox = new ListBox
                {
                    Margin = new Thickness(5),
                    MinHeight = 20,
                    MinWidth = 200
                };

                // Add the checkbox for the category
                var checkBox = new CheckBox
                {
                    Content = category,
                    Margin = new Thickness(5),
                    IsChecked = processedCategories.Contains(category)
                };
                newListBox.Items.Add(checkBox);

                // Calculate the next available row index
                newRowIndex = grid.RowDefinitions.Count - categories.Count + categories.IndexOf(category);

                // Set row and column for the ListBox
                System.Windows.Controls.Grid.SetRow(newListBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(newListBox, 1);

                grid.Children.Add(newListBox);

                // Subscribe to checkbox events
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Add(category);

                // Create the ListBox to hold parameters
                ListBox newListBox = new ListBox
                {
                    MinHeight = 10,
                    MaxHeight = 200,
                    MinWidth = 200,
                    Width = Double.NaN,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                // Helper function to create buttons
                Button CreateButton(string name, string content)
                {
                    return new Button
                    {
                        Name = name,
                        Content = content,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = 20,
                        Height = 20,
                        Margin = new Thickness(5)
                    };
                }

                // Create a StackPanel for buttons
                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Add buttons to StackPanel
                buttonPanel.Children.Add(CreateButton("Up", "Up"));
                buttonPanel.Children.Add(CreateButton("Down", "Down"));
                buttonPanel.Children.Add(CreateButton("Minus", "-"));
                buttonPanel.Children.Add(CreateButton("Plus", "+"));

                // Add buttons StackPanel to the grid
                System.Windows.Controls.Grid.SetRow(buttonPanel, newRowIndex2);
                System.Windows.Controls.Grid.SetColumn(buttonPanel, 2);
                grid.Children.Add(buttonPanel);

                // Add parameters to ListBox
                foreach (var parameter in parameters)
                {
                    newListBox.Items.Add(parameter);
                }

                // Add ListBox to grid
                System.Windows.Controls.Grid.SetRow(newListBox, newRowIndex2);
                System.Windows.Controls.Grid.SetColumn(newListBox, 1);
                grid.Children.Add(newListBox);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Remove(category);  // This stores the selected category in a list.
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
