using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BT
{
    public partial class SimpleForm : Window
    {
        private ExternalCommandData _commandData;
        private Dictionary<string, HashSet<string>> categoryParameters;
        private HashSet<BuiltInCategory> categoryNames;
        private Dictionary<string, List<UIElement>> categoryElements = new Dictionary<string, List<UIElement>>();
        private List<string> sheetNames;
        private string[] nViews = { "Top", "Bottom", "Elevation Right", "Elevation Left", "Elevation North", "Elevation South", "Isometric", "Isometric with Filters" };
        private List<string> selectedNViews = new List<string>();
        private string assemblyName;
        private AssemblyInstance selectedAssembly;
        private int newRowIndex = 5;
        private Dictionary<string, Tuple<ListBox, ListBox>> listBoxDictionary = new Dictionary<string, Tuple<ListBox, ListBox>>();
        private string categoryName;

        public SimpleForm(ExternalCommandData commandData, Dictionary<string, HashSet<string>> categoryParameters, List<string> sheetNames, string assemblyName, AssemblyInstance selectedAssembly, HashSet<BuiltInCategory> uniqueCategories)
        {
            InitializeComponent();
            this.categoryParameters = categoryParameters;
            _commandData = commandData;
            this.sheetNames = sheetNames;
            this.assemblyName = assemblyName;
            this.selectedAssembly = selectedAssembly;  // Store the selected assembly instance
            this.categoryNames = uniqueCategories;
            GeneralDisplay();
            DisplayVistas();
            DisplayTables();

        }
        private void GeneralDisplay()
        {
            optionComboBox.ItemsSource = sheetNames;
            TextBlock assemblyText = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                Text = "Assembly Name: " + assemblyName
            };
            System.Windows.Controls.Grid.SetRow(assemblyText, 1);
            System.Windows.Controls.Grid.SetColumn(assemblyText, 1);
            grid.Children.Add(assemblyText);
            newRowIndex++;
        }
        //Tables
        private void CreateTablesForCategory(string categoryName, int categoryRowIndex)
        {
            // Create new ListBox instances for each category
            var parameterListBox = new ListBox { MaxHeight = 200 };
            var parameterListBox2 = new ListBox { MaxHeight = 200 };

            // Store the ListBox in the Dictionary with the category name as the key
            listBoxDictionary[categoryName] = new Tuple<ListBox, ListBox>(parameterListBox, parameterListBox2);

            // Add the ListBoxes to categoryElements for future removal
            if (!categoryElements.ContainsKey(categoryName))
            {
                categoryElements[categoryName] = new List<UIElement>();
            }

            categoryElements[categoryName].Add(parameterListBox);
            categoryElements[categoryName].Add(parameterListBox2);

            // Populate ListBox with parameters for the category
            var category = categoryParameters[categoryName].ToList();  // Convert HashSet to List
            foreach (var parameter in category)
            {
                parameterListBox.Items.Add(parameter);
            }

            // Create the button panel (StackPanel or Grid)
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Create the Plus button and set its Tag to the correct ListBox
            Button upButton = CreateButton("Up", "U", Up_Click);
            upButton.Tag = parameterListBox2;
            Button downButton = CreateButton("Down", "D", Down_Click);
            downButton.Tag = parameterListBox2;
            Button minusButton = CreateButton("Minus", "-", Minus_Click);
            minusButton.Tag = parameterListBox2;
            Button plusButton = CreateButton("Plus", "+", Plus_Click);
            plusButton.Tag = parameterListBox;  // Store the ListBox reference in the button's Tag

            // Add the button to the button panel
            buttonPanel.Children.Add(upButton);
            buttonPanel.Children.Add(downButton);
            buttonPanel.Children.Add(minusButton);
            buttonPanel.Children.Add(plusButton);

            // Add the button panel to the grid
            System.Windows.Controls.Grid.SetRow(buttonPanel, categoryRowIndex);
            System.Windows.Controls.Grid.SetColumn(buttonPanel, 2);
            grid.Children.Add(buttonPanel);

            // Add the button panel to categoryElements so it can be removed later
            categoryElements[categoryName].Add(buttonPanel);

            // Place the ListBox in the grid
            System.Windows.Controls.Grid.SetRow(parameterListBox, categoryRowIndex);
            System.Windows.Controls.Grid.SetColumn(parameterListBox, 1);
            grid.Children.Add(parameterListBox);

            // Place the ListBox in the grid (second ListBox)
            System.Windows.Controls.Grid.SetRow(parameterListBox2, categoryRowIndex);
            System.Windows.Controls.Grid.SetColumn(parameterListBox2, 3);
            grid.Children.Add(parameterListBox2);
        }
        private Button CreateButton(string name, string content, RoutedEventHandler clickEventHandler)
        {
            var button = new Button
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
        private string GetCategoryNameForButton(Button button)
        {
            // This should map the button back to the category name somehow.
            // Example: Using button's Tag or Name to get the category name
            return button.Name;  // Assumes you have the category name as the button name
        }
        private void Up_Click(object sender, RoutedEventArgs e)
        {
            // Get the ListBox from the Tag property of the clicked button
            var button = sender as Button;
            var listBox2 = button?.Tag as ListBox;
            // Retrieve the corresponding category's ListBox pair from the dictionary
            var categoryName = listBoxDictionary.FirstOrDefault(x => x.Value.Item2 == listBox2).Key;
            // Check if the ListBox and a selected item exist
            if (listBox2 != null && listBox2.SelectedItem != null)
            {
                // Get the selected index
                int selectedIndex = listBox2.SelectedIndex;

                // Ensure the selected item is not the first one (cannot move it up)
                if (selectedIndex > 0)
                {
                    // Get the item at the selected index
                    var selectedItem = listBox2.SelectedItem;

                    // Swap the selected item with the one above it
                    var aboveItem = listBox2.Items[selectedIndex - 1];

                    // Move the items by removing and inserting
                    listBox2.Items.RemoveAt(selectedIndex);
                    listBox2.Items.Insert(selectedIndex - 1, selectedItem);

                    // Optionally, reselect the item after moving
                    listBox2.SelectedItem = selectedItem;
                    listBox2.SelectedIndex = selectedIndex - 1;
                }
            }
        }
        private void Down_Click(object sender, RoutedEventArgs e)
        {
            // Get the ListBox from the Tag property of the clicked button
            var button = sender as Button;
            var listBox2 = button?.Tag as ListBox;

            // Check if the ListBox and a selected item exist
            if (listBox2 != null && listBox2.SelectedItem != null)
            {
                // Get the selected index
                int selectedIndex = listBox2.SelectedIndex;

                // Check if the selected item is not at the last position
                if (selectedIndex < listBox2.Items.Count - 1)
                {
                    // Get the selected item
                    var selectedItem = listBox2.SelectedItem;

                    // Get the item below the selected item (next item)
                    var nextItem = listBox2.Items[selectedIndex + 1];

                    // Swap the selected item with the one below it
                    listBox2.Items.RemoveAt(selectedIndex);
                    listBox2.Items.Insert(selectedIndex + 1, selectedItem);

                    // Optionally, reselect the item after moving it
                    listBox2.SelectedItem = selectedItem;
                    listBox2.SelectedIndex = selectedIndex + 1;
                }
            }
        }
        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            // Get the ListBox from the Tag property of the clicked button
            var button = sender as Button;
            var listBox2 = button?.Tag as ListBox;

            // Get the selected item
            var selectedItem = listBox2.SelectedItem;

            // Retrieve the corresponding category's ListBox pair from the dictionary
            var categoryName = listBoxDictionary.FirstOrDefault(x => x.Value.Item2 == listBox2).Key;

            // Get the first ListBox (listBox1) from the dictionary (the one we want to move items to)
            var parameterListBox = listBoxDictionary[categoryName].Item1;

            // Remove the selected item from the second ListBox (listBox2)
            listBox2.Items.Remove(selectedItem);

            // Add the selected item to the first ListBox (parameterListBox)
            parameterListBox.Items.Add(selectedItem);
        }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            // Get the ListBox from the Tag property of the clicked button
            var button = sender as Button;
            var listBox = button?.Tag as ListBox;

            if (listBox == null || listBox.SelectedItem == null)
                return; // If no item is selected, exit early

            // Get the selected item
            var selectedItem = listBox.SelectedItem;

            // Retrieve the corresponding category's ListBox pair from the dictionary
            var categoryName = listBoxDictionary.FirstOrDefault(x => x.Value.Item1 == listBox || x.Value.Item2 == listBox).Key;

            if (string.IsNullOrEmpty(categoryName))
                return;

            // Get the second ListBox (parameterListBox2) from the dictionary
            var parameterListBox2 = listBoxDictionary[categoryName].Item2;

            // Add the selected item to the second ListBox (parameterListBox2)
            parameterListBox2.Items.Add(selectedItem);

            // Remove the selected item from the first ListBox (listBox)
            listBox.Items.Remove(selectedItem);
        }
        private void DisplayTables()
        {
            newRowIndex++; // Increment before adding the "Tablas" header

            // Create and add the "Tablas" header
            Border tablasBorder = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(0, 10, 0, 5),
            };
            tablasBorder.Child = new TextBlock { Text = "Tablas" };
            System.Windows.Controls.Grid.SetRow(tablasBorder, newRowIndex);
            System.Windows.Controls.Grid.SetColumn(tablasBorder, 1);
            System.Windows.Controls.Grid.SetColumnSpan(tablasBorder, 3);
            grid.Children.Add(tablasBorder);
            newRowIndex++; // Move to the next row after the "Tablas" header

            // Ensure enough row definitions for categories and vistas
            for (int i = grid.RowDefinitions.Count; i < categoryParameters.Count * 4 + nViews.Length; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Iterate through each category
            foreach (var category in categoryParameters)
            {
                // Store the row for each category, starting with a row for the checkbox
                int categoryRowIndex = newRowIndex;

                // Create a checkbox for the category
                var checkBox = new CheckBox
                {
                    Content = category.Key,
                    Margin = new Thickness(0, 10, 0, 5),
                    IsChecked = true // Set initial state to checked
                };

                // Add event handlers to create/remove tables on check/uncheck
                checkBox.Checked += (sender, e) =>
                {
                    categoryName = category.Key; // Ensure categoryName is set when checked
                    CreateTablesForCategory(category.Key, categoryRowIndex);
                };
                checkBox.Unchecked += (sender, e) =>
                {
                    categoryName = category.Key; // Ensure categoryName is set when unchecked
                    RemoveTables(category.Key, categoryRowIndex);
                };

                // Place the checkbox in its own row
                System.Windows.Controls.Grid.SetRow(checkBox, categoryRowIndex);
                System.Windows.Controls.Grid.SetColumn(checkBox, 1);
                grid.Children.Add(checkBox);

                // After checkbox is placed, move the row index for tables
                categoryRowIndex++; // Increment row index for the next elements (tables, buttons)

                // **Call CreateTablesForCategory if the checkbox is checked**
                if (checkBox.IsChecked == true)
                {
                    categoryName = category.Key; // Ensure categoryName is set when checking tables
                    CreateTablesForCategory(category.Key, categoryRowIndex);
                }

                // Increment to the next row after placing tables
                newRowIndex = categoryRowIndex + 1;
            }
        }
        private void RemoveTables(string categoryName, int categoryRowIndex)
        {
            if (categoryElements.ContainsKey(categoryName))
            {
                var elements = categoryElements[categoryName];

                // Iterate through the elements created for this category and remove them from the grid
                foreach (var element in elements)
                {
                    grid.Children.Remove(element);  // Remove the element (ListBox, StackPanel, etc.) from the grid
                }

                // Clear the list of created elements for this category
                categoryElements[categoryName].Clear();
            }

            // Also remove the associated ListBoxes from the dictionary
            if (listBoxDictionary.ContainsKey(categoryName))
            {
                listBoxDictionary.Remove(categoryName);
            }

        }
        //Vistas
        private void DisplayVistas()
        {
            var newColumnIndex = 1;

            // Assuming nViews is a collection of strings representing the views you want to display
            foreach (string vista in nViews)
            {
                if (newColumnIndex >= 3)
                {
                    newColumnIndex = 1;
                    newRowIndex++;
                }

                // Create a new checkbox for each view
                var checkBox = new CheckBox
                {
                    Content = vista,               // Set the content (view name)
                    Margin = new Thickness(0, 10, 0, 5),  // Set margin for the checkbox
                    IsChecked = true               // Set the checkbox to be checked initially
                };

                selectedNViews.Add(vista);
                // Position the checkbox in the grid
                System.Windows.Controls.Grid.SetRow(checkBox, newRowIndex);
                System.Windows.Controls.Grid.SetColumn(checkBox, newColumnIndex);
                System.Windows.Controls.Grid.SetColumnSpan(checkBox, 2);
                grid.Children.Add(checkBox);

                // Attach event handlers to the checkbox
                checkBox.Checked += (sender, e) => HandleChecked(vista);
                checkBox.Unchecked += (sender, e) => HandleUnchecked(vista);

                newColumnIndex++;
            }
        }
        private void HandleChecked(string vista)
        {
                selectedNViews.Add(vista);

            MessageBox.Show(string.Join(", ", selectedNViews));
        }
        private void HandleUnchecked(string vista)
        {

                selectedNViews.Remove(vista);

            MessageBox.Show(string.Join(", ", selectedNViews));
        }
        private void CreateSheetWithAssemblyViews(string titleBlockName, string assemblyName, AssemblyInstance selectedAssembly, string categoryName)
        {
            try
            {
                // Access Revit application and document
                UIDocument uiDoc = _commandData.Application.ActiveUIDocument;
                Document doc = uiDoc.Document;

                // Create a filtered element collector for title blocks
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                var titleBlockSymbols = collector.OfClass(typeof(FamilySymbol))
                                                 .OfCategory(BuiltInCategory.OST_TitleBlocks)
                                                 .ToList();

                // Find the selected title block symbol based on the selected titleBlockName from the ComboBox
                var selectedTitleBlock = titleBlockSymbols
                    .FirstOrDefault(tb => tb.Name == titleBlockName);

                // Start a new transaction for creating the sheet and assembly views
                using (Transaction trans = new Transaction(doc, "Create Sheet and Assembly Views"))
                {
                    trans.Start();

                    try
                    {
                        // Create the ViewSheet with the selected title block
                        ViewSheet newSheet = ViewSheet.Create(doc, selectedTitleBlock.Id);
                        newSheet.Name = assemblyName;

                        // Create assembly views and schedule
                        CreateAssemblyViews(doc, selectedAssembly, newSheet, categoryName);
                        CreateScheduleForElement(doc, selectedAssembly, newSheet);

                        // Commit the transaction if everything is successful
                        trans.Commit();

                        // Show success message
                        MessageBox.Show($"Sheet '{assemblyName}' created and assembly views added successfully!");
                    }
                    catch (Exception ex)
                    {
                        // If an error occurs, roll back the transaction
                        trans.RollBack();
                        MessageBox.Show($"An error occurred while creating the sheet or adding views: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error if there's an exception outside of the transaction
                MessageBox.Show($"An error occurred: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CreateAssemblyViews(Document doc, AssemblyInstance selectedAssembly, ViewSheet newSheet, string categoryName)
        {
            if (selectedNViews.Contains("Isometric"))
            {
                try
                {
                    // Get the location of the selected assembly (the location of the assembly itself, not its elements)
                    Location location = selectedAssembly.Location;

                    // Ensure the location is a LocationPoint (this will be the assembly's reference point)
                    if (location is LocationPoint locationPoint)
                    {
                        XYZ assemblyLocation = locationPoint.Point;
                        // Get the view type for 3D view instead of floor plan, since we are ignoring the level
                        FilteredElementCollector collector = new FilteredElementCollector(doc);
                        collector.OfClass(typeof(ViewFamilyType));
                        ViewFamilyType threeDViewType = collector.Cast<ViewFamilyType>()
                            .FirstOrDefault(vft => vft.ViewFamily == ViewFamily.ThreeDimensional);
                        // Create the 3D View for the selected assembly at its exact location
                        View3D assemblyView = View3D.CreateIsometric(doc, threeDViewType.Id);
                        // Assign the unique name to the view
                        assemblyView.Name = (assemblyName + " - Isometric");
                        // Create a Viewport to place the view on the sheet
                        Viewport viewport = Viewport.Create(doc, newSheet.Id, assemblyView.Id, assemblyLocation);
                    }
                }
                catch (Exception ex)
                {
                    // Show detailed error message
                    MessageBox.Show($"An error occurred: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CreateScheduleForElement(Document doc, AssemblyInstance selectedAssembly, ViewSheet newSheet)
        {
            try
            {
                // Loop through each unique BuiltInCategory in uniqueCategories
                foreach (var bic in categoryNames)
                {
                    // Convert BuiltInCategory to ElementId
                    ElementId categoryElementId = new ElementId((int)bic);
                    // Collector for elements in the category
                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    collector.OfCategory(bic).WhereElementIsNotElementType(); // Ensure it's not a type element
                    var elements = collector.ToList();
                    // Proceed with creating the schedule for the first element of the category
                    Element element = elements.FirstOrDefault();
                    // Create schedule for the category using the ElementId
                    ViewSchedule viewSchedule = ViewSchedule.CreateSchedule(doc, categoryElementId);
                    // Create the schedule instance on the sheet
                    ScheduleSheetInstance scheduleSheetInstance = ScheduleSheetInstance.Create(doc, newSheet.Id, viewSchedule.Id, new XYZ(0, 0, 0));

                    // Confirm that the schedule was placed on the sheet
                    MessageBox.Show($"Schedule for category '{bic}' placed on sheet.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the schedule: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Buttons
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Ensure assemblyName is set and combo box selection is valid
            if (!string.IsNullOrEmpty(assemblyName) && optionComboBox.SelectedItem != null)
            {
                // Get the selected sheet name (title block name) from the combo box
                string selectedSheetName = optionComboBox.SelectedItem.ToString();

                // Pass the selected sheet name to CreateSheet, but the sheet name will be set to assemblyName
                CreateSheetWithAssemblyViews(selectedSheetName, assemblyName, selectedAssembly, categoryName);  // Corrected this line by passing 'assemblyName' instead of 'sheetNames'

                // Close the window after creating the sheet
                this.Close();
            }
            else
            {
                // Show a message if no valid assembly name or sheet is selected
                MessageBox.Show("Please select a valid sheet.");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) { this.Close(); }
    }
}