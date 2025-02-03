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
            int newRowIndex = 0;
            int newRowIndex2 = 0;

            // Add row definitions for each category
            for (int i = grid.RowDefinitions.Count; i < categories.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Loop through each category and create a checkbox and list box
            foreach (string category in categories)
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
                System.Windows.Controls.Grid.SetRow(newListBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(newListBox, 1);
                grid.Children.Add(newListBox);

                // Create a new ListBox for the parameters
                ListBox parameterListBox = new ListBox();
                foreach (var parameter in parameters)
                {
                    parameterListBox.Items.Add(parameter);
                }

                // Set the row and column for the parameter list box
                System.Windows.Controls.Grid.SetRow(parameterListBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(parameterListBox, 2);
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
                System.Windows.Controls.Grid.SetRow(buttonPanel, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(buttonPanel, 3);
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
<<<<<<< HEAD
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
=======
                listbox.Items.Clear();
            }
>>>>>>> d1f189b8784d4e2130429952af3bd14a6deab2e6
        }
        public static void CreateSchedule(Document doc)
        {
            // Create a new Schedule Definition
            ScheduleDefinition scheduleDefinition = new ScheduleDefinition();

            // Create the Schedule View for Walls
            ViewSchedule Schedule = ViewSchedule.CreateSchedule(doc, category);

            // Define fields (parameters) to show in the schedule
            ScheduleField widthField = new ScheduleField(ScheduleFieldType.Parameter);
            widthField.Parameter = BuiltInParameter.WALL_ATTR_WIDTH_PARAM;
            wallSchedule.Definition.AddField(widthField);

            ScheduleField lengthField = new ScheduleField(ScheduleFieldType.Parameter);
            lengthField.Parameter = BuiltInParameter.CURVE_ELEM_LENGTH;
            wallSchedule.Definition.AddField(lengthField);

            // Add sorting and filtering if necessary
            ScheduleSortGroupField sortField = new ScheduleSortGroupField(0); // 0 refers to the first column
            wallSchedule.Definition.AddSortGroupField(sortField);

            // Optional: Add the schedule to a sheet
            ViewSheet sheet = ViewSheet.Create(doc, ElementId.InvalidElementId);
            sheet.AddView(wallSchedule.Id);

            // Commit the changes
            doc.Regenerate();
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
