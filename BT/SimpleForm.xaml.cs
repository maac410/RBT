using Autodesk.Revit.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace BT
{
    public partial class SimpleForm : Window
    {
        private Dictionary<string, HashSet<string>> categoryParameters;

        int newRowIndex = 3; 

        // Constructor that accepts the category parameters dictionary
        public SimpleForm(Dictionary<string, HashSet<string>> categoryParameters)
        {
            InitializeComponent();
            this.categoryParameters = categoryParameters;
            DisplayVistas();
            DisplayTablas();
        }
        private void DisplayTablas()
        {
            // Add a Border to visually separate the area for "Tablas"
            Border tablasBorder = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(0, 10, 0, 5),
            };

            // Create a TextBlock for the "Tablas" label inside the Border
            TextBlock tablasTextBlock = new TextBlock
            {
                Text = "Tablas",
            };
            tablasBorder.Child = tablasTextBlock;

            // Add the Border to the grid
            System.Windows.Controls.Grid.SetRow(tablasBorder, newRowIndex);  // Adjust row as needed
            System.Windows.Controls.Grid.SetColumn(tablasBorder, 1);  // Place in column 1
            System.Windows.Controls.Grid.SetColumnSpan(tablasBorder, 3);  // Span across columns
            grid.Children.Add(tablasBorder);

            newRowIndex++;

            // Ensure enough row definitions
            for (int i = grid.RowDefinitions.Count; i < categoryParameters.Count *5+2; i++) // +1 to account for the Border
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Iterate through the category parameters dictionary
            foreach (var category in categoryParameters)
            {
                // Create the checkbox for the category
                var checkBox = new CheckBox
                {
                    Content = category.Key,  // category name
                    Margin = new Thickness(0, 10, 0, 5),
                    IsChecked = true // Default is checked, change as necessary
                };

                // Add the category checkbox directly into the grid
                System.Windows.Controls.Grid.SetRow(checkBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(checkBox, 1); // Place checkbox in column 1
                grid.Children.Add(checkBox);

                // Increment row index for the next element (to keep checkboxes and parameters separate)
                newRowIndex++;

                // Create and add ListBoxes for parameters related to the category
                ListBox parameterListBox = new ListBox
                {
                    MaxHeight = 200,
                };

                ListBox parameterListBox2 = new ListBox
                {
                    MaxHeight = 200,
                };

                // Populate the ListBoxes with unique parameters for this category
                if (categoryParameters.ContainsKey(category.Key))
                {
                    HashSet<string> uniqueParameters = new HashSet<string>(category.Value);  // Ensures unique parameters

                    foreach (var parameter in uniqueParameters)
                    {
                        parameterListBox.Items.Add(parameter);
                    }
                }

                // Add the ListBoxes into the grid
                System.Windows.Controls.Grid.SetRow(parameterListBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(parameterListBox, 1); // Column 1
                grid.Children.Add(parameterListBox);

                System.Windows.Controls.Grid.SetRow(parameterListBox2, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(parameterListBox2, 3); // Column 3
                grid.Children.Add(parameterListBox2);

                // Add the buttons (Up, Down, Minus, Plus)
                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                buttonPanel.Children.Add(CreateButton("Up", "Up", Up_Click));
                buttonPanel.Children.Add(CreateButton("Down", "Down", Down_Click));
                buttonPanel.Children.Add(CreateButton("Minus", "-", Minus_Click));
                buttonPanel.Children.Add(CreateButton("Plus", "+", Plus_Click));

                // Add the button panel to the grid
                System.Windows.Controls.Grid.SetRow(buttonPanel, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(buttonPanel, 2); // Column 2
                grid.Children.Add(buttonPanel);

                // Increment row index for the next category
                newRowIndex++;
            }
        }

        private void DisplayVistas()
        {
            // Declare the string array for vistas
            string[] nVistas = { "Top", "Bottom", "Elevation Right", "Elevation Left", "Elevation North", "Elevation South", "Isometric" };

            // Iterate through the nVistas array to create a CheckBox for each item
            foreach (string vista in nVistas)
            {
                // Create a new CheckBox for each element in the array
                var checkBox = new CheckBox
                {
                    Content = vista, // Set the content of the checkbox to the current vista name
                    Margin = new Thickness(0, 10, 0, 5),
                    IsChecked = true // Default is checked, change as necessary
                };

                // Add the checkbox directly into the grid
                System.Windows.Controls.Grid.SetRow(checkBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(checkBox, 1); // Place checkbox in the first column
                grid.Children.Add(checkBox);

                newRowIndex++;
            }
        }

        // Method to create a button with event handlers
        private Button CreateButton(string name, string content, RoutedEventHandler clickEventHandler)
        {
            Button button = new Button
            {
                Name = name,
                Content = content,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 20,
                Height = 20,
                Margin = new Thickness(5)
            };

            button.Click += clickEventHandler;
            return button;
        }

        // Placeholder methods for Up, Down, Minus button clicks
        private void Up_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Up button clicked");
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Down button clicked");
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Minus button clicked");
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Plus button clicked");
        }
        // Event handler for Option 1 click
        private void Option1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Option 1 selected.");
        }

        // Event handler for Option 2 click
        private void Option2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Option 2 selected.");
        }

        // Event handler for Option 3 click
        private void Option3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Option 3 selected.");
        }

        // Event handler for the main button to show options
        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            // Optional: Custom logic when the button itself is clicked
        }

        // Methods for Ok and Cancel buttons
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
