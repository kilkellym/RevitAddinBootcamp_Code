# Revit Add-in Bootcamp: Module 03 Cheat Sheet

## Classes

### Creating a Basic Class
```csharp
public class Building 
{
    // Properties
    public string Name { get; set; }
    public string Address { get; set; }
    public int NumberOfFloors { get; set; }
    public double Area { get; set; }
}
```

### Adding a Constructor
```csharp
public class Building
{
    public Building(string _name, string _address, int _numberOfFloors, double _area)
    {
        Name = _name;
        Address = _address;
        NumberOfFloors = _numberOfFloors;
        Area = _area;
    }
}
```

### Creating Class Instance
```csharp
// Using constructor
Building building1 = new Building("Office Tower", "123 Main St", 10, 150000);

// Create list of class instances 
List<Building> buildings = new List<Building>();
buildings.Add(building1);
```

## Rooms and Spatial Elements

### Getting Rooms
```csharp
// Get all rooms
FilteredElementCollector collector = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_Rooms);

// Get room location point
LocationPoint locPoint = (LocationPoint)room.Location;
XYZ roomPoint = locPoint.Point;
```

### String Contains Check
```csharp
// Check room name contains text
if (roomName.Contains("Office"))
{
    // Room name contains "Office"
}
```

## Working with Families

### Family Hierarchy 

1. Family (RFA file)
2. FamilySymbol (Type) 
3. FamilyInstance (Placed in model)

### Get Family Symbol
```csharp
internal FamilySymbol GetFamilySymbolByName(Document doc, string familyName, string symbolName)
{
    FilteredElementCollector collector = new FilteredElementCollector(doc)
        .OfClass(typeof(FamilySymbol));
    
    foreach(FamilySymbol symbol in collector)
    {
        if(symbol.FamilyName == familyName && symbol.Name == symbolName)
            return symbol;
    }
    return null;
}
```

### Place Family Instance
```csharp
// Activate symbol before placing
familySymbol.Activate();

// Create instance at point
FamilyInstance instance = doc.Create.NewFamilyInstance(
    point,
    familySymbol, 
    StructuralType.NonStructural);
```

## Parameters

### Get Parameter Value
```csharp
public static string GetParameterValueAsString(Element element, string paramName)
{
    Parameter param = element.LookupParameter(paramName);
    if(param != null)
        return param.AsString();
    return "";
}

// For built-in parameters
public static string GetParameterValueAsString(Element element, BuiltInParameter bip)
{
    Parameter param = element.get_Parameter(bip);
    if(param != null)
        return param.AsString();
    return "";
}
```

### Set Parameter Value
```csharp
public static void SetParameterValue(Element element, string paramName, string value)
{
    Parameter param = element.LookupParameter(paramName);
    if(param != null)
        param.Set(value);
}
```

## Static Classes and Methods

### Converting Class to Static
```csharp
// Static class declaration
public static class Utils
{
    // Static method
    public static string GetParameterValueAsString(Element elem, string paramName)
    {
        // Method implementation
    }
}

// Using static method
Utils.GetParameterValueAsString(element, "Height");
```

### Add Using Statement
```csharp
// Add namespace to top of file
using RevitAddinBootcamp.Common;

// Now can use Utils directly
Utils.GetParameterValueAsString(element, "Height");
```

## Common Errors

1. Cannot declare instance members in static class - Add static keyword to methods
2. Method not found - Add using statement or use full namespace path
3. Family symbol not activated - Call Activate() before placing an instance
4. Parameter returns null - Check parameter name/built-in parameter exists
