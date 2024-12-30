using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Runtime.InteropServices;
using System.Reflection;
using System;

namespace Bt_First
{
    public class Class1 : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("My Ribbon Panel");
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData = new PushButtonData("cmdMyTest", "My Test", thisAssemblyPath, "Bt_First.MyTest");
            PushButton? pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "Hello World";

            return Result.Succeeded;
        }

        public class MyTest : IExternalCommand
        {

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                var uiapp = commandData.Application;
                var uidoc = uiapp.ActiveUIDocument;

                // Show a simple dialog
                TaskDialog.Show("Revit", "Hello World");

                return Result.Succeeded;
                throw new NotImplementedException();
            }
        }
    }
