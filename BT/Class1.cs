using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Parameter = Autodesk.Revit.DB.Parameter;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace BT
{
    public class Class1 : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            try
            {
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error during shutdown: " + ex.Message);
                return Result.Failed;
            }
        }
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Rbt");
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // Define the relative path to your image (assuming it's in a folder called 'Images' in your project)
            string imageFolderPath = Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Images");
            string iconPath = Path.Combine(imageFolderPath, "C:\\Users\\EC-BT-007\\Source\\Repos\\RBT\\BT\\img\\btLogoSmall.png");
            // Create the PushButtonData with the icon
            PushButtonData buttonData = new PushButtonData("cmdMyTest", "Create Assembly Blueprint", thisAssemblyPath, "BT.MyTest")
            {
                LargeImage = new BitmapImage(new Uri(iconPath)) // Assigning the icon
            };
            // Create the push button and set tooltip
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "Create an Assembly Blueprint";
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class MyTest : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            Autodesk.Revit.UI.Selection.Selection selection = uidoc.Selection;
            Dictionary<string, HashSet<string>> categoryParameters = new Dictionary<string, HashSet<string>>();

            // Create a filtered element collector for the current document
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // List to store the names of the available title blocks
            List<string> sheetNames = new List<string>();
            string assemblyName = "default";
            AssemblyInstance selectedAssembly = null;  // To hold the selected assembly instance

            // Collect FamilySymbols of the OST_TitleBlocks category
            var titleBlockSymbols = collector.OfClass(typeof(FamilySymbol))
                                             .OfCategory(BuiltInCategory.OST_TitleBlocks)
                                             .ToList();

            // Loop through the collected FamilySymbols (TitleBlock family types)
            foreach (Element element in titleBlockSymbols)
            {
                FamilySymbol titleBlockFamilyType = element as FamilySymbol;
                sheetNames.Add(titleBlockFamilyType.Name);
            }

            try
            {
                // Get the selected elements
                ICollection<ElementId> selectedIds = selection.GetElementIds();
                if (selectedIds.Count == 0)
                {
                    TaskDialog.Show("Revit", "You haven't selected any elements.");
                    return Result.Failed;
                }

                // Create a HashSet to store unique BuiltInCategory values
                HashSet<BuiltInCategory> uniqueCategories = new HashSet<BuiltInCategory>();

                // Iterate through the selected elements
                foreach (ElementId id in selectedIds)
                {
                    Element element = doc.GetElement(id);

                    // Check if the element is an AssemblyInstance
                    if (element is AssemblyInstance assemblyInstance)
                    {
                        selectedAssembly = assemblyInstance;
                        assemblyName = assemblyInstance.Name;

                        // Collect member IDs for this AssemblyInstance
                        ICollection<ElementId> memberIds = assemblyInstance.GetMemberIds();

                        foreach (ElementId memberId in memberIds)
                        {
                            // Get the member element
                            Element member = doc.GetElement(memberId);

                            if (member == null) continue;

                            // Get the category of the member
                            Category memberCategory = member.Category;

                            if (memberCategory == null) continue;

                            // Get the BuiltInCategory from the member's Category
                            BuiltInCategory builtInCategory = memberCategory.Id.IntegerValue != -1 ? (BuiltInCategory)memberCategory.Id.IntegerValue : BuiltInCategory.INVALID;

                            // Add the BuiltInCategory to the HashSet (duplicates are automatically handled)
                            if (builtInCategory != BuiltInCategory.INVALID)
                            {
                                uniqueCategories.Add(builtInCategory);
                            }

                            // Initialize the parameter set for this category if not already
                            string categoryName = memberCategory.Name;

                            if (!categoryParameters.ContainsKey(categoryName))
                            {
                                categoryParameters[categoryName] = new HashSet<string>();
                            }

                            // Collect parameters for the member type
                            ElementType memberType = doc.GetElement(member.GetTypeId()) as ElementType;
                            if (memberType != null)
                            {
                                foreach (Parameter param in memberType.Parameters.Cast<Parameter>())
                                {
                                    string paramName = param.Definition.Name;
                                    categoryParameters[categoryName].Add(paramName);
                                }
                            }

                            // Collect parameters for the member itself (not just the type)
                            foreach (Parameter param in member.Parameters.Cast<Parameter>())
                            {
                                string paramName = param.Definition.Name;
                                categoryParameters[categoryName].Add(paramName);
                            }

                            // Now, add schedulable fields to the category parameters
                            List<SchedulableField> schedulableFields = GetSchedulableFieldsForCategory(doc, memberCategory);

                            foreach (SchedulableField field in schedulableFields)
                            {
                                string fieldName = field.GetName(doc);
                                categoryParameters[categoryName].Add(fieldName);
                            }
                        }
                    }
                }

                if (selectedAssembly != null)
                {
                    // Open the SimpleForm, passing the selected assembly instance, category parameters, and sheet names
                    var simpleForm = new SimpleForm(commandData, categoryParameters, sheetNames, assemblyName, selectedAssembly, uniqueCategories);
                    simpleForm.ShowDialog();
                }
                else
                {
                    TaskDialog.Show("Revit", "No assembly instance selected.");
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }

        // Helper method to get schedulable fields for a specific category
        private List<SchedulableField> GetSchedulableFieldsForCategory(Document doc, Category category)
        {
            var scheduleViews = new FilteredElementCollector(doc)
                                .OfClass(typeof(ViewSchedule))
                                .Cast<ViewSchedule>()
                                .ToList();

            foreach (var schedule in scheduleViews)
            {
                if (schedule.Definition.CategoryId.IntegerValue == category.Id.IntegerValue)
                {
                    return schedule.Definition.GetSchedulableFields().ToList();
                }
            }
            return new List<SchedulableField>();
        }
    }
}
