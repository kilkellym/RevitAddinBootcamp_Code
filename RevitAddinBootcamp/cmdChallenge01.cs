namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // FIZZBUZZ Challenge 

            // 1. set variables
            int numFloors = 250;
            double currentElev = 0;
            int floorHeight = 15;
            int fizzCount = 0;
            int buzzCount = 0;
            int fizzbuzzCount = 0;

            // 2. get titleblock
            FilteredElementCollector tbCollector = new FilteredElementCollector(doc);
            tbCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            tbCollector.WhereElementIsElementType();
            ElementId tblockId = tbCollector.FirstElementId();

            // 3. get all view family types
            FilteredElementCollector vftCollector = new FilteredElementCollector(doc);
            vftCollector.OfClass(typeof(ViewFamilyType));

            // 4. get floor plan and ceiling plan view family types 
            ViewFamilyType fpVFT = null;
            ViewFamilyType cpVFT = null;

            foreach (ViewFamilyType curVFT in vftCollector)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    fpVFT = curVFT;
                }
                else if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    cpVFT = curVFT;
                }
            }

            // 4b. check for null values
            if(fpVFT == null)
            {   
                TaskDialog.Show("Error", "There are no floor plan view family types in the model.");
                return Result.Failed;
            }
            else if(cpVFT == null)
            {
                TaskDialog.Show("Error", "There are no ceiling plan view family types in the model.");
                return Result.Failed;
            }

            // 5. create transaction
            Transaction t = new Transaction(doc);
            t.Start("FIZZ BUZZ Challenge");

            // 6. create floors and check FIZZBUZZ
            for (int i = 0; i <= numFloors; i++)
            {
                // 7. create level
                Level newLevel = Level.Create(doc, currentElev);

                if(i < 10)
                    newLevel.Name = $"LEVEL 0{i}";
                else
                    newLevel.Name = $"LEVEL {i}";

                // 8. check for FIZZ, BUZZ, and FIZZBUZZ
                if (i % 3 == 0 && i % 5 == 0)
                {
                    // 8a. FIZZBUZZ - create sheet
                    ViewSheet newSheet = ViewSheet.Create(doc, tblockId);
                    newSheet.SheetNumber = i.ToString();
                    //newSheet.Name = "FIZZBUZZ_" + i.ToString();
                    newSheet.Name = $"FIZZBUZZ_{i}";

                    // BONUS
                    ViewPlan bonusPlan = ViewPlan.Create(doc, fpVFT.Id, newLevel.Id);
                    bonusPlan.Name = $"FIZZBUZZ_{i}";
                    Viewport newVP = Viewport.Create(doc, newSheet.Id, bonusPlan.Id, new XYZ(1, .5, 0));

                    fizzbuzzCount++;
                }
                else if (i % 3 == 0)
                {
                    // 8b. FIZZ - create floor plan
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, fpVFT.Id, newLevel.Id);
                    newFloorPlan.Name = $"FIZZ_{i}";

                    fizzCount ++;
                }
                else if (i % 5 == 0)
                {
                    // 8c. BUZZ - create ceiling plan
                    ViewPlan newCeilingPlan = ViewPlan.Create(doc, cpVFT.Id, newLevel.Id);
                    newCeilingPlan.Name = $"BUZZ_{i}";

                    buzzCount ++;
                }

                // 9. increment elevation
                currentElev += floorHeight;
                //currentElev = currentElev + floorHeight;
            }

            // 10. commit and close transaction
            t.Commit();
            t.Dispose();

            // 11. alert user
            TaskDialog.Show("Complete", $"Created {numFloors} levels." +
                $"Created {fizzCount} FIZZ views. " +
                $"Create {buzzCount} BUZZ views. " +
                $"Created {fizzbuzzCount} FIZZBUZZ sheets.");

            return Result.Succeeded;
        }

        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge01";
            string buttonTitle = "Module\r01";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module01,
                Properties.Resources.Module01,
                "Module 01 Challenge");

            return myButtonData.Data;
        }
    }

}
