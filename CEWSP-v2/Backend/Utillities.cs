﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media;

using CEWSP_v2.Definitions;

namespace CEWSP_v2.Backend
{
    public class Utillities
    {
        /// <summary>
        /// Constructs a tooltip with the main message at the top.
        /// Each submessage gets its own icon
        /// </summary>
        /// <param name="sMainMessage"></param>
        /// <param name="asSubMessages"></param>
        /// <returns></returns>
        public static ToolTip ConstructToolTip(string sMainMessage, params string[] asSubMessages)
        {
            var tip = new ToolTip();

            var topStack = new StackPanel() { Orientation = Orientation.Vertical };

            var mainMessage = new TextBlock() { Text = sMainMessage };


            topStack.Children.Add(mainMessage);

            if (asSubMessages.Count() > 0)
            {
                mainMessage.FontWeight = System.Windows.FontWeights.Bold;

                foreach (var subMessage in asSubMessages)
                {
                    var subStack = new StackPanel() { Orientation = Orientation.Horizontal };

                    var bit = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/info-icon-64.png", UriKind.Relative));
                    var img = new Image()
                    {
                        Source = bit,
                        Width = 20,
                        Height = 20
                    };

                    subStack.Children.Add(img);
                    subStack.Children.Add(new TextBlock() { Text = subMessage, VerticalAlignment = System.Windows.VerticalAlignment.Center });

                    topStack.Children.Add(subStack);
                }
            }


            tip.Content = topStack;

            return tip;
        }
    }
}
