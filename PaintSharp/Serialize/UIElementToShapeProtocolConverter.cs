using System.Windows.Media;

namespace PaintSharp.Serialize
{
    public static class UIElementToShapeProtocolConverter
    {
        public static string StrokeToString(SolidColorBrush stroke)
        {
            return stroke.Color.ToString();
        }
       

        public static string? FillToString(SolidColorBrush? fill)
        {
            if (fill == null) return null;

            // else
            return fill.Color.ToString();
        }

        public static string? StrokeDashArrayToString(DoubleCollection? doubles)
        {
            if (doubles == null) return null;

            return doubles.ToString();
        }
        public static SolidColorBrush StringToBrush(string stringColor)
        {
            var color = (Color)ColorConverter.ConvertFromString(stringColor);
            return new SolidColorBrush(color);
        }

        public static DoubleCollection? StringToStrokeDashArray(string? doubles)
        {
            if (doubles == null) return null;
            var values = new DoubleCollection
            {
                Double.Parse(doubles)
            };
            
            return values;
        }

        public static System.Windows.Point StringToPoint(string x, string y)
        {
            return new System.Windows.Point(Double.Parse(x), Double.Parse(y));
        } 
    }
}
