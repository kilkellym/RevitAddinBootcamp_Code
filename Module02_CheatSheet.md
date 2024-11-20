#Revit Add-in Bootcamp: Module 02 Cheat Sheet

##Select Elements
###Pick Single Element
'''csharp
// Get UI document
UIDocument uidoc = uiapp.ActiveUIDocument;

// Prompt user to select element
Reference pickRef = uidoc.Selection.PickObject(
    ObjectType.Element, 
    "Select element");

// Get element from reference
Element pickElement = doc.GetElement(pickRef);
'''

###Pick Multiple Elements
'''csharp
// Prompt user to select elements by rectangle
List<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select elements").ToList();
'''

##Filter Elements
###Filter for Curve Elements
'''csharp
// Create list for curve elements
List<CurveElement> allCurves = new List<CurveElement>();

// Filter selected elements
foreach(Element elem in pickList)
{
    if(elem is CurveElement)
    {
        allCurves.Add(elem as CurveElement); 
    }
}
'''

###Filter for Model Curves
'''csharp
List<CurveElement> modelCurves = new List<CurveElement>();

foreach(Element elem in pickList)
{
    if(elem is CurveElement)
    {
        CurveElement curveElem = elem as CurveElement;
        if(curveElem.CurveElementType == CurveElementType.ModelCurve)
        {
            modelCurves.Add(curveElem);
        }
    }
}
Transactions
Using Statement
csharpCopy// Using statement automatically disposes transaction
using(Transaction t = new Transaction(doc))
{
    t.Start("Create Elements");
    
    // Create elements here
    Level newLevel = Level.Create(doc, 20);
    Wall newWall = Wall.Create(doc, curve, newLevel.Id, false);
    
    t.Commit();
} // Transaction automatically disposed here
Traditional Method
csharpCopy// Must manually dispose transaction
Transaction t = new Transaction(doc);
t.Start("Create Elements"); 

// Create elements here
Level newLevel = Level.Create(doc, 20);
Wall newWall = Wall.Create(doc, curve, newLevel.Id, false);

t.Commit();
t.Dispose();
Benefits of using statement:

Automatically disposes transaction
Clearly shows transaction scope
Prevents memory leaks
More concise than traditional method
Good practice for resource management

Create Walls
Using Default Wall Type
csharpCopy// Create wall using curve and level
using(Transaction t = new Transaction(doc))
{
    t.Start("Create Wall");
    
    Wall newWall = Wall.Create(
        doc,
        curve,
        newLevel.Id,
        false);  // not structural
    
    t.Commit();
}
Using Specific Wall Type
csharpCopy// Get wall type
FilteredElementCollector wallTypes = new FilteredElementCollector(doc)
    .OfClass(typeof(WallType));

Wall newWall = Wall.Create(
    doc,
    curve,
    wallTypes.FirstElementId(),
    newLevel.Id,
    20,    // height
    0,     // offset
    false, // flip
    false  // structural
);
Create MEP Elements
Get System Type
csharpCopy// Filter for MEP system types
FilteredElementCollector collector = new FilteredElementCollector(doc)
    .OfClass(typeof(MEPSystemType));

MEPSystemType systemType = null;
foreach(Element type in collector)
{
    if(type.Name == "Supply Air")
    {
        systemType = type as MEPSystemType;
        break;
    }
}
Create Duct
csharpCopy// Get duct type
FilteredElementCollector ductTypes = new FilteredElementCollector(doc)
    .OfClass(typeof(DuctType));

Duct newDuct = Duct.Create(
    doc,
    systemType.Id,
    ductTypes.FirstElementId(),
    level.Id,
    startPoint,
    endPoint
);
Create Pipe
csharpCopy// Get pipe type
FilteredElementCollector pipeTypes = new FilteredElementCollector(doc)
    .OfClass(typeof(PipeType));

Pipe newPipe = Pipe.Create(
    doc,
    systemType.Id,
    pipeTypes.FirstElementId(),
    level.Id,
    startPoint,
    endPoint
);
Using Methods
Custom Method Template
csharpCopyinternal ReturnType MethodName(ArgumentType argument)
{
    // Method code here
    return returnValue;
}
Get Type by Name Method
csharpCopyinternal MEPSystemType GetSystemTypeByName(Document doc, string name)
{
    FilteredElementCollector collector = new FilteredElementCollector(doc)
        .OfClass(typeof(MEPSystemType));
        
    foreach(Element type in collector)
    {
        if(type.Name == name)
            return type as MEPSystemType;
    }
    return null;
}
Switch Statements
csharpCopyswitch(variableToCheck)
{
    case "value1":
        // Do something
        break;
    case "value2":
        // Do something else
        break;
    default:
        // Default action
        break;
}
Common Errors

"Transaction not started" - Ensure element creation is inside transaction
"Object reference not set" - Check for null system types
"Collector has no filter" - Verify collector variable names match
"Cannot convert type" - Use proper casting with 'as' keyword

Tips

Remember to activate family symbols before creating instances
Set Detail Level to Fine to see pipes
Use Debug.Print() for troubleshooting
Break points help inspect variable values
Index 0 is first item in lists/arrays
