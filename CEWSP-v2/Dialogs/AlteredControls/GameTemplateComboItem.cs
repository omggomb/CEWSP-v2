using CEWSP_Backend;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CEWSP_v2.Dialogs.AlteredControls
{
    internal class GameTemplateComboItem : ComboBoxItem
    {
        public GameTemplate AssociatedGameTemplate { get; set; }

        public void ConstructHeader()
        {
            var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

            string sImgPath = AssociatedGameTemplate.Directory.FullName + "\\" + AssociatedGameTemplate.IconPath;
            var bitImg = Utillities.DefaultProjectBitmap;
            if (File.Exists(sImgPath))
                bitImg = new BitmapImage(new Uri(sImgPath));

            var img = new Image() { Width = 64, Height = 64, Source = bitImg };
            var textB = new TextBlock()
            {
                Text = AssociatedGameTemplate.Name,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(10, 0, 0, 0)
            };

            stackPanel.Children.Add(img);
            stackPanel.Children.Add(textB);

            Content = stackPanel;
        }
    }
}