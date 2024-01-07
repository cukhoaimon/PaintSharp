
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using US_IShape;

namespace US_Ellipse
{
    public class CircleShape : IShape
    {
        public override string Name => "Circle";
        public CircleShape() {
            Preview = new System.Windows.Media.Imaging.BitmapImage();
            Preview.BeginInit();
            Preview.UriSource = new Uri(
                RelativeToAbsoluteConverter.Convert(@"/circle.png"),
                UriKind.RelativeOrAbsolute);
            Preview.EndInit();
        }

        public override IShape Clone()
        {
            return new CircleShape();
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
            var height = width;

            var element = new Ellipse
            {
                Stroke = Configuration?.Stroke,
                StrokeDashArray = Configuration?.StrokeDashArray,
                StrokeThickness = Configuration == null ? 1.0 : Configuration.StrokeThickness,
                Fill = Configuration?.Fill,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);

            return element;
        }
    }
}
