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
            if (categories.Count > 0)
            {
                // Dynamically add rows based on categories count
                for (int i = grid.RowDefinitions.Count; i < categories.Count; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });  // Adding rows dynamically
                }
                // Loop through each category and create a ListBox
                foreach (string category in categories)
                {
                    // Create a new ListBox for each category
                    ListBox newListBox = new ListBox
                    {
                        Margin = new Thickness(5), // Optional margin for spacing
                        MinHeight = 20,
                        MinWidth = 200
                    };

                    // Create a checkbox for the category
                    var checkBox = new CheckBox
                    {
                        Content = category,
                        Margin = new Thickness(5),
                        IsChecked = processedCategories.Contains(category)
                    };

                    // Add the checkbox to the newly created ListBox
                    newListBox.Items.Add(checkBox);

                    // Get the next available row index for placing the ListBox
                    int newRowIndex = grid.RowDefinitions.Count - categories.Count + categories.IndexOf(category);

                    // Set the row and column for the ListBox
                    System.Windows.Controls.Grid.SetRow(newListBox, newRowIndex);
                    System.Windows.Controls.Grid.SetColumn(newListBox, 1);  // Place it in column 1 (you can change the column as needed)

                    // Add the ListBox to the Grid
                    grid.Children.Add(newListBox);

                    // Subscribe to events for the checkbox
                    checkBox.Checked += CheckBox_Checked;
                    checkBox.Unchecked += CheckBox_Unchecked;
                
            }
        }

    }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Add(category);  // Remove the category from the selected list

                // Create a new ListBox dynamically if it doesn't exist yet
                ListBox newListBox = new ListBox
                {
                    MinHeight = 10,
                    MaxHeight = 200,
                    MinWidth = 200,
                    Width = Double.NaN,  // Auto is equivalent to NaN for width in C#
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                // Helper method to create a button with common properties
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
                        Margin = new Thickness(5) // Adds spacing between buttons
                    };
                }

                // Create a StackPanel to hold the buttons
                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical, // Stack buttons vertically
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Create and add buttons to the StackPanel using the helper method
                buttonPanel.Children.Add(CreateButton("Up", "Up"));
                buttonPanel.Children.Add(CreateButton("Down", "Down"));
                buttonPanel.Children.Add(CreateButton("Minus", "-"));
                buttonPanel.Children.Add(CreateButton("Plus", "+"));

                // Add the StackPanel to your grid or container
                grid.Children.Add(buttonPanel);

                // Dynamically add a new row to the grid
                int newRowIndex = grid.RowDefinitions.Count;  // Get the next available row index
                grid.RowDefinitions.Add(new RowDefinition());  // Add a new row to the Grid

                // Set the row index for the StackPanel if needed
                System.Windows.Controls.Grid.SetRow(buttonPanel, newRowIndex);  // Assign the StackPanel to the new row
                System.Windows.Controls.Grid.SetColumn(buttonPanel, 2);  // Place it in column 1, or change as needed


                // Add parameters to the ListBox
                foreach (var parameter in parameters)
                {
                    newListBox.Items.Add(parameter);
                }

                // Add the new ListBox to the Grid at the new row index
                System.Windows.Controls.Grid.SetRow(newListBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(newListBox, 1);  // Place it in column 1, or change as needed
                grid.Children.Add(newListBox);  // Add the ListBox to the Grid

                // Optionally, if you have other specific logic for adding another ListBox:
                ListBox newListBox2 = new ListBox
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    MinWidth = 200,
                    Width = Double.NaN,  // Auto is equivalent to NaN for width in C#
                    MinHeight = 200,
                    Margin = new Thickness(5) // Optional, for spacing
                };

                // Add this second ListBox to the grid with different row/column or similar positioning
                System.Windows.Controls.Grid.SetRow(newListBox2, newRowIndex);  // Place it on the next row or use your logic
                System.Windows.Controls.Grid.SetColumn(newListBox2, 3);  // Same or different column as needed
                grid.Children.Add(newListBox2);  // Add the second ListBox to the Grid
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
