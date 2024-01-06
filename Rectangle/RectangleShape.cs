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
            int endPointPicker = (Points[1].X - Points[0].X < 0) ? 1 : 0;
            double width = Math.Abs(Points[1].X - Points[0].X);
            double height = Math.Abs(Points[1].Y - Points[0].Y);

            var element = new Rectangle
            {
                Fill= Configuration?.Fill,
                Stroke = Configuration?.Stroke,
                StrokeDashArray = Configuration?.StrokeDashArray,
                StrokeThickness = Configuration == null ? 1.0 : Configuration.StrokeThickness,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(element, Points[endPointPicker].X);
            Canvas.SetTop(element, Points[endPointPicker].Y);

            return element;
        }

        public override IShape Clone()
        {
            return new RectangleShape();
        }

        public override string Name => "Rectangle";
    }
}
