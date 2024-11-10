# Revit Add-in Bootcamp: Module 01 Cheat Sheet

## Quick Links
- [C# Basics](#c-basics) (Video: 00:00 - 29:00)
- [Revit API Essentials](#revit-api-essentials) (Video: 29:00 - 57:00) 
- [Challenge Tips](#fizzbuzz-challenge-tips)
- [Common Errors](#common-errors)

## C# Basics

### Variables (04:12)
```csharp
// Numbers
int wholeNumber = 42;
double decimalNumber = 42.5;

// Text
string text = "Hello";

// Increment number
wholeNumber++;  // adds 1
wholeNumber += 5;  // adds 5
```

### Math Operations (11:23)
```csharp
// Basic math
int sum = 5 + 3;      // addition
int diff = 5 - 3;     // subtraction
int product = 5 * 3;  // multiplication
int quotient = 6 / 2; // division

// Check remainder (modulo)
int remainder = 10 % 3;  // equals 1
```

### Conditional Logic (18:17)
```csharp
// Check if equal
if (number == 5) 
{
    // do something
}

// Check multiple conditions
if (number % 3 == 0 && number % 5 == 0) 
{
    // divisible by both 3 and 5
}

// If-else statement
if (condition) 
{
    // do something
} 
else 
{
    // do something else
}
```

### Loops (28:32)
```csharp
// Loop from 1 to 10
for (int i = 1; i <= 10; i++) 
{
    // code repeats 10 times
}
```

## Revit API Essentials

### Transaction Basics (42:27)
```csharp
// Start a transaction
using (Transaction t = new Transaction(doc))
{
    t.Start("Create Elements");
    
    // Create elements here
    
    t.Commit();
}
```

### Create Level (45:13)
```csharp
// Create a level at specified height (in decimal feet)
Level newLevel = Level.Create(doc, elevation);

// Change level name
newLevel.Name = "Level " + number.ToString();
```

### Create Views (48:57)
```csharp
// Create floor plan
ViewFamilyType floorPlanType = GetFloorPlanType(doc);
ViewPlan floorPlan = ViewPlan.Create(doc, floorPlanType.Id, newLevel.Id);
floorPlan.Name = "FIZZ_" + number.ToString();

// Create ceiling plan
ViewFamilyType ceilingPlanType = GetCeilingPlanType(doc);
ViewPlan ceilingPlan = ViewPlan.Create(doc, ceilingPlanType.Id, newLevel.Id);
ceilingPlan.Name = "BUZZ_" + number.ToString();
```

### Create Sheets (52:31)
```csharp
// Get title block
FilteredElementCollector collector = new FilteredElementCollector(doc);
Element titleBlock = collector
    .OfCategory(BuiltInCategory.OST_TitleBlocks)
    .FirstElement();

// Create sheet
ViewSheet sheet = ViewSheet.Create(doc, titleBlock.Id);
sheet.Name = "FIZZBUZZ_" + number.ToString();
```

### Helper Methods (50:22)
```csharp
// Get floor plan type
private ViewFamilyType GetFloorPlanType(Document doc)
{
    return new FilteredElementCollector(doc)
        .OfClass(typeof(ViewFamilyType))
        .Cast()
        .FirstOrDefault(x => x.ViewFamily == ViewFamily.FloorPlan);
}

// Get ceiling plan type
private ViewFamilyType GetCeilingPlanType(Document doc)
{
    return new FilteredElementCollector(doc)
        .OfClass(typeof(ViewFamilyType))
        .Cast()
        .FirstOrDefault(x => x.ViewFamily == ViewFamily.CeilingPlan);
}
```

## FizzBuzz Challenge Tips

1. Create a loop that goes from 1 to your target number (28:32)
2. Use modulo (`%`) to check if number is divisible by 3 or 5 (15:46)
3. Check for FizzBuzz condition first (divisible by both) (19:23)
4. Put all element creation inside a transaction (42:27)
5. Remember to increment elevation for each new level (45:13)
6. Use meaningful names for views and sheets (49:15)

## Common Errors

1. Missing semicolon - Check end of lines (06:54)
2. Transaction error - Ensure element creation is inside transaction (43:18)
3. Null reference - Check that types (floor plan, ceiling plan) exist (51:03)
4. Cannot modify element - Make sure transaction is started (42:27)

## Video Section Overview
- C# Fundamentals: 00:00 - 29:00
- Revit API Basics: 29:00 - 57:00
- Demo and Testing: 57:00 - end
