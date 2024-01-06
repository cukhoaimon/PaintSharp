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
            int endPointPicker = (Points[1].X - Points[0].X < 0) ? 1 : 0;

            double width = Math.Abs(Points[1].X - Points[0].X);
            double height = Math.Abs(Points[1].Y - Points[0].Y);

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
          
            Canvas.SetLeft(element, Points[endPointPicker].X);
            Canvas.SetTop(element, Points[endPointPicker].Y);

            return element;
        }

        public override IShape Clone()
        {
            return new EllipseShape();
        }

        public override string Name => "Ellipse";
    }

}
