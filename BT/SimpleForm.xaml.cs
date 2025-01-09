<<<<<<<<< Temporary merge branch 1
<<<<<<< HEAD
﻿using BT;
=========
using BT;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BT
{
    public partial class SimpleForm : Window
    {
        public SimpleForm()
        {
            InitializeComponent();
        }

        // Constructor that accepts info and processed categories (HashSet)
        public SimpleForm(string info, HashSet<string> processedCategories) : this()
        {
            // Display the provided info in the ListBox
            DisplayInfo(info, "Box");
            

            // Dynamically create and add checkboxes for each category to mBox
            foreach (var category in processedCategories)
            {
                // Create a new CheckBox
                CheckBox checkBox = new CheckBox();
                checkBox.Content = category; // Set the checkbox content as the category name
                checkBox.Margin = new Thickness(5);

                // Optionally add an event handler for Checked/Unchecked events
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;

                // Wrap the checkbox in a ListBoxItem (mBox requires ListBoxItems)
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = checkBox;

                // Add the ListBoxItem (containing the CheckBox) to mBox
                mBox.Items.Add(listBoxItem);
            }
        }

        // Show the form with the passed info and specify which ListBox to use
        public void ShowDialog(string info, string listBoxName)
        {
            // Display info in the specified ListBox
            DisplayInfo(info, listBoxName);

            // Show the dialog window
            this.ShowDialog();
        }

        // Helper method to handle displaying information in a ListBox
        private void DisplayInfo(string info, string listBoxName)
        {
            // Split the info string into an array of lines
            var formattedCollection = info.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);

            // Clear the existing items from the ListBox
            if (listBoxName == "Box")
            {
                Box.Items.Clear(); // Clear the existing items from Box
                Box.ItemsSource = formattedCollection; // Set the new ItemsSource
            }
            else if (listBoxName == "mBox")
            {
                mBox.Items.Clear(); // Clear the existing items from mBox
                mBox.ItemsSource = formattedCollection; // Set the new ItemsSource
            }
        }


        // Checkbox Checked event handler (optional)
        private HashSet<string> selectedCategories = new HashSet<string>();


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Cast sender to CheckBox
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                // Get the category name from the checkbox content
                string category = checkBox.Content.ToString();

                // Add the category to the selectedCategories HashSet
                selectedCategories.Add(category);

                // Optionally, update the info string with the selected category (if needed)
                ShowDialog($"Categories selected: {category}", "mBox");
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Cast sender to CheckBox
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                // Get the category name from the checkbox content
                string category = checkBox.Content.ToString();

                // Remove the category from the selectedCategories HashSet
                selectedCategories.Remove(category);

                // Optionally, update the info string to reflect the deselection
                ShowDialog($"\nCategory deselected: {category}", "mBox");
            }
        }

        // Cancel Button Click event handler
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
<<<<<<<<< Temporary merge branch 1




//var simpleForm = new SimpleForm(info); 
//simpleForm.ShowDialogWithInfo(info, "mBox");

//var simpleForm = new SimpleForm(info);
//simpleForm.ShowDialogWithInfo(info, "box");
=======
﻿using DocumentFormat.OpenXml.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BT
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class SimpleForm : Window
    {
        public SimpleForm(IEnumerable collection)
        {
            InitializeComponent();
            Box.ItemsSource = collection;
        }
    }
}
>>>>>>> 3e6a30d037e02b8a5ae4c2ee0b3161270a28cf8d
=========
>>>>>>>>> Temporary merge branch 2
