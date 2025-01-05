using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Runtime.InteropServices;
using System.Reflection;
using System;
using Autodesk.Revit.Attributes;

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

    // Correctly apply Transaction to the method that modifies Revit's data
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
}
