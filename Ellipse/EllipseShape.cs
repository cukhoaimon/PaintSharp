using System.Windows;
using US_IShape;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace US_Ellipse
{
    public class EllipseShape : IShape
    {
        public EllipseShape() {
            Preview = new System.Windows.Media.Imaging.BitmapImage();
            Preview.BeginInit();
            Preview.UriSource = new Uri(
                RelativeToAbsoluteConverter.Convert(@"/ellipse.png"),
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

        public override IShape Clone()
        {
            return new EllipseShape();
        }

        public override string Name => "Ellipse";
    }

}
