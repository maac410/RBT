using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Runtime.InteropServices;
using System.Reflection;
using System;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using Autodesk.Revit.UI.Selection;
using DocumentFormat.OpenXml.Spreadsheet;
using Parameter = Autodesk.Revit.DB.Parameter;
using System.Linq;

namespace BT
{
    public class Class1 : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            // Implement shutdown logic if needed
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("BT");
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData = new PushButtonData("cmdMyTest", "My Test", thisAssemblyPath, "BT.MyTest");
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "Hello World";
            return Result.Succeeded;
        }
    }
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("BT");
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData = new PushButtonData("cmdMyTest", "My Test", thisAssemblyPath, "BT.MyTest");
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

            string info = "Selected element IDs are: ";
            bool? hasAssembly = null;

            Autodesk.Revit.UI.Selection.Selection selection = uidoc.Selection;

            try
            {
                ICollection<ElementId> selectedIds = selection.GetElementIds();

                if (selectedIds.Count == 0)
                {
                    TaskDialog.Show("Revit", "You haven't selected any elements.");
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // A HashSet to store processed category names or element type names
                HashSet<string> processedCategories = new HashSet<string>();

                foreach (ElementId id in selectedIds)
                {
                    Element element = doc.GetElement(id);

                    if (element is AssemblyInstance assemblyInstance)
                    {
                        hasAssembly = true;
                        info += $"\n\tAssembly Instance ID: {id.IntegerValue}";

                        // Process the assembly's member elements
                        ICollection<ElementId> memberIds = assemblyInstance.GetMemberIds();

                        foreach (ElementId memberId in memberIds)
                        {
                            Element member = doc.GetElement(memberId);
                            info += $"\n\tAssembly member Instance ID: {memberId.IntegerValue}";

                            // Get the ElementType (or FamilyType) of the member
                            ElementType memberType = doc.GetElement(member.GetTypeId()) as ElementType;
                            if (memberType != null)
                            {
                                // Get the category name or type name (this is what will be tracked)
                                string categoryName = member.Category.Name;
                                ParameterSet parameters = member.Parameters;

                                // If this category hasn't been processed yet, add it to the list
                                if (!processedCategories.Contains(categoryName))
                                {
                                    processedCategories.Add(categoryName);
                                }

                                // Get parameters of the member's type (element type)
                                Parameter[] typeParameters = memberType.Parameters.Cast<Parameter>().ToArray();

                                // Collect information about the parameters of the type
                                foreach (Parameter param in typeParameters)
                                {
                                    string paramName = param.Definition.Name;
                                    string paramValue = param.AsString() ?? param.AsValueString() ?? "No value";

                                    // Add parameter details to the info string
                                    info += $"\n\tCategory: {categoryName}, Type Parameter: {paramName}, Value: {paramValue}";
                                }
                            }
                            else
                            {
                                info += $"\n\tMember ID: {memberId.IntegerValue}, No ElementType found.";
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Revit", "One of the selected elements is not an assembly instance. Execution will stop.");
                        return Autodesk.Revit.UI.Result.Failed;
                    }
                }

                // Show the result if assembly instances were found
                if (hasAssembly.HasValue && hasAssembly.Value && selectedIds.Count < 2)
                {
                    // Show info in a custom Windows Form (SimpleForm)
                    var simpleForm = new SimpleForm(info, processedCategories);  // Pass categories to the form
                    simpleForm.ShowDialog();  // Show the form
                }
                else
                {
                    TaskDialog.Show("Revit", "No assembly instance elements selected or an item that isn't an assembly selected or there is more than one assembly selected.");
                }

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
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

            var collector = new FilteredElementCollector(doc).OfCategory(
    BuiltInCategory.OST_Assemblies);

            var SimpleForm = new SimpleForm(collector);
            SimpleForm.ShowDialog();

            // Show a simple dialog
            TaskDialog.Show("Revit", "Hello World");

            return Result.Succeeded;
        }
    }