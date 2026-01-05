using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Globalization;
using Emdep.Geos.UI.Common;
using System.Windows.Media;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class CustomTrackBarHelper : TickBar
    {
        private IDictionary<double, string> CaptionDict = new Dictionary<double, string>();
        SolidColorBrush myBrush;
        public CustomTrackBarHelper()
        {
            CaptionDict.Add(0, "Scan Item");
            CaptionDict.Add(1, "Scan Location");
            CaptionDict.Add(2, "Done");

        }

        protected override void OnRender(DrawingContext dc)
        {

            FormattedText formattedText = null;

            double maximum = this.Maximum - this.Minimum;
            double y = this.ReservedSpace * 0.5;
            double x = 0;

            if (this.Maximum == 2)
            {
                var sad = CaptionDict[0];

                for (double i = this.Minimum; i <= this.Maximum; i += this.TickFrequency)
                {

                    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() != null)
                    {
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            myBrush = new SolidColorBrush(Colors.Black);

                        else
                            myBrush = new SolidColorBrush(Colors.White);
                    }
                    formattedText = new FormattedText(CaptionDict[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 15, myBrush);
                    formattedText.SetForegroundBrush(myBrush);
                    formattedText.SetFontFamily(GeosApplication.Instance.FontFamilyAsPerTheme);
                    if (this.Minimum == i)
                        x = 0;
                    else
                        x += ((this.ActualWidth / (maximum / this.TickFrequency)) - formattedText.Width / 2);
                    dc.DrawText(formattedText, new Point(x, -2));
                }
            }
            else base.OnRender(dc);

        }
    }
}
