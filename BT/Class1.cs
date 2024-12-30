using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Reflection;


namespace BT
{
    public class Class1 : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application) { 
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("My Ribbon Panel");
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData = new PushButtonData("cmdMyTest", "My Test", thisAssemblyPath, "BT.MyTest");
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "Hello World";

            return Result.Succeeded;
        }

        public class MyTest : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                var uiapp = commandData.Application;
                var uidoc = uiapp.ActiveUIDocument;

                TaskDialog.Show("Revit", "Hello World");

                return Result.Succeeded;
                throw new NotImplementedException();
            }
        }
    }
}
