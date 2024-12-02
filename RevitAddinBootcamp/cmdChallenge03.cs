using Autodesk.Revit.DB.Architecture;

namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge03 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 03 Challenge code goes here

            // 1. define room furniture 
            List<RoomFurniture> furnitureList = GetRoomFurniture();
            int totalCounter = 0;

            // 2. get all rooms
            FilteredElementCollector roomCollector = new FilteredElementCollector(doc);
            roomCollector.OfCategory(BuiltInCategory.OST_Rooms);

            // 3. loop through rooms and insert furniture
            using(Transaction t = new Transaction(doc))
            {
                t.Start("Move in!");
                foreach(Room curRoom in roomCollector)
                {
                    int counter = 0;
                
                    // 3a. get room data
                    string roomName = curRoom.Name;
                    LocationPoint roomLoc = curRoom.Location as LocationPoint;
                    XYZ roomPoint = roomLoc.Point;

                    // 3b. loop through roomFurniture to find match
                    foreach (RoomFurniture curRoomFurn in furnitureList)
                    {
                        if(curRoom.Name.Contains(curRoomFurn.RoomName))
                        {
                            // 3c. get family and activate
                            FamilySymbol curFS = GetFamilySymbol(doc, curRoomFurn.FamilyName, curRoomFurn.FamilyType);
                            curFS.Activate();
                            
                            if (curFS != null)
                            {
                                // 3d. loop through quantity and insert families
                                for(int i = 1; i <= curRoomFurn.Quantity; i++)
                                {
                                    FamilyInstance curFI = doc.Create.NewFamilyInstance(roomPoint, curFS, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                                    counter++;
                                    totalCounter++;

                                    Parameter roomCountParam = curRoom.LookupParameter("Furniture Count");
                                    roomCountParam.Set(counter);
                                }
                            }

                        }
                    }

                    // 4. update furniture count for room
                    Parameter roomCount = curRoom.LookupParameter("Furniture Count");
                    roomCount.Set(counter);
                }
                t.Commit();
            }

            // 5. alert user
            TaskDialog.Show("Complete", $"You moved {totalCounter} pieces of furniture into the building.");

            return Result.Succeeded;
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName, string familyType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach(FamilySymbol curFS in collector)
            {
                if (curFS.FamilyName == familyName && curFS.Name == familyType)
                    return curFS;
            }

            return null;
        }

        private RoomFurniture GetFurnitureForRoom(string name, List<RoomFurniture> furnitureList)
        {
            foreach (RoomFurniture roomFurn in furnitureList)
            {
                if (roomFurn.RoomName == name)
                    return roomFurn;
            }
            return null;
        }

        internal List<RoomFurniture> GetRoomFurniture()
        {
            List<RoomFurniture> roomFurnitureList = new List<RoomFurniture>();

            // Instantiating objects for each row in the tabular data
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
