using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI.Selection;
using Parameter = Autodesk.Revit.DB.Parameter;
using System.Reflection;

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
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("BT");
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData = new PushButtonData("cmdMyTest", "RBT", thisAssemblyPath, "BT.MyTest");
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
            bool? hasAssembly = null;
            Dictionary<string, HashSet<string>> categoryParameters = new Dictionary<string, HashSet<string>>();

            try
            {
                // Get the selected elements
                ICollection<ElementId> selectedIds = selection.GetElementIds();

                if (selectedIds.Count == 0)
                {
                    TaskDialog.Show("Revit", "You haven't selected any elements.");
                    return Result.Failed;
                }

                // Iterate through the selected elements
                foreach (ElementId id in selectedIds)
                {
                    Element element = doc.GetElement(id);

                    // Check if the element is an AssemblyInstance
                    if (element is AssemblyInstance assemblyInstance)
                    {
                        hasAssembly = true;

                        // Get the member IDs (elements inside the assembly)
                        ICollection<ElementId> memberIds = assemblyInstance.GetMemberIds();

                        // Iterate over the members of the assembly
                        foreach (ElementId memberId in memberIds)
                        {
                            Element member = doc.GetElement(memberId);
                            ElementType memberType = doc.GetElement(member.GetTypeId()) as ElementType;
                            string categoryName = member.Category.Name;

                            // Initialize the parameter set for this category if not already
                            if (!categoryParameters.ContainsKey(categoryName))
                            {
                                categoryParameters[categoryName] = new HashSet<string>();
                            }

                            // Collect parameters for the member type
                            if (memberType != null)
                            {
                                Parameter[] typeParameters = memberType.Parameters.Cast<Parameter>().ToArray();

                                foreach (Parameter param in typeParameters)
                                {
                                    string paramName = param.Definition.Name;

                                    // Add the parameter to the respective category's set
                                    categoryParameters[categoryName].Add(paramName);
                                }
                            }

                            // Collect parameters for the member itself (not just the type)
                            Parameter[] elementParameters = member.Parameters.Cast<Parameter>().ToArray();
                            foreach (Parameter param in elementParameters)
                            {
                                string paramName = param.Definition.Name;

                                // Add the parameter to the respective category's set
                                categoryParameters[categoryName].Add(paramName);
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Revit", "One of the selected elements is not an assembly instance.");
                        return Result.Failed;
                    }
                }

                // If an assembly instance was found and less than two assemblies are selected, show the form
                if (hasAssembly.HasValue && hasAssembly.Value && selectedIds.Count < 2)
                {
                    // Pass the categories and their parameters to the SimpleForm correctly
                    var simpleForm = new SimpleForm(categoryParameters);
                    simpleForm.ShowDialog();
                }

                else
                {
                    TaskDialog.Show("Revit", "No assembly instance elements selected, an item that isn't an assembly selected, or more than one assembly selected.");
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
