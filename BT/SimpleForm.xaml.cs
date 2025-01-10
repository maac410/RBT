using Autodesk.Revit.DB;
using DocumentFormat.OpenXml.Spreadsheet;
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

        // Constructor accepting categories, parameters, and processedCategories
        public SimpleForm(List<string> categories, List<string> parameters, HashSet<string> processedCategories)
        {
            InitializeComponent();
            this.categories = categories;
            this.parameters = parameters;
            this.processedCategories = processedCategories;

            // Populate the UI (TextBlocks and ListBox)
            DisplayInfo();
        }

        // Display the information in the UI
        private void DisplayInfo()
        {
            Rotulo.Text = "Rotulo";
            Views.Text = "Views to Create";
            infoTextBlock1.Text = "Tables to be created";

            if (categories.Count > 0)
            {
                string category = categories[0]; // Assuming you only want to display one category

                // Create a single checkbox for the category
                var checkBox = new CheckBox
                {
                    Content = category,
                    Margin = new Thickness(5),
                    IsChecked = processedCategories.Contains(category)
                };

                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;

                // Add the checkbox to the ListBox (mBox)
                mBox.Items.Add(checkBox);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Remove(category);  // Remove the category from the selected list

                // Create a new ListBox dynamically if it doesn't exist yet
                ListBox newListBox = new ListBox
                {
                    MinHeight = 10,
                    MaxHeight = 200,
                    MinWidth = 200
                };

                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal, // Use Horizontal for a row layout
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                // Create buttons
                Button newButtonUp = new Button
                {
                    Name = "Up",
                    Content = "?",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5) // Adds spacing between buttons
                };

                Button newButtonDown = new Button
                {
                    Name = "Down",
                    Content = "?",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5) // Adds spacing between buttons
                };

                Button newButtonMinus = new Button
                {
                    Name = "Minus",
                    Content = "-",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5) // Adds spacing between buttons
                };

                Button newButtonPlus = new Button
                {
                    Name = "Plus",
                    Content = "+",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5) // Adds spacing between buttons
                };

                // Add buttons to the panel
                buttonPanel.Children.Add(newButtonUp);
                buttonPanel.Children.Add(newButtonDown);
                buttonPanel.Children.Add(newButtonMinus);
                buttonPanel.Children.Add(newButtonPlus);

                // Create a new row in the Grid dynamically
                int newRowIndex = grid.RowDefinitions.Count;  // Get the next available row index
                grid.RowDefinitions.Add(new RowDefinition());  // Add a new row to the Grid

                // Add the StackPanel to the Grid (if you want to place buttons on the grid)
                System.Windows.Controls.Grid.SetRow(buttonPanel, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(buttonPanel, 0); // You can change this if needed
                grid.Children.Add(buttonPanel);

                // Add parameters to the ListBox
                foreach (var parameter in parameters)
                {
                    newListBox.Items.Add(parameter);
                }

                // Add the new ListBox to the Grid at the new row index
                System.Windows.Controls.Grid.SetRow(newListBox, newRowIndex + 1);
                System.Windows.Controls.Grid.SetColumn(newListBox, 1);  // Place it in column 1, or change as needed
                grid.Children.Add(newListBox);  // Add the ListBox to the Grid
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Add(category);  // This stores the selected category in a list.
            }
        }

        // OK Button click handler
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // CANCEL Button click handler
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
