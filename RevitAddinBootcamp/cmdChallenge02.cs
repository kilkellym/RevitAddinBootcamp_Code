using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using System.Windows.Media;

namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // 1. prompt user to select elements
            TaskDialog.Show("Select lines", "Select some lines to convert to Revit elements.");
            List<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select some elements").ToList();

            // 2. filter selected elements
            List<CurveElement> filteredList = new List<CurveElement>();

            foreach (Element element in pickList)
            {
                if (element is CurveElement)
                {
                    CurveElement curCurveElement = element as CurveElement;
                    filteredList.Add(curCurveElement);
                }
            }

            // 3. get level
            View curView = doc.ActiveView;
            Parameter levelParam = curView.LookupParameter("Associated Level");
            Parameter levelParam2 = curView.get_Parameter(BuiltInParameter.ASSOCIATED_LEVEL);
            string levelName = levelParam.AsString();
            ElementId levelId = levelParam.AsElementId();

            Level currentLevel = GetLevelByName(doc, levelName);

            // 4. get types
            WallType wt1 = GetWallTypeByName(doc, "Storefront");
            WallType wt2 = GetWallTypeByName(doc, "Generic - 8\"");
            MEPSystemType ductSystem = GetMEPSystemByName(doc, "Supply Air");
            MEPSystemType pipeSystem = GetMEPSystemByName(doc, "Domestic Hot Water");
            DuctType ductType = GetDuctTypeByName(doc, "Default");
            PipeType pipeType = GetPipeTypeByname(doc, "Default");

            // 5. loop through curve elements 
            using(Transaction t = new Transaction(doc))
            {
                t.Start("Create elements");

                foreach(CurveElement currentCurve in filteredList)
                {
                    // 6. get graphicstyle from curve
                    GraphicsStyle currentStyle = currentCurve.LineStyle as GraphicsStyle;
                    string lineStyleName = currentStyle.Name;

                    // 6b. get curve geometry
                    Curve curveGeom = currentCurve.GeometryCurve;

                    if (currentCurve is Arc)
                        continue;

                    XYZ startPoint = curveGeom.GetEndPoint(0);
                    XYZ endPoint = curveGeom.GetEndPoint(1);

                    // 7. use switch statement to create elements
                    switch (lineStyleName)
                    {
                        case "A-GLAZ":
                            Wall wall1 = Wall.Create(doc, curveGeom, wt1.Id,
                                currentLevel.Id, 20, 0, false, false);
                            break;

                        case "A-WALL":
                            Wall wall2 = Wall.Create(doc, curveGeom, wt2.Id,
                                currentLevel.Id, 20, 0, false, false);
                            break;

                        case "M-DUCT":
                            Duct duct = Duct.Create(doc, ductSystem.Id, ductType.Id,
                                currentLevel.Id, curveGeom.GetEndPoint(0), curveGeom.GetEndPoint(1));
                            break;

                        case "P-PIPE":
                            Pipe pipe = Pipe.Create(doc, pipeSystem.Id, pipeType.Id,
                                currentLevel.Id, curveGeom.GetEndPoint(0), curveGeom.GetEndPoint(1));
                            break;

                        default:
                            break;
                    }
                }

                t.Commit();
            }

            return Result.Succeeded;
        }

        private PipeType GetPipeTypeByname(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(PipeType));

            foreach (PipeType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null;
        }

        private DuctType GetDuctTypeByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(DuctType));

            foreach (DuctType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null;
        }

        private MEPSystemType GetMEPSystemByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(MEPSystemType));

            foreach (MEPSystemType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null;
        }

        private WallType GetWallTypeByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            //collector.OfCategory(BuiltInCategory.OST_Walls);
            //collector.WhereElementIsElementType();
            collector.OfClass(typeof(WallType));

            foreach (WallType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null;
        }

        internal Level GetLevelByName(Document doc, string levelName)
        {
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc);
            levelCollector.OfCategory(BuiltInCategory.OST_Levels);
            levelCollector.WhereElementIsNotElementType();

            foreach (Level curLevel in levelCollector)
            {
                if (curLevel.Name == levelName)
                {
                    return curLevel;
                }
            }

            return null;
        }

        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge02";
            string buttonTitle = "Module\r02";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module02,
                Properties.Resources.Module02,
                "Module 02 Challenge");

            return myButtonData.Data;
        }
    }

}
