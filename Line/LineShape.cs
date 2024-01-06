using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using US_IShape;

namespace US_Line
{
    public class LineShape : IShape
    {
        public LineShape() {
            Preview = new System.Windows.Media.Imaging.BitmapImage();
            Preview.BeginInit();
            Preview.UriSource = new Uri(
                RelativeToAbsoluteConverter.Convert(@"/line.png"),
                UriKind.RelativeOrAbsolute);
            Preview.EndInit();
        }

        public override UIElement Draw()
        {
            return new Line()
            {
                X1 = Points[0].X,
                Y1 = Points[0].Y,
                X2 = Points[1].X,
                Y2 = Points[1].Y,
                Stroke = Configuration?.Stroke,
                StrokeThickness = Configuration == null ? 1.0 : Configuration.StrokeThickness,
                StrokeDashArray = Configuration?.StrokeDashArray,
                Fill = Configuration?.Fill,
            };
        }

        public override IShape Clone()
        {
            return new LineShape();
        }

        public override string Name => "Line";
    }

}
