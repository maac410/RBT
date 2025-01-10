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
            // Set the text for the fixed message in infoTextBlock1
            infoTextBlock1.Text = "Tables to be created";

            // Check if we have categories to show, and select one category (if needed)
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

        // Event handler when a checkbox is checked
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Add(category);  // This stores the selected category in a list.

                foreach (var parameter in parameters)  // Assuming 'parameters' is a list of 'Parameter' objects.
                {
                    // Update a ListBox (instead of TextBlock) with each parameter item.
                    infoTextBlock3.Items.Add(parameter);  // Add each parameter to the ListBox
                }
            }
        }

        // Event handler when a checkbox is unchecked
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string category = checkBox.Content.ToString();
                selectedCategories.Add(category);  // This stores the selected category in a list.

                // If you are using TextBlock to show parameters as text:
                StringBuilder sb = new StringBuilder();
                foreach (var parameter in parameters)  // Assuming 'parameters' is a list of 'Parameter' objects.
                {
                    infoTextBlock3.Items.Remove(parameter);  // Add each parameter to the ListBox
                }
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
