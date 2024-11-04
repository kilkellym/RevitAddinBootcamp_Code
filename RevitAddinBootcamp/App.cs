namespace RevitAddinBootcamp
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            // 1. Set up ribbon tab and panel names
            string tabName = "Revit Add-in Bootcamp";
            string panelName = "Challenges";

            // 1. Create ribbon tab
            app.CreateRibbonTab(tabName);

            // 2. Create ribbon panel 
            RibbonPanel panel = Common.Utils.CreateRibbonPanel(app, tabName, panelName);

            // 3. Create button data instances
            PushButtonData btnData1 = cmdChallenge01.GetButtonData();
            PushButtonData btnData2 = cmdChallenge02.GetButtonData();
            PushButtonData btnData3 = cmdChallenge03.GetButtonData();
            PushButtonData btnData4 = cmdChallenge04.GetButtonData();

            // 4. Create buttons
            PushButton myButton1 = panel.AddItem(btnData1) as PushButton;
            PushButton myButton2 = panel.AddItem(btnData2) as PushButton;
            PushButton myButton3 = panel.AddItem(btnData3) as PushButton;
            PushButton myButton4 = panel.AddItem(btnData4) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
