using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;

namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge03 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // THIS IS MY TEST CHANGE
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            int totalCounter = 0;

            // 1. define room furniture data
            List<RoomFurniture> furnitureList = GetRoomFurniture();

            // 2. get all the rooms
            FilteredElementCollector roomCollector = new FilteredElementCollector(doc);
            roomCollector.OfCategory(BuiltInCategory.OST_Rooms);

            // 3. loop through rooms and insert the furniture
            using(Transaction t = new Transaction(doc))
            {
                t.Start("Move in!");
                foreach(Room curRoom in roomCollector)
                {
                    int roomCounter = 0;

                    // 3a. get room data
                    string roomName = curRoom.Name;
                    LocationPoint roomLoc = curRoom.Location as LocationPoint;
                    XYZ roomPoint = roomLoc.Point;

                    // 3b. loop through furnitureList and find matching room
                    foreach (RoomFurniture curRoomFurn in furnitureList)
                    {
                        if(roomName.Contains(curRoomFurn.RoomName))
                        {
                            // 3c. get family symbol and activate
                            FamilySymbol curFS = GetFamilySymbol(doc, curRoomFurn.FamilyName, curRoomFurn.FamilyType);
                            
                            // 3d. check for null
                            if(curFS == null)
                            {
                                TaskDialog.Show("Error", "Could not find specified family. Check your family data.");
                                continue;
                            }

                            curFS.Activate();

                            // 3e. loop through quantity number and insert families
                            for(int i = 1; i <= curRoomFurn.Quantity; i++)
                            {
                                FamilyInstance curFI = doc.Create.NewFamilyInstance(roomPoint, curFS, StructuralType.NonStructural);
                                roomCounter++;
                                totalCounter++;
                            }
                        }
                    }

                    //4. update furniture count for room
                    Parameter roomCount = curRoom.LookupParameter("Furniture Count");

                    if(roomCount != null)
                    {
                        roomCount.Set(roomCounter);
                    }

                }

                t.Commit();
            }

            //5. alert user
            TaskDialog.Show("Complete", $"You moved {totalCounter} pieces of furniture into the building. Great work!");

            return Result.Succeeded;
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName, string familyType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach(FamilySymbol curFS in collector)
            {
                if(curFS.FamilyName == familyName && curFS.Name == familyType)
                    return curFS;
            }

            return null;
        }

        private List<RoomFurniture> GetRoomFurniture()
        {
            // Create a list to hold RoomFurniture instances
            List<RoomFurniture> roomFurnitureList = new List<RoomFurniture>();

            // Add instances directly to the list
            roomFurnitureList.Add(new RoomFurniture("Classroom", "Desk", "Teacher", 1));
            roomFurnitureList.Add(new RoomFurniture("Classroom", "Desk", "Student", 6));
            roomFurnitureList.Add(new RoomFurniture("Classroom", "Chair-Desk", "Default", 7));
            roomFurnitureList.Add(new RoomFurniture("Classroom", "Shelf", "Large", 1));
            roomFurnitureList.Add(new RoomFurniture("Office", "Desk", "Teacher", 1));
            roomFurnitureList.Add(new RoomFurniture("Office", "Chair-Executive", "Default", 1));
            roomFurnitureList.Add(new RoomFurniture("Office", "Shelf", "Small", 1));
            roomFurnitureList.Add(new RoomFurniture("Office", "Chair-Task", "Default", 1));
            roomFurnitureList.Add(new RoomFurniture("VR Lab", "Table-Rectangular", "Small", 1));
            roomFurnitureList.Add(new RoomFurniture("VR Lab", "Table-Rectangular", "Large", 8));
            roomFurnitureList.Add(new RoomFurniture("VR Lab", "Chair-Task", "Default", 9));
            roomFurnitureList.Add(new RoomFurniture("Computer Lab", "Desk", "Teacher", 1));
            roomFurnitureList.Add(new RoomFurniture("Computer Lab", "Desk", "Student", 10));
            roomFurnitureList.Add(new RoomFurniture("Computer Lab", "Chair-Desk", "Default", 11));
            roomFurnitureList.Add(new RoomFurniture("Computer Lab", "Shelf", "Large", 2));
            roomFurnitureList.Add(new RoomFurniture("Student Lounge", "Sofa", "Large", 2));
            roomFurnitureList.Add(new RoomFurniture("Student Lounge", "Table-Coffee", "Square", 2));
            roomFurnitureList.Add(new RoomFurniture("Teacher Lounge", "Sofa", "Small", 2));
            roomFurnitureList.Add(new RoomFurniture("Teacher Lounge", "Table-Coffee", "Large", 1));
            roomFurnitureList.Add(new RoomFurniture("Waiting", "Chair-Waiting", "Default", 2));
            roomFurnitureList.Add(new RoomFurniture("Waiting", "Table-Coffee", "Large", 1));

            return roomFurnitureList;



        }

        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge03";
            string buttonTitle = "Module\r03";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module03,
                Properties.Resources.Module03,
                "Module 03 Challenge");

            return myButtonData.Data;
        }
    }





}
