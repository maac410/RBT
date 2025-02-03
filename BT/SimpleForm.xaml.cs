using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BT
{
    public partial class SimpleForm : Window
    {
        private List<string> categories;
        private List<string> parameters;
        private HashSet<string> processedCategories;
        private HashSet<string> processedParameters;

        public SimpleForm(List<string> categories, List<string> parameters, HashSet<string> processedCategories, HashSet<string> processedParameters)
        {
            InitializeComponent();
            this.categories = categories;
            this.parameters = parameters;
            this.processedCategories = processedCategories;
            this.processedParameters = processedParameters;

            DisplayInfo();
        }

        private void DisplayInfo()
        {
            int newRowIndex = 2;

            // Add row definitions for each category
            for (int i = grid.RowDefinitions.Count; i < categories.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Loop through each category and create a checkbox and list box
            foreach (string category in processedCategories)
            {
                // Create the ListBox to hold the checkbox for the category
                ListBox newListBox = new ListBox();
                var checkBox = new CheckBox
                {
                    Content = category,
                    Margin = new Thickness(5),                   
                    IsChecked = processedCategories.Contains(category) // Check the box if processedCategories contains the category
                };

                // Add checkbox to the ListBox
                newListBox.Items.Add(checkBox);

                // Set the row and column for the ListBox containing the checkbox
                Grid.SetRow(newListBox, newRowIndex);
                Grid.SetColumn(newListBox, 1);
                grid.Children.Add(newListBox);


                // Increment row index for the next iteration
                newRowIndex++;

                // Create a new ListBox for the parameters
                ListBox parameterListBox = new ListBox {
                    MaxHeight = 200,
                    Margin= new Thickness(0,0,0,30)
                };
                foreach (var parameter in processedParameters)
                {
                    parameterListBox.Items.Add(parameter);
                }

                // Set the row and column for the parameter list box
                Grid.SetRow(parameterListBox, newRowIndex);
                Grid.SetColumn(parameterListBox, 1);
                grid.Children.Add(parameterListBox);

                // Add a StackPanel for the buttons below the ListBoxes (Up, Down, Minus, Plus)
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

                // Set row and column for the button panel
                Grid.SetRow(buttonPanel, newRowIndex);
                Grid.SetColumn(buttonPanel, 2);
                grid.Children.Add(buttonPanel);

                // Increment row index for the next iteration
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
