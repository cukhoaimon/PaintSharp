
using System.Windows;
using System.Windows.Controls;
using US_IShape;

namespace US_Square
{
    public class SquareShape : IShape
    {
        public override string Name => "Square";
        public SquareShape()
        {
            Preview = new System.Windows.Media.Imaging.BitmapImage();
            Preview.BeginInit();
            Preview.UriSource = new Uri(
                RelativeToAbsoluteConverter.Convert(@"/square.png"),
                UriKind.RelativeOrAbsolute);
            Preview.EndInit();
        }

        public override IShape Clone()
        {
            return new SquareShape();
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

            var element = new System.Windows.Shapes.Rectangle()
            {
                Fill = Configuration?.Fill,
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
    }

}
