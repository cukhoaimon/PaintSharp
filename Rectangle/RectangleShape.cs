using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using US_IShape;

namespace US_Rectangle
{
    public class RectangleShape : IShape
    {
        public RectangleShape() {
            Preview = new System.Windows.Media.Imaging.BitmapImage();
            Preview.BeginInit();
            Preview.UriSource = new Uri(
                RelativeToAbsoluteConverter.Convert(@"/rectangle.png"),
                UriKind.RelativeOrAbsolute);
            Preview.EndInit();
        }
        public override UIElement Draw()
        {
            var start = Points[0];
            var end = Points[1];

            var left = Math.Min(end.X, start.X);
            var top = Math.Min(end.Y, start.Y);

            var right = Math.Max(end.X, start.X);
            var bottom = Math.Max(end.Y, start.Y);

            var width = right - left;
            var height = bottom - top;

            var element = new Rectangle
            {
                Fill= Configuration?.Fill,
                Stroke = Configuration?.Stroke,
                StrokeDashArray = Configuration?.StrokeDashArray,
                StrokeThickness = Configuration == null ? 1.0 : Configuration.StrokeThickness,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);

            return element;
        }

        public override IShape Clone()
        {
            return new RectangleShape();
        }

        public override string Name => "Rectangle";
    }
}
