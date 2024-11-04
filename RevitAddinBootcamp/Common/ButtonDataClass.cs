using System.Windows.Media.Imaging;

namespace RevitAddinBootcamp.Common
{
    internal class ButtonDataClass
    {
        public PushButtonData Data { get; set; }
        public ButtonDataClass(string name, string text, string className,
            byte[] largeImage,
            byte[] smallImage,
            string toolTip)
        {
            Data = new PushButtonData(name, text, GetAssemblyName(), className);
            Data.ToolTip = toolTip;

            Data.LargeImage = ConvertToImageSource(largeImage);
            Data.Image = ConvertToImageSource(smallImage);

            // set command availability
            string nameSpace = this.GetType().Namespace;
            Data.AvailabilityClassName = $"{nameSpace}.CommandAvailability";
        }
        public ButtonDataClass(string name, string text, string className,
            byte[] largeImage,
            byte[] smallImage,
            byte[] largeImageDark,
            byte[] smallImageDark,
            string toolTip)
        {
            Data = new PushButtonData(name, text, GetAssemblyName(), className);
            Data.ToolTip = toolTip;

            // add check for light vs dark mode
            UITheme theme = UIThemeManager.CurrentTheme;
            if (theme == UITheme.Dark)
            {
                Data.LargeImage = ConvertToImageSource(largeImageDark);
                Data.Image = ConvertToImageSource(smallImageDark);
            }
            else
            {
                Data.LargeImage = ConvertToImageSource(largeImage);
                Data.Image = ConvertToImageSource(smallImage);
            }

            // set command availability
            string nameSpace = this.GetType().Namespace;
            Data.AvailabilityClassName = $"{nameSpace}.CommandAvailability";
        }
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
        public static string GetAssemblyName()
        {
            return GetAssembly().Location;
        }

        public static BitmapImage ConvertToImageSource(byte[] imageData)
        {
            using (MemoryStream mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = mem;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();

                return bmi;
            }
        }
    }
}
