using static System.Net.Mime.MediaTypeNames;
using ElementConverter = PaintSharp.Serialize.UIElementToShapeProtocolConverter;


namespace PaintSharp.Serialize
{
    public class ShapeProtocol
    {
        public string Name { get; set; }
        public string StartX { get; set; }
        public string StartY { get; set; }
        public string EndX { get; set; }
        public string EndY { get; set; }
        public string StrokeThickness { get; set; }
        public string Stroke { get; set; }
        public string? StrokeDashArray { get; set; }
        public string? Fill { get; set; }

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ShapeProtocol() { }
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public ShapeProtocol(US_IShape.IShape shape)
        {
            var config = shape.Configuration ?? throw new Exception("Null config of Shape");

            Name = shape.Name;
            StartX = shape.Points[0].X.ToString();
            StartY = shape.Points[0].Y.ToString();
            EndX = shape.Points[1].X.ToString();
            EndY = shape.Points[1].Y.ToString();
            StrokeThickness = config.StrokeThickness.ToString();
            Stroke = ElementConverter.StrokeToString(config.Stroke);
            StrokeDashArray = ElementConverter.StrokeDashArrayToString(config.StrokeDashArray);
            Fill = ElementConverter.FillToString(config.Fill);
        }

        public static US_IShape.IShape ConvertBack(ShapeProtocol protocol)
        {
            ShapeFactory shapeFactory = ShapeFactory.GetInstance();

            var shape = shapeFactory.Create(protocol.Name);
            shape.Configuration = new US_IShape.State
            {
                StrokeDashArray = ElementConverter.StringToStrokeDashArray(protocol.StrokeDashArray),
                Stroke = ElementConverter.StringToBrush(protocol.Stroke),
                Fill = protocol.Fill != null ? ElementConverter.StringToBrush(protocol.Fill) : null,
                StrokeThickness = Double.Parse(protocol.StrokeThickness)
            };

            shape.Points =
            [
                ElementConverter.StringToPoint(protocol.StartX, protocol.StartY),
                ElementConverter.StringToPoint(protocol.EndX, protocol.EndY),
            ];

            return shape;
        }
    }
}
